using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Phians_BLL;
using Phians_Entity;

namespace phians_laboratory.custom_class
{
    public class MessageHub : Hub
    {
       
        public static Dictionary<string, ClientInfo> CurrClients = new Dictionary<string, ClientInfo>();
        
        //用户注册
        public class ClientInfo
        {
            public string ConnId { get; set; }

            public string ClientName { get; set; }
        }
        ////注册用户
        //public void Register(string clientName)
        //{


           
        //    string connId = Context.ConnectionId;
        //    lock (CurrClients)
        //    {
        //       //判断是否有无效的id
        //        if (CurrClients.ContainsKey(clientName))
        //        {

        //            string connId2 = CurrClients[clientName].ConnId;
        //            if (connId != connId2) {
        //                CurrClients.Remove(clientName);
        //            }
                    
        //        }
        //        //判断是否已经注册过
        //        if (!CurrClients.ContainsKey(clientName))
        //        {
        //            CurrClients.Add(clientName, new ClientInfo { ClientName = clientName, ConnId = connId });
                    
        //        }
        //    }
        //    Clients.All.NowUser(CurrClients);
        //    // Clients.All.broadcastmessage( "成功" + "**" + "2018");
        //   // GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => clientName);

        //}


          public void Register(string clientName)
        {


           TB_MessageRegister model= new TB_MessageRegister();

              model.ConnectionId =  new Guid(Context.ConnectionId);
             // model.UserId=  new Guid(clientName);
              model.RegisterFlag=true;
              model.RegisterDate=DateTime.Now;         
              new CommonBLL().RegisterSignalRBLL(model);
          //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => clientName);

        }
        
        #region 广播消息
        public void broadcastmessage_(string message)
        {
          

           string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           Clients.All.broadcastmessage(message + "**" + _now_time);

            
        }
        #endregion

        #region 指定用户发送消息
        public void client_message(string User_count, string message)
        {
           
                    string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                     //按id发送消息
                    Clients.Client(CurrClients[User_count].ConnId).receive_message(message + "**" + _now_time);

        }
        #endregion
            #region 指定用户发送消息
        public void client_message2(string UserId, string message)
        {

            TB_MessageRegister model = new CommonBLL().getRegisterSignalRInfoLL(new Guid(UserId));
            if (model !=null) {
                string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //按id发送消息
                Clients.Client(model.ConnectionId.ToString()).receive_message(message + "**" + _now_time);
            }
                   

        }
        #endregion

       
        #region 组用户发送消息
        public void client_group(IList<string> User_count, string message)
        {

            
        
            IList<string> User_count_ConnId = new List<string>();
            string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (var item in User_count)
            {
                try
                {
               User_count_ConnId.Add(CurrClients[item].ConnId);
                }
                catch
                {
                   
                }
            }

            Clients.Clients(User_count_ConnId).group_message( message + "**" + _now_time);
        }
        #endregion

        
    

    }
    
}