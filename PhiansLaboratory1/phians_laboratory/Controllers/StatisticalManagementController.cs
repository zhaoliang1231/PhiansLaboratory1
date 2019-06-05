using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Phians_BLL;
using Phians_Entity;
using Phians_Entity.Common;
using phians_laboratory.custom_class;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Phians_Entity.LosslessReport;

namespace phians_laboratory.Controllers
{
    public class StatisticalManagementController : Controller
    {
        //
        // GET: /StatisticalManagement/

        // 实例化统计模块操作类
        private readonly StatisticalManagementBLL _statisticalManagementBll = new StatisticalManagementBLL();

        #region 统计管理

        public ActionResult StatisticalManagement()
        {
            return View();
        }
        
        #region 获取统计数据
        public string report_arrange(FormCollection FormCollection)
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);


            List<SearchList> SearchList = new List<SearchList>();
            
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["StatisticsType"]) ? null : FormCollection["StatisticsType"],//统计类型
                Key = string.IsNullOrEmpty(FormCollection["StatisticsKey"]) ? null : FormCollection["StatisticsKey"]//0 统计科室组 1统计组里人
            });
            //统计科室或班组信息
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["DepartmentGroupID"]) ? null : FormCollection["DepartmentGroupID"],//科室ID
                Key = string.IsNullOrEmpty(FormCollection["GroupName"]) ? null : FormCollection["GroupName"]//组名
            });
            //耗时天数
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ElapsedTimeDay"]) ? null : FormCollection["ElapsedTimeDay"],//耗时天数
                Key = string.IsNullOrEmpty(FormCollection["OverdueTimeDay"]) ? null : FormCollection["OverdueTimeDay"]//逾期天数
            });
            //日期判断
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["dateStart"]) ? null : FormCollection["dateStart"],//开始时间
                Key = string.IsNullOrEmpty(FormCollection["dateEnd"]) ? null : FormCollection["dateEnd"]//结束时间
            });
            //报告类型
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ReportTypeName"]) ? null : FormCollection["ReportTypeName"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["ReportOverdueName"]) ? null : FormCollection["ReportOverdueName"]//报告逾期类型名字
            });
            //退回次数
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["SendBackCount"]) ? null : FormCollection["SendBackCount"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["SendBackCountKey"]) ? null : FormCollection["SendBackCountKey"]//报告逾期类型名字
            });
            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//登录用户

            PageModel.SearchList_ = SearchList;
            #region 判断所需要的值是否为空
            if (FormCollection["StatisticsType"] == "editAlloverdue" || FormCollection["StatisticsType"] == "ReviewAlloverdue" || FormCollection["StatisticsType"] == "IssueAlloverdue" || FormCollection["StatisticsType"] == "Edittime" || FormCollection["StatisticsType"] == "Reviewtime" || FormCollection["StatisticsType"] == "Issuetime")
            {
                if (string.IsNullOrEmpty(FormCollection["ElapsedTimeDay"]))
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "耗时天数不能为空！"));
                }
            }
            if (string.IsNullOrEmpty(FormCollection["DepartmentGroupID"]))
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "科室不能为空！"));
            }
            if (FormCollection["StatisticsType"] == "overdue")
            {
                if (string.IsNullOrEmpty(FormCollection["GroupName"]))
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "班组不能为空！"));
                }
                if (string.IsNullOrEmpty(FormCollection["ReportOverdueName"]))
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "逾期类型不能为空！"));
                }
            }
            #endregion

            try
            {
                int totalRecord;
                List<TB_StatisticsCount> list_model = new StatisticalManagementBLL().GetStatistical(PageModel, out totalRecord);
                PagedResult<TB_StatisticsCount> pagelist = new PagedResult<TB_StatisticsCount>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";
                if (totalRecord==0)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "数据为空！"));
                }
                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region 列表数据
        public string Report_ArrangeList(FormCollection FormCollection)
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);


            List<SearchList> SearchList = new List<SearchList>();

            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["StatisticsType"]) ? null : FormCollection["StatisticsType"],//统计类型
                Key = string.IsNullOrEmpty(FormCollection["StatisticsKey"]) ? null : FormCollection["StatisticsKey"]//0 统计科室组 1统计组里人
            });
            //统计科室或班组信息
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["DepartmentGroupID"]) ? null : FormCollection["DepartmentGroupID"],//科室ID
                Key = string.IsNullOrEmpty(FormCollection["GroupName"]) ? null : FormCollection["GroupName"]//组名
            });
            //耗时天数
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ElapsedTimeDay"]) ? null : FormCollection["ElapsedTimeDay"],//耗时天数
                Key = string.IsNullOrEmpty(FormCollection["OverdueTimeDay"]) ? null : FormCollection["OverdueTimeDay"]//逾期天数
            });
            //日期判断
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["dateStart"]) ? null : FormCollection["dateStart"],//开始时间
                Key = string.IsNullOrEmpty(FormCollection["dateEnd"]) ? null : FormCollection["dateEnd"]//结束时间
            });
            //报告类型
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ReportTypeName"]) ? null : FormCollection["ReportTypeName"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["ReportOverdueName"]) ? null : FormCollection["ReportOverdueName"]//报告逾期类型名字
            });
            //列表下拉框类型文本
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ListType"]) ? null : FormCollection["ListType"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["ListTypeKey"]) ? null : FormCollection["ListTypeKey"]//报告逾期类型名字
            });
            //退回次数
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["SendBackCount"]) ? null : FormCollection["SendBackCount"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["SendBackCountKey"]) ? null : FormCollection["SendBackCountKey"]//报告逾期类型名字
            });
            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//登录用户

            PageModel.SearchList_ = SearchList;

            #region 判断所需要的值是否为空
            if (FormCollection["StatisticsType"] == "editAlloverdue" || FormCollection["StatisticsType"] == "ReviewAlloverdue" || FormCollection["StatisticsType"] == "IssueAlloverdue" || FormCollection["StatisticsType"] == "Edittime" || FormCollection["StatisticsType"] == "Reviewtime" || FormCollection["StatisticsType"] == "Issuetime")
            {
                if (string.IsNullOrEmpty(FormCollection["ElapsedTimeDay"]))
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "耗时天数不能为空！"));
                }
            }
            if (string.IsNullOrEmpty(FormCollection["DepartmentGroupID"]))
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "科室不能为空！"));
            }
            if (FormCollection["DepartmentGroupID"] == "overdue")
            {
                if (string.IsNullOrEmpty(FormCollection["GroupName"]))
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "班组不能为空！"));
                }
            }
            #endregion
            try
            {
                int totalRecord;
                List<TB_NDT_report_title> list_model = new StatisticalManagementBLL().Report_ArrangeList(PageModel, out totalRecord);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";
                if (totalRecord == 0)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "数据为空！"));
                }
                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region 获取科室组
        public string select_group(FormCollection FormCollection)
        {
            try
            {
                Guid GroupParentId =new Guid ( FormCollection["GroupParentId"]);
                List<TB_group> list_model = new StatisticalManagementBLL().GetGroup(GroupParentId);
                return JsonConvert.SerializeObject(list_model);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region 获取逾期类型
        public string GetDicitionaryData(FormCollection FormCollection)
        {
            try
            {
                Guid DicitionaryParentId = new Guid("C3DF6A05-CE66-4D5D-A919-9C1483AD744F");
                List<TB_DictionaryManagement> list_model = new StatisticalManagementBLL().GetDicitionaryData(DicitionaryParentId);
                return JsonConvert.SerializeObject(list_model);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region  加载模板文件
        public string GetReportTemplate(FormCollection FormCollection)
        {
            try
            {
                List<TB_TemplateFile> list_model = new StatisticalManagementBLL().GetReportTemplate();
                return JsonConvert.SerializeObject(list_model);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region 导出报告
        public string Report_ExportExcl(FormCollection FormCollection)
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);


            List<SearchList> SearchList = new List<SearchList>();

            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["StatisticsType"]) ? null : FormCollection["StatisticsType"],//统计类型
                Key = string.IsNullOrEmpty(FormCollection["StatisticsKey"]) ? null : FormCollection["StatisticsKey"]//0 统计科室组 1统计组里人
            });
            //统计科室或班组信息
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["DepartmentGroupID"]) ? null : FormCollection["DepartmentGroupID"],//科室ID
                Key = string.IsNullOrEmpty(FormCollection["GroupName"]) ? null : FormCollection["GroupName"]//组名
            });
            //耗时天数
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ElapsedTimeDay"]) ? null : FormCollection["ElapsedTimeDay"],//耗时天数
                Key = string.IsNullOrEmpty(FormCollection["OverdueTimeDay"]) ? null : FormCollection["OverdueTimeDay"]//逾期天数
            });
            //日期判断
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["dateStart"]) ? null : FormCollection["dateStart"],//开始时间
                Key = string.IsNullOrEmpty(FormCollection["dateEnd"]) ? null : FormCollection["dateEnd"]//结束时间
            });
            //报告类型
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ReportTypeName"]) ? null : FormCollection["ReportTypeName"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["ReportOverdueName"]) ? null : FormCollection["ReportOverdueName"]//报告逾期类型名字
            });
            //列表下拉框类型文本
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ListType"]) ? null : FormCollection["ListType"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["ListTypeKey"]) ? null : FormCollection["ListTypeKey"]//报告逾期类型名字
            });
            //退回次数
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["SendBackCount"]) ? null : FormCollection["SendBackCount"],//报告类型名字
                Key = string.IsNullOrEmpty(FormCollection["SendBackCountKey"]) ? null : FormCollection["SendBackCountKey"]//报告逾期类型名字
            });
            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//登录用户

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_report_title> list_model = new StatisticalManagementBLL().Report_ArrangeList(PageModel, out totalRecord);
                string tempFilePath = Server.MapPath(ConfigurationManager.AppSettings["View_Temp_Folder"].ToString());
                string tempFileName =  "统计导出列表" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";

                //bool flag = false;
                ReturnDALResult ReturnResult = new StatisticalManagementBLL().Report_ExportExcl(list_model, tempFilePath, tempFileName);
                if (ReturnResult.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ConfigurationManager.AppSettings["View_Temp_Folder"].ToString()+ReturnResult.returncontent));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "导出失败"));
            }
            catch (Exception e)
            {
                throw;
            }
            
        }
        #endregion

        #endregion


        #region 个人工作量统计显示
        public ActionResult PersonnelTaskStatistics()
        {
            return View();
    }

        #region 显示页面

        #region 获取报告列表
        public string LoadPersonnelTaskStatistics(FormCollection FormCollection)//获取设备列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();

            #region 搜索条件

            string OperationField = Convert.ToString(FormCollection["OperationField"]);
            string TimeField = "";

            #region 时间判断

            if (OperationField == "Inspection_personnel")
            {
                TimeField = "Inspection_date";
}
            else if (OperationField == "Audit_personnel")
            {
                TimeField = "Audit_date";
            }
            else if (OperationField == "issue_personnel")
            {
                TimeField = "issue_date";
            }

            #endregion

            //人员
            SearchList.Add(new SearchList
            {
                //编制Inspection_personnel；审核Audit_personnel；签发issue_personnel；
                Search = OperationField,//操作类型
                //选择的人的UserCount
                Key = string.IsNullOrEmpty(FormCollection["PersonCount"]) ? null : FormCollection["PersonCount"]
            });

            //开始时间
            SearchList.Add(new SearchList
            {
                //编制report_CreationTime；审核Audit_date；签发issue_date
                Search = TimeField,
                //开始时间
                Key = string.IsNullOrEmpty(FormCollection["StartTime"]) ? null : FormCollection["StartTime"]
            });

            //结束时间
            SearchList.Add(new SearchList
            {
                //编制report_CreationTime；审核Audit_date；签发issue_date
                Search = TimeField,
                //结束时间
                Key = string.IsNullOrEmpty(FormCollection["EndTime"]) ? null : FormCollection["EndTime"]
            });

            #endregion

            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//登录用户

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_report_title> list_model = new StatisticalManagementBLL().LoadPersonnelTaskStatistics(PageModel, out totalRecord);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(totalRecord, list_model, true);//转换成easyui json


                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }



            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_NDT_report_title>(false, "Failed", null));
            }

        }
        #endregion



        #endregion

        #endregion


    }
}
