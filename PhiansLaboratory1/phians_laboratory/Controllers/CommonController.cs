using Phians_BLL;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhiansCommon;
using phians_laboratory.custom_class;
using Phians_BLL.SystemSetting;
using Newtonsoft.Json;
using Phians_Entity.LosslessReport;

namespace phians_laboratory.Controllers
{
    public class CommonController : BaseController
    {
        //
        // GET: /Common/

        public ActionResult Index()
        {
            return View();
        }

        #region 组、人员组合选择系列

        #region 室下拉框
        public JsonResult LoadRoomCombobox()
        {
            dynamic TempTable = new ExpandoObject();
            TempTable.GroupId = "8CFF8E9F-F539-41C9-80CE-06A97F481391";//组管理的最顶头的id

            try
            {
                //链接
                List<ComboboxEntity> mode = new CommonBLL().LoadRoomCombobox(TempTable);
                return Json(mode);
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }
        }

        #endregion

        #region 组下拉框
        public JsonResult LoadGroupCombobox(FormCollection collection)
        {
            dynamic TempTable = new ExpandoObject();
            TempTable.GroupId = new Guid(collection["GroupId"]);//组ID  室下拉框的value

            try
            {
                //链接
                List<LosslessComboboxEntity> mode = new CommonBLL().LoadGroupCombobox(TempTable);
                return Json(mode);
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }
        }

        #endregion

        #region 根据组选择人员
        public JsonResult LoadPersonnelCombobox(FormCollection collection)
        {
            int id = Convert.ToInt32(collection["id"]);//组ID

            try
            {
                //链接
                List<LosslessUserComboboxEntity> mode = new CommonBLL().LoadPersonnelCombobox(id);
                return Json(mode);
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }
        }

        #endregion

        #endregion

        #region 逾期时间下拉框
        public JsonResult LoadDaySetting()
        {
            Guid Parent_id = new Guid("84ABBE5A-4914-42A9-9DE6-C966310A73B4");//逾期时间下拉框 字典父id

            try
            {
                //链接
                List<ComboboxEntityString> mode = new CommonBLL().LoadDaySetting(Parent_id);
                return Json(mode);
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }
        }

        #endregion

        #region 报告类型下拉框——报告模板
        public JsonResult LoadReportType()
        {
            Guid Parent_id = new Guid("2771E4A5-7917-486C-99C6-61398E6BB57F");//报告类型下拉框 字典父id

            try
            {
                //链接
                List<ComboboxEntity> mode = new CommonBLL().LoadReportType(Parent_id);
                return Json(mode);
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }
        }

        #endregion

        public JsonResult GetDictionaryList(FormCollection  FormCollection)
        {
            Guid Parent_id = new Guid(FormCollection["Parent_id"]);
          try
          {
              //链接
              List<ComboboxEntityString> mode = new CommonBLL().GetDictionaryListBLL(Parent_id);
              return Json(mode);
          }
          catch (Exception e)
          {
              return Json("{'total':0,'rows':[]}");//异常返回无数据
          }
      
      }


        public JsonResult GetDictionaryList_id(FormCollection FormCollection)
        {
            Guid Parent_id = new Guid(FormCollection["Parent_id"]);
            try
            {
                //链接
                List<ComboboxEntityString> mode = new CommonBLL().GetDictionaryListIdBLL(Parent_id);
                return Json(mode);
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }

        }

        public string GetDicitionaryContent()
        {
         
            try
            {
 
                Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
             
              Guid  id = new Guid(Convert.ToString(Request["id"]));
              TB_DictionaryManagement model = new CommonBLL().GetDicitionaryContentBLL(id);
              return JsonConvert.SerializeObject(new SingleExecuteResult<TB_DictionaryManagement>(true,"Success", model));                                          
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "Fail"));
            }
        }
    }
}
