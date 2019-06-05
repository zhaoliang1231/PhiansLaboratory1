using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace phians.custom_class
{
    public class Operation_log
    {
        public  static int  operation_log_(string operation_user, string operation_name, string operation_type,string operation_info)
        {

            string operation_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string operation_ip = GetIP4Address();
            string sql1 = "insert into dbo.TB_operation_log(operation_user,operation_name,operation_date,operation_ip,operation_type,operation_info) values('" + operation_user + "','" + operation_name + "','" + operation_date + "','" + operation_ip + "','" + operation_type + "','" + operation_info + "')";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["pubsConnectionString"].ConnectionString);
                SQLHelper.ExecuteNonQuery(con, CommandType.Text, sql1);
                con.Close();
                
            }
            catch
            {

            }
            return 1;

        }
        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;


            foreach (IPAddress IPA in Dns.GetHostAddresses(System.Web.HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
    }
}