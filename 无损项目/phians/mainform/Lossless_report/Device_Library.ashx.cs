using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Configuration;
using phians.custom_class;

namespace phians.mainform.Lossless_report
{
    /// <summary>
    /// Device_Library1 的摘要说明
    /// </summary>
    public class Device_Library1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");            
            string command = context.Request.QueryString["cmd"];
            switch (command)
            {
                case "load_maintenance": load_maintenance(context); break; //加载数据表格
            }
        }

        //获取加载datagrid数据
        private void load_maintenance(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            string h_order = context.Request.Params.Get("order");
            string h_sortname = context.Request.Params.Get("sort");
            if (h_order == null)
            {
                h_order = "asc";
                h_sortname = "id";
            }
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by e." + h_sortname + " " + h_order + ")RowId,* from dbo.Device_Library )a where RowId  >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.Device_Library";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
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