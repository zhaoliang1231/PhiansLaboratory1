using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;
using PhiansCommon;

[assembly: OwinStartup(typeof(phians_laboratory.custom_class.MessageStartup))]

namespace phians_laboratory.custom_class
{
    public class MessageStartup
    {
        public void Configuration(IAppBuilder app)
        {

          
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
           // app.UseCors(CorsOptions.AllowAll);
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => userIdProvider);         
            app.MapSignalR();

       
       
           // GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => clientName);

        
        }
    }
}
