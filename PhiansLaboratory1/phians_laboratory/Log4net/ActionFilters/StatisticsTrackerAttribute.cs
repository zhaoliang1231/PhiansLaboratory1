using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using Phians_Entity;

namespace phians_laboratory.custom_class.ActionFilters
{
    /// <summary>
    /// 统计跟踪器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class StatisticsTrackerAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private readonly string Key = "_thisOnActionMonitorLog_";

        #region Action时间监控
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MonitorLog MonLog = new MonitorLog();
            MonLog.ExecuteStartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff", DateTimeFormatInfo.InvariantInfo));
            MonLog.ControllerName = filterContext.RouteData.Values["controller"] as string;
            MonLog.ActionName = filterContext.RouteData.Values["action"] as string;

            filterContext.Controller.ViewData[Key] = MonLog;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            MonitorLog MonLog = filterContext.Controller.ViewData[Key] as MonitorLog;
            MonLog.ExecuteEndTime = DateTime.Now;
            MonLog.FormCollections = filterContext.HttpContext.Request.Form;//form表单提交的数据
            MonLog.QueryCollections = filterContext.HttpContext.Request.QueryString;//Url 参数
            LoggerHelper.Monitor(MonLog.GetLoginfo());

        }
        #endregion

        #region View 视图生成时间监控
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
         
            MonitorLog MonLog = filterContext.Controller.ViewData[Key] as MonitorLog;
            // BaseController重定向时候MonLog为null
            if (MonLog != null)
            MonLog.ExecuteStartTime = DateTime.Now;

        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            MonitorLog MonLog = filterContext.Controller.ViewData[Key] as MonitorLog;
            // BaseController重定向时候MonLog为null
            if (MonLog != null) {
            MonLog.ExecuteEndTime = DateTime.Now;
            LoggerHelper.Monitor(MonLog.GetLoginfo(MonitorLog.MonitorType.View));
            filterContext.Controller.ViewData.Remove(Key);
            }
        }

        #endregion

        #region 错误日志

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string ControllerName = string.Format("{0}Controller", filterContext.RouteData.Values["controller"] as string);
                string ActionName = filterContext.RouteData.Values["action"] as string;
                string ErrorMsg = string.Format("在执行 controller[{0}] 的 action[{1}] 时产生异常", ControllerName, ActionName);
                LoggerHelper.Error(ErrorMsg, filterContext.Exception);

            

                //  异常页面信息返回
                //判断是视图还是ajax请求
                bool Requestmethod = filterContext.HttpContext.Request.IsAjaxRequest();
                //get或者post
                String method = filterContext.HttpContext.Request.HttpMethod.ToUpper();
                //form提交判断
                if (Requestmethod == false && method == "POST")
                {

                    Requestmethod = true;
                }
                if (Requestmethod)
                {
                    //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    //{
                    //    controller = "GetAuthority",
                    //    action = "GetExceptionMsg",

                    //    Data = new { ExceptionMsg = filterContext.Exception.Message },
                    //    ContentEncoding = System.Text.Encoding.UTF8,
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    //    ContentType = "json"

                    //    //  Data = filterContext.Exception.Message,

                    //}));
                  //  string ss = filterContext.HttpContext.Response.ContentType;
                    filterContext.HttpContext.Response.ContentType = "text/html;application/x-www-form-urlencoded";

                    string messageException= StringToJson(filterContext.Exception.Message);
                    filterContext.HttpContext.Response.Write(JsonConvert.SerializeObject(new ExecuteResult(false, "Exception Message:" + messageException)));                  
                    filterContext.HttpContext.Response.End();

                }
                //filterContext.HttpContext.Response.Redirect("/Account/Login", true);
                return  ;
            }
        }
        #endregion

        public static String StringToJson(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
    /// <summary>
    /// 监控日志对象
    /// </summary>
    public class MonitorLog
    {
        public string ControllerName
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }

        public DateTime ExecuteStartTime
        {
            get;
            set;
        }
        public DateTime ExecuteEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// Form 表单数据
        /// </summary>
        public NameValueCollection FormCollections
        {
            get;
            set;
        }
        /// <summary>
        /// URL 参数
        /// </summary>
        public NameValueCollection QueryCollections
        {
            get;
            set;
        }
        /// <summary>
        /// 监控类型
        /// </summary>
        public enum MonitorType
        {
            Action = 1,
            View = 2
        }
        /// <summary>
        /// 获取监控指标日志
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public string GetLoginfo(MonitorType mtype = MonitorType.Action)
        {
            string ActionView = "Action执行时间监控：";
            string Name = "Action";
            if (mtype == MonitorType.View)
            {
                ActionView = "View视图生成时间监控：";
                Name = "View";
            }
            string Msg = @"
            {0}
            ControllerName：{1}Controller
            {8}Name:{2}
            开始时间：{3}
            结束时间：{4}
            总 时 间：{5}秒
            Form表单数据：{6}
            URL参数：{7}
                    ";
            return string.Format(Msg,
                ActionView,
                ControllerName,
                ActionName,
                ExecuteStartTime,
                ExecuteEndTime,
                (ExecuteEndTime - ExecuteStartTime).TotalSeconds,
                GetCollections(FormCollections),
                GetCollections(QueryCollections),
                Name);
        }

        /// <summary>
        /// 获取Post 或Get 参数
        /// </summary>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public string GetCollections(NameValueCollection Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0)
            {
                return Parameters;
            }
            foreach (string key in Collections.Keys)
            {
                Parameters += string.Format("{0}={1}&", key, Collections[key]);
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&"))
            {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;
        }

    }
}