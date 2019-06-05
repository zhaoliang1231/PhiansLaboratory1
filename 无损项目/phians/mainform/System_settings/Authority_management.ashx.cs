using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Collections;

namespace phians.mainform
{
    /// <summary>
    /// Permissions1 的摘要说明
    /// </summary>
    public class Permissions1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.QueryString["cmd"];//前台传的标示值    

            switch (command)
            {
                case "loadtree": loadtree(context); break;//加载实验室部门信息树
                case "load_userlist": h_load_userlist(context); break;//加载用户用户列表
                case "load_data": load_data(context); break;//加载已分配权限信息
                case "search_people": search_people(context); break;//搜索人员
                case "Permissions": Permissions(context); break;//修改权限
                //case "Detailed_Permissions": Detailed_Permissions(context); break;//修改详细权限
                //case "load_page_tree": load_page_tree(context); break;//修改详细权限（页面树）
                //case "load_operate_Permissions_data": load_operate_Permissions_data(context); break;//加载已分派的详细权限

            }
        }
        //加载实验室部门信息树
        public void loadtree(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select id,Department_name,Parent_id from dbo.TB_department";
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "id", "Department_name", "Parent_id", "0");
            string content1 = result.ToString();
            context.Response.Write(content1);
        }
        StringBuilder result = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        private void GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol]);
                            result.Append(sb.ToString());
                            sb.Clear();
                        }
                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
        }

        //加载用户用户列表
        private void h_load_userlist(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            string ids = context.Request.Params.Get("id");
            int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by dbo.TB_user_info.id)RowId,dbo.TB_user_info.* from dbo.TB_user_info,dbo.TB_user_department,dbo.TB_department where dbo.TB_user_department.User_department=dbo.TB_department.id and dbo.TB_user_department.User_count=dbo.TB_user_info.User_count and dbo.TB_user_department.User_department= '" + id1 + "')a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select dbo.TB_user_info.* from dbo.TB_user_info,dbo.TB_user_department,dbo.TB_department where dbo.TB_user_department.User_department=dbo.TB_department.id and dbo.TB_user_department.User_count=dbo.TB_user_info.User_count and dbo.TB_user_department.User_department= '" + id1 + "' ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();

        }

        //加载已分配权限信息
        public void load_data(HttpContext context)
        {
            String User_count = context.Request.Params.Get("User_count");
            SQLHelper hmylql = new SQLHelper();
            string strsql = "select dbo.TB_jurisdiction_page.fid from dbo.TB_jurisdiction_page,dbo.TB_user_info where dbo.TB_jurisdiction_page.User_count = '" + User_count + "' and dbo.TB_jurisdiction_page.User_count=dbo.TB_user_info.User_count";
            DataSet ds = hmylql.GetDataSet(strsql);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json3(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }
        public static string Dataset2Json3(DataSet ds, int total = -1)
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
                json.Append(",\"rows\":[");
                json.Append(DataTable2Json3(dt));
                json.Append("]}");
            }


            return json.ToString();
        }
        public static string DataTable2Json3(DataTable dt)
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

        //搜索
        private void search_people(HttpContext context)
        {
            string searchs = context.Request.Params.Get("search");
            string search1 = context.Request.Params.Get("search1");
            //string ids = context.Request.Params.Get("id");
            //int id1 = Convert.ToInt32(ids);
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;

            SQLHelper sqla = new SQLHelper();

            string strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_user_info where " + searchs + " LIKE '%" + search1 + "%' )a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            if (searchs == "")
            {
                strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_user_info )a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            }
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_user_info where " + searchs + " like '%" + search1 + "%' ";//按条件查询数据
            if (searchs == "")
            {
                strsql2 = "select * from dbo.TB_user_info ";//按条件查询数据 
            }
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }

        //权限设置--确定修改
        private void Permissions(HttpContext context)
        {
            String SessionCount = context.Request.Params.Get("User_count");
            String SessionName = context.Request.Params.Get("User_name");
            String Web_value = context.Request.Params.Get("Web_value"); //数据--“（页面id）;（checkbox.value）”
            string[] Web_values ;
            Web_values = Web_value.Split(';');
            string[] sqls = new string[100];
            int k = 0;
            List<string> list_fid = new List<string>(); //页面id list
            List<string> list_sql = new List<string>(); //操作页面修改
            SQLHelper hmylql = new SQLHelper();
            //string strsql = "";//按条件查询数据  

            string fid = "SELECT fid from dbo.TB_jurisdiction_page where User_count='" + SessionCount + "'";  //判断数据库是否存在该数据

            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, fid);
            while (dr.Read())
            {
                // 读取两个列值并输出到Label中    
               
                list_fid.Add(dr["fid"].ToString());
            }
            dr.Close();

            for (int i = 0; i < (Web_values.Length); i = i + 2)
            {
                //如果这一项权限没有被勾选，则删除数据库中这一项权限
                if (Web_values[i + 1] == "0")
                {
                    
                    list_sql.Add("delete from dbo.TB_jurisdiction_page where fid='" + Web_values[i] + "' and User_count='" + SessionCount + "'");
               
                }
                //如果为勾选项
                if (Web_values[i + 1] == "1")
                {
                    string value = "0";
                    //判断id 是否是新授权的页面 1为已经授权 0未授权
                    if (list_fid.Contains( Web_values[i]))
                    {
                        value = "1";
                    }
                    //判断数据库是否存在该数据，若不存在则添加进数据库
                    if (value == "0")
                    {
                        list_sql.Add( "insert into dbo.TB_jurisdiction_page (User_count,username,fid) Values ('" + SessionCount + "','" + SessionName + "'," + Web_values[i] + ")");
                       
                    }
                    //if (value == "1")
                    //{
                    //    SQLHelper sql2 = new SQLHelper();
                    //    string date = "select fid from dbo.TB_functional_module where fid='" + Web_values[i] + "'";
                    //    SqlCommand cmd1 = new SqlCommand(date, sql2.getConn());
                    //    SqlDataReader dr1 = cmd1.ExecuteReader();
                    //    while (dr1.Read())
                    //    {
                    //        sqls[i] = "update dbo.TB_jurisdiction_page set User_count='" + SessionCount + "',username='" + SessionName + "',fid=" + dr1["fid"] + " where fid='" + Web_values[i] + "'";
                    //        break;
                    //    }
                    //}
                }

            }

            //=====事务
            try
            {
                
                //l事务
                SQLHelper.ExecuteSqlTran(list_sql);
                context.Response.Write("T");

            }
            catch (Exception)
            {
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }

            //string strsql = "select * from  dbo.TB_functional_module where username=[SessionName]";//按条件查询数据  
            //DataSet ds = hmylql.GetDataSet(strsql);
            ////将Dataset转化为Datable    
            //DataTable dt = ds.Tables[0];
            //int count = dt.Rows.Count;
            //string strJson = Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            //context.Response.Write(strJson);//返回给前台页面    
            //context.Response.End();
        }
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
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{[");
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
                jsonBuilder.Append("]},");
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