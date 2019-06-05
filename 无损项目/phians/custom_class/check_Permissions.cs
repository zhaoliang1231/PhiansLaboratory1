using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace phians.custom_class
{
    public class check_Permissions:IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fid">页面id</param>
        /// <param name="command"> 操作命令</param>
        /// <returns></returns>
        public int check_Permission(HttpContext context, int fid, string command)
        {

            String login_user = Convert.ToString(context.Session["loginAccount"]);

            int flag = 0;
            string Permissions = "SELECT * from dbo.TB_operate_Permissions where fid='" + fid + "' and User_count='" + login_user + "'";  //判断数据库是否存在该数据
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, Permissions);
            while (dr.Read())
            {
                if (dr[command].ToString() == "True")
                {
                    flag = 1;
                }
            }
            dr.Close();
            return flag;
        }
        public int check_page( string login_user, int fid)
        {

           // String login_user = Convert.ToString(context.Session["loginAccount"]);

            string Permissions = "SELECT * from TB_jurisdiction_page where fid='" + fid + "' and User_count='" + login_user + "'";  //判断数据库是否存在该数据
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, Permissions);
            int count = 0;
            while (dr.Read())
            {
                
                    count = 1;
                
            }
            dr.Close();
            return count;
        }
    }
}