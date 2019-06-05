using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace phians.mainform.System_settings
{
    /// <summary>
    /// PageShowSetting 的摘要说明
    /// </summary>
    public class PageShowSetting : IHttpHandler, IRequiresSessionState
    {
        private readonly DBHelper db = new DBHelper();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.QueryString["cmd"];//前台传的标示值    


            switch (command)
            {
                case "loadInfo": loadInfo(context); break;//加载表的所有字段信息
                case "loadPageCombobox": loadPageCombobox(context); break;//加载页面下拉
                case "EditInfo": EditInfo(context); break;//修改字段显示信息

            }
        }

        //加载表的所有字段信息
        public void loadInfo(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            string PageId = context.Request.Params.Get("PageId");//下拉框的id
            //string User_count = context.Request.Params.Get("User_count");
            //int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by FieldSort)RowId,* from TB_PageShowCustom where PageId='" + PageId + "' )a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from TB_PageShowCustom where PageId='" + PageId + "' order by FieldSort";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //加载页面下拉
        public void loadPageCombobox(HttpContext context)
        {
            //int Probe_state = (int)ProbeStateEnum.ZY;
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select * from dbo.TB_functional_module where fid=101 or fid=102 or fid=103 or fid=104 or fid=113";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }
        //修改字段显示信息
        public void EditInfo(HttpContext context)
        {
            String id = context.Request.Params.Get("id");
            String Title = context.Request.Params.Get("Title");//字段名字
            String FieldSort = context.Request.Params.Get("FieldSort");//字段显示顺序
            String hidden = context.Request.Params.Get("hidden");//字段是否显示
            String Sortable = context.Request.Params.Get("Sortable");//字段是否排序
            String Remark = context.Request.Params.Get("Remark");//备注
            string date = DateTime.Now.ToString();                    //保存时间
            string personnel = Convert.ToString(context.Session["loginAccount"]);   //用户
            String sql1 = "update TB_PageShowCustom  set Title ='" + Title + "',FieldSort ='" + FieldSort + "',hidden ='" + hidden + "',Sortable ='"
                + Sortable + "',Operator ='" + personnel + "',OperateDate ='" + date + "',Remark ='" + Remark + "' where id='" + id + "'";

            try
            {
                db.BeginTransaction();
                int i = db.ExecuteNonQueryByTrans(sql1);
                db.CommitTransacton();

                if (i > 0)
                {
                    context.Response.Write("T");

                }
                else
                {
                    context.Response.Write("F");
                }

            }
            catch (System.Exception ex)
            {
                db.RollbackTransaction();
                context.Response.Write("F");
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

                json.Append(DataTable2Json(dt));
                json.Append("]");
            }
            return json.ToString();
        }
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}