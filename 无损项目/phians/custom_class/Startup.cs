using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(phians.Startup))]

namespace phians
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            var userIdProvider = new SignalR_User_id();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => userIdProvider);
      
            app.MapSignalR();
        }
    }
}
