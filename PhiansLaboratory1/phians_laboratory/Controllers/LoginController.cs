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
using Newtonsoft.Json;
namespace phians_laboratory.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {

            HttpCookie UserId = CookiesHelper.GetCookie("UserId");

   
            try
            {
                new Guid(DES_.StringDecrypt(Convert.ToString(UserId.Value)));
                if (UserId != null)
                {


                    Response.Redirect("/mainform");
                }
            }
            catch
            {
                CookiesHelper.AddCookie("UserID", System.DateTime.Now.AddDays(-1));
            }

            return View();

        }
        public ActionResult test()
        {

            return View();


        }
        public ActionResult FunctionalModule()
        {

            return View();


        }

        //测试排程
        //public JsonResult TestSchedule()
        //{
        //    //bool flag = LabScheduleBLL.GenerateSchedule("YKL0608");//传入mtrNo
        //    return Json(true);
        //}


        //public void TestReport()
        //{
        //    new ReportHandleBLL().TestWord();
        //}


        #region 登陆系统
        public string login(TB_UserInfo User_info)
        {

            try
            {
                //后台获取用户名和密码            
                string User_count = DecodeBase64(User_info.UserCount);
                string upassword = DecodeBase64(User_info.UserPwd);
                upassword = DES_.stringEncrypt(upassword);
                //string upassword2 = DecryptDES(upassword, "hxw_2016");
           
              
                //string User_count_state = "正在使用中";
                //检查用户
                Phians_Entity.TB_UserInfo User_current = new UserBLL().UserLogin(User_count, upassword);
                if (User_current != null)
                {
                    if (User_current.CountState == 1)
                    {
                        //Session["loginAccount"] = User_count.Trim();
                        //Session["login_username"] = User_current.UserName;
                        //Session["UserId"] = User_current.UserId;
                        //用户名
                        CookiesHelper.SetCookie("UserName", HttpUtility.UrlEncode(Convert.ToString(User_current.UserName)), DateTime.Now.AddDays(1.0));
                        //加密userid
                        string UserId = DES_.stringEncrypt(Convert.ToString(User_current.UserId));
                        string UserCount = DES_.stringEncrypt(Convert.ToString(User_current.UserCount));
                        //用户id
                        CookiesHelper.SetCookie("UserCount", UserCount, DateTime.Now.AddDays(1.0));
                        CookiesHelper.SetCookie("UserId", UserId, DateTime.Now.AddDays(1.0));
                        CookiesHelper.SetCookie("SignalRID", User_count.Trim(), DateTime.Now.AddDays(1.0));
                        string ff = CookiesHelper.GetCookieValue("UserId").ToString();
                        // GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => User_count);
                        // Response.Redirect("/mainform/index");
                        // RedirectToAction("mainform", "index", new { url = Request.RawUrl });

                        HttpCookie UserId2 = CookiesHelper.GetCookie("UserCount");
                        HttpCookie UserId3 = CookiesHelper.GetCookie("UserId");
                        return JsonConvert.SerializeObject((new ExecuteResult(true, "登录成功")));

                    }
                    else
                    {
                        return JsonConvert.SerializeObject((new ExecuteResult(false, "账户停用")));
                    }

                }
                else
                {
                    return JsonConvert.SerializeObject((new ExecuteResult(false, "账户或者密码")));

                }


            }
            catch (Exception e)
            {
                throw;
            }


        }

        #endregion



        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

    }
}
