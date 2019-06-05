using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace phians
{
    public class message_Hub : Hub,IRequiresSessionState
    {
        //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
        public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["pubsConnectionString"].ConnectionString;

        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        public void send3(string name, string message)
        {
            string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Clients.All.broadcastMessage(name, message + "**" + _now_time);

            
        }
    
        /// <summary>
        /// 页面消息
        /// </summary>
        /// <param name="message_type"></param>
        /// <param name="user_name"></param>
        public void all_message(string message_type, string user_name)
        {
            string charges = "SELECT User_count,message,id from dbo.TB_show_message where message_push_personnel='" + user_name + "'and confirm_push_flag='0'";
            string send_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int i = 0;
            string[] all_id_str = new string[100];
            try
            {
                SqlDataReader charges_dr = SQLHelper.ExecuteReader(CommandType.Text, charges);
                while (charges_dr.Read())
                {
                    string User_count = charges_dr["User_count"].ToString().Trim();
                    string message = charges_dr["message"].ToString();
                    string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Clients.User(User_count).broadcastMessage(message + "**" + _now_time);
                    string id = charges_dr["id"].ToString();
                    int ids = Convert.ToInt32(id);
                    all_id_str[i] = "UPDATE dbo.TB_show_message SET send_time='" + send_time + "', confirm_push_flag='1' where id=" + ids + " ";
                    i++;
                }
                charges_dr.Close();
               

                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    for (int j = 0; j < i; j++)
                    {
                        SQLStringList.Add(all_id_str[j]);
                    }
                    //sql事务
                    SQLHelper.ExecuteSqlTran(SQLStringList);
                }
                catch (Exception)
                {
                    //context.Response.Write("F");
                }
                finally { 
                    //context.Response.End(); 
                }
            }
            catch (Exception e) {
                throw e;
            
            }         

        }
        public void click_message(string message_type, string user_name)
        {
            string charges = "SELECT User_count,message,id from dbo.TB_show_message where message_push_personnel='" + user_name + "'and confirm_push_flag='0'";
            string send_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int i = 0;
            string[] all_id_str = new string[100];
            try
            {
                SqlDataReader charges_dr = SQLHelper.ExecuteReader(CommandType.Text, charges);
                while (charges_dr.Read())
                {
                    string User_count = charges_dr["User_count"].ToString().Trim();
                    string message = charges_dr["message"].ToString();
                    string _now_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Clients.All.broadcastMessage(User_count, message + "**" + _now_time);
                    string id = charges_dr["id"].ToString();
                    int ids = Convert.ToInt32(id);
                    all_id_str[i] = "UPDATE dbo.TB_show_message SET send_time='" + send_time + "', confirm_push_flag='1' where id=" + ids + " ";
                    i++;
                }
                charges_dr.Close();

                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    for (int j = 0; j < i; j++)
                    {
                        SQLStringList.Add(all_id_str[j]);
                    }
                    //sql事务
                    SQLHelper.ExecuteSqlTran(SQLStringList);
                }
                catch (Exception)
                {
                    //context.Response.Write("F");
                }
                finally
                {
                    //context.Response.End(); 
                }
            }
            catch (Exception e)
            {
                throw e;

            }

        }
        public string GetSignalrID()
        {
            if (Context.Request.GetHttpContext().Request.Cookies["SignalRID"] != null)
            {
                return Context.Request.GetHttpContext().Request.Cookies["SignalRID"].Value;
            }
            return "";

        }
        /// <summary>
        /// 测试消息
        /// </summary>
        /// <param name="SignalrID"></param>
        /// <param name="message"></param>
        public void sendmessage(string SignalrID,string message )
        {

         
            string id = Context.ConnectionId;
            try { Clients.User(SignalrID.Trim()).showMessage(message); }
            catch(Exception){}
            //Client系统自带ConnectionId调用；User为自定义ConnectionId调用
           // Clients.Client(SignalrID).addMessage(message);
          
           
        }
        public string get_id()
        {

            
            string id = Context.ConnectionId;
         
            return id;
        }

        //事物
        public static void ExecuteSqlTran(List<string> hxw_SQL_List)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                SqlTransaction tran = connection.BeginTransaction();
                cmd.Connection = tran.Connection;
                cmd.Transaction = tran; //获取或设置将要其执行的事务                  
                try
                {
                    for (int n = 0; n < hxw_SQL_List.Count; n++)
                    {
                        string hxw_sql_str = hxw_SQL_List[n].ToString();
                        if (hxw_sql_str.Trim().Length > 1)
                        {
                            cmd.CommandText = hxw_sql_str;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();

                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tran.Rollback();
                    throw new Exception(E.Message);
                }
            }

        }
    }
}