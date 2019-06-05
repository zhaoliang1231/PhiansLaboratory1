using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class ReportApprovalDAL : IReportApprovalDAL
    {

        #region 加载报告签发
        /// <summary>
        /// 加载报告签发
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadReportIssueList(TPageModel PageModel, out int totalRecord)
        {
            #region 判断登陆人员为签发组并且不为审核人员

            var SqlFlag = PetaPoco.Sql.Builder;
            SqlFlag.Append(" select * from dbo.TB_groupAuthorization where UserId=@0 and GroupId='F967C5C6-BF07-47D0-8383-032C12E7DEE2'", PageModel.SearchList_[4].Key);

            #endregion
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rt.*,ui.User_name as Inspection_personnel_n ,ui2.User_name as Audit_personnel_n,TB_PR.OperateDate ");
            sql.Append(" from dbo.TB_NDT_report_title rt ");
            sql.Append(" left join dbo.TB_user_info ui on rt.Inspection_personnel=ui.User_count ");
            sql.Append(" left join dbo.TB_user_info ui2 on rt.Audit_personnel=ui2.User_count ");
            sql.Append(" left join (select * from TB_ProcessRecord where id in (select Max(id) from TB_ProcessRecord where NodeId=1 group by ReportID)) TB_PR on rt.id=TB_PR.ReportID ");//TB_ProcessRecord去除ReportID字段重复记录,获取id最大值返回

            //当登录人员为签发组并且不为审核人员  则加载报告
            if (DbInstance.CreateDataBase().FirstOrDefault<TB_groupAuthorization>(SqlFlag) != null)
            {
                sql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
                {
                    //审核时间逾期：审核签字时间-编制签字时间
                    if (PageModel.SearchList_[0].Search == "ReviewOverdue")
                    {
                        sql.Append(" and Audit_date-Inspection_personnel_date>= @0 ", PageModel.SearchList_[0].Key);
                    }
                    else if (PageModel.SearchList_[0].Search == "IssueOverdue")//签发逾期
                    {
                        //sql.Append(" and rt.id in ( select ReportID from( select ReportID, min(OperateDate) as EditTime from TB_ProcessRecord  where NodeId='1' group by ReportID)as a where '" + DateTime.Now.ToString() + "'-EditTime>=@0 ", PageModel.SearchList_[0].Key);

                        sql.Append(" and DATEDIFF (day ,TB_PR.OperateDate ,'" + DateTime.Now.ToString() + "') >= '" + PageModel.SearchList_[0].Key + "' ");
                    }
                    else
                    {
                        sql.Append(" and rt." + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
                    }
                }

                //历史签发
                if (PageModel.SearchList_[2].Key == "1")
                {
                    sql.Append(" and state_ != @0", PageModel.SearchList_[1].Key);
                    sql.Append(" and rt.issue_personnel = @0", PageModel.SearchList_[3].Key);

                }
                //待签发
                if (PageModel.SearchList_[2].Key == "0")
                {
                    sql.Append(" and state_ = @0 ", PageModel.SearchList_[1].Key);

                }

                sql.Append(" and " + PageModel.SearchList_[3].Search + " != @0 ", PageModel.SearchList_[3].Key);

                sql.OrderBy(" rt.id desc ");

            }
            else
            {
                sql.Append(" WHERE 1=0 ");

            }


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
        public ReturnDALResult BackIssueReport(TB_NDT_report_title model, Guid LogPersonnel, List<TB_NDT_error_log> TB_NDT_error_log, TB_ProcessRecord TB_ProcessRecord, int IssueState)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.BackIssueReport)).Split('%');
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

                    if (TB_NDT_report_title.state_ != IssueState)
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "该报告已提交或退回，请刷新页面！";
                        return ReturnDALResult;
                    }
                    #region 判断是否有资质签发

                    var sqlQualification = PetaPoco.Sql.Builder;
                    sqlQualification.Append("select * from TB_PersonnelQualification");
                    sqlQualification.Append("where TemplateID=(select id from TB_TemplateFile where FileName=@0) and  UserId=@1 and AuthorizationType=1", TB_NDT_report_title.report_name, LogPersonnel);

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

                    //获取报告审核提交的时间
                    var SelectReportInfo = PetaPoco.Sql.Builder;
                    SelectReportInfo.Append("select * from TB_NDT_report_title where id=@0", model.id);//获取报告信息
                    TB_NDT_report_title ReportInfo = db.FirstOrDefault<TB_NDT_report_title>(SelectReportInfo);

                    DateTime Audit_date = Convert.ToDateTime(ReportInfo.Audit_date);//报告编制提交的时间
                    DateTime NowDateTime = DateTime.Now;

                    #region 流程耗时

                    TimeSpan ts1 = NowDateTime - Audit_date;
                    double Days1 = ts1.TotalDays;
                    TB_ProcessRecord.TimeConsuming = (float)Days1;//耗时

                    #endregion

                    #endregion

                    db.Insert(TB_ProcessRecord);

                    #endregion

                    #region 更改报告信息（有先后顺序：第二步）

                    string[] updatefiled = { "return_info", "return_flag", "Inspection_personnel_date", "state_", "Condition", "Audit_personnel", "Audit_groupid", "level_Inspection", "issue_personnel", "issue_date", "level_Audit", "Audit_date" };
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

        #region 提交签发报告
        /// <summary>
        /// 提交签发报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitIssueReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel, int IssueState)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.SubmitIssueReport)).Split('%');
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


                    if (TB_NDT_report_title.state_ != IssueState)
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "该报告已提交或退回，请刷新页面！";
                        return ReturnDALResult;
                    }

                    #endregion

                    #region 判断是否有权限审核

                    var sqlQualification = PetaPoco.Sql.Builder;
                    sqlQualification.Append("select * from TB_PersonnelQualification");
                    sqlQualification.Append("where TemplateID=(select id from TB_TemplateFile where FileName=@0) and  UserId=@1 and AuthorizationType=1", TB_NDT_report_title.report_name, LogPersonnel);

                    TB_PersonnelQualification PersonnelQualification = db.FirstOrDefault<TB_PersonnelQualification>(sqlQualification);
                    if (PersonnelQualification == null)
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "你没有资质操作！";
                        return ReturnDALResult;
                    }
                    #endregion


                    //报告信息更改
                    string[] updatefiled = { "state_", "Condition", "issue_date", "issue_personnel" };
                    db.Update(model, updatefiled);


                    #region 添加添加报告流程记录

                    #region 获取逾期时间设置 && 判断该环节是否逾期

                    //获取报告审核提交的时间
                    var SelectReportInfo = PetaPoco.Sql.Builder;
                    SelectReportInfo.Append("select * from TB_NDT_report_title where id=@0", model.id);//获取报告信息
                    TB_NDT_report_title ReportInfo = db.FirstOrDefault<TB_NDT_report_title>(SelectReportInfo);

                    #region 判断报告是否提交过

                    TB_DictionaryManagement TB_DictionaryManagement = new TB_DictionaryManagement();

                    //获取字典逾期时间设置
                    var SelectTime = PetaPoco.Sql.Builder;
                    SelectTime.Append("select * from TB_DictionaryManagement where id=@0", "357BE131-75EC-4640-9579-6D3E33FBA19C");//报告签发时间
                    TB_DictionaryManagement = db.FirstOrDefault<TB_DictionaryManagement>(SelectTime);


                    #endregion


                    try
                    {
                        DateTime Audit_date = Convert.ToDateTime(ReportInfo.Audit_date);//报告编制提交的时间
                        TB_ProcessRecord.OverdueSetup = Convert.ToInt32(TB_DictionaryManagement.Project_value);//逾期时间设置
                        DateTime NowDateTime = DateTime.Now;

                        #region 流程耗时

                        TimeSpan ts1 = NowDateTime - Audit_date;
                        double Days1 = ts1.TotalDays;
                        TB_ProcessRecord.TimeConsuming = (float)Days1;//耗时

                        #endregion

                        //判断是否逾期
                        if (Audit_date.AddDays(TB_ProcessRecord.OverdueSetup) < NowDateTime)
                        {
                            TB_ProcessRecord.IsOverdue = true;//逾期
                            TimeSpan ts = NowDateTime - Audit_date.AddDays(TB_ProcessRecord.OverdueSetup);
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


    }
}
