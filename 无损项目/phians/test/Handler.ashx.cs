using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using phians.custom_class;
using System.Configuration;
using System.Data.OleDb;
using System.Web.SessionState;

//using System.Data.OracleClient;
namespace phians.test
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string command = context.Request.Params.Get("cmd");//前台传的标示值    
     
           if (command == "test")
            {//调用修改方法    
                //Modify(context);//我暂时只是绑定，所以把这些给注释了  
                test(context);

            }

           if (command == "test2")
           {//调用修改方法    
               //Modify(context);//我暂时只是绑定，所以把这些给注释了  
               test2(context);

           }
           if (command == "testconn")
           {//调用修改方法    
               //Modify(context);//我暂时只是绑定，所以把这些给注释了  
               testconn(context);

           }
           if (command == "test3")
           {//调用修改方法    

               //select WORKNAME,LJMC from LOCATIONWORKPLANVIEW where TEAM='核岛探伤' or  TEAM='常规岛探伤'


               //Modify(context);//我暂时只是绑定，所以把这些给注释了  
               context.Response.Write("T");
               context.Response.End();
           }
           else {

               context.Response.Write("F");
               context.Response.End();
           }
        }
        public DataSet GetDataSet(string strfaca)
        {
            

            SQLHelper sqla = new SQLHelper();        
          
                using ( OleDbConnection conn = sqla.getConn2())
                {

                    using (OleDbDataAdapter xx = new OleDbDataAdapter(strfaca, conn))
                    {
                        DataSet ds = new DataSet();
                        xx.Fill(ds);
                        return ds;

                    }
                }

           

        }

        public void testconn(HttpContext context)
        {

            //string connString = "User ID=dzmes0214;Password=dzmes;Data Source=MESDB;";
            //OracleConnection conn = new OracleConnection(connString);
            //try
            //{
            //    conn.Open();
            //    context.Response.Write("T");
            //}
            //catch (Exception ex)
            //{
            //    context.Response.Write(ex);
            //}
            //finally
            //{
            //    conn.Close();
            //}
        
        
        }
        public void test(HttpContext context) {


           
            string sql = "select WORKNAME,LJMC from LOCATIONWORKPLANVIEW where TEAM='核岛探伤' or  TEAM='常规岛探伤'";
            try
            {
                DataSet ds = GetDataSet(sql);
                DataTable dt = ds.Tables[0];
                int count = dt.Rows.Count;
                string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
                context.Response.Write(strJson);//返回给前台页面    
            }
            catch (Exception e)
            {
                context.Response.Write(e);

            }
            finally {

                context.Response.End();
            
            
            }
    
   



   
     
           
        
        }


        public void test2 ( HttpContext context) {

            
          SQLHelper sqla = new SQLHelper();
          try
          {

              using (OleDbConnection conn = sqla.getConn2())
              {
                  OleDbCommand comm = new OleDbCommand("select WORKNAME,LJMC from LOCATIONWORKPLANVIEW where TEAM='核岛探伤' or  TEAM='常规岛探伤'", conn);
                  OleDbDataReader dr = comm.ExecuteReader();
                  while (dr.Read())
                  {
                      context.Response.Write("1");
                  }

                  dr.Close();

              }



          }
          catch (Exception e)
          {

              context.Response.Write(e);
          }
          finally {
              context.Response.End();
          
          }
  
        
        }


        public string Dataset2Json1(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {

                json.Append("[");

                json.Append(DataTable2Json2(dt));
                json.Append("]");
            }
            return json.ToString();
        }
        public static string DataTable2Json2(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    string text_ = dt.Rows[i][j].ToString().Trim().Replace("\\", "\\\\");
                    text_ = text_.Replace("\r", "");
                    text_ = text_.Replace("\n", "");
                    //\r\n
                    jsonBuilder.Append(text_);
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }

            return jsonBuilder.ToString();
        }
    
        public static string Dataset2Json(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"total\":");
                if (total == -1)
                {
                    json.Append(dt.Rows.Count);
                }
                else
                {
                    json.Append(total);
                }
                json.Append(",\"rows\":[");
                json.Append(DataTable2Json(dt));
                json.Append("]}");
            }
            return json.ToString();
        }



        /// <summary>   
        /// dataTable转换成Json格式   
        /// </summary>   
        /// <paramname="dt"></param>   
        ///<returns></returns>   
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }

            return jsonBuilder.ToString();
        }  
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}