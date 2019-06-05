using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Phians_BLL;
using Phians_BLL.ManageQuality;
using Phians_Entity;
using phians_laboratory.custom_class;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using phians_laboratory.custom_class.ActionFilters;

namespace phians_laboratory.Controllers
{
    public class ManageQualityController : BaseController
    {
        EnvironmentManageBLL _manageBLL = new EnvironmentManageBLL();

        #region 环境管理
        public ActionResult EnvironmentManage()
        {
            return View();
        }
        #region 获取文件树组
        public ActionResult LoadPageTree()
        {
            Guid id = new Guid(Request["id"]);
            Guid Parent_id = new Guid(Request["Parent_id"]);

            try
            {
                //链接
                string Loadpage = _manageBLL.LoadPageTree(id, Parent_id);
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
                throw;
            }
        }

        #endregion

        //public string AddPageTree(FormCollection collection)
        //    {
        //        TB_DictionaryManagement model = new TB_DictionaryManagement();

        //        //model.id = new Guid();
        //        model.Parent_id = new Guid(Convert.ToString(Request["Parent_id"]));
        //        model.NodeType = "2";
        //        model.Project_name = Convert.ToString(Request["Project_name"]);
        //        model.remarks = Convert.ToString(Request["remarks"]);

        //        if(string.IsNullOrEmpty(model.Project_name)){
        //            return JsonConvert.SerializeObject(new ExecuteResult(true, "名字不能为空"));
        //        }
        //        string pageName=Convert.ToString(Request["pageName"]);//文件类型
        //        Guid UserId = new Guid (CookiesHelper.GetCookie("UserId").Value);
        //        try
        //        {
        //           bool flag = _manageBLL.AddPageTree(model,pageName,UserId);
        //            if(flag==false){
        //                return JsonConvert.SerializeObject(new ExecuteResult(true, "添加失败"));
        //            }
        //            return JsonConvert.SerializeObject(new ExecuteResult(true, "添加成功"));//返回执行结果 ，添加成功可以换成需要的内容
        //        }
        //        catch (Exception e)
        //        {
        //           throw;
        //        }
        //    }

        #region 获取文件列表
        public string GetFileManagement()
        {
            int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            //Guid UserId = new Guid(Session["UserId"].ToString());
            //string file_type = Convert.ToString(Request.Params.Get("file_type"));
            string search = Convert.ToString(Request.Params.Get("search"));
            string key = Convert.ToString(Request.Params.Get("key"));
            string sortName = Convert.ToString(Request.Params.Get("sort"));
            string sortOrder = Convert.ToString(Request.Params.Get("Order"));

            string File_type = Convert.ToString(Request["nodeText"]);//文件类型
            try
            {
                int totalRecord;
                List<TB_FileManagement> list_model = _manageBLL.GetFileManagement(pageIndex, pagesize, out totalRecord, File_type, search, key, sortName, sortOrder);
                PagedResult<TB_FileManagement> pagelist = new PagedResult<TB_FileManagement>(totalRecord, list_model, true);//转换成easyui json
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

        #region 新增文件
        [Authentication]
        public string AddFileManagement()
        {
            int errortype;
            TB_FileManagement model = new TB_FileManagement();
            //model.id = Convert.ToInt32(Request.Params.Get("ids"));
            model.FileNum = Convert.ToString(Request.Params.Get("FileNum"));
            model.FileType = new Guid(Convert.ToString(Request.Params.Get("FileTypeID")));
            model.FileName = Convert.ToString(Request.Params.Get("FileName"));
            model.FileRemark = Convert.ToString(Request.Params.Get("FileRemark"));
            model.Remarks = Convert.ToString(Request.Params.Get("Remarks"));
            model.FilePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.state = 1;//0删除 1没删

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //model.FileName = Path.GetFileName(files[0].FileName);
            model.FileNewName = Guid.NewGuid().ToString();
            string url = ConfigurationManager.AppSettings["MenagementDocuments_File"];//文件存放地址

            //bool flag = _manageBLL.AddFileManagement(files, model, Server.MapPath("/images/"), out errortype);
            bool flag = _manageBLL.AddFileManagement(files, model, url, out errortype);
            if (flag)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "上传成功"));
            }
            else
            {
                if (errortype == 2)
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "该文件编号已存在"));
                else if (errortype == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "文件必须为.doc、.docx、.pdf"));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "上传失败"));
            }

        }
        #endregion

        #region 修改文件
        [Authentication]
        public string UpdateFileManagement()
        {
            try
            {
                bool flag = false;
                int errortype;
                string FileUrl;
                TB_FileManagement model = new TB_FileManagement();
                model.id = Convert.ToInt32(Request.Params.Get("id"));
                model.FileName = Convert.ToString(Request.Params.Get("FileName"));
                model.FileNum = Convert.ToString(Request.Params.Get("FileNum"));
                model.FileType = new Guid(Convert.ToString(Request.Params.Get("FileTypeID")));
                model.FileNewName = Convert.ToString(Request.Params.Get("FileNewName"));
                model.FileFormat = Convert.ToString(Request.Params.Get("FileFormat"));
                model.FileRemark = Convert.ToString(Request.Params.Get("FileRemark"));
                model.Remarks = Convert.ToString(Request.Params.Get("Remarks"));
                model.state = Convert.ToInt32(Request.Params.Get("state"));//0 有效 1无效 2作废
                FileUrl = Convert.ToString(Request.Params.Get("FileUrl"));

                Guid loginer = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

                flag = _manageBLL.UpdateFileManagement(model, loginer, FileUrl, out errortype);
                if (flag)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Successfully"));
                }
                else
                {
                    if (errortype == 2)
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "FileNum Repeat"));
                    else
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operation Failure"));
                }
            }

            catch (Exception E)
            {
                throw;
            }
        }
        #endregion

        #region 删除文件
        [Authentication]
        public string DelFileManagement(FormCollection FormCollection)   //删除状态为1
        {
            try
            {
                bool flag = false;
                Guid loginer = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
                TB_FileManagement model = new TB_FileManagement();
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.FileName = Request.Params.Get("FileName");
                model.FileUrl = Request.Params.Get("FileUrl");
                flag = _manageBLL.DelFileManagement(model, loginer);
                if (flag)
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operate Successfully"));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "Operation Failure"));
            }
            catch (Exception)
            {
                
                throw;
            }
           

        }
        #endregion

        #region 审核文件
        [Authentication]
        public string AuditFileManagement(int id, string FileUrl)   //审核状态为2
        {
            bool flag = false;
            int state = 0;
            TB_FileManagement model = new TB_FileManagement();
            model.id = id;
            model.Personnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.state = 2;
            flag = _manageBLL.AuditFileManagement(model, out  state, FileUrl);
            if (flag)
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "审核成功"));
            else
            {
                if (state == 2)
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "该文件已被审核"));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "审核失败"));
            }

        }
        #endregion

        #region 签发文件
        [Authentication]
        public string IssuedFileManagement(int id, string FileUrl)   //签发状态为3
        {
            bool flag = false;
            int state = 0;
            TB_FileManagement model = new TB_FileManagement();
            model.id = id;
            model.IssuePersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.state = 3;
            flag = _manageBLL.IssuedFileManagement(model, out state, FileUrl);
            if (flag)
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "签发成功"));
            else
            {
                if (state == 3)
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "该文件已被签发"));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "签发失败"));
            }

        }
        #endregion

        #region  导出列表
        [Authentication]
        public string export(string search, string key, int type, string ids)
        {
            string tempFileName = "环境文件列表导出.xls";
            string tempFilePath = Server.MapPath("/Models/");
            bool flag = false;
            flag = _manageBLL.export(search, key, tempFileName, tempFilePath, type, ids);
            if (flag)
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "导出成功，地址为"));
            else
                return JsonConvert.SerializeObject(new ExecuteResult(flag, "导出失败，地址为"));
        }

        #endregion

        #region 判断文件是否存在
        //[Authentication]
        public string IsFileExist(string FileUrl)   //签发状态为3
        {
            //判断路径是否存在
            if (!System.IO.File.Exists(FileUrl))
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "该文件不存在"));
            }
            return JsonConvert.SerializeObject(new ExecuteResult(true, "成功"));

        }
        #endregion
        #endregion

        #region 内部评审
        public ActionResult Internaldocuments()
        {
            return View();
        }
        #endregion

        #region 管理评审
        public ActionResult ManageDocuments()
        {
            return View();
        }
        #endregion

        #region 体系文件
        public ActionResult SystemDocuments()
        {
            return View();
        }
        #endregion

        #region 技术培训文件
        public ActionResult TechnicalTraining()
        {
            return View();
        }
        #endregion



        #region 获取文件信息
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public string downloadFile(FormCollection FormCollection)
        {
            try
            {
                int id = Convert.ToInt32(FormCollection["id"]);

                TB_FileManagement TB_FileManagement = _manageBLL.GetFileModelBLL(id);
                //if (TB_FileManagement != null)
                //{
                //    return JsonConvert.SerializeObject(new ExecuteResult(true, TB_FileManagement.FileUrl));
                if (!System.IO.File.Exists(Server.MapPath(TB_FileManagement.FileUrl)))
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "该文件不存在"));
                }
                return JsonConvert.SerializeObject(new ExecuteResult(true, TB_FileManagement.FileUrl));
                //}
                //else
                //{
                //    return JsonConvert.SerializeObject(new ExecuteResult(false, "Operate Fail"));
                //}
            }
            catch (Exception E)
            {
                throw;
            }



        }
        #endregion


    }
}
