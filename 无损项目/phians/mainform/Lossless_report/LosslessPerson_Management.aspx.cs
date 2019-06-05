using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace phians.phians_js.Lossless_report
{
    public partial class LosslessPerson_Management : System.Web.UI.Page
    {
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            string command = Request.QueryString["cmd"];
            string command2 = Request.Params.Get("cmd");
            switch (command)
            {
                ///********************************************************************人员管理
                case "loadtree": loadtree(); break;//加载实验室组信息树
                case "treeadd": treeadd(); break; //添加树同级项目
                case "edit": edit(); break; //修改树的名称

                case "treeadd_next": treeadd_next(); break; //添加树同级项目
                case "treedel": treedel(); break; //删除树项目
                case "load_userlist": h_load_userlist(); break; //加载用户列表
                case "load_userinfo": load_userinfo(); break; //加载用户信息
                case "technicians_save": h_useredit(); break; //用户基本信息保存
                case "technicians_saveadd": h_useradd(); break; //添加用户信息
                case "load_department": load_department(); break; //显示用户已在部门
                case "tree_add_project": tree_add_project(); break; //将员工指派到其它组
                case "tree_remove_project": tree_remove_project(); break; //将员工从组移除
                case "technicians_del": h_userdel(); break; //将员工从部门移除
                case "technicians_autograph": h_autograph_save(); break; //签名上传
                case "department_people_search": department_people_search(); break; //搜索员工
                case "no_department_personnel": no_department_personnel(); break; //查看未分配组人员
                case "enable_personnel": enable_personnel(); break; //启用人员
                case "disable_personnel": disable_personnel(); break; //停用人员
                case "Reset_password": Reset_password(); break; //重置密码
                case "getDepart": GetDepart(); break; //重置密码
                case "getDepartUser": GetDepartUser(); break;
                case "show_department": show_department(); break;//显示部门
                case "getEquipment_code": GetEquipment_code(); break;


                ///###################################################权限管理

                //case "loadtree1": loadtree1(); break;//加载实验室部门信息树——无损
                //case "load_userlist1": h_load_userlist1(); break;//加载用户用户列表——无损
                case "load_data": load_data(); break;//加载已分配权限信息
                case "search_people": search_people(); break;//搜索人员
                case "Permissions": Permissions(); break;//修改权限

                ///###################################################字典管理
                case "load_info": load_info(); break;//加载字典详细内容
                case "edit_context": edit_context(); break;//加载字典详细内容
                case "insert_context": insert_context(); break;//添加字典详细内容
                case "del_context": del_context(); break;//删除字典详细内容
            }
        }

        //*****************************************************************************************
        ///*****************************************************************************************
        //**********************************    字典管理      *************************************
        ///*****************************************************************************************
        //*****************************************************************************************

        /// <summary>
        /// 删除字典详细内容
        /// </summary>
        /// <param name=""></param>
        public void del_context()
        {
            string id = Request.Params.Get("id");

            string delete_sql_id = str(id);
            string delete_sql = "delete from TB_dictionary_managing_context " + delete_sql_id;

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(delete_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");
            }
            finally
            {
                Response.End();
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
        public void insert_context()
        {

            string Project_name = Request.Params.Get("Project_name");
            //int group_id = Convert.ToInt32(Request.Params.Get("gorop_id"));
            string Project_value = Request.Params.Get("Project_value");
            string Sort_num = Request.Params.Get("Sort_num");
            string remarks = Request.Params.Get("remarks");
            string update_sql = "INSERT INTO TB_dictionary_managing_context (Project_value,Project_name,group_id,Sort_num,remarks) values('" + Project_value + "','" + Project_name + "',17,'" + Sort_num + "','" + remarks + "') ";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(update_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");
            }
            finally
            {
                Response.End();
            }

        }
        public void edit_context()
        {

            string id = Request.Params.Get("id");
            string Project_name = Request.Params.Get("Project_name");
            string Project_value = Request.Params.Get("Project_value");
            string Sort_num = Request.Params.Get("Sort_num");
            string remarks = Request.Params.Get("remarks");
            string update_sql = "update TB_dictionary_managing_context set Project_name='" + Project_name + "',Project_value='" + Project_value + "',Sort_num='" + Sort_num + "',remarks='" + remarks + "' where id='" + id + "'";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(update_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");
            }
            finally
            {
                Response.End();
            }
        }
        //加载字典详细内容
        public void load_info()
        {
            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
            //string project_id = Request.Params.Get("project_id");
            //string User_count = Request.Params.Get("User_count");
            //int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by Sort_num)RowId,* from TB_dictionary_managing_context where group_id=17)a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from TB_dictionary_managing_context where group_id=17 ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();
        }

        //*****************************************************************************************
        ///*****************************************************************************************
        //**********************************    人员管理      *************************************
        ///*****************************************************************************************
        //*****************************************************************************************

        public void edit()
        {
            String id = Request.Params.Get("ids");
            String Department_name = Request.Params.Get("Department_name");
            String sql1 = "update TB_department  set Department_name ='" + Department_name + "'    where id='" + id + "' and  flag_ =0";

            try
            {
                db.BeginTransaction();
                int i = db.ExecuteNonQueryByTrans(sql1);
                db.CommitTransacton();

                if (i > 0)
                {
                    Response.Write("T");

                }
                else
                {
                    Response.Write("内置组不允许修改");
                }

            }
            catch (System.Exception ex)
            {
                db.RollbackTransaction();
                Response.Write("F");
            }
            finally
            {
                Response.End();
            }

        }
        //搜索地址
        public void show_department()
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select Project_name,Project_value from TB_dictionary_managing_context where group_id=6";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = sql2.Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面    
            Response.End();
        }
        //加载实验室部门信息树
        public void loadtree()
        {
            SQLHelper sql2 = new SQLHelper();
            // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select id,Department_name,Parent_id from dbo.TB_department where id=45 or id=46 or id=10 or id=47  or id=48";//只显示无损的组
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "id", "Department_name", "Parent_id", "0");
            string content1 = result.ToString();
            Response.Write(content1);
            Response.End();
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
        //添加同级项目
        public void treeadd()
        {

            String command = Request.QueryString["cmd"];
            String Parent_id = Request.Params.Get("Parent_id");
            String Department_name = Request.Params.Get("Department_name");
            //String login_user = Convert.ToString(Session["loginAccount"]);          
            String sql1 = "INSERT INTO dbo.TB_department (Parent_id,Department_name) values('" + Parent_id + "','" + Department_name + "') ";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);
                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }

        }
        //添加下级树项目
        public void treeadd_next()
        {

            String Parent_id = Request.Params.Get("ids");
            String Department_name = Request.Params.Get("Department_name");
            //String login_user = Convert.ToString(Session["loginAccount"]);

            String sql1 = "INSERT INTO dbo.TB_department (Parent_id,Department_name) values('" + Parent_id + "','" + Department_name + "') ";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }

        }
        //删除树项目
        public void treedel()
        {
            String id = Request.Params.Get("ids");

            String sql1 = "delete from dbo.TB_department  where id='" + id + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }

        }
        //加载用户用户列表
        private void h_load_userlist()
        {
            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
            string ids = Request.Params.Get("id");
            //部门id
            int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by U.id)RowId,U.Address,U.Email,U.Fax,U.MSN,U.Phone,U.Postcode,U.QQ,U.Signature,U.Tel,U.User_count,U.User_count_state,U.User_duties,U.User_job,U.User_name,U.User_sex,U.department,U.department_code,U.id  from dbo.TB_user_info as U,"
                + "dbo.TB_user_department,dbo.TB_department where dbo.TB_user_department.User_department=dbo.TB_department.id and"
                + " dbo.TB_user_department.User_count=U.User_count and dbo.TB_user_department.User_department= '"
                + id1 + "')a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_user_info as U,dbo.TB_user_department as D,dbo.TB_department as PD where"
                + " D.User_department=PD.id and D.User_count=U.User_count and"
                + " D.User_department= '" + id1 + "'";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();

        }
        //搜索员工
        private void department_people_search()
        {
            string search = Request.Params.Get("search");
            string search1 = Request.Params.Get("search1");
            //string ids = Request.Params.Get("id");
            ///int id1 = Convert.ToInt32(ids);

            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_user_info where " + search + " LIKE '%"
                + search1 + "%' )a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            if (search1 == "" || search == "")
            {
                strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_user_info )a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            }
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from  dbo.TB_user_info where " + search + " LIKE '%" + search1 + "%' ";
            if (search1 == "" || search == "")
            {
                strsql2 = "select * from  dbo.TB_user_info";
            }
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();
        }
        //查看未分配组人员
        private void no_department_personnel()
        {
            string search = Request.Params.Get("search");
            string search1 = Request.Params.Get("search1");
            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_user_info WHERE User_count NOT IN ("
                + "SELECT User_count FROM dbo.TB_user_department"
                + "))a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "SELECT * FROM dbo.TB_user_info WHERE User_count NOT IN (SELECT User_count FROM dbo.TB_user_department) ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();

        }
        //启用
        private void enable_personnel()
        {
            String id = Request.Params.Get("id");
            string User_count_state = "正在使用中";

            String sql1 = "update dbo.TB_user_info set User_count_state='" + User_count_state + "' where id='" + id + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                //被操作账户
                String op_User_count = Request.Params.Get("User_count");
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "启用账户", "启用" + op_User_count);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }
        }
        //停用
        private void disable_personnel()
        {
            String id = Request.Params.Get("id");
            //被操作
            String op_User_count = Request.Params.Get("User_count");
            string User_count_state = "停用";

            String sql1 = "update dbo.TB_user_info set User_count_state='" + User_count_state + "' where id='" + id + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "停用账户", "停用" + op_User_count);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }
        }
        //重置密码
        private void Reset_password()
        {
            String id = Request.Params.Get("id");
            string User_pwd = "123456";
            User_pwd = EncryptDES(User_pwd.Trim(), "hxw_2016");
            //被操作用户
            String op_User_count = Request.Params.Get("User_count");
            String sql1 = "update dbo.TB_user_info set User_pwd='" + User_pwd + "' where id='" + id + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "重置密码", "重置" + op_User_count + "密码");
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }
        }

        //加载用户详细信息
        public void load_userinfo()
        {
            String ids = Request.Params.Get("ids");
            SQLHelper sql2 = new SQLHelper();
            // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select * from dbo.TB_user_info where id ='" + ids + "'";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面    
            Response.End();

        }
        public string Dataset2Json(DataSet ds, int total = -1)
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

                    json.Append(dt.Rows.Count);
                }
                json.Append(",\"rows\":[");
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
        //员工信息新增
        private void h_useradd()
        {
            String User_count = Request.Params.Get("User_count");
            String User_name = Request.Params.Get("User_name");
            String User_job = Request.Params.Get("User_job");
            String User_duties = Request.Params.Get("User_duties");
            String Tel = Request.Params.Get("Tel");
            String Phone = Request.Params.Get("Phone");
            String Fax = Request.Params.Get("Fax");
            String Email = Request.Params.Get("Email");
            String QQ = Request.Params.Get("QQ");
            String MSN = Request.Params.Get("MSN");
            String Address = Request.Params.Get("Address");
            String Postcode = Request.Params.Get("Postcode");
            String Remarks = Request.Params.Get("Remarks");

            string[] User_counts = User_count.Split(',');
            string[] User_names = User_name.Split(',');

            if (User_counts.Length == User_names.Length) { } else {
                Response.Write("请保持用户账号个用户名数量一致");
                Response.End();
            }

            //组id
            int User_department = Convert.ToInt32(Request.Params.Get("User_department"));
            //组名称
            String Department_name = Request.Params.Get("Department_name");
            //部门id
            String department_code = Request.Params.Get("department_code");
            //部门名称
            String department = Request.Params.Get("department");

            string User_count_state = "正在使用中";
            string User_pwd = "123456";
            User_pwd = EncryptDES(User_pwd, "hxw_2016");

            String insert_sql = "";
            String insert_sql2 = "";
            try
            {
                db.BeginTransaction();
                for (int i = 0; i < User_counts.Length; i++)
                {
                    SqlParameter[] para = 
                {
                    new SqlParameter("@User_count"+i,User_counts[i]),
                    new SqlParameter("@User_name"+i,User_names[i]),
                    new SqlParameter("@User_job"+i,User_job),
                    new SqlParameter("@User_duties"+i,User_duties),                   
                    new SqlParameter("@Tel"+i,Tel),
                    new SqlParameter("@Phone"+i,Phone),
                    new SqlParameter("@Fax"+i,Fax),
                    new SqlParameter("@Email"+i,Email),
                    new SqlParameter("@QQ"+i,QQ),
                    new SqlParameter("@MSN"+i,MSN),
                    new SqlParameter("@Address"+i,Address),
                    new SqlParameter("@Postcode"+i,Postcode),
                    new SqlParameter("@Remarks"+i,Remarks),
                    new SqlParameter("@User_count_state"+i,User_count_state),
                    new SqlParameter("@User_pwd"+i,User_pwd),
                    new SqlParameter("@department_code"+i,department_code),
                    new SqlParameter("@department"+i,department)
                };

                    SqlParameter[] para2 = 
                {
                    new SqlParameter("@User_count",User_counts[i]),
                    new SqlParameter("@User_department",User_department),
                    new SqlParameter("@Department_name",Department_name),
                   
                };
                  

                    insert_sql = "INSERT INTO dbo.TB_user_info (User_count,User_name,User_job,User_duties,Tel,Phone,Fax,Email,QQ,MSN,Address,Postcode,Remarks,"
                    + "User_pwd,User_count_state,department_code,department) values(@User_count" + i + ",@User_name" + i + ",@User_job" + i + ",@User_duties" + i + ","
                    + "@Tel" + i + ",@Phone" + i + ",@Fax" + i + ",@Email" + i + ",@QQ" + i + ",@MSN" + i + ",@Address" + i + ",@Postcode" + i + ",@Remarks" + i + ","
                    + "@User_pwd" + i + ",@User_count_state" + i + ",@department_code" + i + ",@department" + i + ") ";
                    db.ExecuteNonQueryByTrans(insert_sql, para);

                    insert_sql2 = "INSERT INTO dbo.TB_user_department (User_count,User_department,Department_name) values(@User_count,@User_department,@Department_name)";
                    db.ExecuteNonQueryByTrans(insert_sql2, para2);
                }
                db.CommitTransacton();
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "新增账户", "新增" + User_count);
                Response.Write("T");
            }
            catch
            {
                db.RollbackTransaction();
                Response.Write("F");
            }
            finally { Response.End(); }



            //try
            //{
            //    //SQL语句
            //    List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
            //    List<string> SQLStringList = new List<string>();

            //    SQLStringList.Add(insert_sql);
            //    SQLStringList2.Add(para);
            //    SQLStringList.Add(insert_sql2);
            //    SQLStringList2.Add(para2);
            //    //SQLStringList.Add(sql1);
            //    //SQLStringList.Add(sql2);
            //    //sql事务
            //    SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
            //    //被操作账户
            //    // String op_User_count = Request.Params.Get("User_count");
            //    string loginAccount = Convert.ToString(Session["loginAccount"]);
            //    string login_username = Convert.ToString(Session["login_username"]);
            //    Operation_log.operation_log_(loginAccount, login_username, "新增账户", "新增" + User_count);
            //    Response.Write("T");
            //}
            //catch (Exception)
            //{
            //    Response.Write("F");

            //}
            //finally { Response.End(); }

        }
        //显示已在部门
        private void load_department()
        {
            String User_count = Request.Params.Get("User_count");
            SQLHelper sql2 = new SQLHelper();
            // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select User_department,Department_name,User_count from dbo.TB_user_department where User_count='" + User_count + "'";
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "User_department", "Department_name", "0", "0");
            string content1 = result.ToString();
            Response.Write(content1);
            Response.End();
        }
        //将员工指派到其它部门 
        private void tree_add_project()
        {
            String User_count = Request.Params.Get("User_count");
            String User_department = Request.Params.Get("User_department");
            String Department_name = Request.Params.Get("Department_name");

            String sql1 = "if((select count(*) from dbo.TB_user_department where User_count='" + User_count + "' and User_department='" + User_department + "')=0)"
                + " begin INSERT INTO dbo.TB_user_department (User_count,User_department,Department_name) values ('" + User_count + "','" + User_department + "','"
                + Department_name + "') end";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }
        }
        //将员工从部门移除
        private void tree_remove_project()
        {
            String User_count = Request.Params.Get("User_count");
            String User_department = Request.Params.Get("User_department");

            string select_info = "select count(0) from dbo.TB_user_department where User_count='" + User_count + "' and (User_department=45 or User_department=46)";
            int count = Convert.ToInt32(db.ExecuteScalar(select_info).ToString());
            if (count == 1)
            {
                Response.Write("人员至少存在一个组中");
                Response.End();
            }
            String sql1 = "delete dbo.TB_user_department where User_count='" + User_count + "' and User_department='" + User_department + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }
        }
        public byte[] SetImageToByteArray(FileUpload FileUpload1)
        {
            Stream stream = FileUpload1.PostedFile.InputStream;
            byte[] photo = new byte[FileUpload1.PostedFile.ContentLength];
            stream.Read(photo, 0, FileUpload1.PostedFile.ContentLength);
            stream.Close();
            return photo;
        }
        /// 签名保存
        private void h_autograph_save()
        {
            Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.Charset = "UTF-8";
            // 文件上传后的保存路径
            HttpPostedFile file = Request.Files["Filedata"];
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);      //原始文件名称
                string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称
                int ids = Convert.ToInt32(Request.Params.Get("ids"));
                //string uploadPath = Request.MapPath("upload_Folder" + "\\");
                //文件保存路径
                string uploadPaths = Server.MapPath(ConfigurationManager.AppSettings["signature_pic"].ToString());
                file.SaveAs(uploadPaths + saveName);

                string filename_url = ConfigurationManager.AppSettings["signature_pic"].ToString() + saveName;
                String sql1 = "UPDATE dbo.TB_user_info SET Signature='" + filename_url + "',Signature_format='" + fileExtension + "' where id='" + ids + "' ";
                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    SQLStringList.Add(sql1);

                    //sql事务
                    SQLHelper.ExecuteSqlTran(SQLStringList);
                    Response.Write("T");
                }
                catch (Exception)
                {
                    Response.Write("F");

                }
                finally { Response.End(); }
            }
            else
            {
                Response.Write("F");
                Response.End();
            }
        }
        //员工信息更新
        private void h_useredit()
        {

            string ids = Request.Params.Get("ids");
            string User_job = Request.Params.Get("User_job");
            string User_duties = Request.Params.Get("User_duties");
            string Tel = Request.Params.Get("Tel");
            string Phone = Request.Params.Get("Phone");
            string Fax = Request.Params.Get("Fax");
            string Email = Request.Params.Get("Email");
            string QQ = Request.Params.Get("QQ");
            string MSN = Request.Params.Get("MSN");
            string Address = Request.Params.Get("Address");
            string Postcode = Request.Params.Get("Postcode");
            string Remarks = Request.Params.Get("Remarks");
            string department_code = Request.Params.Get("department_code");
            string department = Request.Params.Get("department");
            string sql1 = "UPDATE dbo.TB_user_info SET User_job='" + User_job + "',User_duties='" + User_duties + "',Tel='" + Tel + "',Phone='" + Phone + "',Fax='"
                + Fax + "',Email='" + Email + "', QQ='" + QQ + "',MSN='" + MSN + "',Address='" + Address + "',Postcode='" + Postcode + "',Remarks='"
                + Remarks + "',department='" + department + "' ,department_code='" + department_code + "' where id='" + ids + "'";
            //      String sql1 = "INSERT INTO dbo.TB_Supplier_info_contacts (g_id_c,name_c,sex_c,department_c,duties_c,tel_c,phone_c,c_fax_c,Email_c,QQ_c,MSN_c,post_code_c,remarks_c,appraiser) values('" + g_id_c + "','" + name_c + "','" + sex_c + "','" + department_c + "','" + duties_c + "','" + tel_c + "','" + phone_c + "','" + c_fax_c + "','" + Email_c + "','" + QQ_c + "','" + MSN_c + "','" + post_code_c + "','" + remarks_c + "','" + login_user + "') ";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }

        }
        //员工信息删除
        private void h_userdel()
        {
            String User_count = Request.Params.Get("User_count");
            String User_department = Request.Params.Get("User_department");

            String sql1 = "delete from dbo.TB_user_department  where User_count='" + User_count + "' and User_department='" + User_department + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");

            }
            finally { Response.End(); }

        }
        //默认密钥向量 
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为8位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        private void GetDepart()
        {
            string sql = "SELECT id as value,Department_name as text from TB_department ";
            DataTable dt = new DBHelper().ExecuteDataTable(sql);
            StringBuilder json = new StringBuilder();
            json.Append("[");
            string str = new JsonHelper().DataTableToJson(dt);
            json.Append(str);
            json.Append("]");
            Response.Write(json.ToString());
            Response.End();
        }

        private void GetDepartUser()
        {
            string id = Request.Params.Get("id");
            if (string.IsNullOrEmpty(id))
            {
                Response.Write("[]");
                Response.End();
            }
            string sql = "SELECT ui.user_count as value,ui.user_name as text from TB_user_info ui INNER JOIN TB_user_department ud on ui.User_count = ud.User_count  where ud.User_department=" + id;
            DataTable dt = new DBHelper().ExecuteDataTable(sql);
            StringBuilder json = new StringBuilder();
            json.Append("[");
            string str = new JsonHelper().DataTableToJson(dt);
            json.Append(str);
            json.Append("]");
            Response.Write(json.ToString());
            Response.End();
        }

        //搜索地址
        public void GetEquipment_code()
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select Project_name,Project_value from TB_dictionary_managing_context where group_id=7";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = sql2.Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面    
            Response.End();
        }


        //*****************************************************************************************
        ///*****************************************************************************************
        //**********************************    权限管理      *************************************
        ///*****************************************************************************************
        //*****************************************************************************************

        ///加载已分配权限信息
        public void load_data()
        {
            String User_count = Request.Params.Get("User_count");
            SQLHelper hmylql = new SQLHelper();
            string strsql = "select dbo.TB_jurisdiction_page.fid from dbo.TB_jurisdiction_page,dbo.TB_user_info where dbo.TB_jurisdiction_page.User_count = '" + User_count + "' and dbo.TB_jurisdiction_page.User_count=dbo.TB_user_info.User_count";
            DataSet ds = hmylql.GetDataSet(strsql);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json3(ds, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面    
            Response.End();
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
        private void search_people()
        {
            string searchs = Request.Params.Get("search");
            string search1 = Request.Params.Get("search1");
            //string ids = Request.Params.Get("id");
            //int id1 = Convert.ToInt32(ids);
            string getpage1 = Request.Params.Get("page");
            string getrows1 = Request.Params.Get("rows");
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
            Response.Write(strJson);//返回给前台页面   
            Response.End();
        }

        //权限设置--确定修改
        private void Permissions()
        {
            String SessionCount = Request.Params.Get("User_count");
            String SessionName = Request.Params.Get("User_name");
            String Web_value = Request.Params.Get("Web_value"); //数据--“（页面id）;（checkbox.value）”
            string[] Web_values;
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
                    if (list_fid.Contains(Web_values[i]))
                    {
                        value = "1";
                    }
                    //判断数据库是否存在该数据，若不存在则添加进数据库
                    if (value == "0")
                    {
                        list_sql.Add("insert into dbo.TB_jurisdiction_page (User_count,username,fid) Values ('" + SessionCount + "','" + SessionName + "'," + Web_values[i] + ")");

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
                Response.Write("T");

            }
            catch (Exception)
            {
                Response.Write("F");
            }
            finally
            {
                Response.End();
            }

            //string strsql = "select * from  dbo.TB_functional_module where username=[SessionName]";//按条件查询数据  
            //DataSet ds = hmylql.GetDataSet(strsql);
            ////将Dataset转化为Datable    
            //DataTable dt = ds.Tables[0];
            //int count = dt.Rows.Count;
            //string strJson = Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            //Response.Write(strJson);//返回给前台页面    
            //Response.End();
        }

    }
}
