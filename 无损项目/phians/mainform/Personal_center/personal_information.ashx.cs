using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    /// <summary>
    /// personal_information1 的摘要说明
    /// </summary>
    public class personal_information1 : IHttpHandler, IRequiresSessionState
    {

        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.Params.Get("cmd"); //context.Request.QueryString["cmd"];
            switch (command)
            {
                case "load_certificate": load_certificate(context); break;//加载证书信息
                case "load_personnel_info": load_personnel_info(context); break;//加载个人资料
                case "technicians_autograph": technicians_autograph(context); break;//签名保存
                case "save_personnel_info": save_personnel_info(context); break;//保存个人信息资料
                case "add_certificate": add_certificate(context); break;//添加证书
                case "del_certificate": del_certificate(context); break;//删除证书
                case "overdue_query": overdue_query(context); break;//过期查询
                case "due_warning": due_warning(context); break;//到期预警
            }
        }

        //资质证书项目加载
        private void load_certificate(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);

            //int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_technicians_aptitude where User_count= '" + login_user + "')a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from  dbo.TB_technicians_aptitude where User_count= '" + login_user + "' ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //加载个人资料
        private void load_personnel_info(HttpContext context)
        {
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);
            string personnel_info = "";


            //SQLHelper sql1 = new SQLHelper();
            string daysx = "select TD.Department_name,TU.*  from  dbo.TB_user_info as TU left join dbo.TB_user_department as TD on  TU.User_count=TD.User_count where TU.User_count='" + login_user + "'";


            DataTable dt = db.ExecuteDataTable(daysx);

            DataTable tmp = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (tmp.Select("id='"+dt.Rows[i]["id"].ToString()+"'").Length==0)
                {
                    tmp.Rows.Add(dt.Rows[i].ItemArray);
                }
                else
                {
                    tmp.Select("id='" + dt.Rows[i]["id"].ToString() + "'")[0]["Department_name"] += "," + dt.Rows[i]["Department_name"].ToString();
                }
            }
            string strJson = jsonHelper.DataTableToJson(tmp);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
            
            //for (int i = 0; i < dt.Rows.Count;i++ )
            //{
            //        personnel_info = dt.Rows[1]["User_job"].ToString() + "," + dt.Rows[1]["User_duties"].ToString() + "," + dt.Rows[1]["Tel"].ToString() + "," + dt.Rows[1]["Phone"].ToString()
            //            + "," + dt.Rows[1]["Fax"].ToString() + "," + dt.Rows[1]["Email"].ToString() + "," + dt.Rows[1]["QQ"].ToString() + "," + dt.Rows[1]["MSN"].ToString()
            //            + "," + dt.Rows[1]["Address"].ToString() + "," + dt.Rows[1]["Postcode"].ToString() + "," + dt.Rows[1]["Remarks"].ToString() + ","
            //            + dt.Rows[1]["Signature"].ToString() + "," + dt.Rows[1]["Department_name"].ToString() ;
                
            //}

            //SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, daysx);
            //while (dr.Read())
            //{
            //    personnel_info = dr["User_job"].ToString() + "," + dr["User_duties"].ToString() + "," + dr["Tel"].ToString() + "," + dr["Phone"].ToString()
            //        + "," + dr["Fax"].ToString() + "," + dr["Email"].ToString() + "," + dr["QQ"].ToString() + "," + dr["MSN"].ToString()
            //        + "," + dr["Address"].ToString() + "," + dr["Postcode"].ToString() + "," + dr["Remarks"].ToString() + ","
            //        + dr["Signature"].ToString() + "," + dr["Department_name"].ToString();
            //}
            //dr.Close();
            //context.Response.Write(personnel_info);//返回给前台页面   
            //context.Response.End();
        }
        //资质证书增加
        private void add_certificate(HttpContext context)
        {
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);
            string login_name = Convert.ToString(context.Session["login_username"]);

            String certificate_name = context.Request.Params.Get("certificate_name");
            String certificate_num = context.Request.Params.Get("certificate_num");
            String Issuing_unit = context.Request.Params.Get("Issuing_unit");
            String effective_date = context.Request.Params.Get("effective_date");
            String remarks = context.Request.Params.Get("remarks");
            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String insert_sql = "INSERT INTO dbo.TB_technicians_aptitude (login_user,User_count,User_name,certificate_name,certificate_num,Issuing_unit,effective_date,remarks) values('" + login_user + "','" + login_user + "','" + login_name + "','" + certificate_name + "','" + certificate_num + "','" + Issuing_unit + "','" + effective_date + "','" + remarks + "') ";
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
        //资质证书删除
        private void del_certificate(HttpContext context)
        {
            String certificates = context.Request.Params.Get("certificates");
            string[] ids = certificates.Split(';');    //分割字符串 每单遇到分号分割一次
            string conditions = "";
            for (int i = 0; i < ids.Length; i++)
            {
                if (i == 0)
                {
                    conditions = "where id = " + ids[i];
                }
                if (i > 0)
                {
                    conditions = conditions + " or id = " + ids[i];
                }
            }

            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String delete_sql = "delete dbo.TB_technicians_aptitude " + conditions + "";
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
        //资质证书到期预警
        private void due_warning(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);
            string date = DateTime.Now.ToLocalTime().ToString();                    //时间
            int days = 30;

            //int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_technicians_aptitude where User_count= '" 
                + login_user + "' and (effective_date-" + days + ")<='" + date + "' and effective_date>='" + date + "')a where RowId >= '" 
                + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_technicians_aptitude where User_count= '" + login_user + "' and (effective_date-" 
                + days + ")<='" + date + "' and effective_date>='" + date + "'";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //资质证书过期查询
        private void overdue_query(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);
            string date = DateTime.Now.ToLocalTime().ToString();                    //时间

            //int id1 = Convert.ToInt32(ids);
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by id)RowId,* from dbo.TB_technicians_aptitude where User_count= '" + login_user + "' and effective_date<'" + date + "')a where RowId >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_technicians_aptitude where User_count= '" + login_user + "' and effective_date<'" + date + "'";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        ///签名保存
        private void technicians_autograph(HttpContext context)
        {
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);
            HttpFileCollection files = context.Request.Files;
            String old_url = context.Request.Params.Get("old_url");


            HttpPostedFile file = files[0];
            string fileName2 = Path.GetFileName(file.FileName);

            String update_sql = "";
            string filename_url = "";
            if (files.Count > 0 && !string.IsNullOrEmpty(fileName2))
            {//获取第一个文件
                string fileName = Path.GetFileName(file.FileName);      //原始文件名称
                string fileExtension = Path.GetExtension(fileName).ToLower();         //文件扩展名
                if (fileExtension != ".png" && fileExtension != ".jpg")
                {
                    context.Response.Write("请上传图片");
                    context.Response.End();
                }
                string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称
                //int ids = Convert.ToInt32(context.Request.Params.Get("ids"));
                //string uploadPath = context.Request.MapPath("upload_Folder" + "\\");
                //文件保存路径
                string uploadPaths = context.Server.MapPath(ConfigurationManager.AppSettings["signature_pic"].ToString());
                file.SaveAs(uploadPaths + saveName);
                filename_url = ConfigurationManager.AppSettings["signature_pic"].ToString() + saveName;
                //原始文件名称,包含扩展名  
                string filename = fileName2.Substring(0, fileName2.LastIndexOf("."));     //文件名称，去掉扩展名 
                fileExtension = Path.GetExtension(fileName2);

                string old_url1 = context.Server.MapPath(old_url);

                if (System.IO.File.Exists(old_url1))
                {
                    System.IO.File.Delete(old_url1);
                }

                update_sql = "UPDATE dbo.TB_user_info SET Signature='" + filename_url + "',Signature_format='" + fileExtension + "' where User_count='" + login_user + "' ";
            }
            else {
                context.Response.Write("请选择签名");
                context.Response.End();
            }


            try
            {


                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    SQLStringList.Add(update_sql);
                    //事务
                    SQLHelper.ExecuteSqlTran(SQLStringList);
                    context.Response.Write(filename_url);
            }
            catch (Exception)
            {

                context.Response.Write("F");
                context.Response.End();
            }
            finally
            {
                context.Response.End();
            }




        }
        //private void technicians_autograph(HttpContext context)
        //{
        //    //登录用户
        //    string login_user = Convert.ToString(context.Session["loginAccount"]);

        //    context.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
        //    context.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
        //    context.Response.Charset = "UTF-8";
        //    // 文件上传后的保存路径
        //    HttpPostedFile file = context.Request.Files["Filedata"];
        //    if (file != null)
        //    {
        //        string fileName = Path.GetFileName(file.FileName);      //原始文件名称
        //        string fileExtension = Path.GetExtension(fileName);         //文件扩展名
        //        string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称
        //        //int ids = Convert.ToInt32(context.Request.Params.Get("ids"));
        //        //string uploadPath = context.Request.MapPath("upload_Folder" + "\\");
        //        //文件保存路径
        //        string uploadPaths = context.Server.MapPath(ConfigurationManager.AppSettings["signature_pic"].ToString());
        //        file.SaveAs(uploadPaths + saveName);

        //        //SQLHelper sql2 = new SQLHelper();
        //        //SqlConnection con = sql2.getConn();
        //        string filename_url = ConfigurationManager.AppSettings["signature_pic"].ToString() + saveName;
        //        String update_sql = "UPDATE dbo.TB_user_info SET Signature='" + filename_url + "',Signature_format='" + fileExtension + "' where User_count='" + login_user + "' ";
        //        //SqlCommand cmd1 = new SqlCommand(sql1, con);
        //        //int count = cmd1.ExecuteNonQuery();
        //        //con.Close();
        //        //if (count >= 1)
        //        //{
        //        //    context.Response.Write(saveName);
        //        //    context.Response.End();
        //        //}
        //        //else
        //        //{
        //        //    context.Response.Write("F");
        //        //    context.Response.End();
        //        //}
        //        try
        //        {
        //            //SQL语句
        //            List<string> SQLStringList = new List<string>();
        //            SQLStringList.Add(update_sql);
        //            //事务
        //            SQLHelper.ExecuteSqlTran(SQLStringList);
        //            context.Response.Write(filename_url);
                    
        //        }
        //        catch (Exception)
        //        {
        //            context.Response.Write("F");
        //        }
        //        finally
        //        {
        //            context.Response.End();
        //        }
        //    }
        //    else
        //    {
        //        context.Response.Write("F");
        //        context.Response.End();
        //    }
        //}
        

        ///保存个人信息资料
        private void save_personnel_info(HttpContext context)
        {
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);

            String Phone = context.Request.Params.Get("Phone1");
            //String Department_name = context.Request.Params.Get("Department_name1");
            String User_job = context.Request.Params.Get("User_job1");
            String User_duties = context.Request.Params.Get("User_duties1");
            String Tel = context.Request.Params.Get("Tel1");
            String Fax = context.Request.Params.Get("Fax1");
            String Email = context.Request.Params.Get("Email1");
            String QQ = context.Request.Params.Get("QQ1");
            String MSN = context.Request.Params.Get("MSN1");
            String Address = context.Request.Params.Get("Address1");
            String Postcode = context.Request.Params.Get("Postcode1");
            String Remarks = context.Request.Params.Get("Remarks1");

            //SQLHelper sql2 = new SQLHelper();
            //SqlConnection con = sql2.getConn();
            String update_sql = "update dbo.TB_user_info set Phone='" + Phone + "',User_job='" + User_job + "',User_duties='" + User_duties + "',Tel='" + Tel
                + "',Fax='" + Fax + "',Email='" + Email + "',QQ='" + QQ + "',MSN='" + MSN + "',Address='" + Address + "',Postcode='" + Postcode + "',Remarks='"
                + Remarks + "' where User_count = '" + login_user + "'";
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}