using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace phians.mainform.System_settings
{
    /// <summary>
    /// System_log_new 的摘要说明
    /// </summary>
    public class System_log_new : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string Parent_id = context.Request.Params.Get("ids");
            string command = context.Request.QueryString["cmd"];
            switch (command)
            {
                case "system_log_info": system_log_info(context); break;
                case "system_log_search": system_log_search(context); break;
                case "system_log_view": system_log_view(context); break;
            }
        }
        //加载系统日志列表
        private void system_log_info(HttpContext context)
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
             //string ids = context.Request.Params.Get("id");
             //int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int first = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;

            SQLHelper sqla = new SQLHelper();
            string strfaca = "select operation_user, operation_name, operation_ip, operation_date, operation_type,operation_info from (select row_number() over (order by id desc)RowId,* from dbo.TB_operation_log )a where RowId>='" + first + "' and RowId<='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将 DataSet 转换为 Datable
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_operation_log";
            string strJson = sqla.Dataset2Json(ds, strsql2, count); //DataSet数据转化为Json数据 
            context.Response.Write(strJson); //返回给前台页面
            context.Response.End();
        }

        //搜索员工
        private void system_log_search(HttpContext context)
        {
            string search1 = context.Request.Params.Get("search3");
            string search = context.Request.Params.Get("search2");

            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int first = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;

            if (search == "operation_date")
            {
                search = " convert(varchar(10)," + search + ",120) ";
            }
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select operation_user, operation_name, operation_ip, operation_date, operation_type, operation_info from (select row_number()over(order by id)RowId,* from dbo.TB_operation_log where " + search + " LIKE '%" + search1 + "%')a where RowId >= '" + first + "' and RowId <='" + newrow + "' ";
            if (search == "")
            {
                strfaca = "select operation_user, operation_name, operation_ip, operation_date, operation_type, operation_info from (select row_number()over(order by id)RowId, * from dbo.TB_operation_log)a where RowId >= '" + first + "' and RowId <='" + newrow + "' ";
            }
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable   
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_operation_log where " + search + " LIKE '%" + search1 + "%'";
            if (search == "")
            {
                strsql2 = "select * from dbo.TB_operation_log";
            }
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据   
            context.Response.Write(strJson);//返回给前台页面
            context.Response.End();
        }

        //查看详细日志内容
        private void system_log_view(HttpContext context)
        {

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