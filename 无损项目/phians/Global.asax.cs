using phians.custom_class;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
//using System.Web.Routing;
//using Combres;

namespace phians
{
    public class Global : System.Web.HttpApplication
    {

      //调度器
        IScheduler scheduler;
        //调度器工厂
        ISchedulerFactory factory;
        protected void Application_Start()
        {

            //1、创建一个调度器
            factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();
            scheduler.Start();
            //2、创建一个任务
            IJobDetail job = JobBuilder.Create<TimeJob>().WithIdentity("job1", "group1").Build();

            //3、创建一个触发器
            //DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithCronSchedule("0 30 21 ? * MON-FRI")     //周一周五执行一次
                //.StartAt(runTime)
                .Build();

            //4、将任务与触发器添加到调度器中
            scheduler.ScheduleJob(job, trigger);
            //5、开始执行
            scheduler.Start();
     
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //   在应用程序关闭时运行的代码
            if (scheduler != null)
            {
                scheduler.Shutdown(true);
            }
        }


        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为 
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
            // 或 SQLServer，则不会引发该事件。 
            //string strUserId = Session["loginAccount"] as string;
            //ArrayList list = Application.Get("GLOBAL_USER_LIST") as ArrayList;
            //if (strUserId != null && list != null)
            //{
            //    list.Remove(strUserId);
            //    Application.Add("GLOBAL_USER_LIST", list);
            //}
        }
      
        //protected void FormsAuthentication_OnAuthenticate(Object sender,FormsAuthenticationEventArgs e)
        //{
        //    if (Request.Cookies[FormsAuthentication.FormsCookieName] == null)
        //    {
        //        Response.Redirect("../login/login.aspx");
        //    }
        //}
    }
}