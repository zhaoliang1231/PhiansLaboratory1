using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Phians_BLL;
using Phians_Entity;
using PhiansCommon;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Dynamic;
using phians_laboratory.custom_class;
using Phians_BLL.SystemSetting;
using System.Configuration;
using phians_laboratory.custom_class.ActionFilters;
using Phians_BLL.ManageQuality;
using Phians_Entity.Common;
using System.Net;

namespace phians_laboratory.Controllers
{
    public class SystemSettingsController : BaseController
    {

        #region  ///页面：系统日志
        // GET: /SystemSettings/日志
        [Authentication]
        public ActionResult SystemLog()
        {
            return View();
        }

        #region 加载系统日志列表

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
                List<OperationLog> list = new OperationLogBLL().GetPageList(pageIndex, pagesize, out totalRecord, search, key);
                PagedResult<OperationLog> pagelist = new PagedResult<OperationLog>(totalRecord, list, true);//转换成easyui json
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
        #endregion

        #region ///页面：权限管理
         [Authentication]
        public ActionResult AuthorityManagement()
        {

            return View();
        }
        #region 获取系统列表
        public JsonResult GetSystemList()
        {
            try
            {

                List<ComboboxEntity> LModel = new AuthorityManagementBLL().GetSystemList();

                return Json(LModel);
            }
            catch (Exception)
            {

                return Json(new ExecuteResult(false, "Error"));
            }

        }
        #endregion

        #region 加载页面树
        public ActionResult LoadPageTree(FormCollection FormCollection)
        {
            try
            {
                Guid PageId = new Guid(FormCollection["PageId"]);
                //链接
                string Loadpage = new AuthorityManagementBLL().LoadPageTree(PageId);
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

        #region 加载页面列表树---不用
        //public string LoadPageTbleTree()
        //{

        //    string ParentId = Convert.ToString(Request["ParentId"]);

        //    int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
        //    int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
        //    //string order = Request.Params.Get("order");
        //    //string sortby = Request.Params.Get("sort");
        //    //string search = Request.Params.Get("search");
        //    //string search = Request.Params.Get("search");
        //    //string key = Request.Params.Get("key");
        //    try
        //    {
        //        int totalRecord;
        //        List<TB_FunctionalModule> list = new AuthorityManagementBLL().LoadPageTbleTree(ParentId, pageIndex, pagesize, out totalRecord);
        //        PagedResult<TB_FunctionalModule> pagelist = new PagedResult<TB_FunctionalModule>(totalRecord, list, true);//转换成easyui json
        //        var iso = new IsoDateTimeConverter();
        //        iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

        //        return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
        //    }
        //    catch (Exception e)
        //    {
        //        return "{'total':0,'success':True,'rows':[]}";//异常返回无数据
        //    }
        //}

        #endregion

        #region 加载组树
        public ActionResult GetAuthGroupTree()
        {
            string GroupParentId = Convert.ToString(Request["GroupParentId"]);

            try
            {
                //链接
                string Loadpage = new AuthorityManagementBLL().GetGroupTree(GroupParentId);
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

        #region 加载组人员
        [HttpPost]
        public string GetGroupPerList()
        {

            Guid GroupId = new Guid(Request["GroupId"]);//部门id

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
                List<TB_UserInfo> list = new AuthorityManagementBLL().GetGroupPerList(GroupId, pageIndex, pagesize, out totalRecord, search, key);
                PagedResult<TB_UserInfo> pagelist = new PagedResult<TB_UserInfo>(totalRecord, list, true);//转换成easyui json
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

        #region 组/人员 (页面、按钮)授权||取消授权
        //[Authentication]
        public string GroupAuthority(FormCollection FormCollection)
        {
            List<TB_PageAuthorization> mode = new List<TB_PageAuthorization> { };
            bool flag = Convert.ToBoolean(FormCollection["flag"]);//标识 组授权为false  人员授权true   
            bool flag2 = Convert.ToBoolean(FormCollection["flag2"]);//标识 取消授权为false  授权为true   
            bool flag3 = Convert.ToBoolean(FormCollection["flag3"]);//标识 按钮操作false  页面操作为true   //主要作用于写日志区别

            string GroupName = Convert.ToString(FormCollection["GroupName"]);//组名称
            Guid CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人员

            Guid SystemId = new Guid(FormCollection["SystemId"]);
            string ids = Convert.ToString(FormCollection["ids"]);//页面id
            string PageIds = Convert.ToString(FormCollection["PageIds"]);//页面授权时为页面PageId   按钮授权时为按钮id
            string ModuleNames = Convert.ToString(FormCollection["ModuleNames"]);//页面名称
            string ButtonNames = Convert.ToString(FormCollection["ButtonNames"]) ;//按钮名称

            string[] PageId = PageIds.Split(',');
            string[] id = ids.Split(',');

            //人员授权
            if (flag)
            {
                string UserId = Convert.ToString(FormCollection["UserId"]);//多选人员id
                string[] UserIds = UserId.Split(',');
                for (int k = 0; k < UserIds.Length; k++)
                {
                    for (int i = 0; i < PageId.Length; i++)
                    {
                        TB_PageAuthorization mode1 = new TB_PageAuthorization();
                        //mode1.id = Convert.ToInt32(id[i]);//这个表的id是主键 标识
                        //mode1.GroupId = GroupId;
                        mode1.UserId = new Guid(UserIds[k]);
                        // mode1.username = username;
                        mode1.SystemId = SystemId;
                        mode1.PageId = new Guid(PageId[i]);
                        mode1.Flag = flag;
                        mode.Add(mode1);

                    }
                }
            }
            else //组授权
            {
                Guid GroupId = new Guid(Request["GroupId"]);
                for (int i = 0; i < PageId.Length; i++)
                {
                    TB_PageAuthorization mode1 = new TB_PageAuthorization();
                    //mode1.id = Convert.ToInt32(id[i]);//这个表的id是主键 标识
                    mode1.GroupId = GroupId;
                    // mode1.UserId = UserId;
                    // mode1.username = username;
                    mode1.SystemId = SystemId;
                    mode1.PageId = new Guid(PageId[i]);
                    mode1.Flag = flag;
                    mode.Add(mode1);
                }
            }


            try
            {
                new AuthorityManagementBLL().GroupAuthority(mode, ModuleNames, GroupName, CreatePersonnel, flag2, flag3, ButtonNames);
                return JsonConvert.SerializeObject(new ExecuteResult(true, "授权成功"));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "授权失败"));
            }
        }

        #endregion

        #region 组/人员 页面授权回显
        public ActionResult ShowAuthorizedPage(FormCollection FormCollection)
        {
            List<TB_PageAuthorization> mode = new List<TB_PageAuthorization> { };
            bool flag = Convert.ToBoolean(FormCollection["flag"]);//标识 组授权为false  人员授权true   

            Guid GroupId = new Guid();
            Guid UserId = new Guid();
            if (flag) { UserId = new Guid(FormCollection["UserId"]); } else { GroupId = new Guid(Request["GroupId"]); }

            dynamic table = new ExpandoObject();

            table.flag = flag;
            table.UserId = UserId;
            table.GroupId = GroupId;
            table.PageId = new Guid(FormCollection["PageId"]);

            try
            {
                //链接
                string Loadpage = new AuthorityManagementBLL().ShowAuthorizedPage(table);
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


        #region <<<<<<<<<<<<<<按钮权限操作

        #region 加载按钮权限表格
        [HttpPost]
        public string GetButtonAuthorityList(FormCollection FormCollection)
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(Request.Params.Get("page"));
            PageModel.Pagesize = Convert.ToInt32(Request.Params.Get("rows"));


            List<SearchList> SearchList = new List<SearchList>();
            //下拉框内容
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            //判断页面的id
            SearchList.Add(new SearchList { Search = "ParentId", Key = new Guid(Request["PageId"]) }); //页面的id也就是button的ParentId

            #region 判断选择的是人员或是组 （来回显信息）

            Guid GroupOrPersonnelId = new Guid();
            string flag = Convert.ToString(FormCollection["flag"]);//标识 组授权为false  人员授权true   
            if (flag == "false")
            {
                GroupOrPersonnelId = new Guid(Request["Id"]);//GroupId
                SearchList.Add(new SearchList { Search = "GroupId", Key = GroupOrPersonnelId }); //根据组查找授权

            }
            else if (flag == "true")
            {
                GroupOrPersonnelId = new Guid(Request["Id"]);//UserId
                SearchList.Add(new SearchList { Search = "UserId", Key = GroupOrPersonnelId }); //根据人员查找授权

            }
            else
            {
                return "{'total':0,'success':True,'rows':[]}";//异常返回无数据
            }

            #endregion
            PageModel.SearchList_ = SearchList;



            //int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            //int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            ////string order = Request.Params.Get("order");
            ////string sortby = Request.Params.Get("sort");
            ////string search = Request.Params.Get("search");
            //string search = Request.Params.Get("search");
            //string key = Request.Params.Get("key");

            //dynamic table = new ExpandoObject();
            //table.ParentId = ParentId;

            try
            {
                int totalRecord;
                List<TB_FunctionalModule> list = new AuthorityManagementBLL().GetButtonAuthorityList(PageModel, out totalRecord);
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

        #region 回显按钮权限表格
        [HttpPost]
        public string ShowButtonAuthorityList()
        {
            int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));


            bool flag = Convert.ToBoolean(Request["flag"]);//标识 组授权为false  人员授权true   
            Guid ParentId = new Guid(Request["PageId"]);//页面的id也就是button的ParentId

            Guid GroupId = new Guid();
            Guid UserId = new Guid();
            if (flag) { UserId = new Guid(Request["UserId"]); } else { GroupId = new Guid(Request["GroupId"]); }

            dynamic table = new ExpandoObject();

            table.flag = flag;
            table.UserId = UserId;
            table.GroupId = GroupId;
            table.ParentId = ParentId;

            try
            {
                int totalRecord;
                List<TB_FunctionalModule> list = new AuthorityManagementBLL().ShowButtonAuthorityList(table, pageIndex, pagesize, out totalRecord);
                PagedResult<TB_FunctionalModule> pagelist = new PagedResult<TB_FunctionalModule>(totalRecord, list, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                return "{'total':0,'success':true,'rows':[]}";//异常返回无数据
            }
        }

        #endregion



        #endregion

        #endregion

        #region ///页面：字典管理
         [Authentication]
        public ActionResult DictionaryManagement()
        {
            return View();
        }
        #region 获取文件树组
        public ActionResult LoadDictionaryPageTree()
        {
            Guid id = new Guid(Request["id"]);
            Guid Parent_id = new Guid(Request["Parent_id"]);
            //Guid ID = new Guid("8c20e217-5dc9-4b11-83bc-0325b47f7808");
            try
            {
                //链接
                string Loadpage = new DictionaryManagementBLL().LoadPageTree(id, Parent_id);
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

        #region 加载字典内容
        public string LoadDicitionaryData()
        {
            int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //Guid UserId = new Guid(Session["UserId"].ToString());
            //string file_type = Convert.ToString(Request.Params.Get("file_type"));
            string search = Convert.ToString(Request.Params.Get("search"));
            string key = Convert.ToString(Request.Params.Get("key"));

            Guid nodeid = new Guid(Request["nodeid"].ToString());//文件类型
            try
            {
                int totalRecord;
                List<TB_DictionaryManagement> list_model = new DictionaryManagementBLL().LoadDicitionaryData(pageIndex, pagesize, out totalRecord, nodeid, search, key);
                PagedResult<TB_DictionaryManagement> pagelist = new PagedResult<TB_DictionaryManagement>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_DictionaryManagement>(false, "失败", null));
            }
        }

        #endregion

        #region 添加字典类容
        [Authentication]
        public string AddDicitionaryData()
        {
            TB_DictionaryManagement model = new TB_DictionaryManagement();
         
            //model.id = new Guid();
            model.Parent_id = new Guid(Convert.ToString(Request["Parent_id"]));
            model.NodeType = Convert.ToString(Request["NodeType"]);
            model.Project_name = Convert.ToString(Request["Project_name"]);
            model.remarks = Convert.ToString(Request["remarks"]);
            model.Project_value = Convert.ToString(Request["Project_value"]);
            model.Sort_num = Convert.ToInt32(Request["Sort_num"]);
            model.DictionaryState = 1;//默认字段状态有效
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            try
            {
                Guid NodeId = new Guid();
                bool flag = new DictionaryManagementBLL().AddPageTree(model, UserId, out NodeId);
                if (flag == false)
                {
                    if (NodeId == new Guid())
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(true, "The project already exists!"));
                    }
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "操作失败"));
                }
                return JsonConvert.SerializeObject(new Result_data(true, "操作成功", NodeId));//返回执行结果 ，添加成功可以换成需要的内容
            }
            catch (Exception e)
            {
                throw;
            }


        }

        #endregion

        #region 修改字典
        [Authentication]
        public string EditDicitionaryData()
        {
            TB_DictionaryManagement model = new TB_DictionaryManagement();

            //model.id = new Guid();
            model.Parent_id = new Guid(Convert.ToString(Request["Parent_id"]));
            model.id = new Guid(Convert.ToString(Request["id"]));
            model.NodeType = "2";
            model.Project_name = Convert.ToString(Request["Project_name"]);
            model.remarks = Convert.ToString(Request["remarks"]);
            model.Project_value = Convert.ToString(Request["Project_value"]);
            model.Sort_num = Convert.ToInt32(Request["Sort_num"]);

            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            int errorType;
            try
            {
                bool flag = new DictionaryManagementBLL().EditDictionary(model, UserId, out errorType);
                if (flag == false)
                {
                    if (errorType == 2)
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(true, "The project already exists!"));
                    }
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "Operate Fail"));
                }
                return JsonConvert.SerializeObject(new ExecuteResult(true, "Operate Success"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "Operate Error"));
            }
        }

        #endregion

        #region 删除字典类容
        [Authentication]
        public string DelDicitionaryData()
        {
            TB_DictionaryManagement model = new TB_DictionaryManagement();
            model.Project_name = Convert.ToString(Request["Project_name"]);
            model.id = new Guid(Convert.ToString(Request["Parent_id"]));

            if (model.Project_name == "初始编制" || model.Project_name == "退回编制" || model.Project_name == "报告审核" || model.Project_name == "报告签发")
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "禁止删除！"));
            }
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            try
            {
                bool flag = new DictionaryManagementBLL().DelDicitionaryData(model, UserId);
                if (flag == false)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "删除失败"));
                }
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "删除成功"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "删除失败"));
            }


        }

        #endregion

        #region 停用启用字典
        [Authentication]
        public string EditDictionaryState(FormCollection FormCollection)
        {
            TB_DictionaryManagement model = new TB_DictionaryManagement();
            model.Parent_id = new Guid(Convert.ToString(FormCollection["Parent_id"]));
            //model.Project_name = Convert.ToString(Request["Project_name"]);
            model.id = new Guid(Convert.ToString(FormCollection["Parent_id"]));
            //model.NodeType = Convert.ToString(Request["NodeType"]);
            int DictionaryState = Convert.ToInt32(FormCollection["DictionaryState"]);
            model.DictionaryState = DictionaryState == 1 ? 1 : 0;

            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            try
            {

                bool flag = new DictionaryManagementBLL().EditDictionaryState(model, UserId);
                if (flag == true)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "删除成功"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "删除失败"));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "删除失败"));
            }


        }

        #endregion
        #endregion

        #region ///页面：人员管理
         [Authentication]
        public ActionResult PersonalManagement()
        {
            return View();
        }

        #region 加载部门树
        //public ActionResult GetDepartmentList()
        //{

        //    string ParentId = Convert.ToString(Request["ParentId"]);

        //    try
        //    {
        //        //链接
        //        string Loadpage = new PersonalManagementBLL().GetDepartmentList(ParentId);
        //        if (Loadpage != null)
        //        {
        //            return Content(Loadpage);
        //        }
        //        else
        //        {
        //            return Content("F");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Content("异常");
        //    }
        //}

        #endregion

        #region >>>>>>人员操作

        #region 加载部门人员
        public string GetDepPerList(FormCollection FormCollection)
        {


            //Guid NodeId = new Guid(FormCollection["NodeId"]);//部门id       

            TPageModel PageModel = new TPageModel();

            PageModel.PageIndex = Convert.ToInt32(Request.Params.Get("page"));
            PageModel.Pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //Guid UserId = new Guid(Session["UserId"].ToString());
            PageModel.SortName = Convert.ToString(FormCollection["sort"]);
            PageModel.SortOrder = Convert.ToString(FormCollection["order"]);
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
                List<TB_UserInfo> list = new PersonalManagementBLL().GetDepPerList(PageModel, out totalRecord);
                PagedResult<TB_UserInfo> pagelist = new PagedResult<TB_UserInfo>(totalRecord, list, true);//转换成easyui json
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

        #region 添加人员
        [Authentication]
        public string AddPersonnel(FormCollection collection)
        {
            TB_UserInfo model = new TB_UserInfo();

            model.UserCount = Convert.ToString(collection["UserCount"]);
            model.UserName = Convert.ToString(collection["UserName"]);
            model.UserPwd = DES_.stringEncrypt("123456");
            model.CountState = 1;
            model.UserNsex = Convert.ToBoolean(collection["UserNsex"]);
            model.Tel = Convert.ToString(collection["Tel"]);
            model.Phone = Convert.ToString(collection["Phone"]);
            model.Fax = Convert.ToString(collection["Fax"]);
            model.Email = Convert.ToString(collection["Email"]);
            model.Postcode = Convert.ToString(collection["Postcode"]);
            model.QQ = Convert.ToString(collection["QQ"]);
            model.Address = Convert.ToString(collection["Address"]);
            //model.Signature = Convert.ToString(collection["Signature"]);
            model.Remarks = Convert.ToString(collection["Remarks"]);
            model.sort_num = Convert.ToInt32(collection["sort_num"]);
            model.CreateDate = DateTime.Now;
            model.CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.JobNum = Convert.ToString(collection["JobNum"]);
            model.Profession = Convert.ToString(collection["Profession"]);
            model.Job = Convert.ToString(collection["Job"]);
            //model.BU = collection["BU"];

            TB_groupAuthorization TB_groupAuthorization = new TB_groupAuthorization();
            TB_groupAuthorization.GroupId = new Guid(collection["NodeId"]);//GroupId
            TB_groupAuthorization.GroupLeader = false;//GroupLeader
            TB_groupAuthorization.Group_id = Convert.ToInt32(collection["Group_id"]);

            TB_User_department departmentmodel = new TB_User_department();

            //model2.User_id = new Guid(DES_.DecryptUserId(CookiesHelper.GetCookie("UserId").Value));//用户id
            departmentmodel.NodeId = new Guid(collection["NodeId"]);//部门当前节点id

            try
            {
                ReturnDALResult ReturnDALResultmodel = new PersonalManagementBLL().AddPersonnel(model, TB_groupAuthorization, departmentmodel);
                if (ReturnDALResultmodel.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "Operate Success"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResultmodel.returncontent));//返回执行结果 ，添加成功可以换成需要的内容1
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 人员信息修改

        [HttpPost]
        [Authentication]
        public string EditPersonnel(FormCollection collection)
        {
            try
            {

            TB_UserInfo model = new TB_UserInfo();

            //model.UserCount = Convert.ToString(collection["UserCount"]);
            //model.UserPwd = Convert.ToString(collection["UserPwd"]);
            model.UserId = new Guid(collection["UserId"].ToString());
            model.UserName = Convert.ToString(collection["UserName"]);
            model.UserNsex = Convert.ToBoolean(collection["UserNsex"]);
            model.Tel = Convert.ToString(collection["Tel"]);
            model.Phone = Convert.ToString(collection["Phone"]);
            model.Fax = Convert.ToString(collection["Fax"]);
            model.Email = Convert.ToString(collection["Email"]);
            model.Postcode = Convert.ToString(collection["Postcode"]);
            model.QQ = Convert.ToString(collection["QQ"]);
            model.Address = Convert.ToString(collection["Address"]);
            model.Signature = Convert.ToString(collection["Signature"]);
            model.Remarks = Convert.ToString(collection["Remarks"]);
            //model.sort_num = Convert.ToInt32(collection["sort_num"]);
            // model.CountState = Convert.ToInt32(collection["CountState"]);
            model.JobNum = Convert.ToString(collection["JobNum"]);
            model.Profession = collection["Profession"];
            model.Job = collection["Job"];
            model.BU = collection["BU"];
            //入职时间维护
            //TB_UserHireAndLeave UserHireAndLeavemodel = new TB_UserHireAndLeave();
            //UserHireAndLeavemodel.UserId = new Guid(collection["UserId"].ToString());
            //UserHireAndLeavemodel.HireDate = Convert.ToDateTime(collection["HireDate"]);
            //if (!string.IsNullOrEmpty(collection["LeaveDate"]))
            //{
            //    UserHireAndLeavemodel.LeaveDate = Convert.ToDateTime(collection["LeaveDate"]);
            //}

            dynamic TempTable = new ExpandoObject();
            TempTable.CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

            
                bool flag = new PersonalManagementBLL().EditPersonnel(model, TempTable);
                if (flag)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Success"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operation Fail"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 人员停用/启用

        [HttpPost]
        [Authentication]
        public string DelPersonnel()
        {
            try
            {
            int del_flag = Convert.ToInt32(Request["flag"]);//标识 1为启用 0为停用
            Guid UserId = new Guid(Request["UserId"]);//被操作员 id
            Guid CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人员
            string UserName = Convert.ToString(Request["UserName"]);//人员名称
            if (Request["UserId"].ToLower() == "8cff8e9f-f539-41c9-80ce-06a97f481390")
            {

                return JsonConvert.SerializeObject(new ExecuteResult(false, "You cannot operate this account"));//返回执行结果 ，添加成功可以换成需要的内容
            }
           
                if (UserId != CreatePersonnel)
                {
                    bool flag = new PersonalManagementBLL().DelPersonnel(UserId, UserName, CreatePersonnel, del_flag);
                    if (flag)
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Success"));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Fail"));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "You cannot operate on yourself"));//返回执行结果 ，添加成功可以换成需要的内容
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 人员重置密码

        [HttpPost]
        [Authentication]
        public string ResetPerPwd()
        {
            try
            {
            Guid UserId = new Guid(Request["UserId"]);//被操作人员 id
            Guid CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人员
            string UserName = Convert.ToString(Request["UserName"]);//人员名称

            string User_pwd = DES_.stringEncrypt("123456");



           
                bool flag = new PersonalManagementBLL().ResetPerPwd(UserId, UserName, CreatePersonnel, User_pwd);
                if (flag)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Success,Password:'123456'"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Fail"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //默认密钥向量 
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为8位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }


        #endregion

        #region 上传用户签名
        [Authentication]
        public string UpdateUserImg()
        {
            dynamic model = new ExpandoObject();
            model.UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人员id
            model.UserName = Convert.ToString(Request["UserName"]);//被操作人员名称   //人员管理上传的不一定是自己的签名
            model.UserId2 = Convert.ToString(Request["UserId"]);//被操作人员id

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //HttpPostedFileBase fileitem = files[0];

            #region 上传
            if (files != null && files.Count > 0)
            {
                string fileName = Path.GetFileName(files[0].FileName);          //原始文件名称
                string fileExtension = Path.GetExtension(fileName).ToLower();  //文件扩展名
                string fileName2 = Guid.NewGuid().ToString();//文件名
                if (fileExtension != ".png" && fileExtension != ".jpg")
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "上传失败"));
                }
                string filePath = Server.MapPath(ConfigurationManager.AppSettings["signature_pic"].ToString());//保存路径
                //判断路径是否存在
                if (System.IO.File.Exists(filePath + fileName2 + fileExtension))
                {
                    System.IO.File.Delete(filePath + fileName2 + fileExtension);
                }


                string update_url = ConfigurationManager.AppSettings["signature_pic"].ToString() + fileName2 + fileExtension;
                files[0].SaveAs(filePath + fileName2 + fileExtension);//保存路径+文件名+格式
                //修改数据库保存路径
                try
                {
                    model.Signature = update_url;//修改用户路径
                    bool flag = new PersonalManagementBLL().UpdateUserImg(model);
                    if (flag)
                    {
                        return JsonConvert.SerializeObject(new SingleExecuteResult<dynamic>(flag, "上传成功", model));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "上传失败"));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "上传失败"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            #endregion
        }
        #endregion

        #region 上传用户头像
        [Authentication]
        public string UploadUserPortrait()
        {
            dynamic model = new ExpandoObject();
            model.UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人员id
            model.UserName = Convert.ToString(Request["UserName"]);//被操作人员名称   //人员管理上传的不一定是自己的签名
            model.UserId2 = Convert.ToString(Request["UserId"]);//被操作人员id

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //HttpPostedFileBase fileitem = files[0];

            #region 上传
            if (files != null && files.Count > 0)
            {
                string fileName = Path.GetFileName(files[0].FileName);          //原始文件名称
                string fileExtension = Path.GetExtension(fileName).ToLower();  //文件扩展名
                string fileName2 = Guid.NewGuid().ToString();//文件名
                if (fileExtension != ".png" && fileExtension != ".jpg")
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, ".jpg和.png上传格式不正确"));
                }
                string filePath = Server.MapPath(ConfigurationManager.AppSettings["UserPortrait"].ToString());//保存路径
                //判断路径是否存在
                if (System.IO.File.Exists(filePath + fileName2 + fileExtension))
                {
                    System.IO.File.Delete(filePath + fileName2 + fileExtension);
                }


                string update_url = ConfigurationManager.AppSettings["UserPortrait"].ToString() + fileName2 + fileExtension;
                files[0].SaveAs(filePath + fileName2 + fileExtension);//保存路径+文件名+格式
                //修改数据库保存路径
                try
                {
                    model.Portrait = update_url;//上传路径
                    bool flag = new PersonalManagementBLL().UploadUserPortrait(model);
                    if (flag)
                    {
                        return JsonConvert.SerializeObject(new SingleExecuteResult<dynamic>(flag, "上传成功", model));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "上传失败"));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "文件不存在上传失败"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            #endregion
        }
        #endregion

        #region 加载部门人员
        public string GetCancelPerson(FormCollection FormCollection)
        {



            TPageModel PageModel = new TPageModel();

            PageModel.PageIndex = Convert.ToInt32(Request.Params.Get("page"));
            PageModel.Pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //Guid UserId = new Guid(Session["UserId"].ToString());
            PageModel.SortName = Convert.ToString(FormCollection["sort"]);
            PageModel.SortOrder = Convert.ToString(FormCollection["order"]);
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
                List<TB_UserInfo> list = new PersonalManagementBLL().GetCancelPerson(PageModel, out totalRecord);
                PagedResult<TB_UserInfo> pagelist = new PagedResult<TB_UserInfo>(totalRecord, list, true);//转换成easyui json
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

        #region  删除人员
        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="Model">用户model</param>
        /// <returns></returns>
        //public string DeletePerson(FormCollection FormCollection)
        //{
        //    try
        //    {
        //    string UserId = Convert.ToString(FormCollection["UserId"]);//被操作人员 id

        //    //admin账户不可删除
        //    if (UserId == "8CFF8E9F-F539-41C9-80CE-06A97F481390" || UserId == "8cff8e9f-f539-41c9-80ce-06a97f481390")
        //    {
        //        return JsonConvert.SerializeObject(new ExecuteResult(false, "This account is not allowed to delete!"));
        //    }

        //    Guid OperarePerson = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人员
        //    //string UserName = Convert.ToString(FormCollection["UserName"]);//人员名称
        //    TB_UserInfo Model = new TB_UserInfo();
        //    Model.UserId = new Guid(UserId);


            
        //        ReturnDALResult ResultModel = new PersonalManagementBLL().DeletePerson(Model);
        //        if (ResultModel.Success == 1)
        //        {
        //            return JsonConvert.SerializeObject(new ExecuteResult(true, "Operate Success"));//返回执行结果 ，添加成功可以换成需要的内容
        //        }
        //        else
        //        {
        //            return JsonConvert.SerializeObject(new ExecuteResult(false, ResultModel.returncontent));//返回执行结果 ，添加成功可以换成需要的内容
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #endregion

        #region >>>>>>组操作

        #region 加载组树
        public ActionResult GetGroupTree()
        {
            string GroupParentId = Convert.ToString(Request["ParentId"]);

            try
            {
                //链接
                string Loadpage = new PersonalManagementBLL().GetGroupTree(GroupParentId);
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

        #region 组信息加载——回显

        [HttpPost]
        public string LoadGroupInfo()
        {
            try
            {
                Guid GroupId = new Guid(Request["GroupId"]);
           
                TB_group model = new PersonalManagementBLL().LoadGroupInfo(GroupId);
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_group>(true, "成功", model));//返回实体的结果{"Success":true,"Messege":"成功","Data":{}}
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_group>(false, "失败", null));
            }
        }

        #endregion

        #region 添加组
        [Authentication]
        public string AddGroup(FormCollection collection)
        {
            try
            {
                //Guid PageIds = Guid.New6Guid();
            TB_group model = new TB_group();

            //model.NodeId = new Guid(collection["NodeId"]);
            model.GroupParentId = new Guid(collection["GroupParentId"]);
            model.GroupName = Convert.ToString(collection["GroupName"]);
            model.CreateDate = DateTime.Now;
            model.CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.Remarks = Convert.ToString(collection["Remarks"]);
            //model.CreatePersonnel = new Guid("8CFF8E9F-F539-41C9-80CE-06A97F481390");

            
                TB_group TB_group = new PersonalManagementBLL().AddGroup(model);
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_group>(true, "Success!", TB_group));
                //return JsonConvert.SerializeObject(new ExecuteResult(true, "添加成功"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 组信息修改
        [Authentication]
        [HttpPost]
        public string EditGroup(FormCollection collection)
        {

            try
            {
            TB_group model = new TB_group();

            model.GroupId = new Guid(collection["GroupId"]);
            model.GroupName = Convert.ToString(collection["GroupName"]);
            model.Remarks = Convert.ToString(collection["Remarks"]);
            Guid CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

            bool flag = new PersonalManagementBLL().EditGroup(model, CreatePersonnel);
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
                throw;
            }
        }

        #endregion

        #region 组删除
        [Authentication]
        [HttpPost]
        public string DelGroup(Guid GroupId, string GroupName)
        {
            try
            {

                if (GroupId.ToString() == "8cff8e9f-f539-41c9-80ce-06a97f481391")
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, "Root 不允许删除"));//返回执行结果 ，添加成功可以换成需要的内容
                }
               Guid CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
           
                bool flag = new PersonalManagementBLL().DelGroup(GroupId, GroupName, CreatePersonnel);
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
                throw;
            }
        }

        #endregion

        #endregion

        #region >>>>>>人员-组操作

        #region 加载部门人员
        public string GetPersonnelList(FormCollection FormCollection)
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            // string ID = Convert.ToString(FormCollection["ID"]);
            //Guid UserId = new Guid(Session["UserId"].ToString());

            List<SearchList> SearchList = new List<SearchList>();
            //下拉框内容
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_UserInfo> list = new PersonalManagementBLL().GetPersonnelListBll(PageModel, out totalRecord);
                PagedResult<TB_UserInfo> pagelist = new PagedResult<TB_UserInfo>(totalRecord, list, true);//转换成easyui json
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

        #region 加载部门人员
        public string GetgroupPersonnelList(FormCollection FormCollection)
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            Guid GroupId = new Guid(FormCollection["GroupId"]);
            //Guid UserId = new Guid(Session["UserId"].ToString());

            List<SearchList> SearchList = new List<SearchList>();
            ////下拉框内容
            //SearchList.Add(new SearchList
            //{
            //    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
            //    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            //});

            // PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_UserInfo> list = new PersonalManagementBLL().GetPernonnelGroupBLL(GroupId, PageModel, out totalRecord);
                PagedResult<TB_UserInfo> pagelist = new PagedResult<TB_UserInfo>(totalRecord, list, true);//转换成easyui json
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

        #region 显示人员授权组
        public string GetPerGroupTree(FormCollection FormCollection)
        {
            //string GroupParentId = Convert.ToString(Request["GroupParentId"]);
            Guid UserId = new Guid(FormCollection["UserId"]);

            int pageIndex = Convert.ToInt32(FormCollection["page"]);
            int pageSize = Convert.ToInt32(FormCollection["rows"]);
            //string order = Request.Params.Get("order");
            //string sortby = Request.Params.Get("sort");
            //string search = Request.Params.Get("search");
            //string search = Request.Params.Get("search");
            //string key = Request.Params.Get("key");
            try
            {
                int totalRecord;
                List<TB_group> list = new PersonalManagementBLL().GetPerGroupTree(UserId, pageIndex, pageSize, out totalRecord);
                PagedResult<TB_group> pagelist = new PagedResult<TB_group>(totalRecord, list, true);//转换成easyui json
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

        #region 添加人员到组
        [Authentication]
        public string AddPerToGroup(FormCollection collection)
        {
            try
            {
            TB_groupAuthorization model = new TB_groupAuthorization();

            model.Group_id = Convert.ToInt32(collection["Group_id"]);
            model.GroupId = new Guid(collection["GroupId"]);
            model.UserId = new Guid(collection["UserId"]);
            model.GroupLeader = Convert.ToBoolean(collection["GroupLeader"]);

            dynamic TempTable = new ExpandoObject();
            TempTable.UserName = Convert.ToString(Request["UserName"]);
            TempTable.GroupName = Convert.ToString(Request["GroupName"]);
            TempTable.CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

           
                string flag1 = new PersonalManagementBLL().AddPerToGroup(model, TempTable);
                if (flag1 == "true")
                {
                    bool flag = true;
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Success"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else if (flag1 == "该人员已在组内")
                {
                    bool flag = false;
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "该人员已在组或基础组中存在！"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    bool flag = false;
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Fail"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        #endregion

        #region 从组删除人员
        [Authentication]
        public string DelPerToGroup(FormCollection FormCollection)
        {
            try
            {
            int id = Convert.ToInt32(FormCollection["id"]);//id
            Guid CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人员
            string UserName = Convert.ToString(FormCollection["UserName"]);//被操作人员名称
            string GroupName = Convert.ToString(FormCollection["GroupName"]);//移除的组名称

          
                bool flag = new PersonalManagementBLL().DelPerToGroup(id, UserName, GroupName, CreatePersonnel);
                return JsonConvert.SerializeObject(new ExecuteResult(true, "Operate Success"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            catch (Exception e)
            {

                throw;
            }
        }

        #endregion

        #endregion

        #region 证书操作

        #region 加载证书列表
        public string GetUserCertificate(FormCollection FormCollection)
        {

            Guid UserID = new Guid(FormCollection["UserID"]);//用户ID

            int pageIndex = Convert.ToInt32(FormCollection["page"]);
            int pagesize = Convert.ToInt32(FormCollection["rows"]);


            try
            {
                int totalRecord;
                List<TB_CertificateManagement> list = new PersonalManagementBLL().GetUserCertificate(pageIndex, pagesize, UserID, out totalRecord);
                PagedResult<TB_CertificateManagement> pagelist = new PagedResult<TB_CertificateManagement>(totalRecord, list, true);//转换成easyui json
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

        #region 添加证书
        public string AddCertificateData(FormCollection FormCollection)
        {
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

            TB_CertificateManagement Certificate_model = new TB_CertificateManagement();
            Certificate_model.Id = Guid.NewGuid();
            Certificate_model.UserId = new Guid(FormCollection["UserId"]);
            Certificate_model.CertificateNum = Convert.ToString(FormCollection["CertificateNum"]);
            Certificate_model.CertificateName = Convert.ToString(FormCollection["CertificateName"]);
            Certificate_model.CertificateType = new Guid(FormCollection["CertificateType"]);
            Certificate_model.IssuingUnit = Convert.ToString(FormCollection["IssuingUnit"]);
            Certificate_model.IssueDate = Convert.ToDateTime(FormCollection["IssueDate"]);
            Certificate_model.ValidDate = Convert.ToDateTime(FormCollection["ValidDate"]);
            Certificate_model.Profession = Convert.ToString(FormCollection["Profession"]);
            Certificate_model.Quarters = Convert.ToString(FormCollection["Quarters"]);
            Certificate_model.Grade = Convert.ToString(FormCollection["Grade"]);
            Certificate_model.CertificateSate = Convert.ToInt32(FormCollection["CertificateSate"]);
            Certificate_model.CreateDate = Convert.ToDateTime(FormCollection["CreateDate"]);
            Certificate_model.CreatePersonnel = UserId;
            Certificate_model.remarks = Convert.ToString(FormCollection["remarks"]);

            int errortype;
            try
            {
                Guid NodeId = new Guid();
                bool flag = new PersonalManagementBLL().AddCertificateData(Certificate_model, UserId, out errortype);
                if (flag == false)
                {
                    if (errortype == 3)
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(false, "Duplicate Content!"));
                    }
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "Operation Unsuccessful!"));
                }
                return JsonConvert.SerializeObject(new Result_data(true, "Operation Succeed!", NodeId));//返回执行结果 ，添加成功可以换成需要的内容
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "Operation Unsuccessful!"));
            }


        }
        #endregion

        #region 获取证书类别
        public string GetDictionaryList()
        {
            try
            {
                List<ComboboxEntity> list = new PersonalManagementBLL().GetDictionaryList();
                return JsonConvert.SerializeObject(list);//返回easyUI json
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_EquipmentInfo>(false, "失败", null));
            }

        }
        #endregion

        #region 获取证书附件列表
        public string GetCertificateAppendixList(FormCollection FormCollection)//获取样品列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            List<SearchList> SearchList = new List<SearchList>();

            SearchList.Add(new SearchList { Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"], Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"] });
            //ID
            SearchList.Add(new SearchList { Search = "CertificateId", Key = FormCollection["Id"] });
            PageModel.SearchList_ = SearchList;


            try
            {
                int totalRecord;
                List<TB_CertificateAppendix> list_model = new PersonalManagementBLL().GetCertificateAppendixList(PageModel, out totalRecord);
                PagedResult<TB_CertificateAppendix> pagelist = new PagedResult<TB_CertificateAppendix>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                return "{'total':0,'success':True,'rows':[]}";
            }
        }


        #endregion

        #region 新增证书附件
        [Authentication]
        public string AddCertificateFile(FormCollection FormCollection)
        {
            int errortype;
            bool flag = false;
            TB_CertificateAppendix model = new TB_CertificateAppendix();
            model.Id = Guid.NewGuid();
            model.CertificateId = new Guid(FormCollection["CertificateId"]);
            model.CreateDate = DateTime.Now;
            model.CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            string url = ConfigurationManager.AppSettings["CertificateAttachment"];//文件存放地址
            if (files != null && files.Count > 0)
            {
                string fileName = Path.GetFileName(files[0].FileName);          //文件名称
                string fileExtension = Path.GetExtension(fileName).ToLower();  //文件扩展名
                string filePath = url;//保存路径
                model.DocumentName = Convert.ToString(FormCollection["DocumentName"]);

                if (fileExtension != ".doc" && fileExtension != ".docx" && fileExtension != ".pdf" && fileExtension != ".png" && fileExtension != ".jpg")
                {
                    errortype = 1;
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "文件必须为.doc或.docx类型"));
                }
                model.DocumentFormat = fileExtension;

                //判断路径是否存在
                if (!System.IO.File.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                model.DocumentUrl = filePath + model.Id + model.DocumentFormat;
                files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(filePath) + model.Id + model.DocumentFormat);//保存路径+文件名+格式
            }
            flag = new PersonalManagementBLL().AddCertificateFile(model, out errortype);
            if (flag)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operation Succeed"));
            }
            else
            {
                if (errortype == 2)
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "file already exists"));
                else if (errortype == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "The file type is incorrect."));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operation Unsuccessful"));
            }

        }
        #endregion

        #region 删除证书
        [Authentication]
        public string DelCertificateAppendix(FormCollection FormCollection)
        {
            TB_CertificateManagement model = new TB_CertificateManagement();
            model.Id = new Guid(FormCollection["Id"]);
            model.UserName_n = Convert.ToString(FormCollection["UserName_n"]);

            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            try
            {
                bool flag = new PersonalManagementBLL().DelCertificateAppendix(model, UserId);
                if (flag == false)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "Operation Unsuccessful"));
                }
                return JsonConvert.SerializeObject(new ExecuteResult(true, "Operation Succeed"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "Operation Unsuccessful"));
            }


        }

        #endregion

        #region 删除文件
        [Authentication]
        public string DelFileManagement(FormCollection FormCollection)
        {
            bool flag = false;
            Guid loginer = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            TB_CertificateAppendix model = new TB_CertificateAppendix();
            model.Id = new Guid(FormCollection["id"]);
            model.CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            //model.FileName = Request.Params.Get("FileName");
            //model.FileUrl = Request.Params.Get("FileUrl");
            flag = new PersonalManagementBLL().DelFileManagement(model);
            if (flag)
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operation Succeed"));
            else
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operation Unsuccessful"));

        }

        #endregion

        #endregion

        #endregion

        #region ///页面：显示设置
         [Authentication]
        public ActionResult PageShowSetting()
        {
            return View();
        }
        #region  获取所有字段列表
        public string loadInfo(FormCollection FormCollection)
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
                Search = string.IsNullOrEmpty(FormCollection["PageId"]) ? null : FormCollection["PageId"],
                Key = string.IsNullOrEmpty(FormCollection["PageIdkey"]) ? null : FormCollection["PageIdkey"]
            });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_PageShowCustom> list_model = new PageShowSettingBLL().loadInfo(PageModel, out totalRecord);
                PagedResult<TB_PageShowCustom> pagelist = new PagedResult<TB_PageShowCustom>(totalRecord, list_model, true);//转换成easyui json
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

        #region  修改显示字段信息
        public string EditInfo(FormCollection FormCollection)
        {
            try
            {
                List<TB_PageShowCustom> Models = new List<TB_PageShowCustom>();
                string id = Convert.ToString(FormCollection["ids"]);
                string FieldName = FormCollection["FieldName"];
                string Title = FormCollection["Title"];
                int FieldSort = Convert.ToInt32(FormCollection["FieldSort"]);
                bool hidden = Convert.ToBoolean(FormCollection["hidden"] == "0" ? "false" : "true");
                bool Sortable = Convert.ToBoolean(FormCollection["Sortable"] == "0" ? "false" : "true");
                string Operator = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                DateTime OperateDate = DateTime.Now;
                string Remark = FormCollection["Remark"];

                string[] ids = id.Split(',');

                for (int i = 0; i < ids.Length; i++)
                {
                    TB_PageShowCustom model = new TB_PageShowCustom();
                    model.id = Convert.ToInt32(ids[i]);
                    model.hidden = hidden;
                    model.Sortable = Sortable;
                    model.Operator = Operator;
                    model.OperateDate = OperateDate;
                    model.Remark = Remark;

                    if (ids.Length == 1) 
                    {
                        model.FieldName = FieldName;
                        model.Title = Title;
                        model.FieldSort = FieldSort;
                    }

                    Models.Add(model);

                }

                //获取登陆用户
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));


                ReturnDALResult resultmodel = new PageShowSettingBLL().EditInfo(Models, LogPersonnel);
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

        #endregion
        //==============================================

        #region ///页面：显示人员管理页面
         [Authentication]
        public ActionResult ViewPersonalManagement()
        {
            return View();
        }
        #endregion

        #region ///页面：部门管理
         [Authentication]

        public ActionResult DepartmentManagement()
        {
            return View();
        }

        #region 加载部门管理列表
        public ActionResult GetDepartmentManagementList()
        {

            string ParentId = Convert.ToString(Request["ParentId"]);

            try
            {
                //链接
                string Loadpage = new OrganizationBLL().GetDepartmentManagementList(ParentId);
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

        #region 部门模块信息加载

        [HttpPost]
        public string LoadDepartmentModuleInfo()
        {
            Guid NodeId = new Guid(Request["NodeId"]);
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
                List<TB_Organization> list = new OrganizationBLL().LoadDepartmentModuleInfo(NodeId, pageIndex, pagesize, out totalRecord);
                PagedResult<TB_Organization> pagelist = new PagedResult<TB_Organization>(totalRecord, list, true);//转换成easyui json
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

        #region 部门信息加载

        [HttpPost]
        public string LoadDepartmentInfo()
        {
            Guid NodeId = new Guid(Request["NodeId"]);
            try
            {
                TB_Organization model = new OrganizationBLL().LoadDepartmentInfo(NodeId);
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_Organization>(true, "成功", model));//返回实体的结果{"Success":true,"Messege":"成功","Data":{}}
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_Organization>(false, "失败", null));
            }
        }

        #endregion

        #region 添加部门
        [Authentication]
        public string AddDepartment(FormCollection collection)
        {
            //Guid PageIds = Guid.New6Guid();
            TB_Organization model = new TB_Organization();

            //model.NodeId = new Guid(collection["NodeId"]);
            model.ParentId = new Guid(collection["ParentId"]);
            model.NodeName = Convert.ToString(collection["NodeName"]);
            model.OrganizationCode = Convert.ToString(collection["OrganizationCode"]);
            model.remarks = Convert.ToString(collection["remarks"]);
            model.NodeType = Convert.ToInt32(collection["NodeType"]);
            model.Address = Convert.ToString(collection["Address"]);
            model.Phone = Convert.ToString(collection["Phone"]);
            model.PostCode = Convert.ToString(collection["PostCode"]);
            model.SortNum = Convert.ToInt32(collection["SortNum"]);
            model.CreateDate = DateTime.Now;
            model.CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            //model.CreatePersonnel = new Guid("8CFF8E9F-F539-41C9-80CE-06A97F481390");

            try
            {
                TB_Organization TB_Organization = new OrganizationBLL().AddDepartment(model);
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_Organization>(true, "Success!", TB_Organization));

                //switch (ReturnDALResult.Success)
                //{

                //    case 1: return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.PrimaryKey));//返回执行结果 ，添加成功可以换成需要的内容
                //    case 2: return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));//返回执行结果 ，添加成功可以换成需要的内容
                //    default: return JsonConvert.SerializeObject(new ExecuteResult(false, "添加失败"));

                //}


            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "添加失败"));
            }
        }

        #endregion

        #region 部门信息修改
        [Authentication]
        [HttpPost]
        public string EditDepartment(FormCollection collection)
        {
            TB_Organization model = new TB_Organization();

            model.NodeId = new Guid(collection["NodeId"]);
            //model.ParentId = new Guid(collection["ParentId"]);
            model.NodeName = Convert.ToString(collection["NodeName"]);
            model.OrganizationCode = Convert.ToString(collection["OrganizationCode"]);
            model.remarks = Convert.ToString(collection["remarks"]);
            model.NodeType = Convert.ToInt32(collection["NodeType"]);
            model.Address = Convert.ToString(collection["Address"]);
            model.Phone = Convert.ToString(collection["Phone"]);
            model.PostCode = Convert.ToString(collection["PostCode"]);
            model.SortNum = Convert.ToInt32(collection["SortNum"]);
            try
            {
                bool flag = new OrganizationBLL().EditDepartment(model);
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

        #region 部门删除
        [Authentication]
        [HttpPost]
        public string DelDepartment(Guid NodeId, string NodeName)
        {
            Guid CreatePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            try
            {
                bool flag = new OrganizationBLL().DelDepartment(NodeId, NodeName, CreatePersonnel);
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

        #endregion

    }


}
