using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Phians_BLL;
using Phians_Entity;
using Phians_Entity.Common;
using phians_laboratory.custom_class;
using phians_laboratory.custom_class.ActionFilters;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phians_laboratory.Controllers
{
    public class MonitoringManagementController : Controller
    {
        #region 无损监控管理

        #region 无损监控管理view
         [Authentication]
        public ActionResult TemperatureRecord()
        {
            return View();
        }
        #endregion

        #region 获取报告列表
        public string load_list(FormCollection FormCollection)//获取设备列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];
            // string ID = Convert.ToString(FormCollection["ID"]);
            //Guid UserId = new Guid(Session["UserId"].ToString());

            List<SearchList> SearchList = new List<SearchList>();

            #region 搜索条件

            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search1"]) ? null : FormCollection["search1"],
                Key = string.IsNullOrEmpty(FormCollection["key1"]) ? null : FormCollection["key1"]
            });
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search2"]) ? null : FormCollection["search2"],
                Key = string.IsNullOrEmpty(FormCollection["key2"]) ? null : FormCollection["key2"]
            });
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search3"]) ? null : FormCollection["search3"],
                Key = string.IsNullOrEmpty(FormCollection["key3"]) ? null : FormCollection["key3"]
            });
            #endregion

            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//登录用户

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_report_title> list_model = new ItemMonitoringBLL().load_list(PageModel, out totalRecord);
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

        #region 获取报告错误列表
        public string load_Errorlist(FormCollection FormCollection)//获取设备列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];
            // string ID = Convert.ToString(FormCollection["ID"]);
            //Guid UserId = new Guid(Session["UserId"].ToString());

            List<SearchList> SearchList = new List<SearchList>();

            #region 搜索条件

            SearchList.Add(new SearchList
            {
                Key = string.IsNullOrEmpty(FormCollection["id"]) ? null : FormCollection["id"]
            });

            #endregion

            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//登录用户

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_error_Certificate> list_model = new ItemMonitoringBLL().load_Errorlist(PageModel, out totalRecord);
                PagedResult<TB_NDT_error_Certificate> pagelist = new PagedResult<TB_NDT_error_Certificate>(totalRecord, list_model, true);//转换成easyui json


                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }



            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_NDT_error_Certificate>(false, "Failed", null));
            }

        }
        #endregion

        #endregion
    }
}
