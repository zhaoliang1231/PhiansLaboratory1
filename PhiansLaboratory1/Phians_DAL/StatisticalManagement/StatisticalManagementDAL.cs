using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity.Common;
using Phians_Entity.LosslessReport;

namespace Phians_DAL
{
    class StatisticalManagementDAL : IStatisticalManagementDAL
    {
        #region 统计管理

        #region 无损获取统计数据

        public List<TB_StatisticsCount> GetStatistical(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;

            string StatisticsSearch = PageModel.SearchList_[0].Search;//统计类型
            string StatisticsKey = PageModel.SearchList_[0].Key;//0 统计科室组 1统计组里人
            string DepartmentGroupID = PageModel.SearchList_[1].Search;//科室ID
            string GroupName = PageModel.SearchList_[1].Key;//组名

            int ElapsedTimeDay = Convert.ToInt32(PageModel.SearchList_[2].Search);//耗时天数
            int OverdueTimeDay = 0;
            if (!string.IsNullOrEmpty(PageModel.SearchList_[2].Key))
            {
                string[] ProjectNameValue = PageModel.SearchList_[2].Key.Split('+');//逾期天数
                OverdueTimeDay = Convert.ToInt32(ProjectNameValue.Last());

            }

            string dateStart = PageModel.SearchList_[3].Search;
            string dateEnd = PageModel.SearchList_[3].Key;

            string ReportType = PageModel.SearchList_[4].Search;//报告类型名字
            string ReportOverdueName = PageModel.SearchList_[4].Key;//逾期类型名字

            string SendBackCount = PageModel.SearchList_[5].Search;//签发退回次数
            //统计类型
            if (!string.IsNullOrEmpty(StatisticsKey))
            {

                switch (StatisticsSearch)
                {
                    case "report_Edit"://报告量：检验人
                        #region  0 统计科室组 1统计组里人
                        if (StatisticsKey == "0")//
                        {
                            sql.Append(@"select TU.GroupName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount 
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Inspection_personnel=TU.UserCount 
                                where TR.state_!=1 and TU.GroupParentId=@0 ", DepartmentGroupID);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by TU.GroupName");
                        }

                        else
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(UserName) as StatisticsCount from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG left join dbo.TB_group on TG.Group_id=TB_group.id 
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId 
                                where TG.GroupId=@0)as us left join TB_NDT_report_title TR on us.UserCount=TR.Inspection_personnel where TR.state_!=1", GroupName);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }

                        #endregion
                        break;
                    case "report_Issue"://签发量
                        #region  0 统计科室组 1统计组里人
                        if (StatisticsKey == "0")//
                        {
                            sql.Append(@"select TU.GroupName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount  from (select * from TB_NDT_report_title )as TR left join  (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    ) as TU on TR.issue_personnel=TU.UserCount where TR.state_!=1 and TU.GroupParentId=@0", DepartmentGroupID);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by TU.GroupName");
                        }

                        else
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(UserName) as StatisticsCount from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG left join dbo.TB_group on TG.Group_id=TB_group.id 
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId 
                                where TG.GroupId=@0)as us left join TB_NDT_report_title TR on us.UserCount=TR.issue_personnel where TR.state_!=1", GroupName);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }

                        break;
                        #endregion
                    case "report_Audit"://审核量
                        #region  0 统计科室组 1统计组里人
                        if (StatisticsKey == "0")//
                        {
                            sql.Append(@"select TU.GroupName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount 
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Audit_personnel=TU.UserCount 
                                where TR.state_!=1 and TU.GroupParentId=@0 ", DepartmentGroupID);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by TU.GroupName");
                        }

                        else
                        {
                            sql.Append(@" select UserName as StatisticsType,COUNT(UserName) as StatisticsCount from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG left join dbo.TB_group on TG.Group_id=TB_group.id 
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId 
                                where TG.GroupId=@0)as us left join TB_NDT_report_title TR on us.UserCount=TR.Audit_personnel where TR.state_!=1", GroupName);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }

                        #endregion
                        break;
                    case "report_Type"://报告类型
                        #region 报告类型
                        sql.Append("select COUNT(report_name) as StatisticsCount,report_name as StatisticsType from TB_NDT_report_title where state_!=1 and report_name is not null  ");
                        if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and Inspection_date between '" + dateStart + "' and GETDATE() ");
                        }
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                        }
                        sql.Append(" group by report_name ");
                        #endregion
                        break;
                    case "IssueSendBack"://退回次数统计
                        #region
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select GroupName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.id in (select report_id from ( select COUNT(el.report_id) as backcount,el.report_id as report_id from dbo.TB_NDT_error_log el group by el.report_id) a where backcount>=@0) and us.GroupParentId=@1", SendBackCount, DepartmentGroupID);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.GroupName");

                        }
                        else
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from ( 
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.id in (select report_id from ( select COUNT(el.report_id) as backcount,el.report_id as report_id from dbo.TB_NDT_error_log el group by el.report_id) a where backcount>=@0) and us.GroupId=@1", SendBackCount, GroupName);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }

                        break;
                        #endregion
                    case "error_type"://错误类型
                        #region 科室错误类型
                        sql.Append(@"select COUNT(el.error_remarks) as StatisticsCount,el.error_remarks as StatisticsType from dbo.TB_NDT_error_log el left join (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            ) ud on el.addpersonnel=ud.UserCount where 1=1");
                        //0 科室
                        if (StatisticsKey == "0")
                        {
                            sql.Append(" and ud.GroupParentId=@0", DepartmentGroupID);
                        }
                        // 组
                        else
                        {
                            sql.Append("and ud.GroupId=@0", GroupName);
                        }
                        if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and el.add_date between '" + dateStart + "' and GETDATE() ");
                        }
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and el.add_date  between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                        }
                        sql.Append("group by el.error_remarks");
                        #endregion
                        break;
                    case "Edittime": //编制耗时
                        #region 编制耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select GroupName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.Inspection_personnel_date-rt.ReportCreationTime>=@0 and rt.state_=4 and us.GroupParentId=@1", ElapsedTimeDay, DepartmentGroupID);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.GroupName");
                        }
                        else
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from ( 
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.Inspection_personnel_date-rt.ReportCreationTime>=@0 and rt.state_=4 and us.GroupId=@1", ElapsedTimeDay, GroupName);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }

                        break;
                        #endregion
                    case "Reviewtime":
                        #region 审核耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select GroupName as StatisticsType,COUNT(Audit_personnel) as StatisticsCount from TB_NDT_report_title as rt left join ( select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId  )as us  on us.UserCount=rt.Audit_personnel where rt.Audit_date-rt.Inspection_personnel_date>= @0 and rt.state_=4  and us.GroupParentId=@1", ElapsedTimeDay, DepartmentGroupID);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.GroupName");
                        }
                        else
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(Audit_personnel) as StatisticsCount from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId  )as us  on us.UserCount=rt.Audit_personnel where rt.Audit_date-rt.Inspection_personnel_date>= @0 and rt.state_=4  and us.GroupId=@1", ElapsedTimeDay, GroupName);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }
                        break;
                        #endregion
                    case "Issuetime"://签发耗时
                        #region 签发耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select GroupName as StatisticsType,COUNT(issue_personnel) as StatisticsCount from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId  )as us  on us.UserCount=rt.issue_personnel where rt.issue_date - rt.Audit_date>=@0  and rt.state_=4 and us.GroupParentId=@1", ElapsedTimeDay, DepartmentGroupID);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.GroupName");
                        }
                        else
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(issue_personnel) as StatisticsCount from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.issue_personnel where rt.issue_date - rt.Audit_date>=@0 and  rt.state_=4  and us.GroupId=@1", ElapsedTimeDay, GroupName);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }
                        break;
                        #endregion
                    case "Alltime"://总耗时：签发签字时间-报告创建时间
                        #region 总耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select GroupName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel where rt.issue_date - rt.ReportCreationTime>=@0 and rt.state_=4  and us.GroupParentId=@1", ElapsedTimeDay, DepartmentGroupID);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.ReportCreationTime between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.ReportCreationTime between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.GroupName");
                        }
                        else
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel where rt.issue_date - rt.ReportCreationTime>=@0 and  rt.state_=4 and   us.GroupId=@1", ElapsedTimeDay, GroupName);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.ReportCreationTime between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.ReportCreationTime between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                        }
                        break;
                        #endregion
                    case "passing_rate"://一次通过率
                        #region 一次通过率
                        if (StatisticsKey == "0")//科室
                        {
                            #region 所有报告数量
                            sql.Append(@"with t1 as(select GroupName,Count(GroupName) 报告数量 from TB_NDT_report_title 
                            left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                    from dbo.TB_groupAuthorization as TG
                                    left join dbo.TB_group on TG.Group_id=TB_group.id
                                    left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId ) as us on TB_NDT_report_title.Inspection_personnel=us.UserCount where  state_!=1 and GroupName is not null and us.GroupParentId=@0 group by  us.GroupName ),", DepartmentGroupID);
                            #endregion

                            #region 报告一次通过的数量
                            sql.Append(@"t2 as(select GroupName,Count(GroupName) 一次通过数量 from TB_NDT_report_title left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId) as us on TB_NDT_report_title.Inspection_personnel=us.UserCount where  state_=4  and return_flag=0 and us.GroupParentId=@0 group by  us.GroupName )", DepartmentGroupID);
                            #endregion

                            #region 计算一次通过率
                            sql.Append("select t1.GroupName as StatisticsType ,cast (CAST( t2.一次通过数量 AS decimal)/CAST ( t1.报告数量  as decimal)*100 as decimal(10,0)) as StatisticsCount from t1 left join t2 on t1.GroupName=t2.GroupName where t2.GroupName is not null");
                            #endregion
                        }
                        else
                        {
                            #region 所有报告数量
                            sql.Append(@"with t1 as(select Count(UserCount) 报告数量 ,us.UserCount from TB_NDT_report_title left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                    from dbo.TB_groupAuthorization as TG
                                    left join dbo.TB_group on TG.Group_id=TB_group.id
                                    left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId ) as us on TB_NDT_report_title.Inspection_personnel=us.UserCount where GroupId=@0  and state_=4 group by  UserCount), ", GroupName);
                            #endregion

                            #region 报告一次通过的数量
                            sql.Append(@"t2 as(select us.UserCount ,Count(UserCount) 一次通过数量 from TB_NDT_report_title left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                    from dbo.TB_groupAuthorization as TG
                                    left join dbo.TB_group on TG.Group_id=TB_group.id
                                    left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId ) as us on TB_NDT_report_title.Inspection_personnel=us.UserCount where GroupId=@0 and state_=4  and return_flag=0 group by  UserCount ) ", GroupName);
                            #endregion

                            #region 计算一次通过率
                            sql.Append("select TU.User_name as StatisticsType ,cast (CAST( t2.一次通过数量 AS decimal)/CAST ( t1.报告数量  as decimal)*100 as decimal(10,0)) as StatisticsCount from t1 left join t2 on t1.UserCount=t2.UserCount left join dbo.TB_user_info as TU on t1.UserCount= TU.User_count   where t1.报告数量 is not null and t2.一次通过数量 is not null");
                            #endregion
                        }
                        break;
                        #endregion
                    case "overdue":
                        #region 初始编制
                        if (ReportOverdueName == "初始编制")
                        {
                            //和用户分组表连接
                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from TB_NDT_report_title  rt
                                left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                    from dbo.TB_groupAuthorization as TG
                                    left join dbo.TB_group on TG.Group_id=TB_group.id
                                    left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel");
                            //满足逾期的报告
                            sql.Append(@"where us.GroupId=@0 and  rt.id in(select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 and TakeBack=0   group by ReportID ) ", GroupName);

                        }
                        #endregion

                        #region 退回编制
                        if (ReportOverdueName == "退回编制")
                        {

                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from TB_NDT_report_title  rt
                                    left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel");
                            sql.Append(@"where rt.id in(select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 and TakeBack=1 group by ReportID) and us.GroupId=@0", GroupName);


                        }
                        #endregion

                        #region 报告审核
                        if (ReportOverdueName == "报告审核")
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(Audit_personnel) as StatisticsCount from TB_NDT_report_title  rt
                                    left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Audit_personnel");
                            sql.Append(@"where rt.id in(
                                          select ReportID from TB_ProcessRecord where  IsOverdue=1 and (NodeId=1 or NodeId=13)
                                        ) and us.GroupId=@0", GroupName);


                        }
                        #endregion

                        #region 报告签发
                        if (ReportOverdueName == "报告签发")
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(issue_personnel) as StatisticsCount from TB_NDT_report_title  rt
                            left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG
                                left join dbo.TB_group on TG.Group_id=TB_group.id
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.issue_personnel");
                            sql.Append(@"where rt.id in(
                                select ReportID from TB_ProcessRecord where  IsOverdue=1 and NodeId=5 
                                    ) and us.GroupId=@0", GroupName);
                        }

                        #endregion
                        if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and rt.Inspection_date between '" + dateStart + "' and GETDATE() ");
                        }
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                        }
                        sql.Append("group by us.UserName");
                        break;
                    case "overdueReportType":
                        #region 初始编制
                        if (ReportOverdueName == "初始编制")
                        {
                            //和用户分组表连接
                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from TB_NDT_report_title  rt
                                            left join dbo.TB_UserInfo us on rt.Inspection_personnel=us.UserCount");
                            //满足逾期的报告
                            sql.Append(@"where rt.report_name=@0 and  rt.id in(select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 group by ReportID) ", ReportType);
                            sql.Append("group by us.UserName");
                        }
                        #endregion

                        #region 退回编制
                        if (ReportOverdueName == "退回编制")
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from TB_NDT_report_title  rt
                                    left join dbo.TB_UserInfo us on rt.Inspection_personnel=us.UserCount");
                            sql.Append(@"where rt.id in( select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 and TakeBack=1 group by ReportID ) and rt.report_name=@0", ReportType);
                            sql.Append("group by us.UserName");

                        }
                        #endregion

                        #region 报告审核
                        if (ReportOverdueName == "报告审核")
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(Audit_personnel) as StatisticsCount from TB_NDT_report_title  rt
                            left join dbo.TB_UserInfo TU on rt.Audit_personnel=TU.UserCount ");
                            sql.Append(@"where rt.id in(  select ReportID from TB_ProcessRecord where  IsOverdue=1 and (NodeId=1 or NodeId=13) ) and rt.report_name=@0", ReportType);
                            sql.Append("group by TU.UserName");

                        }
                        #endregion

                        #region 报告签发
                        if (ReportOverdueName == "报告签发")
                        {
                            sql.Append(@"select UserName as StatisticsType,COUNT(issue_personnel) as StatisticsCount from TB_NDT_report_title  rt
                            left join dbo.TB_UserInfo TU on rt.issue_personnel=TU.UserCount ");
                            sql.Append(@"where rt.id in( select ReportID from TB_ProcessRecord where  IsOverdue=1 and NodeId=5  ) and rt.report_name=@0", ReportType);
                            sql.Append("group by TU.UserName");
                        }

                        #endregion
                        break;
                    case "editAlloverdue":
                        #region 编制累计耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select GroupName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where  us.GroupParentId=@0 and rt.id in(select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord where NodeId=0 or( NodeId =0 and TakeBack=0) group by ReportID )as a where dateTime>@1)", DepartmentGroupID, ElapsedTimeDay);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.GroupName");

                        }

                        else
                        {
                            #region 班组编制累计耗时

                            sql.Append(@"select UserName as StatisticsType,COUNT(Inspection_personnel) as StatisticsCount from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where  us.GroupId=@0 and rt.id in(select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord where NodeId=0 or( NodeId =0 and TakeBack=0) group by ReportID )as a where dateTime>@1)", GroupName, ElapsedTimeDay);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by us.UserName");
                            #endregion
                        }
                        #endregion
                        break;
                    case "ReviewAlloverdue":

                        #region 科室审核累计耗时
                        if (StatisticsKey == "0")//科室
                        {

                            sql.Append(@"select TU.GroupName as StatisticsType,COUNT(Audit_personnel) as StatisticsCount 
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Audit_personnel=TU.UserCount ");
                            sql.Append(@"where TR.state_!=1 and TU.GroupParentId=@0 and TU.GroupName is not null and TR.id in( 
                                        select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                                        where NodeId=3 or NodeId=1 or NodeId=13 group by ReportID) as a where DateTime>=@1)  ", DepartmentGroupID, ElapsedTimeDay);


                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by TU.GroupName");


                        }
                        #endregion

                        #region 班组审核累计耗时
                        else
                        {

                            sql.Append(@"select UserName as StatisticsType,COUNT(Audit_personnel) as StatisticsCount
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Audit_personnel=TU.UserCount ");
                            sql.Append(@" where TR.state_!=1 and TU.GroupName is not null and TU.GroupId=@0 and TR.id in(  select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                                        where NodeId=3 or NodeId=1 or NodeId=13 group by ReportID) as a where DateTime>=@1) ", GroupName, ElapsedTimeDay);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by TU.UserName");

                        }

                        #endregion
                        break;
                    case "IssueAlloverdue":

                        #region 科室签发累计耗时
                        if (StatisticsKey == "0")//科室
                        {

                            sql.Append(@"select TU.GroupName as StatisticsType,COUNT(issue_personnel) as StatisticsCount 
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.issue_personnel=TU.UserCount ");
                            sql.Append(@"where TR.state_!=1 and TU.GroupName is not null and TU.GroupParentId=@0 and TR.id in( select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                                        where NodeId=4 or NodeId=5 group by ReportID) as a where DateTime>=@1)", DepartmentGroupID, ElapsedTimeDay);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by TU.GroupName");
                        }
                        #endregion

                        #region 班组签发累计耗时
                        else
                        {

                            sql.Append(@"select UserName as StatisticsType,COUNT(issue_personnel) as StatisticsCount
                                from (select * from TB_NDT_report_title )as TR left join  
                            (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG
                                left join dbo.TB_group on TG.Group_id=TB_group.id
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )
                        as TU on TR.issue_personnel=TU.UserCount ");
                            sql.Append(@"where TR.state_!=1 and TU.GroupId=@0 and TR.id in(select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                               where NodeId=4 or NodeId=5 group by ReportID) as a where DateTime>=@1) ", GroupName, ElapsedTimeDay);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                            sql.Append("group by TU.UserName");

                        }
                        #endregion
                        break;
                    default: break;
                }

            }
            List<TB_StatisticsCount> model = new List<TB_StatisticsCount>();
            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                if (string.IsNullOrEmpty(sql.ToString()))
                {
                    totalRecord = 0;
                    return model;
                }
                model = db.Fetch<TB_StatisticsCount>(sql);
                totalRecord = 1;
                return model;
            }
        }

        #endregion

        #region 统计列表数据
        public List<TB_NDT_report_title> Report_ArrangeList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;

            string StatisticsSearch = PageModel.SearchList_[0].Search;//统计类型
            string StatisticsKey = PageModel.SearchList_[0].Key;//0 统计科室组 1统计组里人
            string DepartmentGroupID = PageModel.SearchList_[1].Search;//科室ID
            string GroupName = PageModel.SearchList_[1].Key;//组名

            int ElapsedTimeDay = Convert.ToInt32(PageModel.SearchList_[2].Search);//耗时天数
            int OverdueTimeDay = 0;
            if (!string.IsNullOrEmpty(PageModel.SearchList_[2].Key))
            {
                string[] ProjectNameValue = PageModel.SearchList_[2].Key.Split('+');//逾期天数
                OverdueTimeDay = Convert.ToInt32(ProjectNameValue.Last());

            }

            string dateStart = PageModel.SearchList_[3].Search;
            string dateEnd = PageModel.SearchList_[3].Key;

            string ReportType = PageModel.SearchList_[4].Search;//报告类型名字
            string ReportOverdueName = PageModel.SearchList_[4].Key;//逾期类型名字

            string ListType = PageModel.SearchList_[5].Search;//列表下拉框内容
            string ListTypeKey = PageModel.SearchList_[5].Key;//列表下拉框条件

            string SendBackCount = PageModel.SearchList_[6].Search;//签发退回次数
            //统计类型
            if (!string.IsNullOrEmpty(StatisticsKey))
            {

                switch (StatisticsSearch)
                {
                    case "report_Edit"://报告量：检验人
                        #region  0 统计科室组 1统计组里人
                        if (StatisticsKey == "0")//
                        {
                            sql.Append(@"select TR.*,TU.GroupName
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Inspection_personnel=TU.UserCount 
                                where TR.state_!=1 and TU.GroupParentId=@0 and TU.GroupName=@1", DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }

                        else
                        {
                            sql.Append(@"select TR.*,us.GroupName,us.UserName from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG left join dbo.TB_group on TG.Group_id=TB_group.id 
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId 
                                where TG.GroupId=@0)as us left join TB_NDT_report_title TR on us.UserCount=TR.Inspection_personnel where TR.state_!=1 and us.UserName=@1", GroupName, ListType);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }

                        #endregion
                        break;
                    case "report_Issue"://签发量
                        #region  0 统计科室组 1统计组里人
                        if (StatisticsKey == "0")//
                        {
                            sql.Append(@"select TR.*,TU.GroupName from (select * from TB_NDT_report_title )as TR left join  (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    ) as TU on TR.issue_personnel=TU.UserCount where TR.state_!=1 and TU.GroupParentId=@0 and TU.GroupName=@1", DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }

                        else
                        {
                            sql.Append(@"select TR.*,us.GroupName,us.UserName from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG left join dbo.TB_group on TG.Group_id=TB_group.id 
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId 
                                where TG.GroupId=@0)as us left join TB_NDT_report_title TR on us.UserCount=TR.issue_personnel where TR.state_!=1 and us.UserName=@1", GroupName, ListType);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.issue_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }

                        break;
                        #endregion
                    case "report_Audit"://审核量
                        #region  0 统计科室组 1统计组里人
                        if (StatisticsKey == "0")//
                        {
                            sql.Append(@"select TR.*,TU.GroupName
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Audit_personnel=TU.UserCount 
                                where TR.state_!=1 and TU.GroupParentId=@0 and TU.GroupName=@1 ", DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }

                        else
                        {
                            sql.Append(@" select TR.*,us.GroupName,us.UserName from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG left join dbo.TB_group on TG.Group_id=TB_group.id 
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId 
                                where TG.GroupId=@0)as us left join TB_NDT_report_title TR on us.UserCount=TR.Audit_personnel where TR.state_!=1 and us.UserName=@1", GroupName, ListType);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Audit_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }

                        #endregion
                        break;
                    case "report_Type"://报告类型
                        #region 报告类型
                        sql.Append("select * from TB_NDT_report_title where state_!=1 and report_name=@0", ListType);
                        if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and Inspection_date between '" + dateStart + "' and GETDATE() ");
                        }
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                        }

                        #endregion
                        break;
                    case "IssueSendBack"://退回次数统计
                        #region 退回次数统计
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select rt.* from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.id in (select report_id from ( select COUNT(el.report_id) as backcount,el.report_id as report_id from dbo.TB_NDT_error_log el group by el.report_id) a where backcount>=@0) and us.GroupParentId=@1 and us.GroupName=@2", SendBackCount, DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }
                        else
                        {
                            sql.Append(@"select rt.* from ( 
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.id in (select report_id from ( select COUNT(el.report_id) as backcount,el.report_id as report_id from dbo.TB_NDT_error_log el group by el.report_id) a where backcount>=@0) and us.GroupId=@1 and us.UserName=@2", SendBackCount, GroupName, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }
                        #endregion
                        break;
                    case "error_type"://错误类型
                        #region 科室错误类型
                        sql.Append(@"select TR.* from dbo.TB_NDT_error_log el left join (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            ) ud on el.addpersonnel=ud.UserCount left join TB_NDT_report_title TR on el.report_id=TR.id where 1=1 and el.error_remarks=@0", ListType);
                        //0 科室
                        if (StatisticsKey == "0")
                        {
                            sql.Append(" and ud.GroupParentId=@0", DepartmentGroupID);
                        }
                        // 组
                        else
                        {
                            sql.Append("and ud.GroupId=@0", GroupName);
                        }
                        if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and el.add_date between '" + dateStart + "' and GETDATE() ");
                        }
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and el.add_date  between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                        }

                        #endregion
                        break;
                    case "Edittime": //编制耗时
                        #region 编制耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select rt.*,us.GroupName from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.Inspection_personnel_date-rt.ReportCreationTime>=@0 and rt.state_=4 and us.GroupParentId=@1 and us.GroupName=@2", ElapsedTimeDay, DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        else
                        {
                            sql.Append(@"select rt.* from ( 
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where rt.Inspection_personnel_date-rt.ReportCreationTime>=@0 and rt.state_=4 and us.GroupId=@1 and us.UserName=@2", ElapsedTimeDay, GroupName, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append(" and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }

                        break;
                        #endregion
                    case "Reviewtime":
                        #region 审核耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select rt.*,us.GroupName from TB_NDT_report_title as rt left join ( select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId  )as us  on us.UserCount=rt.Audit_personnel where rt.Audit_date-rt.Inspection_personnel_date>= @0 and rt.state_=4  and us.GroupParentId=@1 and us.GroupName=@2", ElapsedTimeDay, DepartmentGroupID, ListType);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        else
                        {
                            sql.Append(@"select rt.* from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId  )as us  on us.UserCount=rt.Audit_personnel where rt.Audit_date-rt.Inspection_personnel_date>= @0 and rt.state_=4  and us.GroupId=@1 and us.UserName=@2 ", ElapsedTimeDay, GroupName, ListType);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Audit_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        break;
                        #endregion
                    case "Issuetime"://签发耗时
                        #region 签发耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select rt.* from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId  )as us  on us.UserCount=rt.issue_personnel where rt.issue_date - rt.Audit_date>=@0  and rt.state_=4 and us.GroupParentId=@1 and us.GroupName=@2", ElapsedTimeDay, DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        else
                        {
                            sql.Append(@"select rt.* from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.issue_personnel where rt.issue_date - rt.Audit_date>=@0 and  rt.state_=4  and us.GroupId=@1 and us.UserName=@2", ElapsedTimeDay, GroupName, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.issue_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        break;
                        #endregion
                    case "Alltime"://总耗时：签发签字时间-报告创建时间
                        #region 总耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select rt.*,us.GroupName from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel where rt.issue_date - rt.ReportCreationTime>=@0 and rt.state_=4  and us.GroupParentId=@1 and us.GroupName=@2", ElapsedTimeDay, DepartmentGroupID, ListType);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        else
                        {
                            sql.Append(@"select rt.* from TB_NDT_report_title as rt left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel where rt.issue_date - rt.ReportCreationTime>=@0 and  rt.state_=4 and us.GroupId=@1 and us.UserName=@2", ElapsedTimeDay, GroupName, ListType);
                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and rt.Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        break;
                        #endregion
                    case "passing_rate"://一次通过率
                        #region 一次通过率
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select TB_NDT_report_title.*,us.GroupName from TB_NDT_report_title left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId) as us on TB_NDT_report_title.Inspection_personnel=us.UserCount where  state_=4  and us.GroupParentId=@0 ", DepartmentGroupID);
                            if (ListTypeKey == "0")//一次通过报告
                            {
                                sql.Append("and return_flag=0");
                            }
                            else
                            {
                                sql.Append("and return_flag!=0");
                            }


                        }
                        else
                        {
                            sql.Append(@"select TB_NDT_report_title.* from TB_NDT_report_title left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                from dbo.TB_groupAuthorization as TG
                                left join dbo.TB_group on TG.Group_id=TB_group.id
                                left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId ) as us on TB_NDT_report_title.Inspection_personnel=us.UserCount  where GroupId=@0  and state_=4 ", GroupName);
                            if (ListTypeKey == "0")//一次通过报告
                            {
                                sql.Append("and return_flag=0");
                            }
                            else
                            {
                                sql.Append("and return_flag!=0");
                            }
                        }
                        break;
                        #endregion
                    case "overdue":
                        #region 初始编制
                        if (ReportOverdueName == "初始编制")
                        {
                            //和用户分组表连接
                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                    from dbo.TB_groupAuthorization as TG
                                    left join dbo.TB_group on TG.Group_id=TB_group.id
                                    left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel");
                            //满足逾期的报告
                            sql.Append(@"where us.GroupId=@0 and  rt.id in(select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 and TakeBack=0 group by ReportID ) ", GroupName);

                        }
                        #endregion

                        #region 退回编制
                        if (ReportOverdueName == "退回编制")
                        {

                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                    left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Inspection_personnel");
                            sql.Append(@"where rt.id in( select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 and TakeBack=1 group by ReportID) and us.GroupId=@0", GroupName);

                        }
                        #endregion

                        #region 报告审核
                        if (ReportOverdueName == "报告审核")
                        {

                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                    left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.Audit_personnel");
                            sql.Append(@"where rt.id in( 
                                        select ReportID from TB_ProcessRecord where  IsOverdue=1 and (NodeId=1 or NodeId=13)
                                         ) and us.GroupId=@0", GroupName);

                        }
                        #endregion

                        #region 报告签发
                        if (ReportOverdueName == "报告签发")
                        {

                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                    left join (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount ,TB_UserInfo.UserName 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId )as us  on us.UserCount=rt.issue_personnel");
                            sql.Append(@"where rt.id in( select ReportID from TB_ProcessRecord where  IsOverdue=1 and NodeId=5 ) and us.GroupId=@0", GroupName);
                        }

                        #endregion
                        if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and rt.Inspection_date between '" + dateStart + "' and GETDATE() ");
                        }
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                        }
                        break;
                    case "overdueReportType":
                        #region 初始编制
                        if (ReportOverdueName == "初始编制")
                        {
                            //和用户分组表连接
                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                            left join dbo.TB_UserInfo us on rt.Inspection_personnel=us.UserCount");
                            //满足逾期的报告
                            sql.Append(@"where rt.report_name=@0 and  rt.id in(select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 group by ReportID) ", ReportType);
                        }
                        #endregion

                        #region 退回编制
                        if (ReportOverdueName == "退回编制")
                        {

                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                            left join dbo.TB_UserInfo us on rt.Inspection_personnel=us.UserCount");
                            sql.Append(@"where rt.id in(select ReportID from TB_ProcessRecord where NodeId=0 and IsOverdue=1 and TakeBack=1 group by ReportID ) and rt.report_name=@0", ReportType);

                        }
                        #endregion

                        #region 报告审核
                        if (ReportOverdueName == "报告审核")
                        {


                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                    left join dbo.TB_UserInfo TU on rt.Audit_personnel=TU.UserCount ");
                            sql.Append(@"where rt.id in( select ReportID from TB_ProcessRecord where  IsOverdue=1 and (NodeId=1 or NodeId=13)) and rt.report_name=@0", ReportType);
                        }
                        #endregion

                        #region 报告签发
                        if (ReportOverdueName == "报告签发")
                        {

                            sql.Append(@"select rt.* from TB_NDT_report_title  rt
                                    left join dbo.TB_UserInfo TU on rt.issue_personnel=TU.UserCount ");
                            sql.Append(@"where rt.id in( select ReportID from TB_ProcessRecord where  IsOverdue=1 and NodeId=5 ) and rt.report_name=@0", ReportType);

                        }

                        #endregion
                        break;
                    case "editAlloverdue":
                        #region 编制累计耗时
                        if (StatisticsKey == "0")//科室
                        {
                            #region 科室编制累计耗时

                            sql.Append(@"select rt.* from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel ");
                            sql.Append(@" where  us.GroupParentId=@0 and rt.id in( select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord where NodeId=0 or( NodeId =0 and TakeBack=0) group by ReportID )as a where dateTime>@1
                                    ) and us.GroupName=@2 ", DepartmentGroupID, ElapsedTimeDay, ListType);

                            #endregion
                        }
                        else
                        {
                            #region 班组编制累计耗时

                            sql.Append(@"select rt.* from (
                            select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName
                            from dbo.TB_groupAuthorization as TG
                            left join dbo.TB_group on TG.Group_id=TB_group.id
                            left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                            )as us left join TB_NDT_report_title as rt on us.UserCount=rt.Inspection_personnel where  us.GroupId=@0 and rt.id in(
                                select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord where NodeId=0 or( NodeId =0 and TakeBack=0) group by ReportID )as a where dateTime>@1
                                )and us.UserName=@2", GroupName, ElapsedTimeDay, ListType);
                            #endregion
                        }
                        if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and rt.Inspection_date between '" + dateStart + "' and GETDATE() ");
                        }
                        if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                        {
                            sql.Append("and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                        }

                        #endregion
                        break;
                    case "ReviewAlloverdue":

                        #region 科室审核累计耗时
                        if (StatisticsKey == "0")//科室
                        {

                            sql.Append(@"select TR.*
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Audit_personnel=TU.UserCount ");
                            sql.Append(@"where TR.state_!=1 and TU.GroupParentId=@0 and TU.GroupName is not null and TR.id in( select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                                        where NodeId=3 or NodeId=1 or NodeId=13 group by ReportID )as a where dateTime>@1 ) and TU.GroupName=@1 ", DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }

                        }
                        #endregion

                        #region 班组审核累计耗时
                        else
                        {

                            sql.Append(@"select TR.*
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.Audit_personnel=TU.UserCount ");
                            sql.Append(@"where TR.state_!=1 and TU.GroupName is not null and TU.GroupId=@0 and TR.id in(select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                                        where NodeId=3 or NodeId=1 or NodeId=13 group by ReportID )as a where dateTime>@1) and TU.UserName=@1", GroupName, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }

                        #endregion
                        break;
                    case "IssueAlloverdue":

                        #region 科室签发累计耗时
                        if (StatisticsKey == "0")//科室
                        {
                            sql.Append(@"select TR.*
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.issue_personnel=TU.UserCount ");
                            sql.Append(@"where TR.state_!=1 and TU.GroupName is not null and TU.GroupParentId=@0 and TR.id in(select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                                        where NodeId=4 or NodeId=5 group by ReportID) as a where DateTime>=@1 ) and TU.GroupName=@1", DepartmentGroupID, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        #endregion

                        #region 班组签发累计耗时
                        else
                        {
                            sql.Append(@"select TR.*
                                        from (select * from TB_NDT_report_title )as TR left join  
                                    (select TG.*,TB_group.GroupName,TB_group.GroupParentId,TB_UserInfo.UserCount,TB_UserInfo.UserName 
                                        from dbo.TB_groupAuthorization as TG
                                        left join dbo.TB_group on TG.Group_id=TB_group.id
                                        left join dbo.TB_UserInfo on TG.UserId=TB_UserInfo.UserId
                                    )
                                as TU on TR.issue_personnel=TU.UserCount 
                                where TR.state_!=1 and TU.GroupId=@0 and TR.id in(select ReportID from( select ReportID, SUM(TimeConsuming) as DateTime from dbo.TB_ProcessRecord 
                                        where NodeId=4 or NodeId=5 group by ReportID) as a where DateTime>=@1 ) and TU.UserName=@1", GroupName, ListType);

                            if (!string.IsNullOrEmpty(dateStart) && string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and GETDATE() ");
                            }
                            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                            {
                                sql.Append("and TR.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'");
                            }
                        }
                        #endregion
                        break;
                    default: break;
                }

            }
            List<TB_NDT_report_title> model = new List<TB_NDT_report_title>();
            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                if (string.IsNullOrEmpty(sql.ToString()))
                {
                    totalRecord = 0;
                    return model;
                }
                var result = db.Page<TB_NDT_report_title>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }


        }

        #endregion

        #region 获取分组
        public List<TB_group> GetGroup(Guid GroupParentId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_group where GroupParentId=@0", GroupParentId);
            if (GroupParentId.ToString() == "8cff8e9f-f539-41c9-80ce-06a97f481391")
            {
                sql.Append("and (GroupId=@0 or GroupId=@1 or GroupId=@2 or GroupId=@3 )",
                    "8CFF8E9F-F539-41C9-80CE-06A97F481393",
                    "5CFDF201-8DD4-499C-AD0B-34F77920B036",
                    "0D652422-8338-41E0-952A-67C23BD15DAD",
                    "158D233D-1E20-4A9F-B3CF-716B248D59D0"
                    );
            }

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                List<TB_group> model = db.Fetch<TB_group>(sql);
                return model;
            }
        }
        #endregion

        #region 获取逾期类型
        public List<TB_DictionaryManagement> GetDicitionaryData(Guid DicitionaryParentId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select *,Project_name+'+'+Project_value as ProjectNameValue ");
            sql.Append(" from TB_DictionaryManagement where Parent_id=@0", DicitionaryParentId);

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                List<TB_DictionaryManagement> model = db.Fetch<TB_DictionaryManagement>(sql);
                return model;
            }
        }
        #endregion

        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_TemplateFile> GetReportTemplate()
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from dbo.TB_TemplateFile ");
            sql.OrderBy(" id desc ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                List<TB_TemplateFile> model = db.Fetch<TB_TemplateFile>(sql);
                return model;
            }
        }

        #endregion

        #endregion

        #region 个人工作量统计显示

        #region 个人工作量统计显示
        /// <summary>
        /// 个人工作量统计显示
        /// </summary>
        /// <param name="PageModel">搜索条件</param>
        /// <param name="totalRecord">搜索数量</param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadPersonnelTaskStatistics(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rt.*,ui1.User_name as Inspection_personnel_n,ui2.User_name as Audit_personnel_n,ui3.User_name as issue_personnel_n ");
            sql.Append(" from dbo.TB_NDT_report_title rt ");
            sql.Append(" left join tb_user_info ui1 on rt.Inspection_personnel=ui1.User_count ");
            sql.Append(" left join tb_user_info ui2 on rt.Audit_personnel=ui2.User_count ");
            sql.Append(" left join tb_user_info ui3 on rt.issue_personnel=ui3.User_count ");
            sql.Append(" WHERE 1=1 ");

            #region 搜索功能
            //人员和操作类型都存在
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and rt." + PageModel.SearchList_[0].Search + " = @0 ", PageModel.SearchList_[0].Key);
            }
            //选择了人员，没选操作类型
            if (string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and (rt.Inspection_personnel = @0 or rt.Audit_personnel = @1 or rt.issue_personnel = @2 ) ", PageModel.SearchList_[0].Key, PageModel.SearchList_[0].Key, PageModel.SearchList_[0].Key);
            }

            //开始时间
            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[1].Key))
            {
                sql.Append(" and rt." + PageModel.SearchList_[1].Search + " >= @0 ", PageModel.SearchList_[1].Key);

            }
            //结束时间
            if (!string.IsNullOrEmpty(PageModel.SearchList_[2].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[2].Key))
            {
                sql.Append(" and rt." + PageModel.SearchList_[2].Search + " <= @0 ", PageModel.SearchList_[2].Key);

            }

            #endregion

            if (!string.IsNullOrEmpty(PageModel.SortName) && !string.IsNullOrEmpty(PageModel.SortOrder))
            {
                sql.OrderBy("rt." + PageModel.SortName + " " + PageModel.SortOrder);

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

        #endregion

    }
}
