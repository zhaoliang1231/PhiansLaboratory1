using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Phians_Entity;
using Phians_BLL;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using PhiansCommon;
using phians_laboratory.custom_class;
using Microsoft.AspNet.SignalR;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace phians_laboratory.Controllers
{
    public class FunctionalModuleController : BaseController
    {
        //
        // GET: /FunctionalModule/

        public ActionResult FunctionalModule()
        {
            return View();
        }

        #region 页面树加载
        public ActionResult LoadPage()
        {

            string ParentId =Convert.ToString( Request["ParentId"]);
         

            try
            {
                //链接
                string Loadpage = new FunctionalModuleBLL().FunctionalModuleLoad(ParentId);
                if (Loadpage != null)
                {
                    return Content(Loadpage);
                }
                else
                {
                    return Content("F");
                }
            }
            catch (Exception e)
            {
                return Content("异常");
            }
        }

        #endregion

        #region 页面信息加载

        [HttpPost]
        public string LoadPageInfo()
        {
            Guid PageId = new Guid(Request["PageId"]);
            try
            {
                TB_FunctionalModule model = new FunctionalModuleBLL().FunctionalModuleLoadPageInfo(PageId);
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_FunctionalModule>(true, "成功", model));//返回实体的结果{"Success":true,"Messege":"成功","Data":{}}
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_FunctionalModule>(false, "失败", null));
            }
        }

        #endregion

        #region 模块信息加载

        [HttpPost]
        public string LoadModuleInfo()
        {
            Guid PageId = new Guid(Request["PageId"]);
            int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //string order = Request.Params.Get("order");
            //string sortby = Request.Params.Get("sort");
            //string search = Request.Params.Get("search");
            //string search = Request.Params.Get("search");
            //string key = Request.Params.Get("key");
            try
            {
                int totalRecord;
                List<TB_FunctionalModule> list = new FunctionalModuleBLL().LoadModuleInfo(PageId,pageIndex, pagesize, out totalRecord);
                PagedResult<TB_FunctionalModule> pagelist = new PagedResult<TB_FunctionalModule>(totalRecord, list, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                return "{'total':0,'success':True,'rows':[]}";//异常返回无数据
            }
        }

        #endregion


        #region 页面添加

        [HttpPost]
        public string AddPage(FormCollection collection)
        {

            TB_FunctionalModule model = new TB_FunctionalModule();
            model.ParentId = new Guid(collection["PageId"]);
            model.NodeType = Convert.ToInt32(collection["NodeType"]);
            model.IconCls = Convert.ToString(collection["IconCls"]);
            model.U_url = Convert.ToString(collection["U_url"]);
            model.ModuleName = Convert.ToString(collection["ModuleName"]);
            model.SortNum = Convert.ToInt32(collection["SortNum"]);
            model.Remarks = Convert.ToString(collection["Remarks"]);
            model.PermissionFlag = Convert.ToBoolean(collection["PermissionFlag"]);
            model.PageId = Guid.NewGuid();

            //dynamic table = new ExpandoObject();
            //table.ParentId = new Guid(collection["PageId"]);
            //table.NodeType = Convert.ToInt32(collection["NodeType"]);
            //table.IconCls = Convert.ToString(collection["IconCls"]);
            //table.U_url = Convert.ToString(collection["U_url"]);
            //table.ModuleName = Convert.ToString(collection["ModuleName"]);
            //table.SortNum = Convert.ToInt32(collection["SortNum"]);
            //table.Remarks = Convert.ToString(collection["Remarks"]);
            //table.PermissionFlag = Convert.ToBoolean(collection["PermissionFlag"]);

            try
            {
                Guid flag = new FunctionalModuleBLL().FunctionalModuleAdd(model);

                Guid temp =new Guid("00000000-0000-0000-0000-000000000000");
                if (flag == temp)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "添加失败"));
                }
                else
                {
                    return JsonConvert.SerializeObject(new Result_data(true, "添加成功", flag));//返回执行结果 ，添加成功可以换成需要的内容

                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "添加失败"));
            }
        }

        #endregion

        #region 页面修改

        [HttpPost]
        public string EditPage(FormCollection collection)
        {

            //string pageId = collection["PageId"].ToString();
            TB_FunctionalModule model = new TB_FunctionalModule();
            model.ModuleName = Convert.ToString(Request["ModuleName"]);
            model.NodeType = Convert.ToInt32(collection["NodeType"]);
            model.IconCls = Convert.ToString(collection["IconCls"]);
            model.U_url = Convert.ToString(collection["U_url"]);
            model.ModuleName = Convert.ToString(collection["ModuleName"]);
            model.SortNum = Convert.ToInt32(collection["SortNum"]);
            model.Remarks = Convert.ToString(collection["Remarks"]);
            model.PermissionFlag = Convert.ToBoolean(collection["PermissionFlag"]);
            model.PageId = new Guid(collection["PageId"]);
            try
            {
                bool flag = new FunctionalModuleBLL().FunctionalModuleEdit(model);
                if (flag)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "修改成功"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "修改失败"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "修改失败"));
            }
        }

        #endregion

        #region 页面删除

        [HttpPost]
        public string DelPage(Guid PageId)
        {

            try
            {
                bool flag = new FunctionalModuleBLL().FunctionalModuleDel(PageId);
                if (flag)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "删除成功"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "删除失败"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "删除失败"));
            }
        }

        #endregion

    }
}
