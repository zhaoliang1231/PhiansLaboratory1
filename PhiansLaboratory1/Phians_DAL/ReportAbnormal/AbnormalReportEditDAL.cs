using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity.Common;
using PetaPoco;

namespace Phians_DAL
{
    class AbnormalReportEditDAL : IAbnormalReportEditDAL
    {

        #region 获取异常报告信息
        /// <summary>
        /// 获取异常报告信息
        /// </summary>
        /// <param name="MTRNO">MTR</param>
        /// <returns></returns>
        public TB_UnusualCertificate LoadUnusualCertificateInfo(Guid id)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from TB_UnusualCertificate  ");
            sql.Append("where Id =@0 ", id);

            using (var db = DbInstance.CreateDataBase())
            {
                TB_UnusualCertificate TB_UnusualCertificate = db.FirstOrDefault<TB_UnusualCertificate>(sql);
                return TB_UnusualCertificate;

            }

        }

        #endregion

        #region 加载异常报告信息
        /// <summary>
        /// 加载异常申请信息
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="PageModel">页面传参</param>
        /// <param name="totalRecord">输出总记录</param>      
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_error_Certificate> GetUnusualCertificateList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            //sql.Append("select * from TB_UnusualCertificate WHERE 1=1 ");

            sql.Append(" select uc.*,ui.user_name as review_personnel_n ,ui2.user_name as  accept_personnel_n ,TB_NDT_R.report_num,TB_NDT_R.report_name,TB_NDT_R.clientele,TB_NDT_R.clientele_department,TB_NDT_R.id as ReportId FROM TB_NDT_error_Certificate as  uc");
            sql.Append("left join tb_user_info as  ui on uc.review_personnel = ui.user_count");
            sql.Append("left join tb_user_info as  ui2 on uc.accept_personnel = ui2.user_count");
            sql.Append("left join tb_user_info as  ui3 on uc.constitute_personnel = ui3.user_count");
            sql.Append("left join tb_user_info as  ui4 on uc.review_personnel_word = ui4.user_count");
            sql.Append("left join TB_NDT_report_title as  TB_NDT_R on TB_NDT_R.id = uc.report_id");
            sql.Append(" where 1=1 ");


            //0 搜索条件 1历史审核还是待审核 2页面类型 3报告状态 4或5登录人员
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }
            string pageType = PageModel.SearchList_[2].Key;//页面类型
            switch (pageType)
            {
                case "Edit":
                    //历史编制
                    if (PageModel.SearchList_[1].Key == "1")
                    {
                        sql.Append("and (uc.accept_state != @0 or uc.accept_state != @1) and uc.constitute_personnel=@2", PageModel.SearchList_[3].Key, PageModel.SearchList_[4].Key, PageModel.SearchList_[5].Key);

                    }
                    //待编制
                    if (PageModel.SearchList_[1].Key == "0")
                    {
                        sql.Append("and (uc.accept_state = @0 or uc.accept_state = @1)  and uc.constitute_personnel=@2", PageModel.SearchList_[3].Key, PageModel.SearchList_[4].Key, PageModel.SearchList_[5].Key);

                    }
                    break;
                case "Audit"://审核页面
                    //历史审核
                    if (PageModel.SearchList_[1].Key == "1")
                    {
                        sql.Append("and uc.accept_state != @0 and uc.review_personnel_word=@1 ", PageModel.SearchList_[3].Key, PageModel.SearchList_[4].Key);

                    }
                    //待审核
                    if (PageModel.SearchList_[1].Key == "0")
                    {
                        sql.Append("and uc.accept_state = @0 and uc.review_personnel_word=@1 ", PageModel.SearchList_[3].Key, PageModel.SearchList_[4].Key);
                    }
                    break;
                default: break;
            }


            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_error_Certificate>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 提交报告到下一环节
        public ReturnDALResult SubmitAbnormalReportReview(TB_NDT_error_Certificate Model, TB_ProcessRecord TB_ProcessRecord, TB_NDT_report_title reportModule, Guid Operator, string PageType)
        {
            string[] AllInfo = new string[3];// 审核报告
            string OperationType = "";
            string OperationInfo = "";
            switch (PageType)
            {
                case "Edit"://编制页面 
                    AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ErrorReportSubmitAudit)).Split('%');//编制提交到审核
                    break;
                case "Audit":
                    AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ErrorReportSubmitIssue)).Split('%');//审核提交到签发
                    break;
                //case "Issue":
                //   AllInfo=(GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ErrorReportSubmitFinish)).Split('%');//报告完成
                //   break;

                default:
                    break;
            }
            ReturnDALResult ReturnDALResult = new ReturnDALResult();

            //判断报告是否已经提交或退回，如果已经被操作则提示刷新列表
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from TB_NDT_error_Certificate where accept_state=@0 and Id=@1", Convert.ToInt32(Model.accept_state), Model.id);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    #region 获取报告信息

                    var selectReport = PetaPoco.Sql.Builder;
                    selectReport.Append("select * from TB_NDT_report_title where id = @0 ", Model.report_id);

                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(selectReport);

                    OperationType = AllInfo[0];
                    OperationInfo = AllInfo[0] + AllInfo[1] + TB_NDT_report_title.report_num;

                    #endregion

                    OperationType = AllInfo[0];
                    OperationInfo = AllInfo[0] + AllInfo[1] + TB_NDT_report_title.report_num;


                    TB_NDT_error_Certificate UnusualCertificatemodel = db.FirstOrDefault<TB_NDT_error_Certificate>(sql);
                    //判断任务是否存在
                    if (UnusualCertificatemodel == null)
                    {

                        switch (PageType)
                        {
                            case "Edit"://编制页面 
                                string[] UpdateColumns = { "accept_state", "review_personnel_word", "constitute_date", "constitute_personnel" };
                                db.Update(Model, UpdateColumns);//影响行数
                                break;
                            case "Audit":

                                #region 保存历史报告版本(必须先保存原报告)

                                TB_NDT_EditionCertificate EditionCertificate = new TB_NDT_EditionCertificate(); //版本报告储存表
                                EditionCertificate.ReportId = TB_NDT_report_title.id;
                                EditionCertificate.CertificateFormat = TB_NDT_report_title.report_format;
                                EditionCertificate.CertificateUrl = TB_NDT_report_title.report_url;
                                EditionCertificate.Operator = TB_ProcessRecord.Operator;
                                EditionCertificate.OperationDate = DateTime.Now;

                                db.Insert(EditionCertificate);
                                #endregion


                                #region 保存异常报告信息

                                string[] UpdateColumns2 = { "accept_state", "review_personnel_word_date" };
                                db.Update(Model, UpdateColumns2);//影响行数

                                #endregion

                                #region 保存原报告信息（更新异常报告文件的连接，和报告格式）
                                var selectErrorReport = PetaPoco.Sql.Builder;
                                selectErrorReport.Append("select * from TB_NDT_error_Certificate where id=@0", Model.id);
                                TB_NDT_error_Certificate ErrorReportInfo = db.FirstOrDefault<TB_NDT_error_Certificate>(selectErrorReport);

                                reportModule.report_url = ErrorReportInfo.File_url;
                                reportModule.report_format = ErrorReportInfo.File_format;
                                string[] reportModuleColumns = { "state_", "report_url", "report_format" };
                                db.Update(reportModule, reportModuleColumns);

                                #endregion

                                break;
                            default:
                                break;
                        }

                        #region 添加报告流程记录

                        db.Insert(TB_ProcessRecord);

                        #endregion

                        #region 工作消息

                        if (PageType == "Edit")
                        {
                            #region 工作消息

                            string Message = "你有一个新待审核的已编制异常报告：" + TB_NDT_report_title.report_num;
                            string MessageType = "待审核的已编制异常报告";
                            string CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                            #region 搜索编制人员的UserId

                            var getUser_sql = PetaPoco.Sql.Builder;
                            getUser_sql.Append("select * from TB_UserInfo where UserCount = @0 ", Model.review_personnel_word);

                            Guid UserId = db.FirstOrDefault<TB_UserInfo>(getUser_sql).UserId;

                            #endregion

                            var InsertMessage = PetaPoco.Sql.Builder;
                            //任务消息
                            InsertMessage.Append("insert into  dbo.TB_Message(UserId, MessageType, Message, CreateTime, PushPersonnel) values (@0,@1,@2,@3,@4)", UserId, MessageType, Message, CreateTime, Operator);
                            db.Execute(InsertMessage);

                            #endregion
                        }


                        #endregion

                        //添加日志
                        OperationLog operation_log = CommonDAL.operation_log(Operator, OperationType, OperationInfo);
                        db.Insert(operation_log);
                        db.CompleteTransaction();
                        ReturnDALResult.Success = 1;
                    }
                    //任务不在
                    else
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "任务不在请刷新页面";
                    }



                }
                catch (Exception E)
                {

                    ReturnDALResult.Success = 2;
                    ReturnDALResult.returncontent = E.ToString();
                    db.AbortTransaction();

                }
                return ReturnDALResult;

            }
        }

        #endregion

        #region 退回报告
        public ReturnDALResult BackAbnormalReviewReport(TB_NDT_error_Certificate module, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ErrorReportEditSendBackEdit)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + module.report_id;

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    string[] updatefiled = { "review_personnel_word_date", "accept_state", "review_remarks_word", "review_personnel_word" };
                    db.Update(module, updatefiled);


                    #region 工作消息

                    string Message = "你有一个新待编制异常报告：" + module.report_id;
                    string MessageType = "待编制异常报告";
                    string CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                    #region 搜索编制人员的UserId

                    var getUser_sql = PetaPoco.Sql.Builder;
                    getUser_sql.Append("select * from TB_UserInfo where UserCount = @0 ", module.review_personnel_word);

                    Guid UserId = db.FirstOrDefault<TB_UserInfo>(getUser_sql).UserId;

                    #endregion

                    var InsertMessage = PetaPoco.Sql.Builder;
                    //任务消息
                    InsertMessage.Append("insert into  dbo.TB_Message(UserId, MessageType, Message, CreateTime, PushPersonnel) values (@0,@1,@2,@3,@4)", UserId, MessageType, Message, CreateTime, LogPersonnel);
                    db.Execute(InsertMessage);

                    #endregion

                    #region 添加报告流程记录

                    db.Insert(TB_ProcessRecord);

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

                    ReturnDALResult.Success = 0;
                    ReturnDALResult.returncontent = e.ToString();
                }
            }

            return ReturnDALResult;
        }
        #endregion
    }
}
