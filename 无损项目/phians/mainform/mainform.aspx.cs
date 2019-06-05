using Microsoft.AspNet.SignalR;
using phians.custom_class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            string SessionName = Convert.ToString(Session["loginAccount"]);
            string login_username = Convert.ToString(Session["login_username"]);
            string command = Request["cmd"];
            if (string.IsNullOrEmpty(SessionName))
                {
                    Response.Write("F");
                    Response.End();
                }
                switch (command)
                {
                    //case "logincheck": loginchecks(context); break;
                    case "load_m_menu": load_m_menu(); break;
                    case "change_password": change_password(); break;//修改密码
                    case "check_session": check_session(); break;
                }
                if (SessionName != "")
                {
                    //显示用户名
                    Label1.Text = "当前登录用户:" + login_username;
                    HiddenField1.Value = SessionName;
                    string now_date = DateTime.Today.ToString("yyyy");
                    bottom_text.Text = " @2009-" + now_date + " 广东清大菲恩工业数据技术有限公司";
                 //  new1 = new HtmlGenericControl("div");
                    //加载信息
                }
                bottom_text.Attributes.Add("onclick", "display()");                     
        }


        public void check_session() {
            string SessionName = Convert.ToString(Session["loginAccount"]);
            if (string.IsNullOrEmpty(SessionName))
            {
                Response.Write("F");
            }

            else {
                Response.Write("T");
            }
         
            Response.End();
        
        
        }
        //修改密码
        public void change_password()
        {
          

            string new_password = DecodeBase64(Request.Params.Get("new_password").Trim());
            string phians_old_pwd = DecodeBase64(Request.Params.Get("phians_old_pwd").Trim());

            phians_old_pwd = EncryptDES(phians_old_pwd, "hxw_2016");
            new_password = EncryptDES(new_password, "hxw_2016");

            string Sessioncount = Convert.ToString(Session["loginAccount"]).Trim();
            string SessionName = Convert.ToString(Session["login_username"]).Trim();
            SQLHelper hmylql = new SQLHelper();
            string strsql = "select * from dbo.TB_user_info  where User_count='" + Sessioncount + "' and User_pwd='" + phians_old_pwd + "' ";
            DataSet ds = hmylql.GetDataSet(strsql);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            if (count > 0)
            {
                string change_sql = " UPDATE dbo.TB_user_info SET  User_pwd= '" + new_password + "'where User_count='" + Sessioncount + "' ";
                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    SQLStringList.Add(change_sql);
                    //sql事务
                    SQLHelper.ExecuteSqlTran(SQLStringList);
                    Operation_log.operation_log_(SessionName, SessionName, "修改密码", "修改密码");
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
            else if (count == 0)
            {

                Response.Write("原密码错误");
               Response.End();
            }

        }

        //显示菜单
        private void load_m_menu()
        {
            string SessionName = Convert.ToString(Session["loginAccount"]);
            //int[] am_numbers = new int[50];

            
            SQLHelper hmylql = new SQLHelper();

            string strsql = "select f.m_number,f.s_number,f.s_1_number,f.i_iconCls,f.m_name,f.group_id,f.u_url FROM dbo.TB_functional_module AS f "
                + " LEFT JOIN dbo.TB_jurisdiction_page AS j ON f.fid=j.fid WHERE j.User_count='" + SessionName + "' ORDER BY f.sort_num,f.group_id";
            DataSet ds = hmylql.GetDataSet(strsql);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json(ds, count);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面    
            Response.End();
        }

        public void Clear(object sender, EventArgs e)
        {
            string strUserId = Convert.ToString(Session["loginAccount"]);  
           
            Session["loginAccount"] = null;
            ClearClientPageCache();
            Response.Redirect("/login/login.aspx");
        }
        public void ClearClientPageCache()
        {
            //清除浏览器缓存 
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();
        }
        /// DataSet转换成Json格式   
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
                json.Append(",\"rows\":[");
                json.Append(DataTable2Json(dt));
                json.Append("]}");
            }
            return json.ToString();
        }
        /// dataTable转换成Json格式   
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

      

        //密码加密
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

        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }
}
