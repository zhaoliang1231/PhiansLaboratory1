using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public class Send_message
    {
        public void send_message_fuction(){
            var hub = GlobalHost.ConnectionManager.GetHubContext<message_Hub>();
            string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hub.Clients.User("yushaohui").broadcastMessage("2017002" + "**" + _now_time);      
        }
        public void send_usercount(string User_count, string message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<message_Hub>();
            string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hub.Clients.User(User_count).broadcastMessage(message + "**" + _now_time);
        }

    }
}