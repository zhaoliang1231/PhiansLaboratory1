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
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phians_laboratory.Controllers
{
    public class PersonalCenterController : BaseController
    {
        #region  个人中心
        public ActionResult PersonalInformation()
        {
            return View();
        }

        OperationUserBLL _userBll = new OperationUserBLL();

        #region 获取用户基础信息
        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        /// <param name="id">用户user_id</param>
        /// <returns></returns>
        public string GetUser()
        {
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

            try
            {
                TB_UserInfo model = _userBll.GetUserByParam(UserId);
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_UserInfo>(true, "成功", model));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_UserInfo>(false, "失败", null));
            }
        }
        #endregion

        #region 获取用户证书信息
        /// <summary>
        /// 获取用户证书信息
        /// </summary>
        /// <param name="id">用户user_id</param>
        /// <returns></returns>
        public string GetUserCertificate()
        {
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            int pageSize = Convert.ToInt32(Request.Params.Get("rows"));

            try
            {
                int totalRecord;
                List<TB_CertificateManagement> CertificateList = new PersonalManagementBLL().GetUserCertificate(pageIndex, pageSize, UserId, out totalRecord);
                PagedResult<TB_CertificateManagement> pagelist = new PagedResult<TB_CertificateManagement>(totalRecord, CertificateList, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_UserInfo>(false, "失败", null));
            }
        }
        #endregion

        #region 修改用户基础资料
        public string UpdateUser(FormCollection collection)
        {
            TB_UserInfo model = new TB_UserInfo();
            model.UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.UserName = Convert.ToString(collection["UserName"]);
            model.Tel = Convert.ToString(collection["Tel"]);
            model.Phone = Convert.ToString(collection["Phone"]);
            model.Fax = Convert.ToString(collection["Fax"]);
            model.Email = Convert.ToString(collection["Email"]);
            model.QQ = Convert.ToString(collection["QQ"]);
            model.Address = Convert.ToString(collection["Address"]);
            model.Remarks = Convert.ToString(collection["Remarks"]);
            model.UserNsex = Convert.ToBoolean(collection["UserNsex"]);
            model.Postcode = Convert.ToString(collection["Postcode"]);
            model.Profession = Convert.ToString(collection["Profession"]);
            model.Job = Convert.ToString(collection["Job"]);
            //model.JobNum = Convert.ToString(collection["JobNum"]);
            // model.sort_num = Convert.ToInt32(Request.Params.Get("sort_num"));

            try
            {
                bool flag = _userBll.UpdateUser(model);
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

        #region 修改用户签名
        public string UpdateUserImg(FormCollection FormCollection)
        {
            TB_UserInfo model = new TB_UserInfo();
            model.UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//个人中心上传的是自己的签名
            model.UserName = Convert.ToString(HttpUtility.UrlDecode(CookiesHelper.GetCookie("UserName").Value));
            model.Signature = Convert.ToString(FormCollection["Signature"]);
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //HttpPostedFileBase fileitem = files[0];

            #region 上传
            if (files != null && files.Count > 0)
            {
                //string fileName = Path.GetFileName(files[0].FileName);          //原始文件名称
                //string fileExtension = Path.GetExtension(fileName).ToLower();  //文件扩展名
                ////string fileName2 = model.UserId.ToString();//文件名
                //if (fileExtension != ".png" && fileExtension != ".jpg")
                //{
                //    return JsonConvert.SerializeObject(new ExecuteResult(false, "上传格式不正确！"));
                //}
                string tempPath = ConfigurationManager.AppSettings["signature_pic"].ToString();//保存路径
                //判断路径是否存在
                if (System.IO.File.Exists(Server.MapPath(model.Signature)))
                {
                    System.IO.File.Delete(Server.MapPath(model.Signature));
                }

                string[] Extension = { ".png", ".jpg" };//支持文件格式
                Resultinfo Resultinfo = new FileOperate().Filesave(files, tempPath, Extension);//保存文件并输出文件信息等
                if (!Resultinfo.Sucess)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, Resultinfo.ReturnContent));
                }

                model.Signature = Resultinfo.FileInfo[0].FileRelativeUrl;//文件相对路径

                //string FileNameGuid = Guid.NewGuid().ToString() + ".png";//文件新名字
                //string update_url = filePath + FileNameGuid;
                //files[0].SaveAs(update_url);//保存成功保存文件

                //修改数据库保存路径
                try
                {
                    bool flag = _userBll.UpdateUserImg(model);
                    if (flag)
                    {
                        return JsonConvert.SerializeObject(new SingleExecuteResult<TB_UserInfo>(flag, "上传成功", model));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "上传失败"));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                }
                catch (Exception e)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "上传失败"));
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "文件不存在！"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            #endregion

        }
        #endregion

        #region 上传用户头像
        public string UploadUserPortrait(FormCollection FormCollection)
        {
            TB_UserInfo model = new TB_UserInfo();
            model.UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.UserName = Convert.ToString(HttpUtility.UrlDecode(CookiesHelper.GetCookie("UserName").Value));
            model.Portrait = Convert.ToString(FormCollection["Portrait"]);

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //HttpPostedFileBase fileitem = files[0];

            #region 上传
            if (files != null && files.Count > 0)
            {
                //string fileName = Path.GetFileName(files[0].FileName);          //原始文件名称
                //string fileExtension = Path.GetExtension(fileName).ToLower();  //文件扩展名
                //string fileName2 = Guid.NewGuid().ToString();//文件名
                //if (fileExtension != ".png" && fileExtension != ".jpg")
                //{
                //    return JsonConvert.SerializeObject(new ExecuteResult(false, "上传格式不正确"));
                //}
                string tempPath = ConfigurationManager.AppSettings["UserPortrait"].ToString();//相对路径
                //判断源文件是否存在
                if (System.IO.File.Exists(Server.MapPath(model.Portrait)))
                {
                    System.IO.File.Delete(Server.MapPath(model.Portrait));
                }

                string[] Extension = { ".png", ".jpg" };//支持文件格式
                Resultinfo Resultinfo = new FileOperate().Filesave(files, tempPath, Extension);//保存文件并输出文件信息等
                if (!Resultinfo.Sucess)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, Resultinfo.ReturnContent));
                }
                model.Portrait = Resultinfo.FileInfo[0].FileRelativeUrl;//文件相对路径


                //修改数据库保存路径
                try
                {
                    bool flag = _userBll.UploadUserPortrait(model);
                    if (flag)
                    {
                        return JsonConvert.SerializeObject(new SingleExecuteResult<TB_UserInfo>(flag, "上传成功", model));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(flag, "上传失败"));//返回执行结果 ，添加成功可以换成需要的内容
                    }
                }
                catch (Exception e)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "上传失败"));
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "上传失败"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            #endregion
            //model.Tel = Convert.ToString(Request.Params.Get("Tel"));
            //model.Phone = Convert.ToString(Request.Params.Get("Phone"));
            //model.Fax = Convert.ToString(Request.Params.Get("Fax"));
            //model.Email = Convert.ToString(Request.Params.Get("Email"));
            //model.Postcode = Convert.ToString(Request.Params.Get("Postcode"));
            //model.QQ = Convert.ToString(Request.Params.Get("QQ"));
            //model.Address = Convert.ToString(Request.Params.Get("Address"));
            //model.Remarks = Convert.ToString(Request.Params.Get("Remarks"));
            //model.sort_num = Convert.ToInt32(Request.Params.Get("sort_num"));


        }
        #endregion

        #endregion

        #region  工作消息
        public ActionResult WorkMessage()
        {
            return View();
        }

        #region 加载消息列表
        /// <summary>
        /// 获取未读消息列表
        /// </summary>
        /// <returns></returns>
        public string GetUnReadMessage(FormCollection FormCollection)
        {

            int pageIndex = Convert.ToInt32(FormCollection["page"]);
            int pagesize = Convert.ToInt32(FormCollection["rows"]);

            string search = Convert.ToString(FormCollection["search"]);
            string key = Convert.ToString(FormCollection["key"]);
            int flag = Convert.ToInt32(FormCollection["flag"]);
            //用户id
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));


            try
            {
                int totalRecord;
                List<TB_Message> list = new MessageBLL().GetUnReadMessage(pageIndex, pagesize, out totalRecord, UserId, flag, search, key);
                PagedResult<TB_Message> pagelist = new PagedResult<TB_Message>(totalRecord, list, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                return "{'total':0,'success':True,'rows':[]}";//异常返回无数据
            }
        }
        //public string GetReadMessage()
        //{

        //    int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
        //    int pagesize = Convert.ToInt32(Request.Params.Get("rows"));

        //    //用户id
        //    Guid UserId = new Guid(Session["UserId"].ToString());
        //    //确认时间（为空表示未读消息）
        //    //string ConfirmTime_date = Convert.ToString(Request.Params.Get("ConfirmTime")); ;
        //    try
        //    {
        //        int totalRecord;
        //        List<TB_Message> list = new MessageBLL().GetReadMessage(pageIndex, pagesize, out totalRecord, UserId);
        //        PagedResult<TB_Message> pagelist = new PagedResult<TB_Message>(totalRecord, list, true);//转换成easyui json
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

        #region  确认消息

        /// <summary>
        /// 确认消息
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public string EditMessage(string ids)
        {

            TB_Message model = new TB_Message();
            //model.id = Convert.ToInt32(Request.Params.Get("ids"));

            try
            {
                bool flag = new MessageBLL().MessageEdit(ids);
                if (flag)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "确认成功"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "确认失败"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "确认失败"));
            }
        }
        #endregion

        #region  确认全部消息

        public string EditAllMessage()
        {
            //用户id
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            try
            {
                bool flag = new MessageBLL().MessageAllEdit(UserId);
                if (flag)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "确认成功"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(flag, "确认失败"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "确认失败"));
            }
        }
        #endregion
        #endregion
    }
}
