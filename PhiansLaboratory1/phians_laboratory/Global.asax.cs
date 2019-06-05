using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Quartz;
using Quartz.Impl;
using phians_laboratory.QuartzTimeJob;


namespace phians_laboratory
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        //1,调度器
        Task<IScheduler> taskScheduler;
        IScheduler scheduler1;
        //调度器工厂
        ISchedulerFactory factory = new StdSchedulerFactory();
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(Server.MapPath("Log4net.config")));
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
      
           //RouteTable.Routes.AddCombresRoute("Combres");
            //SqlDependency.Start(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);


            #region  创建定时事务
            //创建一个调度器
            taskScheduler = factory.GetScheduler();
            scheduler1 = taskScheduler.Result;
            //调度工作
            IJobDetail job = JobBuilder.Create<TJDeleteTemp>().WithIdentity("job1", "group1").Build();
            //3、创建一个触发器

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithCronSchedule("0 27 16 ? * MON-FRI")     //周一至周五的上午10:15触发
                .Build();

            //3.1另外一种触发器
            //ISimpleTrigger trigger1 = (ISimpleTrigger)TriggerBuilder.Create()
            //     .WithIdentity("trigger1", "group1")
            //     .StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();

            //4、将任务与触发器添加到调度器中
           scheduler1.ScheduleJob(job, trigger);
            //5、开始执行
            scheduler1.Start();
            #endregion

        }
        protected void Application_End(object sender, EventArgs e)
        {
            //SqlDependency.Stop(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            //   在应用程序关闭时运行的代码关闭定时事务
            if (scheduler1 != null)
            {
                scheduler1.Shutdown(true);
            }
        }

    }
}