using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using PhiansCommon.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class ReportReviewDAL : IReportReviewDAL
    {
        #region 加载报告审核
        /// <summary>
        /// 加载报告审核
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadReportReviewList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rt.*,ui.User_name as Inspection_personnel_n,TB_PR.OperateDate from dbo.TB_NDT_report_title rt ");
            sql.Append(" left join  dbo.TB_groupAuthorization as TB_u on TB_u.Group_id= rt.Audit_groupid ");
            sql.Append(" left join dbo.TB_user_info ui on rt.Inspection_personnel=ui.User_count ");
            sql.Append(" left join (select * from TB_ProcessRecord where id in (select Max(id) from TB_ProcessRecord where NodeId=0 group by ReportID)) TB_PR on rt.id=TB_PR.ReportID ");//TB_ProcessRecord去除ReportID字段重复记录,获取id最大值返回
            sql.Append(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                //审核逾期
                if (PageModel.SearchList_[0].Search == "ReviewOverdue")
                {
                    sql.Append(" and DATEDIFF (day ,TB_PR.OperateDate ,'" + DateTime.Now.ToString() + "') >= '" + PageModel.SearchList_[0].Key + "' ");

                    //sql.Append("and rt.id in ( select ReportID from( select ReportID, min(OperateDate) as EditTime from TB_ProcessRecord  where NodeId='0' group by ReportID)as a where '" + DateTime.Now.ToString() + "'-EditTime>=" + PageModel.SearchList_[0].Key + ")");
                }
                else
                {
                    sql.Append(" and " + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
                }
            }

            //登录用户
            sql.Append(" and TB_u." + PageModel.SearchList_[1].Search + " = @0 ", PageModel.SearchList_[1].Key);

            //历史审核
            if (PageModel.SearchList_[3].Key == "1")
            {
                sql.Append(" and state_ != @0", PageModel.SearchList_[2].Key);
                sql.Append(" and rt.Audit_personnel = @0", PageModel.SearchList_[5].Key);

            }
            //待审核
            if (PageModel.SearchList_[3].Key == "0")
            {
                sql.Append(" and state_ = @0 ", PageModel.SearchList_[2].Key);

            }

            sql.OrderBy(" rt.id desc ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_report_title>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 退回报告编制
        /// <summary>
        /// 退回报告编制
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_NDT_error_log"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult BackReviewReport(TB_NDT_report_title model, Guid LogPersonnel, List<TB_NDT_error_log> TB_NDT_error_log, TB_ProcessRecord TB_ProcessRecord, int AuditState)
        {

            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.BackReviewReport)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    #region 判断报告是否已提交或退回

                    var selectReport = PetaPoco.Sql.Builder;
                    selectReport.Append("select * from dbo.TB_NDT_report_title where id=@0", model.id);
                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(selectReport);

                    if (TB_NDT_report_title.state_ != AuditState)
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "该报告已提交或退回，请刷新页面！";
                        return ReturnDALResult;
                    }
                    #region 判断是否有资质审核

                    var sqlQualification = PetaPoco.Sql.Builder;
                    sqlQualification.Append("select * from TB_PersonnelQualification");
                    sqlQualification.Append("where TemplateID=(select id from TB_TemplateFile where FileName=@0) and  UserId=@1 and AuthorizationType=0", TB_NDT_report_title.report_name, LogPersonnel);

                    TB_PersonnelQualification PersonnelQualification = db.FirstOrDefault<TB_PersonnelQualification>(sqlQualification);
                    if (PersonnelQualification == null)
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "你没有资质操作！";
                        return ReturnDALResult;
                    }
                    #endregion
                    //判断报告是否存在
                    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url)))
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "报告文件不存在！";
                        return ReturnDALResult;

                    }

                    #endregion

                    #region 添加添加报告流程记录（有先后顺序：第一步）

                    #region 获取流程耗时

                    //获取报告编制提交的时间
                    var SelectReportInfo = PetaPoco.Sql.Builder;
                    SelectReportInfo.Append("select * from TB_NDT_report_title where id=@0", model.id);//获取报告信息
                    TB_NDT_report_title ReportInfo = db.FirstOrDefault<TB_NDT_report_title>(SelectReportInfo);

                    DateTime Inspection_personnel_date = Convert.ToDateTime(ReportInfo.Inspection_personnel_date);//报告编制提交的时间
                    DateTime NowDateTime = DateTime.Now;

                    #region 流程耗时

                    TimeSpan ts1 = NowDateTime - Inspection_personnel_date;
                    double Days1 = ts1.TotalDays;
                    TB_ProcessRecord.TimeConsuming = (float)Days1;//耗时

                    #endregion

                    #endregion

                    db.Insert(TB_ProcessRecord);

                    #endregion


                    #region 更改报告信息（有先后顺序：第二步）

                    string[] updatefiled = { "return_info", "return_flag", "Inspection_personnel_date", "state_", "Condition", "Audit_personnel", "Audit_groupid", "level_Inspection" };
                    db.Update(model, updatefiled);

                    #endregion

                    #region 错误信息

                    foreach (var item in TB_NDT_error_log)
                    {
                        db.Insert(item);
                    }
                    #endregion

                    #region 搜索编制人员的UserId

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from TB_UserInfo where UserCount = @0 ", TB_ProcessRecord.Operator);

                    Guid UserId = db.FirstOrDefault<TB_UserInfo>(sql).UserId;

                    #endregion

                    #region 工作消息

                    string Message = "你有一个新待编制的报告：" + model.report_num;
                    string MessageType = "待编制报告";
                    string CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    var InsertMessage = PetaPoco.Sql.Builder;
                    //任务消息
                    InsertMessage.Append("insert into  dbo.TB_Message(UserId, MessageType, Message, CreateTime, PushPersonnel) values (@0,@1,@2,@3,@4)", UserId, MessageType, Message, CreateTime, LogPersonnel);
                    db.Execute(InsertMessage);

                    #endregion


                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ReturnDALResult.Success = 1;
                    ReturnDALResult.returncontent = "退回成功！";
                    ReturnDALResult.MessagePerson = UserId.ToString();

                }
                catch (Exception e)
                {
                    db.AbortTransaction();

                    throw;

                }
            }

            return ReturnDALResult;
        }

        #endregion

        #region 提交审核报告

        #region 获取报告文档版本记录信息
        /// <summary>
        /// 获取报告文档版本记录信息
        /// </summary>
        /// <param name="id">报告id</param>
        /// <returns></returns>
        public TB_NDT_RevisionsRecord GetRevisionsRecord(int id)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select top 1 * from dbo.TB_NDT_RevisionsRecord where report_id=@0 ", id);
                sql.OrderBy(" id desc ");
                return db.FirstOrDefault<TB_NDT_RevisionsRecord>(sql);

            }

        }

        #endregion

        #region 提交审核报告
        /// <summary>
        /// 提交审核报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitReviewReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel, int AuditState)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.SubmitReviewReport)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    #region 判断报告是否已提交或退回

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from dbo.TB_NDT_report_title where id=@0", model.id);
                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(sql);

                    if (TB_NDT_report_title.state_ != AuditState)
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "该报告已提交或退回，请刷新页面！";
                        return ReturnDALResult;
                    }
                    #region 判断是否有权限审核

                    var sqlQualification = PetaPoco.Sql.Builder;
                    sqlQualification.Append("select * from TB_PersonnelQualification");
                    sqlQualification.Append("where TemplateID=(select id from TB_TemplateFile where FileName=@0) and  UserId=@1 and AuthorizationType=0", TB_NDT_report_title.report_name, LogPersonnel);

                    TB_PersonnelQualification PersonnelQualification = db.FirstOrDefault<TB_PersonnelQualification>(sqlQualification);
                    if (PersonnelQualification== null)
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "你没有资质操作！";
                        return ReturnDALResult;
                    }
                    #endregion

                    #endregion

                    #region 更改报告信息
                    //报告审核提交到签发
                    if (model.state_ == 3)
                    {
                        string[] updatefiled = { "level_Audit", "state_", "Condition", "issue_personnel", "Audit_date", "Audit_personnel" };
                        db.Update(model, updatefiled);


                        #region 添加添加报告流程记录

                        #region 获取逾期时间设置 && 判断该环节是否逾期

                        //获取报告编制提交的时间
                        var SelectReportInfo = PetaPoco.Sql.Builder;
                        SelectReportInfo.Append("select * from TB_NDT_report_title where id=@0", model.id);//获取报告信息
                        TB_NDT_report_title ReportInfo = db.FirstOrDefault<TB_NDT_report_title>(SelectReportInfo);

                        #region 判断获取逾期时间设置

                        TB_DictionaryManagement TB_DictionaryManagement = new TB_DictionaryManagement();

                        //获取字典逾期时间设置
                        var SelectTime = PetaPoco.Sql.Builder;
                        SelectTime.Append("select * from TB_DictionaryManagement where id=@0", "B507ACED-15EB-47EC-96A9-DEACAC53C729");//报告审核时间
                        TB_DictionaryManagement = db.FirstOrDefault<TB_DictionaryManagement>(SelectTime);


                        #endregion


                        try
                        {
                            DateTime Inspection_personnel_date = Convert.ToDateTime(ReportInfo.Inspection_personnel_date);//报告编制提交的时间
                            TB_ProcessRecord.OverdueSetup = Convert.ToInt32(TB_DictionaryManagement.Project_value);//逾期时间设置
                            DateTime NowDateTime = DateTime.Now;

                            #region 流程耗时

                            TimeSpan ts1 = NowDateTime - Inspection_personnel_date;
                            double Days1 = ts1.TotalDays;
                            TB_ProcessRecord.TimeConsuming = (float)Days1;//耗时

                            #endregion

                            //判断是否逾期
                            if (Inspection_personnel_date.AddDays(TB_ProcessRecord.OverdueSetup) < NowDateTime)
                            {
                                TB_ProcessRecord.IsOverdue = true;//逾期
                                TimeSpan ts = NowDateTime - Inspection_personnel_date.AddDays(TB_ProcessRecord.OverdueSetup);
                                double Days = ts.TotalDays;
                                TB_ProcessRecord.OverdueTime = (float)Days;//逾期耗时

                            }
                            else
                            {
                                TB_ProcessRecord.IsOverdue = false;//不逾期
                            }

                        }
                        catch (Exception)
                        {
                            db.AbortTransaction();
                            throw;
                        }

                        #endregion


                        db.Insert(TB_ProcessRecord);

                        #endregion

                    }
                    //报告审核提交到完成
                    else if (model.state_ == 4)
                    {
                        string[] updatefiled = { "level_Audit", "state_", "Audit_date", "Audit_personnel" };
                        db.Update(model, updatefiled);

                        #region 添加添加报告流程记录

                        db.Insert(TB_ProcessRecord);

                        #endregion
                    }
                    else
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "失败";
                        return ReturnDALResult;

                    }

                    #endregion

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ReturnDALResult.Success = 1;
                    ReturnDALResult.returncontent = "提交成功！";

                }
                catch (Exception e)
                {
                    db.AbortTransaction();

                    throw;

                }
            }

            return ReturnDALResult;
        }
        #endregion

        #endregion

        #region --退回原因

        #region 加载全部退回原因
        /// <summary>
        /// 加载全部退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_DictionaryManagement> AllErrorInfo(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from dbo.TB_DictionaryManagement ");
            sql.Append(" WHERE 1=1 and Parent_id=@0 ", "BDA1459A-DF02-4425-8743-C7BEB66523B2");
            sql.OrderBy(" id ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_DictionaryManagement>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 添加退回原因
        /// <summary>
        /// 添加退回原因
        /// </summary>
        /// <param name="model"></param>
        /// <param name="report_num"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult AddErrorInfo(TB_NDT_error_log model, string report_num, Guid LogPersonnel)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddErrorInfo)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + report_num;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();


                    #region 判断是否已经添加

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from dbo.TB_NDT_error_log where error_remarks = @0 and report_id = @1 ", model.error_remarks, model.report_id);

                    if (db.FirstOrDefault<TB_NDT_error_log>(sql) != null)
                    {
                        ResultModel.Success = 0;
                        ResultModel.returncontent = "不可重复添加！";

                    }
                    #endregion

                    #region 添加退回原因

                    db.Insert(model);

                    #endregion

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "提交成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();

                    ResultModel.Success = 0;
                    ResultModel.returncontent = E.ToString();
                    throw;

                    // throw;
                }

            }

            return ResultModel;
        }

        #endregion

        #region 加载已选退回原因
        /// <summary>
        /// 加载已选退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <param name="id">report_id</param>
        /// <returns></returns>
        public List<TB_NDT_error_log> ReturnErrorInfo(TPageModel PageModel, out int totalRecord, int report_id)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from dbo.TB_NDT_error_log ");
            sql.Append(" WHERE report_id=@0 ", report_id);
            sql.OrderBy(" id ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_error_log>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #endregion

        #region 审核人员自己退回未开始签发的报告
        /// <summary>
        /// 审核人员自己退回未开始签发的报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult TakeBackReviewReport(TB_NDT_report_title model, Guid LogPersonnel, TB_ProcessRecord TB_ProcessRecord)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.TakeBackReviewReport)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    #region 判断报告是否为审核状态

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from dbo.TB_NDT_report_title where id=@0", model.id);
                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(sql);


                    if (TB_NDT_report_title.state_ != (int)LosslessReportStatusEnum.Issue)//报告审核状态
                    {
                        db.AbortTransaction();

                        ResultModel.Success = 0;
                        ResultModel.returncontent = "该报告不为签发状态！";
                        return ResultModel;
                    }

                    #endregion

                    #region 判断报告开始状态

                    if (TB_NDT_report_title.Condition == 1)//报告为已开始状态
                    {
                        db.AbortTransaction();

                        ResultModel.Success = 0;
                        ResultModel.returncontent = "该报告已经签发中！";
                        return ResultModel;
                    }

                    #endregion

                    #region 更改报告信息

                    model.Condition = 0;//报告状态未开始；
                    //更改报告信息
                    string[] updatefiled = { "level_Audit", "Audit_date", "state_", "Condition", "issue_personnel" };
                    db.Update(model, updatefiled);

                    #endregion

                    #region 添加报告流程记录

                    #region 将已有的(报告审核提交到报告签发的)流程更改成（历史的）

                    //获取上一个流程的信息
                    var SelectProcessInfo = PetaPoco.Sql.Builder;
                    SelectProcessInfo.Append("select top 1* from TB_ProcessRecord where ReportID=@0 and NodeId=@1 ", model.id, (int)TB_ProcessRecordNodeIdEnum.ReviewToIssue);//拉回到报告审核 上一条为报告审核提交到报告签发
                    SelectProcessInfo.OrderBy(" OperateDate desc ");
                    TB_ProcessRecord AfProcessRecord = db.FirstOrDefault<TB_ProcessRecord>(SelectProcessInfo);

                    //保证所有的（TB_ProcessRecordNodeIdEnum.ReviewToIssue）的耗时（自己拉回的耗时算进审核）
                    AfProcessRecord.TakeBack = true; //将上一次提交操作标识为已被自己退回

                    string[] TempUpdate = { "TakeBack" };
                    db.Update(AfProcessRecord, TempUpdate);//更改上一个流程的记录

                    #endregion

                    db.Insert(TB_ProcessRecord);

                    #endregion

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "操作成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    throw;
                }

            }

            return ResultModel;

        }

        #endregion

        #region 判断人员资质
        /// <summary>
        /// 判断人员资质
        /// </summary>
        /// <param name="model">人员资质信息</param>
        /// <returns></returns>
        public ReturnDALResult JudgingPersonnelQualifications(TB_PersonnelQualification model)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    #region 判断报告是否为审核状态

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from dbo.TB_PersonnelQualification where TemplateID=@0 and UserId=@1 and AuthorizationType=@2", model.TemplateID, model.UserId, model.AuthorizationType);
                    TB_PersonnelQualification TB_PersonnelQualification = db.FirstOrDefault<TB_PersonnelQualification>(sql);

                    //如果没有资质
                    if (TB_PersonnelQualification == null)
                    {
                        db.AbortTransaction();

                        ResultModel.Success = 0;
                        ResultModel.returncontent = "你没有资质进行操作！";
                        return ResultModel;
                    }

                    #endregion

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "操作成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    throw;
                }

            }

            return ResultModel;
        }

        #endregion


    }


}
