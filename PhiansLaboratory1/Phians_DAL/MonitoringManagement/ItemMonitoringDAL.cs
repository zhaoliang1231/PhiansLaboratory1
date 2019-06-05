using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class ItemMonitoringDAL : IItemMonitoringDAL
    {

        #region 获取报告列表
        /// <summary>
        /// 加载获取报告列表
        /// </summary>
        /// <param name="PageModel">加载的行数页数、搜索内容、MTR单号</param>
        /// <param name="totalRecord">totalRecord</param>
        /// <returns>项目信息表</returns>
        public List<TB_NDT_report_title> load_list(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rt.*,ui1.User_name as Inspection_personnel_n,ui2.User_name as Audit_personnel_n,ui3.User_name as issue_personnel_n ,TB_G.GroupName as Audit_groupid_n ");
            sql.Append(" from dbo.TB_NDT_report_title rt ");
            sql.Append(" left join tb_user_info ui1 on rt.Inspection_personnel=ui1.User_count ");
            sql.Append(" left join tb_user_info ui2 on rt.Audit_personnel=ui2.User_count ");
            sql.Append(" left join tb_user_info ui3 on rt.issue_personnel=ui3.User_count ");
            //sql.Append(" left join dbo.TB_groupAuthorization as td on rt.Audit_groupid=td.Group_id ");
            sql.Append(" left join dbo.TB_group as TB_G on TB_G.id=rt.Audit_groupid ");
            sql.Append(" WHERE 1=1 ");

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
                    sql.Append(" and rt.Audit_date-rt.Inspection_personnel_date>=@0 ", PageModel.SearchList_[0].Key);
                }
                else if (PageModel.SearchList_[0].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append(" and rt.issue_date-rt.Audit_date>=@0 ", PageModel.SearchList_[0].Key);
                }
                else if (PageModel.SearchList_[0].Search == "EditOverdue")//编制逾期:检验人签字时间-创建时间
                {
                    sql.Append(" and rt.Inspection_personnel_date-rt.report_CreationTime>=@0 ", PageModel.SearchList_[0].Key);
                }
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
                    sql.Append("and rt.Audit_date-rt.Inspection_personnel_date>=@0", PageModel.SearchList_[1].Key);
                }
                else if (PageModel.SearchList_[1].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append("and rt.issue_date-rt.AuditIssueRetrunTime>=@0", PageModel.SearchList_[1].Key);
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
                    sql.Append("and rt.Audit_date-rt.Inspection_personnel_date>=@0", PageModel.SearchList_[2].Key);
                }
                else if (PageModel.SearchList_[2].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append("and rt.issue_date-rt.AuditIssueRetrunTime>=@0", PageModel.SearchList_[2].Key);
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
                    sql.Append("and rt.Audit_date-rt.Inspection_personnel_date>=@0", PageModel.SearchList_[3].Key);
                }
                else if (PageModel.SearchList_[3].Search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sql.Append("and rt.issue_date-rt.AuditIssueRetrunTime>=@0", PageModel.SearchList_[3].Key);
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

            if (!string.IsNullOrEmpty(PageModel.SortName) && !string.IsNullOrEmpty(PageModel.SortOrder)) 
            {
                sql.OrderBy("rt." + PageModel.SortName + " " + PageModel.SortOrder);

            }

            //sql.OrderBy(" rt.issue_date,rt.Audit_date desc ");
            //sql.OrderBy(" rt.id desc ");
            
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_report_title>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion


        #region 加载错误报告
        public List<TB_NDT_error_Certificate> load_Errorlist(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select  ui1.User_name as accept_personnel_n,ui2.User_name as review_personnel_n,ui3.User_name as constitute_personnel_n,ui4.User_name as review_personnel_word_n,TE.* from TB_NDT_error_Certificate TE");
            sql.Append("left join tb_user_info ui1 on TE.accept_personnel=ui1.User_count");
            sql.Append("left join tb_user_info ui2 on TE.review_personnel=ui2.User_count");
            sql.Append("left join tb_user_info ui3 on TE.constitute_personnel=ui3.User_count");
            sql.Append("left join tb_user_info ui4 on TE.review_personnel_word=ui4.User_count");
            sql.Append(" where report_id=@0", PageModel.SearchList_[0].Key);

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_error_Certificate>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion
    }
}
