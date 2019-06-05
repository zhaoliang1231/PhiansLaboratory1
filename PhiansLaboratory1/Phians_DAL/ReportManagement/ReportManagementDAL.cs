using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using Phians_Entity.LosslessReport;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class ReportManagementDAL : IReportManagementDAL
    {
        #region 加载报告管理信息
        /// <summary>
        /// 加载报告管理信息
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadReportManageList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rt.*,ui1.User_name as Inspection_personnel_n,ui2.User_name as Audit_personnel_n,ui3.User_name as issue_personnel_n ,TB_G.GroupName as Audit_groupid_n ");
            sql.Append(" from dbo.TB_NDT_report_title rt ");
            sql.Append(" left join tb_user_info ui1 on rt.Inspection_personnel=ui1.User_count ");
            sql.Append(" left join tb_user_info ui2 on rt.Audit_personnel=ui2.User_count ");
            sql.Append(" left join tb_user_info ui3 on rt.issue_personnel=ui3.User_count ");
            //sql.Append(" left join dbo.TB_groupAuthorization as td on rt.Audit_groupid=td.Group_id ");
            sql.Append(" left join dbo.TB_group as TB_G on TB_G.id=rt.Audit_groupid ");
            sql.Append(" WHERE 1=1  and IsScrap != 1 ");//不为报废报告

            #region 搜索功能

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                if (PageModel.SearchList_[0].Search == "Inspection_date")
                {
                    sql.Append(" and convert(varchar(10), rt." + PageModel.SearchList_[0].Search + ",120) like '%" + PageModel.SearchList_[0].Key + "%' ");
                }
                else if ((PageModel.SearchList_[0].Search == "Inspection_personnel" || PageModel.SearchList_[0].Search == "Audit_personnel" || PageModel.SearchList_[0].Search == "issue_personnel") && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
                {
                    var select_count = PetaPoco.Sql.Builder;
                    select_count.Append(" select * from TB_UserInfo where UserName =@0 ", PageModel.SearchList_[0].Key);
                    TB_UserInfo TB_UserInfo = DbInstance.CreateDataBase().FirstOrDefault<TB_UserInfo>(select_count);
                    if (TB_UserInfo != null)
                    {
                        if (TB_UserInfo.UserCount == "")
                        {
                            sql.Append(" and 1=0 ");
                        }
                        else
                        {
                            sql.Append(" and rt." + PageModel.SearchList_[0].Search + " like '%" + TB_UserInfo.UserCount + "%' ");
                        }
                    }
                }
                else if (PageModel.SearchList_[0].Search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sql.Append(" and rt.Audit_date-rt.Inspection_personnel_date>=" + PageModel.SearchList_[0].Key + " ");
                }
                else if (PageModel.SearchList_[0].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append(" and rt.issue_date-rt.Audit_date>=" + PageModel.SearchList_[0].Key + "");
                }
                //else if (PageModel.SearchList_[0].Search == "EditOverdue")//编制逾期:检验人签字时间-创建时间
                //{
                //    sql.Append(" and rt.Inspection_personnel_date-rt.report_CreationTime>="+PageModel.SearchList_[0].Key+"" );
                //}
                else
                {
                    sql.Append(" and rt." + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
                }
            }

            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[1].Key))
            {
                if (PageModel.SearchList_[1].Search == "Inspection_date")
                {
                    sql.Append(" and convert(varchar(10), rt." + PageModel.SearchList_[1].Search + ",120) like '%" + PageModel.SearchList_[1].Key + "%'");
                }
                else if ((PageModel.SearchList_[1].Search == "Inspection_personnel" || PageModel.SearchList_[1].Search == "Audit_personnel" || PageModel.SearchList_[1].Search == "issue_personnel") && !string.IsNullOrEmpty(PageModel.SearchList_[1].Key))
                {
                    var select_count = PetaPoco.Sql.Builder;
                    select_count.Append(" select * from TB_UserInfo where UserName =@0 ", PageModel.SearchList_[1].Key);
                    TB_UserInfo TB_UserInfo = DbInstance.CreateDataBase().FirstOrDefault<TB_UserInfo>(select_count);
                    if (TB_UserInfo != null)
                    {
                        if (TB_UserInfo.UserCount == "")
                        {
                            sql.Append(" and 1=0 ");
                        }
                        else
                        {
                            sql.Append(" and rt." + PageModel.SearchList_[1].Search + " like '%" + TB_UserInfo.UserCount + "%' ");
                        }
                    }
                }
                else if (PageModel.SearchList_[1].Search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sql.Append("and rt.Audit_date-rt.Inspection_personnel_date>=" + PageModel.SearchList_[1].Key + "");
                }
                else if (PageModel.SearchList_[1].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append("and rt.issue_date-rt.Audit_date>=" + PageModel.SearchList_[1].Key + "");
                }
                else
                {

                    sql.Append(" and rt." + PageModel.SearchList_[1].Search + " like '%" + PageModel.SearchList_[1].Key + "%'");
                }
            }

            if (!string.IsNullOrEmpty(PageModel.SearchList_[2].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[2].Key))
            {
                if (PageModel.SearchList_[2].Search == "Inspection_date")
                {
                    sql.Append(" and convert(varchar(10), rt." + PageModel.SearchList_[2].Search + ",120) like '%" + PageModel.SearchList_[2].Key + "%'");
                }
                else if ((PageModel.SearchList_[2].Search == "Inspection_personnel" || PageModel.SearchList_[2].Search == "Audit_personnel" || PageModel.SearchList_[2].Search == "issue_personnel") && !string.IsNullOrEmpty(PageModel.SearchList_[2].Key))
                {
                    var select_count = PetaPoco.Sql.Builder;
                    select_count.Append(" select * from TB_UserInfo where UserName =@0 ", PageModel.SearchList_[2].Key);
                    TB_UserInfo TB_UserInfo = DbInstance.CreateDataBase().FirstOrDefault<TB_UserInfo>(select_count);
                    if (TB_UserInfo != null)
                    {
                        if (TB_UserInfo.UserCount == "")
                        {
                            sql.Append(" and 1=0 ");
                        }
                        else
                        {
                            sql.Append(" and rt." + PageModel.SearchList_[2].Search + " like '%" + TB_UserInfo.UserCount + "%' ");
                        }
                    }
                }
                else if (PageModel.SearchList_[2].Search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sql.Append("and rt.Audit_date-rt.Inspection_personnel_date>=" + PageModel.SearchList_[2].Key + "");
                }
                else if (PageModel.SearchList_[2].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append("and rt.issue_date-rt.Audit_date>=" + PageModel.SearchList_[2].Key + "");
                }
                else
                {

                    sql.Append(" and rt." + PageModel.SearchList_[2].Search + " like '%" + PageModel.SearchList_[2].Key + "%'");
                }
            }

            if (!string.IsNullOrEmpty(PageModel.SearchList_[3].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[3].Key))
            {
                if (PageModel.SearchList_[3].Search == "Inspection_date")
                {
                    sql.Append(" and convert(varchar(10), rt." + PageModel.SearchList_[3].Search + ",120) like '%" + PageModel.SearchList_[3].Key + "%'");
                }
                else if ((PageModel.SearchList_[3].Search == "Inspection_personnel" || PageModel.SearchList_[3].Search == "Audit_personnel" || PageModel.SearchList_[3].Search == "issue_personnel") && !string.IsNullOrEmpty(PageModel.SearchList_[3].Key))
                {
                    var select_count = PetaPoco.Sql.Builder;
                    select_count.Append(" select * from TB_UserInfo where UserName =@0 ", PageModel.SearchList_[3].Key);
                    TB_UserInfo TB_UserInfo = DbInstance.CreateDataBase().FirstOrDefault<TB_UserInfo>(select_count);
                    if (TB_UserInfo != null)
                    {
                        if (TB_UserInfo.UserCount == "")
                        {
                            sql.Append(" and 1=0 ");
                        }
                        else
                        {
                            sql.Append(" and rt." + PageModel.SearchList_[3].Search + " like '%" + TB_UserInfo.UserCount + "%' ");
                        }
                    }
                }
                else if (PageModel.SearchList_[3].Search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sql.Append("and rt.Audit_date-rt.Inspection_personnel_date>=" + PageModel.SearchList_[3].Key + "");
                }
                else if (PageModel.SearchList_[3].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append("and rt.issue_date-rt.Audit_date>=" + PageModel.SearchList_[3].Key + "");
                }
                else
                {

                    sql.Append(" and rt." + PageModel.SearchList_[3].Search + " like '%" + PageModel.SearchList_[3].Key + "%'");
                }
            }

            //if (!string.IsNullOrEmpty(PageModel.SearchList_[4].Search))
            //{
            //    if (PageModel.SearchList_[4].Search == "Inspection_date")
            //    {
            //        sql.Append(" and convert(varchar(10), rt." + PageModel.SearchList_[4].Search + ",120) like '%" + PageModel.SearchList_[4].Key + "%'");
            //    }
            //    else if ((PageModel.SearchList_[4].Search == "Inspection_personnel" || PageModel.SearchList_[4].Search == "Audit_personnel" || PageModel.SearchList_[4].Search == "issue_personnel") && !string.IsNullOrEmpty(PageModel.SearchList_[4].Key))
            //    {
            //        var select_count = PetaPoco.Sql.Builder;
            //        select_count.Append(" select * from TB_UserInfo where UserName =@0 ", PageModel.SearchList_[4].Key);
            //        TB_UserInfo TB_UserInfo = DbInstance.CreateDataBase().FirstOrDefault<TB_UserInfo>(select_count);
            //        if (TB_UserInfo != null)
            //        {
            //            if (TB_UserInfo.UserCount == "")
            //            {
            //                sql.Append(" and 1=0 ");
            //            }
            //            else
            //            {
            //                sql.Append(" and rt." + PageModel.SearchList_[4].Search + " like '%" + PageModel.SearchList_[4].Key + "%' ");
            //            }
            //        }
            //    }
            //    else if (PageModel.SearchList_[4].Search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
            //    {
            //        sql.Append("and rt.Audit_date-rt.Inspection_personnel_date>=@0", PageModel.SearchList_[4].Key);
            //    }
            //    else if (PageModel.SearchList_[4].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
            //    {
            //        sql.Append("and rt.issue_date-rt.AuditIssueRetrunTime>=@0", PageModel.SearchList_[4].Key);
            //    }
            //    else
            //    {

            //        sql.Append(" and rt." + PageModel.SearchList_[4].Search + " like '%" + PageModel.SearchList_[4].Key + "%'");
            //    }
            //}

            #endregion

            //报告状态为报告完成 or 报告异常申请
            sql.Append(" and (rt.state_ = " + PageModel.SearchList_[4].Key + " or rt.state_ = " + PageModel.SearchList_[5].Key + ") ");


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

        #region 添加线下报告信息
        /// <summary>
        /// 添加线下报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult AddUnderLineReportInfo(TB_NDT_report_title model, Guid LogPersonnel)
        {

            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddUnderLineReportInfo)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;


            var GetReportSql = PetaPoco.Sql.Builder;
            GetReportSql.Append("select * from TB_NDT_report_title where report_num=@0 and IsScrap != 1 ", model.report_num);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    //判断报告编号是否重复
                    if (db.FirstOrDefault<TB_NDT_report_title>(GetReportSql) != null)
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "报告编号已经存在！";
                        return ReturnDALResult;

                    }

                    //添加报告信息
                    db.Insert(model);


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

        #region 异常报告申请
        /// <summary>
        /// 异常报告申请
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_NDT_error_Certificate"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitAbnormalReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, TB_NDT_error_Certificate error_Certificate, Guid LogPersonnel)
        {

            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.SubmitAbnormalReport)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;


            var GetReportSql = PetaPoco.Sql.Builder;
            GetReportSql.Append("select * from TB_NDT_report_title where id=@0 ", model.id);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    TB_NDT_report_title Module = db.FirstOrDefault<TB_NDT_report_title>(GetReportSql);
                    if (Module != null && Module.state_ == model.state_)
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 2;
                        ReturnDALResult.returncontent = "请勿重复提交！";
                        return ReturnDALResult;
                    }

                    if (Module != null && Module.IsUnderLine == true)
                    {
                        db.AbortTransaction();

                        ReturnDALResult.Success = 2;
                        ReturnDALResult.returncontent = "线下报告不能走异常流程！";
                        return ReturnDALResult;
                    }

                    #region 更改报告信息

                    string[] updatefiled = { "state_" };
                    db.Update(model, updatefiled);

                    #endregion

                    #region 添加添加报告流程记录

                    db.Insert(TB_ProcessRecord);

                    #endregion

                    #region 添加异常报告

                    db.Insert(error_Certificate);

                    #endregion

                    #region 搜索审核人员的UserId

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from TB_UserInfo where UserCount = @0 ", error_Certificate.review_personnel);

                    Guid UserId = db.FirstOrDefault<TB_UserInfo>(sql).UserId;

                    #endregion

                    #region 工作消息

                    string Message = "你有一个新待审核的异常报告：" + model.report_num;
                    string MessageType = "待审核的异常报告";
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
                    ReturnDALResult.returncontent = "提交成功！";
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

        #region 批量下载

        #region 选择下载
        /// <summary>
        /// 选择下载
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<ReportManagementDownload> Choosedownload(string ids)
        {

            List<ReportManagementDownload> DownloadInfoList = new List<ReportManagementDownload>();
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    #region 搜索选择的报告信息

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select report_url,report_num,report_format from TB_NDT_report_title where id in (" + ids + ") ");

                    List<TB_NDT_report_title> ReportInfo = db.Fetch<TB_NDT_report_title>(sql);
                    foreach (var item in ReportInfo)
                    {
                        ReportManagementDownload DownloadInfo = new ReportManagementDownload();
                        string newreport_url = item.report_url.ToString().Substring(1, item.report_url.ToString().Length - 1);
                        DownloadInfo.report_url = newreport_url;
                        DownloadInfo.newfilename = (item.report_num.ToString()).Replace("/", "_") + item.report_format;
                        DownloadInfoList.Add(DownloadInfo);
                    }
                    #endregion

                    db.CompleteTransaction();

                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                    throw;

                }
            }

            return DownloadInfoList;

        }

        #endregion

        #region 选择下载
        /// <summary>
        /// 选择下载
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportManagementDownload> Searchdownload(dynamic model)
        {
            List<ReportManagementDownload> DownloadInfoList = new List<ReportManagementDownload>();

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    #region 搜索选择的报告信息

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append(" select report_url,report_num,report_format from TB_NDT_report_title ");
                    sql.Append(" where report_url!='' ");

                    #region 搜索条件

                    if (!string.IsNullOrEmpty(model.search))
                    {
                        if (model.search == "Inspection_date")
                        {
                            sql.Append(" and convert(varchar(10), " + model.search + ",120) like '%" + model.key + "%'");
                        }
                        else
                        {
                            sql.Append(" and " + model.search + " like '%" + model.key + "%'");
                        }
                    }
                    if (!string.IsNullOrEmpty(model.search1))
                    {
                        if (model.search1 == "Inspection_date")
                        {
                            sql.Append(" and convert(varchar(10), " + model.search1 + ",120) like '%" + model.key + "%'");
                        }
                        else
                        {
                            sql.Append(" and " + model.search1 + " like '%" + model.key1 + "%'");
                        }
                    }
                    if (!string.IsNullOrEmpty(model.search2))
                    {
                        if (model.search2 == "Inspection_date")
                        {
                            sql.Append(" and convert(varchar(10), " + model.search2 + ",120) like '%" + model.key + "%'");
                        }
                        else
                        {
                            sql.Append(" and " + model.search2 + " like '%" + model.key2 + "%'");
                        }
                    }
                    if (!string.IsNullOrEmpty(model.search3))
                    {
                        if (model.search3 == "Inspection_date")
                        {
                            sql.Append(" and convert(varchar(10), " + model.search3 + ",120) like '%" + model.key3 + "%'");
                        }
                        else
                        {
                            sql.Append(" and " + model.search3 + " like '%" + model.key3 + "%'");
                        }
                    }

                    #endregion

                    List<TB_NDT_report_title> ReportInfo = db.Fetch<TB_NDT_report_title>(sql);
                    foreach (var item in ReportInfo)
                    {
                        ReportManagementDownload DownloadInfo = new ReportManagementDownload();

                        string newreport_url = item.report_url.ToString().Substring(0, item.report_url.ToString().Length);
                        DownloadInfo.report_url = (newreport_url.Replace("/", @"\"));
                        DownloadInfo.newfilename = ((item.report_num.ToString()).Replace("/", "_") + item.report_format);
                        DownloadInfoList.Add(DownloadInfo);
                    }
                    #endregion

                    db.CompleteTransaction();

                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                    throw;

                }
            }

            return DownloadInfoList;

        }

        #endregion


        #endregion

        #region 根据列导出报告
        /// <summary>
        /// 根据列导出报告
        /// </summary>
        /// <param name="search">搜索条件</param>
        /// <param name="key">搜索值</param>
        /// <param name="tempFilePath">导出路径</param>
        /// <param name="type">导出类型</param>
        /// <param name="ids">报告id集合</param>
        /// <param name="columns">列集合</param>
        /// <returns></returns>
        public System.Data.DataTable Report_ExportExcl(string search3, string key3, string search2, string key2, string search1, string key1, string search, string key, int type, string ids, string columns)
        {
            try
            {
                var sql = PetaPoco.Sql.Builder;

                sql.Append(" select " + columns + " from  TB_NDT_report_title where", columns);
                if (type == 2)//选择下载
                {
                    string[] id = ids.Split(',');
                    sql.Append(" id=@0", id[0]);
                    for (int i = 0; i < id.Length; i++)
                    {
                        sql.Append("or id=@0", id[i]);
                    }
                }
                else
                {
                    sql.Append("1=1");
                    //搜索下载
                    if (!string.IsNullOrEmpty(search3) && !string.IsNullOrEmpty(key3))
                    {
                        sql.Append(" and " + search3 + " like @0 ", "%" + key3 + "%");
                    }
                    if (!string.IsNullOrEmpty(search2) && !string.IsNullOrEmpty(key2))
                    {
                        sql.Append(" and " + search2 + " like @0 ", "%" + key2 + "%");
                    }
                    if (!string.IsNullOrEmpty(search1) && !string.IsNullOrEmpty(key1))
                    {
                        sql.Append(" and " + search1 + " like @0 ", "%" + key1 + "%");
                    }
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
                    {
                        sql.Append(" and " + search + " like @0 ", "%" + key + "%");
                    }
                }

                using (var db = DbInstance.CreateDataBase())
                {
                    List<TB_NDT_report_title> reportList = db.Fetch<TB_NDT_report_title>(sql);
                    //PetaPoco框架自带分页
                    System.Data.DataTable result = ListToDataTable.ListToDataTable_(reportList);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region 页面字段显示List
        /// <summary>
        /// 页面字段显示List
        /// </summary>
        /// <param name="PageId">页面id</param>
        /// <returns></returns>
        public List<TB_PageShowCustom> loadPageShowSetting(string PageId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from");
            sql.Append(" TB_PageShowCustom ");
            sql.Append(" where PageId=@0 and hidden=0", PageId);
            sql.OrderBy(" FieldSort ");

            using (var db = DbInstance.CreateDataBase())
            {
                return db.Fetch<TB_PageShowCustom>(sql);
            }
        }
        #endregion

        #region 获取报告信息
        /// <summary>
        /// 获取报告信息
        /// </summary>
        /// <param name="id">报告id</param>
        /// <returns></returns>
        public TB_NDT_report_title LoadReportInfo(int id)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from");
            sql.Append(" TB_NDT_report_title ");
            sql.Append(" where id = @0 ", id);

            using (var db = DbInstance.CreateDataBase())
            {
                return db.FirstOrDefault<TB_NDT_report_title>(sql);
            }
        }

        #endregion
    }
}
