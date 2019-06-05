using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace phians
{
    public class rewrite_IHttpModule : IHttpModule, IRequiresSessionState
    {
        #region IHttpModule 成员

     
        //2.在Init()方法中对Context进行注册AcquireRequestState事件。
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        //3.完善AcquireRequestState方法，然后判断session过期
        public void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            string path = app.Request.RawUrl;
           
            string[] name_1 = path.Split('?');
            int n = name_1[0].ToLower().IndexOf("login.aspx");//第一次 7 第二次-1 
            //忽略阅读报告页面的请求
            bool flag = name_1[0] == "/mainform/Lossless_report/ReportManager.ashx" ? false : true;
            string name2 = name_1[0].Substring(name_1[0].LastIndexOf(".") + 1);
            string name3 = name_1[0].Substring(name_1[0].LastIndexOf(".") + 1);
            if ((n == -1 && (name2 == "aspx" || name2 == "ashx")) && flag)
            {
                if (app.Context.Session["loginAccount"] == null)
                {
                    //app.Response.Write("<script>window.parent.login();</script>");
                    app.Context.Response.Write("<script>location.href = '/index.html' </script>");
                    app.Response.End();
                    // app.Server.Transfer("/login2.aspx"); 
                    //app.Context.Response.Write("<script language='javascript'>" + "parent.window.open('/login2.aspx', '_top')" + "</script>");
                }
            }        
         
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}