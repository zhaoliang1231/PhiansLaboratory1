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
using phians_laboratory.custom_class.ActionFilters;
using Phians_Entity.Common;
using System.Configuration;


namespace phians_laboratory.Controllers
{
    public class ProjectReviewController : BaseController
    {
        //
        // GET: /ProjectReview/

        #region 资质管理
        public ActionResult QualificationManagement()
        {
            return View();
        }
        #region  加载模板文件
        public string LoadReportList(FormCollection FormCollection)
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            //状态为在用
            SearchList.Add(new SearchList { Search = "state", Key = 1 });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_TemplateFile> list_model = new QualificationManagementBLL().LoadReportList(PageModel, out totalRecord);
                PagedResult<TB_TemplateFile> pagelist = new PagedResult<TB_TemplateFile>(totalRecord, list_model, true);//转换成easyui json
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

        #region 加载所有人员
        public string GetAllUserList(FormCollection FormCollection)
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
            //0 审核权限（所有用户） 1签发权限（签发组）
            SearchList.Add(new SearchList { Search = "AuthorizationType", Key = FormCollection["AuthorizationType"] });

            PageModel.SearchList_ = SearchList;


            try
            {
                int totalRecord;
                List<TB_UserInfo> list = new QualificationManagementBLL().GetAllUserList(PageModel, out totalRecord);
                PagedResult<TB_UserInfo> pagelist = new PagedResult<TB_UserInfo>(totalRecord, list, true);//转换成easyui json
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

        #region  加载已经授权人员
        public string GetQualificationUserList(FormCollection FormCollection)
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            SearchList.Add(new SearchList { Search = "AuthorizationType", Key = FormCollection["AuthorizationType"] });//授权类型（0审核；1签发）
            SearchList.Add(new SearchList { Search = "TemplateID", Key = FormCollection["TemplateID"] });//模板id
            //SearchList.Add(new SearchList { Search = "UserId", Key = FormCollection["UserId"] });//人员id
            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_PersonnelQualification> list_model = new QualificationManagementBLL().GetQualificationUserList(PageModel, out totalRecord);
                PagedResult<TB_PersonnelQualification> pagelist = new PagedResult<TB_PersonnelQualification>(totalRecord, list_model, true);//转换成easyui json
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

        #region 添加权限
        //[Authentication]
        public string AddQualificationPerson(FormCollection FormCollection)
        {
            try
            {

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                //string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                TB_PersonnelQualification model = new TB_PersonnelQualification();
                model.ID = Guid.NewGuid();
                model.TemplateID = Convert.ToInt32(FormCollection["TemplateID"]);
                model.UserId = new Guid(FormCollection["UserId"]);
                model.AuthorizationType = Convert.ToInt32(FormCollection["AuthorizationType"]);
                model.AddPersonnel=LogPersonnel;
                model.AddDate = DateTime.Now;
                model.Remarks = FormCollection["Remarks"];

                ReturnDALResult ReturnDALResult = new QualificationManagementBLL().AddQualificationPerson(model, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }
        #endregion

        #region 删除权限
        //[Authentication]
        public string DelQualificationPerson(FormCollection FormCollection)
        {
            TB_PersonnelQualification model = new TB_PersonnelQualification();
            model.ID = new Guid(FormCollection["ID"]);
            model.TemplateID = Convert.ToInt32(FormCollection["TemplateID"]);
            model.UserId = new Guid(FormCollection["UserId"]);

            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            try
            {
                ReturnDALResult ReturnDALResult = new QualificationManagementBLL().DelQualificationPerson(model, UserId);
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "Operation Unsuccessful"));
            }


        }

        #endregion
        #endregion

        #region ///页面：模板文件


        public ActionResult TemplateFile()
        {
            return View();
        }



        #region  加载模板文件
        public string LoadReportTemplate(FormCollection FormCollection)
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            //状态为在用
            SearchList.Add(new SearchList { Search = "state", Key = 1 });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_TemplateFile> list_model = new ProjectReviewBLL().LoadReportTemplate(PageModel, out totalRecord);
                PagedResult<TB_TemplateFile> pagelist = new PagedResult<TB_TemplateFile>(totalRecord, list_model, true);//转换成easyui json
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

        #region 添加模板文件
        [Authentication]
        public string ReportTemplateAdd(FormCollection FormCollection)
        {
            try
            {

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                TB_TemplateFile model = new TB_TemplateFile();
                model.FileNum = Convert.ToString(FormCollection["FileNum"]);//文件编号
                model.FileName = Convert.ToString(FormCollection["FileName"]);//文件名称
                model.ReviewLevel = Convert.ToInt32(FormCollection["ReviewLevel"]);//ReviewLevel（评审等级：2 or 3）
                model.FileType = new Guid(FormCollection["FileType"]);//报告类型  来自字典管理的guid
                model.Remark = Convert.ToString(FormCollection["Remark"]);//备注
                model.AddDate = DateTime.Now;
                model.AddPersonnel = login_user;
                model.state = true;//在用

                #region 上传的文件

                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files != null && files.Count > 0)
                {

                    string fileName = Path.GetFileName(files[0].FileName);          //原始文件名称
                    Guid NewfileName = Guid.NewGuid();                                    //新文件名
                    string fileExtension = Path.GetExtension(fileName).ToLower();   //文件扩展名

                    if (fileExtension != ".doc" && fileExtension != ".docx")//word格式
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(false, "请上传word格式的模板！"));
                    }


                    string tempPath = ConfigurationManager.AppSettings["Lossless_report"].ToString();//存储模板的相对路径
                    string filePath = Server.MapPath(tempPath);//保存路径

                    string update_url = filePath + NewfileName + fileExtension;//路径+文件名+格式
                    files[0].SaveAs(update_url);//保存路径+文件名+格式

                    string save_url = tempPath + NewfileName + fileExtension;//相对路径

                    model.FileFormat = fileExtension;//文件格式
                    model.FileUrl = save_url;//文件相对路径
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "请选择要上传的文档！"));
                }

                #endregion


                ReturnDALResult ReturnDALResult = new ProjectReviewBLL().ReportTemplateAdd(model, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }
        #endregion

        #region 修改模板文件
        [Authentication]
        public string ReportTemplateEdit(FormCollection FormCollection)
        {
            try
            {

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                TB_TemplateFile model = new TB_TemplateFile();
                model.ID = Convert.ToInt32(FormCollection["ID"]);
                model.FileNum = Convert.ToString(FormCollection["FileNum"]);//文件编号
                model.FileName = Convert.ToString(FormCollection["FileName"]);//文件名称
                model.FileType = new Guid(FormCollection["FileType"]);//报告类型  来自字典管理的guid
                model.ReviewLevel = Convert.ToInt32(FormCollection["ReviewLevel"]);//ReviewLevel（评审等级：2 or 3）
                model.Remark = Convert.ToString(FormCollection["Remark"]);

                ReturnDALResult ReturnDALResult = new ProjectReviewBLL().ReportTemplateEdit(model, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }
        #endregion

        #region 删除模板文件
        [Authentication]
        public string ReportTemplateDel(FormCollection FormCollection)
        {
            try
            {

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                TB_TemplateFile model = new TB_TemplateFile();
                model.ID = Convert.ToInt32(FormCollection["ID"]);//文件ID
                model.FileNum = Convert.ToString(FormCollection["FileNum"]);
                model.state = false;

                ReturnDALResult ReturnDALResult = new ProjectReviewBLL().ReportTemplateDel(model, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }
        #endregion


        #region 注释代码

        #region  加载模板文件
        //public string report_Read()
        //{
        //    string field_name = Request.Params.Get("field_name");
        //    string File_url = "";

        //    switch (field_name)
        //    {
        //        case "热处理模板3": File_url = "/upload_Folder/Lossless_report/1.docx"; break;
        //        case "目视报告模板_VT_63": File_url = "/upload_Folder/Lossless_report/2.doc"; break;
        //        case "DT模板空白_5版": File_url = "/upload_Folder/Lossless_report/3.docx"; break;
        //        case "ECT-涡流检验报告模板RPV": File_url = "/upload_Folder/Lossless_report/4.doc"; break;
        //        case "ECT-涡流检验报告模板SG": File_url = "/upload_Folder/Lossless_report/5.doc"; break;
        //        case "LT-氦检漏报告模板_4版123": File_url = "/upload_Folder/Lossless_report/6.doc"; break;
        //        case "MT-磁轭法和触头法磁粉检验报告3版123": File_url = "/upload_Folder/Lossless_report/7.doc"; break;
        //        case "MT-磁粉检验报告床式": File_url = "/upload_Folder/Lossless_report/8.doc"; break;
        //        case "UT-超声波测厚报告": File_url = "/upload_Folder/Lossless_report/10.doc"; break;
        //        case "UT-超声波检验报告1-正页": File_url = "/upload_Folder/Lossless_report/11.doc"; break;
        //        case "水压试验报告模板21": File_url = "/upload_Folder/Lossless_report/12.doc"; break;
        //        case "自动超声波检验报告1-检验报告": File_url = "/upload_Folder/Lossless_report/13.doc"; break;
        //        case "PT-液体渗透检验报告": File_url = "/upload_Folder/Lossless_report/26.doc"; break;
        //        case "RT-射线检验报告1": File_url = "/upload_Folder/Lossless_report/27.doc"; break;
        //        default:
        //            break;
        //    }

        //    return File_url;
        //}
        #endregion

        #region 修改模板文件
        //[Authentication]
        //public string report_Edit(string ids)
        //{
        //    string field_name = Request.Params.Get("field_name");
        //    string File_url = "";

        //    switch (field_name)
        //    {
        //        case "热处理模板3": File_url = "/upload_Folder/Lossless_report/1.docx，.docx"; break;
        //        case "目视报告模板_VT_63": File_url = "/upload_Folder/Lossless_report/2.doc，.doc"; break;
        //        case "DT模板空白_5版": File_url = "/upload_Folder/Lossless_report/3.docx，.docx"; break;
        //        case "ECT-涡流检验报告模板RPV": File_url = "/upload_Folder/Lossless_report/4.doc，.doc"; break;
        //        case "ECT-涡流检验报告模板SG": File_url = "/upload_Folder/Lossless_report/5.doc，.doc"; break;
        //        case "LT-氦检漏报告模板_4版123": File_url = "/upload_Folder/Lossless_report/6.doc，.doc"; break;
        //        case "MT-磁轭法和触头法磁粉检验报告3版123": File_url = "/upload_Folder/Lossless_report/7.doc，.doc"; break;
        //        case "MT-磁粉检验报告床式": File_url = "/upload_Folder/Lossless_report/8.doc，.doc"; break;
        //        case "UT-超声波测厚报告": File_url = "/upload_Folder/Lossless_report/10.doc，.doc"; break;
        //        case "UT-超声波检验报告1-正页": File_url = "/upload_Folder/Lossless_report/11.doc，.doc"; break;
        //        case "水压试验报告模板21": File_url = "/upload_Folder/Lossless_report/12.doc，.doc"; break;
        //        case "自动超声波检验报告1-检验报告": File_url = "/upload_Folder/Lossless_report/13.doc，.doc"; break;
        //        case "PT-液体渗透检验报告": File_url = "/upload_Folder/Lossless_report/26.doc，.doc"; break;
        //        case "RT-射线检验报告1": File_url = "/upload_Folder/Lossless_report/27.doc，.doc"; break;
        //        default:
        //            break;
        //    }
        //    string loginAccount =  new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value)).ToString();
        //    string login_username = Convert.ToString(Session["login_username"]);
        //    //Operation_log.operation_log_(loginAccount, login_username, "无损异常报告模板", "无损报告模板" + field_name + "修改");
        //    //context.Response.Write(File_url);//返回给前台页面   
        //    //context.Response.End();
        //    return File_url;
        //}
        #endregion

        #endregion

        #endregion
    }
}
