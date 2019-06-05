using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Phians_BLL;
using Phians_Entity;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phians_laboratory.Controllers
{
    public class SystemLogController : BaseController
    {
        //
        // GET: /SystemLog/
        public ActionResult Index()
        {
            return View();
        }


        public string GetPageList()
        {

            int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //string order = Request.Params.Get("order");
            //string sortby = Request.Params.Get("sort");
            //string search = Request.Params.Get("search");
            string search = Request.Params.Get("search");
            string key = Request.Params.Get("key");
            try
            {
                int totalRecord;
                List<OperationLog> list = new OperationLogBLL().GetPageList(pageIndex, pagesize, out totalRecord,search,key);
                PagedResult<OperationLog> pagelist = new PagedResult<OperationLog>(totalRecord, list,true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";
                
                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                return "{'total':0,'success':True,'rows':[]}";//异常返回无数据
            }
        }

        //public string  test() {

        //    return WebApi.OperateTestItemWebApi.ConfirmTestItem("http://localhost:6378/api/Web_API/ConfirmTestItem", "2018-05-30 08:10", "2018/05/30 08:16", "admin", "Pass", "829f407e-6137-4ff9-82cd-86e5ba04c531");
        //}
    }
}
