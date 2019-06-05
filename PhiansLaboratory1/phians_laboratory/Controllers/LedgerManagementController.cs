using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PhiansCommon;
using Phians_BLL;
using phians_laboratory.custom_class;
using Phians_Entity;
using System.Dynamic;
using phians_laboratory.custom_class.ActionFilters;
using Phians_Entity.Common;
using System.Configuration;
using System.IO;

namespace phians_laboratory.Controllers
{
    public class LedgerManagementController : BaseController
    {
        EquipmentManagementBLL _EquiMentBll = new EquipmentManagementBLL();
        FixtureManagementBLL _FixtureBLL = new FixtureManagementBLL();
        #region 设备管理

        public ActionResult EquipmentLibrary()
        {
            return View();
        }


        #region /////////设备操作

        #region  获取设备列表
        public string load_maintenance(FormCollection FormCollection)//获取设备列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(Request.Params.Get("page"));
            PageModel.Pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //Guid UserId = new Guid(Session["UserId"].ToString());
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];
            List<SearchList> SearchList = new List<SearchList>();
            //查询条件1
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_equipment_library> list_model = _EquiMentBll.GetEquipmentList(PageModel, out totalRecord);
                PagedResult<TB_NDT_equipment_library> pagelist = new PagedResult<TB_NDT_equipment_library>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region  新增设备
        [Authentication]
        public string Device_add(FormCollection FormCollection)
        {

            TB_NDT_equipment_library model = new TB_NDT_equipment_library();
            model.equipment_nem = FormCollection["equipment_nem"];
            model.equipment_Type = FormCollection["equipment_Type"];
            model.equipment_num = FormCollection["equipment_num"];
            model.range_ = FormCollection["range_"];
            model.Manufacture = FormCollection["Manufacture"];
            model.effective = Convert.ToDateTime(FormCollection["effective"]);
            model.E_state = Convert.ToInt32(FormCollection["E_state"]);
            model.Remarks = FormCollection["Remarks"];
            model.personnel_ = FormCollection["personnel_"];
            model.personnel_time = DateTime.Now;



            //获取登陆用户
            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            ReturnDALResult ReturnDALResult = _EquiMentBll.AddEquipment(model, LogPersonnel);
            if (ReturnDALResult.Success == 1)
                return JsonConvert.SerializeObject(new ExecuteResult(true, "添加成功"));
            else
            {
                if (ReturnDALResult.Success == 2)
                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "添加异常"));
            }
        }
        #endregion

        #region  修改设备
        [Authentication]
        public string Device_edit(FormCollection FormCollection)
        {
            try
            {

                int existFlag = 0;//用于判断设备编号是否存在 0为不存在，1为存在
                dynamic model = new ExpandoObject();
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.equipment_nem = FormCollection["equipment_nem"];
                model.equipment_Type = FormCollection["equipment_Type"];
                model.range_ = FormCollection["range_"];
                model.Manufacture = FormCollection["Manufacture"];
                model.effective = Convert.ToDateTime(FormCollection["effective"]);
                model.E_state = Convert.ToInt32(FormCollection["E_state"]);
                model.Remarks = FormCollection["Remarks"];
                //model.personnel_time =Convert.ToDateTime(FormCollection["personnel_time"]);


                //获取登陆用户
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));


                ReturnDALResult resultmodel = _EquiMentBll.EditEquipment(model, LogPersonnel);
                if (resultmodel.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "修改成功"));
                else
                {
                    if (existFlag == 2)
                        return JsonConvert.SerializeObject(new ExecuteResult(false, resultmodel.returncontent));
                    else
                        return JsonConvert.SerializeObject(new ExecuteResult(false, "修改失败"));
                }
            }
            catch (Exception E)
            {
                throw;
            }

        }
        #endregion

        #region 删除设备
        [Authentication]
        public string Device_delete(string id, string equipment_nem)//逻辑删除
        {
            try
            {


                //获取登陆用户
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

                ReturnDALResult resultmodel = _EquiMentBll.DelEquipment(id, LogPersonnel, equipment_nem);

                if (resultmodel.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "删除成功"));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "删除失败"));
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #endregion

        #endregion


        #region 探头库管理

        #region 视图
        [Authentication]
        public ActionResult EquipmentManagement()
        {
            return View();
        }
        #endregion

        #region  获取探头列表
        public string load_maintenanceProbe(FormCollection FormCollection)//获取设备列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(Request.Params.Get("page"));
            PageModel.Pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //Guid UserId = new Guid(Session["UserId"].ToString());
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];
            List<SearchList> SearchList = new List<SearchList>();
            //查询条件1
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_probe_library> list_model = _FixtureBLL.GetProbeList(PageModel, out totalRecord);
                PagedResult<TB_NDT_probe_library> pagelist = new PagedResult<TB_NDT_probe_library>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion


        #region  新增探头
        [Authentication]
        public string Probe_addProbe(TB_NDT_probe_library model)
        {

            //TB_NDT_probe_library model = new TB_NDT_probe_library();
            //model.Probe_name = FormCollection["Probe_name"];
            //model.Probe_num = FormCollection["Probe_num"];
            //model.Probe_type = FormCollection["Probe_type"];
            //model.Probe_size = FormCollection["Probe_size"];
            //model.Probe_frequency = FormCollection["Probe_frequency"];
            //model.Coil_Size = FormCollection["Coil_Size"];
            //model.Probe_Length = FormCollection["Probe_Length"];
            //model.Cable_Length = FormCollection["Cable_Length"];
            //model.Mode_L = FormCollection["Mode_L"];
            //model.Chip_size = FormCollection["Chip_size"];
            //model.Angle = FormCollection["Angle"];
            //model.Nom_Angle = FormCollection["Nom_Angle"];
            //model.Shoe = FormCollection["Shoe"];
            //model.Probe_state = Convert.ToInt32(FormCollection["Probe_state"]);
            //model.remarks = FormCollection["remarks"];
            //model.Probe_Manufacture = FormCollection["Probe_Manufacture"];
            //model.Mode_T = FormCollection["Mode_T"];



            //获取登陆用户
            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            ReturnDALResult ReturnDALResult = _FixtureBLL.Probe_add(model, LogPersonnel);
            if (ReturnDALResult.Success == 1)
                return JsonConvert.SerializeObject(new ExecuteResult(true, "添加成功"));
            else
            {
                if (ReturnDALResult.Success == 2)
                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "添加异常"));
            }
        }
        #endregion

        #region  修改探头
        [Authentication]
        public string Device_editProbe(FormCollection FormCollection,TB_NDT_probe_library model)
        {
            try
            {
                model.id = Convert.ToInt32(FormCollection["id"]);
                //TB_NDT_probe_library model = new TB_NDT_probe_library();
                //model.id = Convert.ToInt32(FormCollection["id"]);
                //model.Probe_name = FormCollection["Probe_name"];
                //model.Probe_num = FormCollection["Probe_num"];
                //model.Probe_type = FormCollection["Probe_type"];
                //model.Probe_size = FormCollection["Probe_size"];
                //model.Probe_frequency = FormCollection["Probe_frequency"];
                //model.Coil_Size = FormCollection["Coil_Size"];
                //model.Probe_Length = FormCollection["Probe_Length"];
                //model.Cable_Length = FormCollection["Cable_Length"];
                //model.Mode_L = FormCollection["Mode_L"];
                //model.Chip_size = FormCollection["Chip_size"];
                //model.Angle = FormCollection["Angle"];
                //model.Nom_Angle = FormCollection["Nom_Angle"];
                //model.Shoe = FormCollection["Shoe"];
                //model.Probe_state = Convert.ToInt32(FormCollection["Probe_state"]);
                //model.remarks = FormCollection["remarks"];
                //model.Probe_Manufacture = FormCollection["Probe_Manufacture"];
                //model.Mode_T = FormCollection["Mode_T"];


                //获取登陆用户
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));


                ReturnDALResult resultmodel = _FixtureBLL.Probe_edit(model, LogPersonnel);
                if (resultmodel.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "修改成功"));
                else
                {
                    if (resultmodel.Success == 2)
                        return JsonConvert.SerializeObject(new ExecuteResult(false, resultmodel.returncontent));
                    else
                        return JsonConvert.SerializeObject(new ExecuteResult(false, "修改失败"));
                }
            }
            catch (Exception E)
            {
                throw;
            }

        }
        #endregion

        #region 删除探头
        [Authentication]
        public string Device_deleteProbe(FormCollection FormCollection)//逻辑删除
        {
            try
            {
                TB_NDT_probe_library model = new TB_NDT_probe_library();


                if (string.IsNullOrEmpty(FormCollection["id"]))
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, "请选择任务"));
                }
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.Probe_name = FormCollection["Probe_name"];
                //获取登陆用户
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

                ReturnDALResult resultmodel = _FixtureBLL.Probe_delete(model, LogPersonnel);

                if (resultmodel.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "删除成功"));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, resultmodel.returncontent));
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region 导入探头
        [Authentication]

        public string importProbe(FormCollection FormCollection)
        {
            try
            {
                //获取文件集合
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string File_url = ConfigurationManager.AppSettings["View_Temp_Folder"];//文件存放地址
                string Fileformat = Path.GetExtension(Path.GetFileName(files[0].FileName)).ToLower();  //文件扩展名
                string FileNewname = Guid.NewGuid().ToString() + Fileformat;//新文件名
                Guid OperatePerson = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
                if (Fileformat != ".xls" && Fileformat != ".xlsx" && Fileformat != ".csv")
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "文件后缀名必须为'.xls/..xlsx'")); ;
                }
                //判断路径是否存在
                if (!System.IO.File.Exists(File_url))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(File_url));
                }

                files[0].SaveAs(Server.MapPath(File_url) + FileNewname);//保存路径+文件名+格式
                ReturnDALResult ReturnDALResult = _FixtureBLL.importProbe(Server.MapPath(File_url) + FileNewname, OperatePerson);
                if (ReturnDALResult.Success == 1)
                {
                    if (!string.IsNullOrEmpty(ReturnDALResult.returncontent))
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(true, "导入成功，探头编号重复:" + ReturnDALResult.returncontent));
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(true, "导入成功"));
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "导入失败！"));
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #endregion


        #region 试块库管理

        #region 视图
        [Authentication]
        public ActionResult TestBlockLibrary()
        {
            return View();
        }
        #endregion

        #region  获取试块库列表
        public string load_TestBlockLibrary(FormCollection FormCollection)//获取设备列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(Request.Params.Get("page"));
            PageModel.Pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //Guid UserId = new Guid(Session["UserId"].ToString());
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];
            List<SearchList> SearchList = new List<SearchList>();
            //查询条件1
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_TestBlockLibrary> list_model =new TestBlockLibraryBLL().load_TestBlockLibrary(PageModel, out totalRecord);
                PagedResult<TB_NDT_TestBlockLibrary> pagelist = new PagedResult<TB_NDT_TestBlockLibrary>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion


        #region  新增试块库
        [Authentication]
        public string Add_TestBlockLibrary(TB_NDT_TestBlockLibrary model)
        {
            model.CalBlockID = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            model.CreatePerson=DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
            //获取登陆用户
            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            ReturnDALResult ReturnDALResult = new TestBlockLibraryBLL().Add_TestBlockLibrary(model, LogPersonnel);
            if (ReturnDALResult.Success == 1)
                return JsonConvert.SerializeObject(new ExecuteResult(true, "添加成功"));
            else
            {
                if (ReturnDALResult.Success == 2)
                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "添加异常"));
            }
        }
        #endregion

        #region  修改试块库
        [Authentication]
        public string Edit_TestBlockLibrary(FormCollection FormCollection, TB_NDT_TestBlockLibrary model)
        {
            try
            {
                model.ID = Convert.ToInt32(FormCollection["id"]);
                //TB_NDT_probe_library model = new TB_NDT_probe_library();
                //model.id = Convert.ToInt32(FormCollection["id"]);
                //model.Probe_name = FormCollection["Probe_name"];
                //model.Probe_num = FormCollection["Probe_num"];
                //model.Probe_type = FormCollection["Probe_type"];
                //model.Probe_size = FormCollection["Probe_size"];
                //model.Probe_frequency = FormCollection["Probe_frequency"];
                //model.Coil_Size = FormCollection["Coil_Size"];
                //model.Probe_Length = FormCollection["Probe_Length"];
                //model.Cable_Length = FormCollection["Cable_Length"];
                //model.Mode_L = FormCollection["Mode_L"];
                //model.Chip_size = FormCollection["Chip_size"];
                //model.Angle = FormCollection["Angle"];
                //model.Nom_Angle = FormCollection["Nom_Angle"];
                //model.Shoe = FormCollection["Shoe"];
                //model.Probe_state = Convert.ToInt32(FormCollection["Probe_state"]);
                //model.remarks = FormCollection["remarks"];
                //model.Probe_Manufacture = FormCollection["Probe_Manufacture"];
                //model.Mode_T = FormCollection["Mode_T"];


                //获取登陆用户
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));


                ReturnDALResult resultmodel = new TestBlockLibraryBLL().Edit_TestBlockLibrary(model, LogPersonnel);
                if (resultmodel.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "修改成功"));
                else
                {
                    if (resultmodel.Success == 2)
                        return JsonConvert.SerializeObject(new ExecuteResult(false, resultmodel.returncontent));
                    else
                        return JsonConvert.SerializeObject(new ExecuteResult(false, "修改失败"));
                }
            }
            catch (Exception E)
            {
                throw;
            }

        }
        #endregion

        #region 删除试块库
        [Authentication]
        public string Del_TestBlockLibrary(FormCollection FormCollection)//逻辑删除
        {
            try
            {
                TB_NDT_TestBlockLibrary model = new TB_NDT_TestBlockLibrary();


                if (string.IsNullOrEmpty(FormCollection["id"]))
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, "请选择任务"));
                }
                model.ID = Convert.ToInt32(FormCollection["id"]);
                model.CalBlock = FormCollection["CalBlock"];
                //获取登陆用户
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

                ReturnDALResult resultmodel = new TestBlockLibraryBLL().Del_TestBlockLibrary(model, LogPersonnel);

                if (resultmodel.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "删除成功"));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, resultmodel.returncontent));
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region 导入试块
        [Authentication]

        public string importTestBlockLibrary(FormCollection FormCollection)
        {
            try
            {
                //获取文件集合
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string File_url = ConfigurationManager.AppSettings["View_Temp_Folder"];//文件存放地址
                string Fileformat = Path.GetExtension(Path.GetFileName(files[0].FileName)).ToLower();  //文件扩展名
                string FileNewname = Guid.NewGuid().ToString() + Fileformat;//新文件名
                Guid OperatePerson = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
                if (Fileformat != ".xls" && Fileformat != ".xlsx" && Fileformat != ".csv")
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "文件后缀名必须为'.xls/..xlsx'")); ;
                }
                //判断路径是否存在
                if (!System.IO.File.Exists(File_url))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(File_url));
                }

                files[0].SaveAs(Server.MapPath(File_url) + FileNewname);//保存路径+文件名+格式
                ReturnDALResult ReturnDALResult = new TestBlockLibraryBLL().importTestBlockLibrary(Server.MapPath(File_url) + FileNewname, OperatePerson);
                if (ReturnDALResult.Success == 1)
                {
                    if (!string.IsNullOrEmpty(ReturnDALResult.returncontent))
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(true, "导入成功，探头编号重复:" + ReturnDALResult.returncontent));
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(true, "导入成功"));
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "导入失败！"));
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #endregion


    }
}
