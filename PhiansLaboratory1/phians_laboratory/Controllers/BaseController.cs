using Newtonsoft.Json;
using Phians_BLL;
using Phians_Entity;
using phians_laboratory.custom_class;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace phians_laboratory.Controllers
{
    //-----------------------------------------------------------------------------------------------------
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        //验证用户是否登录

        //-----------------------------------------------------------------------------------------------------
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            // var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //-----------------------------------------------------------------------------------------------------
            var controllerName = filterContext.RouteData.Values["controller"] as string;
            var ActionName = filterContext.RouteData.Values["action"] as string;
            string RequestUrl = "/" + controllerName + "/" + ActionName;
            HttpCookie UserId = CookiesHelper.GetCookie("UserId");
            //var userName = Session["loginAccount"] as String;

            //判断是否登陆对有权限要求的界面
            if (UserId == null && filterContext.ActionDescriptor.IsDefined(typeof(phians_laboratory.custom_class.ActionFilters.Authentication), false))
            {
                //重定向至登录页面
                //filterContext.Result = RedirectToAction("Index", "Login", new { url = Request.RawUrl });

                //判断是视图还是ajax请求
                bool Requestmethod = HttpContext.Request.IsAjaxRequest();
                //get或者post
                String method = Request.HttpMethod.ToUpper();
                //form提交判断
                if (Requestmethod == false && method == "POST")
                {

                    Requestmethod = true;
                }

                switch (Requestmethod)
                {
                    //Action(ajax访问)
                    case true: filterContext.Result = RedirectToAction("UnLogin", "GetAuthority"); break;
                    //   Action(view访问)
                    case false: filterContext.Result = Redirect("/Login"); break;
                }


                return;
            }
            //主页判断
            if (UserId == null && controllerName == "mainform" && ActionName == "Index")
            {

                //重定向至登录页面
                //filterContext.Result = RedirectToAction("Index", "Login", new { url = Request.RawUrl });

                //判断是视图还是ajax请求
                bool Requestmethod = HttpContext.Request.IsAjaxRequest();
                //get或者post
                String method = Request.HttpMethod.ToUpper();
                //form提交判断
                if (Requestmethod == false && method == "POST")
                {

                    Requestmethod = true;
                }

                switch (Requestmethod)
                {
                    //Action(ajax访问)
                    case true: filterContext.Result = RedirectToAction("UnLogin", "GetAuthority"); break;
                    //   Action(view访问)
                    case false: filterContext.Result = Redirect("/Login"); break;
                }


                return;
            }
            //-----------------------------------------------------------------------------------------------------
            #region 权限判断

            //判断是否加需呀权限验证（action前加过滤标签 Authentication）
            if (filterContext.ActionDescriptor.IsDefined(typeof(phians_laboratory.custom_class.ActionFilters.Authentication), false))
            {



                //判断是视图还是ajax请求
                bool Requestmethod = HttpContext.Request.IsAjaxRequest();
                //get或者post
                String method = Request.HttpMethod.ToUpper();
                //form提交判断
                if (Requestmethod == false && method == "POST")
                {

                    Requestmethod = true;
                }
                //管理员账户过滤权限
                if (DES_.StringDecrypt(UserId.Value).ToLower() != "8cff8e9f-f539-41c9-80ce-06a97f481390")
                {
                    check_Authority(filterContext, new Guid(DES_.StringDecrypt(UserId.Value)), RequestUrl, Requestmethod);
                }
            }

            //-----------------------------------------------------------------------------------------------------
            #endregion
            //-----------------------------------------------------------------------------------------------------

        }
        //-----------------------------------------------------------------------------------------------------
        #region 检查权限
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        public void check_Authority(ActionExecutingContext filterContext, Guid UserId, string RequestUrl, bool OperateType)
        {
            CommonBLL CommonBLL_ = new CommonBLL();
            bool flag = CommonBLL_.GetAuthorization_BLL(UserId, RequestUrl);
            if (!flag)
            {
                switch (OperateType)
                {
                    //Action(ajax访问)
                    case true: filterContext.Result = RedirectToAction("Authorityshow", "GetAuthority"); break;
                    //   Action(view访问)
                    case false: filterContext.Result = RedirectToAction("AuthorityView", "GetAuthority"); break;
                }

            } 

        }
        //-----------------------------------------------------------------------------------------------------
        #endregion
        //-----------------------------------------------------------------------------------------------------

        //public override void OnException(ExceptionContext filterContext)
        //{
        //    //获取异常信息，入库保存
        //    Exception Error = filterContext.Exception;
        //    string Message = Error.Message;//错误信息
        //    string Url = Request.RawUrl;//错误发生地址

        //    filterContext.ExceptionHandled = true;
        //    filterContext.Result = new RedirectResult("/Error/Show/");//跳转至错误提示页面
        //}

    }
}


