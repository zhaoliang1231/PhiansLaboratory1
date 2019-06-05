using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phians_Entity;
using Phians_BLL;

namespace phians_laboratory.custom_class
{
    public class SendMessage
    {
        public void send_message_fuction(string User_count, string message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          
            hub.Clients.User(User_count).broadcastMessage("2017002" + "**" + _now_time);
        }

        #region 指定用户发送消息
        public void client_message(string UserId, string message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            TB_MessageRegister model = new CommonBLL().getRegisterSignalRInfoLL(new Guid(UserId));
            if (model != null)
            {
                string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //按id发送消息
                hub.Clients.Client(model.ConnectionId.ToString()).receive_message(message + "**" + _now_time);
            }

            hub.Clients.User(UserId).broadcastMessage("2017002" + "**" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        }
        #endregion

    }

      
}