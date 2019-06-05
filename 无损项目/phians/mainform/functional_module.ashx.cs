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
    /// functional_module1 的摘要说明
    /// </summary>
    public class functional_module1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.QueryString["cmd"];//前台传的标示值    

            switch (command)
            {
                case "load_page_tree": load_page_tree(context); break;//页面信息树
                case "treedel": treedel(context); break;//删除页面
                case "load_page_info": load_page_info(context); break;//编辑页面信息加载
                case "treeedit": treeedit(context); break;//编辑页面
                case "treeadd": treeadd(context); break;//添加同级页面
                case "treeadd_next": treeadd_next(context); break;//添加下级页面
            }
        }

        //页面信息树
        public void load_page_tree(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select fid,m_name,Parent_id from dbo.TB_functional_module ORDER BY group_id,sort_num";
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "fid", "m_name", "Parent_id", "0");
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

        //删除页面
        public void treedel(HttpContext context)
        {
            String fid = context.Request.Params.Get("fid");
            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();

            string select_sql = "select * from dbo.TB_functional_module where fid='" + fid + "'";
            string m_number = "";
            string group_id = "";
            SqlDataReader dr1 = SQLHelper.ExecuteReader(CommandType.Text, select_sql);
            while (dr1.Read())
            {
                // 读取两个列值并输出到Label中    
                if (dr1["m_number"].ToString() != "")
                {
                    m_number = dr1["m_number"].ToString();
                    group_id = dr1["group_id"].ToString();
                }
            }
            dr1.Close();

            string delete_sql = "";
            if (m_number == "0")
            {
                delete_sql = "delete from dbo.TB_functional_module where group_id='" + group_id + "'";
            }
            else {
                delete_sql = "delete from dbo.TB_functional_module where fid='" + fid + "'";
            }
            //SqlCommand cmd1 = new SqlCommand(sql1, con);
            //int count = cmd1.ExecuteNonQuery();
            //con.Close();
            //if (count >= 1)
            //{
            //    context.Response.Write("T");
            //    context.Response.End();
            //}
            //else
            //{
            //    context.Response.Write("F");
            //    context.Response.End();
            //}
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(delete_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, context);
            }
            catch (Exception)
            {
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }
        }
        //编辑页面信息加载
        public void load_page_info(HttpContext context)
        {
            String fid = context.Request.Params.Get("fid");
            string data = "";

            string select_m_number = "select * from dbo.TB_functional_module where fid='" + fid + "'";
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, select_m_number);
            while (dr.Read())
            {
                data = dr["m_name"].ToString() + "，" + dr["i_iconCls"].ToString() + "，" + dr["u_url"].ToString() + "，" + dr["remarks"].ToString();
                break;
            }
            dr.Close();
            context.Response.Write(data);
            context.Response.End();
        }
        //编辑页面
        public void treeedit(HttpContext context)
        {
            String fid = context.Request.Params.Get("fid");
            String m_name = context.Request.Params.Get("m_name");
            String u_url = context.Request.Params.Get("u_url");
            String remarks = context.Request.Params.Get("remarks");
            String i_iconCls = context.Request.Params.Get("i_iconCls");

            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String update_sql = "update dbo.TB_functional_module set m_name='" + m_name + "', u_url='" + u_url + "', remarks='" + remarks + "', i_iconCls='"
                + i_iconCls + "' where fid='" + fid + "'";
            //SqlCommand cmd1 = new SqlCommand(sql1, con);
            //int count = cmd1.ExecuteNonQuery();
            //con.Close();
            //if (count >= 1)
            //{
            //    context.Response.Write("T");
            //    context.Response.End();
            //}
            //else
            //{
            //    context.Response.Write("F");
            //    context.Response.End();
            //}
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(update_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, context);
            }
            catch (Exception)
            {
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }
        }
        //添加同级项目
        public void treeadd(HttpContext context)
        {
            String fid = context.Request.Params.Get("fid");
            String m_number = "";
            string Parent_id_1 = "";
            string group_id_1 = "";
            string select_m_number = "select * from dbo.TB_functional_module where fid='" + fid + "'";
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, select_m_number);
            while (dr.Read())
            {
                m_number = dr["m_number"].ToString();
                Parent_id_1 = dr["Parent_id"].ToString();
                group_id_1 = dr["group_id"].ToString();
                break;
            }
            dr.Close();

            if (m_number == "0")
            {
                int fid1 = 0;
                string select_fid = "select fid from dbo.TB_functional_module ORDER BY fid DESC";
                SqlDataReader dr1 = SQLHelper.ExecuteReader(CommandType.Text, select_fid);
                while (dr1.Read())
                {
                    fid1 = Convert.ToInt32(dr1["fid"].ToString()) + 1; break;
                }
                dr1.Close();

                int group_id = 0;
                string select_group_id = "select group_id from dbo.TB_functional_module ORDER BY group_id DESC";
                SqlDataReader dr2 = SQLHelper.ExecuteReader(CommandType.Text, select_group_id);
                while (dr2.Read())
                {
                    group_id = Convert.ToInt32(dr2["group_id"].ToString()) + 1; break;
                }
                dr2.Close();

                int sort_num = 0;
                string select_sort_num = "SELECT * FROM dbo.TB_functional_module WHERE m_number='0' ORDER BY sort_num DESC";
                SqlDataReader dr3 = SQLHelper.ExecuteReader(CommandType.Text, select_sort_num);
                while (dr3.Read())
                {
                    sort_num = Convert.ToInt32(dr3["sort_num"].ToString()) + 1; break;
                }
                dr3.Close();

                String m_name = context.Request.Params.Get("m_name");
                String u_url = context.Request.Params.Get("u_url");
                String remarks = context.Request.Params.Get("remarks");
                String i_iconCls = context.Request.Params.Get("i_iconCls");

                //SQLHelper sql2 = new SQLHelper();
                //SqlConnection con = sql2.getConn();
                String insert_sql = "INSERT INTO dbo.TB_functional_module (fid, Parent_id, m_number, s_number, s_1_number, i_iconCls, m_name, group_id, u_url,"
                    + " sort_num, remarks) values('" + fid1 + "','62','0',NULL,NULL,'" + i_iconCls + "','" + m_name + "','" + group_id + "','"
                    + u_url + "','" + sort_num + "','" + remarks + "') ";
                //SqlCommand cmd1 = new SqlCommand(sql1, con);
                //int count = cmd1.ExecuteNonQuery();
                //con.Close();
                //if (count >= 1)
                //{
                //    context.Response.Write("T");
                //    context.Response.End();
                //}
                //else
                //{
                //    context.Response.Write("F");
                //    context.Response.End();
                //}
                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    SQLStringList.Add(insert_sql);
                    //事务
                    SQLHelper.ExecuteSqlTran(SQLStringList, context);
                }
                catch (Exception)
                {
                    context.Response.Write("F");
                }
                finally
                {
                    context.Response.End();
                }
            }
            else
            {
                int fid1 = 0;
                string select_fid = "select fid from dbo.TB_functional_module ORDER BY fid DESC";
                SqlDataReader dr1 = SQLHelper.ExecuteReader(CommandType.Text, select_fid);
                while (dr1.Read())
                {
                    fid1 = Convert.ToInt32(dr1["fid"].ToString()) + 1; break;
                }
                dr1.Close();

                int sort_num = 0;
                string select_sort_num = "SELECT * FROM dbo.TB_functional_module WHERE group_id='" + group_id_1 + "' ORDER BY sort_num DESC";
                SqlDataReader dr2 = SQLHelper.ExecuteReader(CommandType.Text, select_sort_num);
                while (dr2.Read())
                {
                    sort_num = Convert.ToInt32(dr2["sort_num"].ToString()) + 1; break;
                }
                dr2.Close();

                String m_name = context.Request.Params.Get("m_name");
                String u_url = context.Request.Params.Get("u_url");
                String remarks = context.Request.Params.Get("remarks");
                String i_iconCls = context.Request.Params.Get("i_iconCls");

                //SQLHelper sql2 = new SQLHelper();
                //SqlConnection con = sql2.getConn();
                String insert_sql2 = "INSERT INTO dbo.TB_functional_module (fid, Parent_id, m_number, s_number, s_1_number, i_iconCls, m_name, group_id, u_url,"
                    + " sort_num, remarks) values('" + fid1 + "','" + Parent_id_1 + "',NULL,'1',NULL,'" + i_iconCls + "','" + m_name + "','" + group_id_1 + "','"
                    + u_url + "','" + sort_num + "','" + remarks + "') ";
                //SqlCommand cmd1 = new SqlCommand(sql1, con);
                //int count = cmd1.ExecuteNonQuery();
                //con.Close();
                //if (count >= 1)
                //{
                //    context.Response.Write("T");
                //    context.Response.End();
                //}
                //else
                //{
                //    context.Response.Write("F");
                //    context.Response.End();
                //}
                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    SQLStringList.Add(insert_sql2);
                    //事务
                    SQLHelper.ExecuteSqlTran(SQLStringList, context);
                }
                catch (Exception)
                {
                    context.Response.Write("F");
                }
                finally
                {
                    context.Response.End();
                }
            }
        }
        //添加下级页面
        public void treeadd_next(HttpContext context)
        {
            String m_name = context.Request.Params.Get("m_name");
            String u_url = context.Request.Params.Get("u_url");
            String remarks = context.Request.Params.Get("remarks");
            String i_iconCls = context.Request.Params.Get("i_iconCls");
            String fid = context.Request.Params.Get("fid");

            string group_id_1 = "";
            string select_m_number = "select * from dbo.TB_functional_module where fid='" + fid + "'";
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, select_m_number);
            while (dr.Read())
            {
                group_id_1 = dr["group_id"].ToString();
                break;
            }
            dr.Close();

            int fid1 = 0;
            string select_fid = "select fid from dbo.TB_functional_module ORDER BY fid DESC";
            SqlDataReader dr1 = SQLHelper.ExecuteReader(CommandType.Text, select_fid);
            while (dr1.Read())
            {
                fid1 = Convert.ToInt32(dr1["fid"].ToString()) + 1; break;
            }
            dr1.Close();

            int sort_num = 0;
            string select_sort_num = "SELECT * FROM dbo.TB_functional_module WHERE group_id='" + group_id_1 + "' ORDER BY sort_num DESC";
            SqlDataReader dr2 = SQLHelper.ExecuteReader(CommandType.Text, select_sort_num);
            while (dr2.Read())
            {
                sort_num = Convert.ToInt32(dr2["sort_num"].ToString()) + 1; break;
            }
            dr2.Close();

            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String insert_sql = "INSERT INTO dbo.TB_functional_module (fid, Parent_id, m_number, s_number, s_1_number, i_iconCls, m_name, group_id, u_url,"
                + " sort_num, remarks) values('" + fid1 + "','" + fid + "',NULL,'1',NULL,'" + i_iconCls + "','" + m_name + "','" + group_id_1 + "','"
                + u_url + "','" + sort_num + "','" + remarks + "') ";
            //SqlCommand cmd1 = new SqlCommand(sql1, con);
            //int count = cmd1.ExecuteNonQuery();
            //con.Close();
            //if (count >= 1)
            //{
            //    context.Response.Write("T");
            //    context.Response.End();
            //}
            //else
            //{
            //    context.Response.Write("F");
            //    context.Response.End();
            //}
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(insert_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, context);
            }
            catch (Exception)
            {
                context.Response.Write("F");
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