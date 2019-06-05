using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace phians.test
{
    /// <summary>
    /// test 的摘要说明
    /// </summary>
    public class test : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.Params.Get("cmd");//前台传的标示值    

            if (command == "test")
            {//调用修改方法    
                //Modify(context);//我暂时只是绑定，所以把这些给注释了  
                test0(context);

            }

            if (command == "test2")
            {//调用修改方法    
                //Modify(context);//我暂时只是绑定，所以把这些给注释了  
                test2(context);

            }
              if (command == "test22")
            {//调用修改方法    
                //Modify(context);//我暂时只是绑定，所以把这些给注释了  
                test22(context);

            }
            
            if (command == "test3")
            {//调用修改方法    
                //Modify(context);//我暂时只是绑定，所以把这些给注释了  
                context.Response.Write("T");
                context.Response.End();
            }
            else
            {

                context.Response.Write("F");
                context.Response.End();
            }
        }


        public DataSet GetDataSet(string strfaca)
        {

            SQLHelper sqla = new SQLHelper();

            using (OleDbConnection conn = sqla.getConn2())
            {

                using (OleDbDataAdapter xx = new OleDbDataAdapter(strfaca, conn))
                {
                    DataSet ds = new DataSet();
                    xx.Fill(ds);
                    return ds;

                }
            }



        }
        public void test0(HttpContext context)
        {



            string sql = "select WORKNAME,LJMC from LOCATIONWORKPLANVIEW where TEAM='核岛探伤' or  TEAM='常规岛探伤'";
            string sql2 = "select WORKNAME,LJMC from LOCATIONWORKPLANVIEW ";
            try
            {
                DataSet ds = GetDataSet(sql);
                DataTable dt = ds.Tables[0];
                int count = dt.Rows.Count;
                string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
                context.Response.Write(count.ToString());//返回给前台页面    
            }
            catch (Exception e)
            {
                context.Response.Write(e);

            }
            finally
            {

                context.Response.End();


            }









        }
        public void test22(HttpContext context)
        {



            string sql = "select WORKNAME,LJMC from LOCATIONWORKPLANVIEW ";
         
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
            finally
            {

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
    

        public void test2(HttpContext context)
        {


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
            finally
            {
                context.Response.End();

            }


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