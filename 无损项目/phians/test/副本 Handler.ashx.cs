using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace phians.test
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler4 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string command = context.Request.QueryString["test"];//前台传的标示值    
            if (command == "add")
            {//调用添加方法    
                //Add(context);//我暂时只是绑定，所以把这些给注释了  
            }
            else if (command == "modify")
            {//调用修改方法    
                //Modify(context);//我暂时只是绑定，所以把这些给注释了  
            }
            else
            {//调用查询方法    
                Query(context);
            }  
        }
        private void Query(HttpContext context)
        {
            SQLHelper sqla = new SQLHelper();//这个是我自己写的一个sqlhelp类，其实sql执行语句，网上有很多这样的类  
            string strfaca = "select * from  dbo.TB_functional_module";//这里根据你自己的情况修改就行了  
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
 
            context.Response.End();  
        }
        /// <summary>  
        /// DataSet转换成Json格式   
        /// </summary>   
        /// <paramname="ds">DataSet</param>  
        ///<returns></returns>   
        public static string Dataset2Json(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                //{"total":5,"rows":[  
                json.Append("{\"total\":");
                if (total == -1)
                {
                    json.Append(dt.Rows.Count);
                }
                else
                {
                    json.Append(total);
                }
                json.Append(",\"rowss\":[");
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