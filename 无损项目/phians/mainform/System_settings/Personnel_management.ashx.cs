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

namespace phians.mainform
{
    /// <summary>
    /// technicians_info_management 的摘要说明
    /// </summary>
    public class technicians_info_management : IHttpHandler, IRequiresSessionState
    {
        private readonly DBHelper db = new DBHelper();
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            context.Response.ContentType = "text/html";

            String Parent_id = context.Request.Params.Get("ids");
            string command = context.Request.Params.Get("cmd"); //context.Request.QueryString["cmd"];
            switch (command)
            {
                case "loadtree": loadtree(context); break;//加载实验室组信息树
                case "treeadd": treeadd(context); break; //添加树同级项目
                case "edit": edit(context); break; //修改树的名称
                    
                case "treeadd_next": treeadd_next(context); break; //添加树同级项目
                case "treedel": treedel(context); break; //删除树项目
                case "load_userlist": h_load_userlist(context); break; //加载用户列表
                case "load_userinfo": load_userinfo(context); break; //加载用户信息
                case "technicians_save": h_useredit(context); break; //用户基本信息保存
                case "technicians_saveadd": h_useradd(context); break; //添加用户信息
                case "load_department": load_department(context); break; //显示用户已在部门
                case "tree_add_project": tree_add_project(context); break; //将员工指派到其它组
                case "tree_remove_project": tree_remove_project(context); break; //将员工从组移除
                case "technicians_del": h_userdel(context); break; //将员工从部门移除
                case "technicians_autograph": h_autograph_save(context); break; //签名上传
                case "department_people_search": department_people_search(context); break; //搜索员工
                case "no_department_personnel": no_department_personnel(context); break; //查看未分配组人员
                case "enable_personnel": enable_personnel(context); break; //启用人员
                case "disable_personnel": disable_personnel(context); break; //停用人员
                case "Reset_password": Reset_password(context); break; //重置密码
                case "getDepart": GetDepart(context); break; //重置密码
                case "getDepartUser": GetDepartUser(context); break;
                case "getEquipment_code": GetEquipment_code(context); break;
            }
        }

        public void edit(HttpContext context)
        {
            String id = context.Request.Params.Get("ids");
            String Department_name = context.Request.Params.Get("Department_name");
            String sql1 = "update TB_department  set Department_name ='" + Department_name + "'    where id='" + id + "' and  flag_ =0";
           
          try
            {
                db.BeginTransaction();
              int i=  db.ExecuteNonQueryByTrans(sql1);
                db.CommitTransacton();

                if (i > 0)
                {
                    context.Response.Write("T");

                }
                else {
                    context.Response.Write("内置组不允许修改");
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

        //加载实验室部门信息树
        public void loadtree(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select id,Department_name,Parent_id from dbo.TB_department";
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "id", "Department_name", "Parent_id", "0");
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
        //添加同级项目
        public void treeadd(HttpContext context)
        {

            String command = context.Request.QueryString["cmd"];
            String Parent_id = context.Request.Params.Get("Parent_id");
            String Department_name = context.Request.Params.Get("Department_name");
            //String login_user = Convert.ToString(context.Session["loginAccount"]);          
            String sql1 = "INSERT INTO dbo.TB_department (Parent_id,Department_name) values('" + Parent_id + "','" + Department_name + "') ";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);
                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        //添加下级树项目
        public void treeadd_next(HttpContext context)
        {

            String Parent_id = context.Request.Params.Get("ids");
            String Department_name = context.Request.Params.Get("Department_name");
            //String login_user = Convert.ToString(context.Session["loginAccount"]);

            String sql1 = "INSERT INTO dbo.TB_department (Parent_id,Department_name) values('" + Parent_id + "','" + Department_name + "') ";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        //删除树项目
        public void treedel(HttpContext context)
        {
            String id = context.Request.Params.Get("ids");

            String sql1 = "delete from dbo.TB_department  where id='" + id + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        //加载用户用户列表
        private void h_load_userlist(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            string ids = context.Request.Params.Get("id");
            //部门id
            int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by U.id)RowId,U.Address,U.Email,U.Fax,U.MSN,U.Phone,U.Postcode,U.QQ,U.Signature,U.Tel,U.User_count,U.User_count_state,U.User_duties,U.User_job,U.User_name,U.User_sex,U.department,U.department_code,U.id,U.Remarks  from dbo.TB_user_info as U,"
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
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();

        }
        //搜索员工
        private void department_people_search(HttpContext context)
        {
            string search = context.Request.Params.Get("search");
            string search1 = context.Request.Params.Get("search1");
            //string ids = context.Request.Params.Get("id");
            ///int id1 = Convert.ToInt32(ids);

            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
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
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //查看未分配组人员
        private void no_department_personnel(HttpContext context)
        {
            string search = context.Request.Params.Get("search");
            string search1 = context.Request.Params.Get("search1");
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
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
            string strsql2 = "SELECT * FROM dbo.TB_user_info WHERE User_count NOT IN (SELECT User_count FROM dbo.TB_user_department ) ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();

        }
        //启用
        private void enable_personnel(HttpContext context)
        {
            String id = context.Request.Params.Get("id");
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
                String op_User_count = context.Request.Params.Get("User_count");
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "启用账户", "启用" + op_User_count);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }
        }
        //停用
        private void disable_personnel(HttpContext context)
        {
            String id = context.Request.Params.Get("id");
            //被操作
            String op_User_count = context.Request.Params.Get("User_count");
            string User_count_state = "停用";

            String sql1 = "update dbo.TB_user_info set User_count_state='" + User_count_state + "' where id='" + id + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "停用账户", "停用" + op_User_count);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }
        }
        //重置密码
        private void Reset_password(HttpContext context)
        {
            String id = context.Request.Params.Get("id");
            string User_pwd = "123456";
            User_pwd = EncryptDES(User_pwd.Trim(), "hxw_2016");
            //被操作用户
            String op_User_count = context.Request.Params.Get("User_count");
            String sql1 = "update dbo.TB_user_info set User_pwd='" + User_pwd + "' where id='" + id + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "重置密码", "重置" + op_User_count + "密码");
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }
        }

        //加载用户详细信息
        public void load_userinfo(HttpContext context)
        {
            String ids = context.Request.Params.Get("ids");
            SQLHelper sql2 = new SQLHelper();
            // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select * from dbo.TB_user_info where id ='" + ids + "'";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();

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
        private void h_useradd(HttpContext context)
        {
            String User_count = context.Request.Params.Get("User_count");
            String User_name = context.Request.Params.Get("User_name");
            String User_job = context.Request.Params.Get("User_job");
            String User_duties = context.Request.Params.Get("User_duties");
            String Tel = context.Request.Params.Get("Tel");
            String Phone = context.Request.Params.Get("Phone");
            String Fax = context.Request.Params.Get("Fax");
            String Email = context.Request.Params.Get("Email");
            String QQ = context.Request.Params.Get("QQ");
            String MSN = context.Request.Params.Get("MSN");
            String Address = context.Request.Params.Get("Address");
            String Postcode = context.Request.Params.Get("Postcode");
            String Remarks = context.Request.Params.Get("Remarks");
            //组id
            int User_department = Convert.ToInt32(context.Request.Params.Get("User_department"));
            //组名称
            String Department_name = context.Request.Params.Get("Department_name");
            //部门id
            String department_code = context.Request.Params.Get("department_code");
            //部门名称
            String department = context.Request.Params.Get("department");

            string User_count_state = "正在使用中";
            string User_pwd = "123456";
            User_pwd = EncryptDES(User_pwd, "hxw_2016");

            SqlParameter[] para = 
                {
                    new SqlParameter("@User_count",User_count),
                    new SqlParameter("@User_name",User_name),
                    new SqlParameter("@User_job",User_job),
                    new SqlParameter("@User_duties",User_duties),                   
                    new SqlParameter("@Tel",Tel),
                    new SqlParameter("@Phone",Phone),
                    new SqlParameter("@Fax",Fax),
                    new SqlParameter("@Email",Email),
                    new SqlParameter("@QQ",QQ),
                    new SqlParameter("@MSN",MSN),
                    new SqlParameter("@Address",Address),
                    new SqlParameter("@Postcode",Postcode),
                    new SqlParameter("@Remarks",Remarks),
                    new SqlParameter("@User_count_state",User_count_state),
                    new SqlParameter("@User_pwd",User_pwd),
                    new SqlParameter("@department_code",department_code),
                    new SqlParameter("@department",department)
                };
            SqlParameter[] para2 = 
                {
                    new SqlParameter("@User_count1",User_count),
                    new SqlParameter("@User_department",User_department),
                    new SqlParameter("@Department_name",Department_name),
                   
                };

            //String sql1 = "INSERT INTO dbo.TB_user_info (User_count,User_name,User_job,User_duties,Tel,Phone,Fax,Email,QQ,MSN,Address,Postcode,Remarks,"
            //    + "User_department,User_pwd,User_count_state) values('" + User_count + "','" + User_name + "','" + User_job + "','" + User_duties + "','"
            //    + Tel + "','" + Phone + "','" + Fax + "','" + Email + "','" + QQ + "','" + MSN + "','" + Address + "','" + Postcode + "','" + Remarks + "','"
            //    + User_department + "','" + User_pwd + "','" + User_count_state + "') ";
            //String sql2 = "INSERT INTO dbo.TB_user_department (User_count,User_department,Department_name) values('" + User_count + "','"
            //    + User_department + "','" + Department_name + "')";
            String insert_sql = "INSERT INTO dbo.TB_user_info (User_count,User_name,User_job,User_duties,Tel,Phone,Fax,Email,QQ,MSN,Address,Postcode,Remarks,"
              + "User_pwd,User_count_state,department_code,department) values(@User_count,@User_name,@User_job,@User_duties,"
              + "@Tel,@Phone,@Fax,@Email,@QQ,@MSN,@Address,@Postcode,@Remarks,"
              + "@User_pwd,@User_count_state,@department_code,@department) ";

            String insert_sql2 = "INSERT INTO dbo.TB_user_department (User_count,User_department,Department_name) values(@User_count1,@User_department,@Department_name)";
            try
            {
                //SQL语句
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                List<string> SQLStringList = new List<string>();

                SQLStringList.Add(insert_sql);
                SQLStringList2.Add(para);
                SQLStringList.Add(insert_sql2);
                SQLStringList2.Add(para2);
                //SQLStringList.Add(sql1);
                //SQLStringList.Add(sql2);
                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                //被操作账户
                // String op_User_count = context.Request.Params.Get("User_count");
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "新增账户", "新增" + User_count);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        //显示已在部门
        private void load_department(HttpContext context)
        {
            String User_count = context.Request.Params.Get("User_count");
            SQLHelper sql2 = new SQLHelper();
            // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select User_department,Department_name,User_count from dbo.TB_user_department where User_count='" + User_count + "'";
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "User_department", "Department_name", "0", "0");
            string content1 = result.ToString();
            context.Response.Write(content1);

        }
        //将员工指派到其它部门 
        private void tree_add_project(HttpContext context)
        {
            String User_count = context.Request.Params.Get("User_count");
            String User_department = context.Request.Params.Get("User_department");
            String Department_name = context.Request.Params.Get("Department_name");

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
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }
        }
        //将员工从部门移除
        private void tree_remove_project(HttpContext context)
        {
            String User_count = context.Request.Params.Get("User_count");
            String User_department = context.Request.Params.Get("User_department");

            String sql1 = "delete dbo.TB_user_department where User_count='" + User_count + "' and User_department='" + User_department + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }
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
        private void h_autograph_save(HttpContext context)
        {
            context.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            context.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            context.Response.Charset = "UTF-8";
            // 文件上传后的保存路径
            HttpPostedFile file = context.Request.Files["Filedata"];
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);      //原始文件名称
                string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称
                int ids = Convert.ToInt32(context.Request.Params.Get("ids"));
                //string uploadPath = context.Request.MapPath("upload_Folder" + "\\");
                //文件保存路径
                string uploadPaths = context.Server.MapPath(ConfigurationManager.AppSettings["signature_pic"].ToString());
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
                    context.Response.Write(filename_url);
                }
                catch (Exception)
                {
                    context.Response.Write("F");

                }
                finally { context.Response.End(); }
            }
            else
            {
                context.Response.Write("F");
                context.Response.End();
            }
        }
        //员工信息更新
        private void h_useredit(HttpContext context)
        {

            string ids = context.Request.Params.Get("ids");
            string User_job = context.Request.Params.Get("User_job");
            string User_duties = context.Request.Params.Get("User_duties");
            string Tel = context.Request.Params.Get("Tel");
            string Phone = context.Request.Params.Get("Phone");
            string Fax = context.Request.Params.Get("Fax");
            string Email = context.Request.Params.Get("Email");
            string QQ = context.Request.Params.Get("QQ");
            string MSN = context.Request.Params.Get("MSN");
            string Address = context.Request.Params.Get("Address");
            string Postcode = context.Request.Params.Get("Postcode");
            string Remarks = context.Request.Params.Get("Remarks");
            string department_code = context.Request.Params.Get("department_code");
            string department = context.Request.Params.Get("department");
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
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        //员工信息删除
        private void h_userdel(HttpContext context)
        {
            String User_count = context.Request.Params.Get("User_count");
            String User_department = context.Request.Params.Get("User_department");

            String sql1 = "delete from dbo.TB_user_department  where User_count='" + User_count + "' and User_department='" + User_department + "'";
            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(sql1);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

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

        private void GetDepart(HttpContext context)
        {
            string sql = "SELECT id as value,Department_name as text from TB_department ";
            DataTable dt = new DBHelper().ExecuteDataTable(sql);
            StringBuilder json = new StringBuilder();
            json.Append("[");
            string str = new JsonHelper().DataTableToJson(dt);
            json.Append(str);
            json.Append("]");
            context.Response.Write(json.ToString());
            context.Response.End();
        }

        private void GetDepartUser(HttpContext context)
        {
            string id = context.Request.Params.Get("id");
            if (string.IsNullOrEmpty(id))
            {
                context.Response.Write("[]");
                context.Response.End();
            }
            string sql = "SELECT ui.user_count as value,ui.user_name as text from TB_user_info ui INNER JOIN TB_user_department ud on ui.User_count = ud.User_count  where ud.User_department=" + id;
            DataTable dt = new DBHelper().ExecuteDataTable(sql);
            StringBuilder json = new StringBuilder();
            json.Append("[");
            string str = new JsonHelper().DataTableToJson(dt);
            json.Append(str);
            json.Append("]");
            context.Response.Write(json.ToString());
            context.Response.End();
        }

        //搜索地址
        public void GetEquipment_code(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select Project_name,Project_value from TB_dictionary_managing_context where group_id=7";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = sql2.Dataset2Json(ds, count);//DataSet数据转化为Json数据    
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