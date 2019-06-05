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
    /// Dictionary_management 的摘要说明
    /// </summary>
    public class Dictionary_management : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.QueryString["cmd"];
            switch (command)
            {
                case "loadtree": loadtree(context); break;//字典树
                case "treeedit": treeedit(context); break;//编辑项目
                case "treedel": treedel(context); break;//删除项目
                case "treeadd": treeadd(context); break;//添加同级项目
                case "treeadd_next": treeadd_next(context); break;//添加下级项目
                case "load_info": load_info(context); break;//加载字典详细内容
                case "edit_context": edit_context(context); break;//加载字典详细内容
                case "insert_context": insert_context(context); break;//添加字典详细内容
                case "del_context": del_context(context); break;//删除字典详细内容
            }
        }


        public void del_context(HttpContext context)
        {
            string id = context.Request.Params.Get("id");
           
            string delete_sql_id = str(id);
            string delete_sql = "delete from TB_dictionary_managing_context "+delete_sql_id;

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
        string str(string delete_ids)
        {
            string[] ids = delete_ids.Split(';');    //分割字符串 每单遇到分号分割一次
            string id = "";
            for (int i = 0; i < ids.Length; i++)
            {
                if (i == 0)
                {
                    id = "where id = " + ids[i];
                }
                if (i > 0)
                {
                    id = id + " or id = " + ids[i];
                }
            }
            return id;
        }
        public void insert_context(HttpContext context) {

            string Project_name = context.Request.Params.Get("Project_name");
            int group_id = Convert.ToInt32(context.Request.Params.Get("gorop_id"));
            string Project_value = context.Request.Params.Get("Project_value");
            string Sort_num = context.Request.Params.Get("Sort_num");
            string remarks = context.Request.Params.Get("remarks");
            string update_sql = "INSERT INTO TB_dictionary_managing_context (Project_value,Project_name,group_id,Sort_num,remarks) values('" + Project_value + "','" + Project_name + "','" + group_id + "','" + Sort_num + "','" + remarks + "') ";

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
        public void edit_context(HttpContext context) {

            string id = context.Request.Params.Get("id");
            string Project_name = context.Request.Params.Get("Project_name");
            string Project_value = context.Request.Params.Get("Project_value");
            string  Sort_num =context.Request.Params.Get("Sort_num");
            string remarks = context.Request.Params.Get("remarks");
            string update_sql = "update TB_dictionary_managing_context set Project_name='" + Project_name + "',Project_value='" + Project_value + "',Sort_num='" + Sort_num + "',remarks='" + remarks + "' where id='" + id + "'";

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
        //加载字典详细内容
        public void load_info(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            string project_id = context.Request.Params.Get("project_id");
            //string User_count = context.Request.Params.Get("User_count");
            //int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by Sort_num)RowId,* from TB_dictionary_managing_context where group_id='" + project_id + "')a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from TB_dictionary_managing_context where group_id='" + project_id + "' ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //添加同级项目
        public void treeadd(HttpContext context)
        {
            String command = context.Request.QueryString["cmd"];
            String Parent_id = context.Request.Params.Get("Parent_id");
            String Project_name = context.Request.Params.Get("Project_name");
            String insert_sql = "INSERT INTO TB_dictionary_managing_project (Parent_id,Project_name) values('" + Parent_id + "','" + Project_name + "') ";           
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

        //添加下级项目
        public void treeadd_next(HttpContext context)
        {

            String Parent_id = context.Request.Params.Get("ids");
            String Project_name = context.Request.Params.Get("Project_name");

            String insert_sql = "INSERT INTO TB_dictionary_managing_project (Parent_id,Project_name) values('" + Parent_id + "','" + Project_name + "') ";
          
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
        //删除项目
        public void treedel(HttpContext context)
        {
            String id = context.Request.Params.Get("ids");
            String delete_sql = "delete from TB_dictionary_managing_project  where id='" + id + "'";
          
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
        //加载实验室资质信息树
        public void loadtree(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select id,Project_name,Parent_id from TB_dictionary_managing_project";
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "id", "Project_name", "Parent_id", "0");
            string content1 = result.ToString();
            context.Response.Write(content1);
        }
        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
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

        public void treeedit(HttpContext context)
        {
            String id = context.Request.Params.Get("ids");
            String Project_name = context.Request.Params.Get("Project_name");         
            String update_sql = "update TB_dictionary_managing_project set Project_name='" + Project_name + "' where id='" + id + "'";
           
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}