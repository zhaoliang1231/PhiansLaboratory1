using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using phians.custom_class;
using DecryptEncryptionApplication;

namespace phians
{
    public partial class login2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SessionName = Convert.ToString(Session["loginAccount"]);
            if (Request["cmd"] == "login")
            {
                login1();
            }
            else if (SessionName != "")
            {

                Response.Write("<script>location.href = '/mainform/mainform.aspx' </script>");
                Response.End();

            }
        }
        //sql的存储过程
        /// <summary>
        /// IF OBJECT_ID (N'check_user', N'P') IS NOT NULL
        //    DROP procedure check_user;
        //GO
        //CREATE procedure check_user(@User_count NVARCHAR(50),
        //@User_pwd NVARCHAR(200),
        //@User_count_state NVARCHAR(50)

        //)
        //AS
        //    SELECT * FROM dbo.TB_user_info WHERE User_count=@User_count AND User_pwd=@User_pwd AND User_count_state=@User_count_state
        //GO
        /// </summary>

        public const string DEFAULT_ENCRYPT_KEY = "hxw12345";
        /// <summary>
        /// 登录判断
        /// </summary>
        public void login1()
        {

            #region 检测验证

            string StrVerification = ConfigurationManager.AppSettings["Verification"].ToString();
            string Verification = DecryptEncryption.StringDecrypt(StrVerification);
            string[] Verifications = Verification.Split('*');

            if (Verifications[2] != "true")
            {
                if (DateTime.Now.ToLocalTime() > Convert.ToDateTime(Verifications[1]))
                {

                    //  Response.ContentType = "text/plain";
                    Response.Write('V');//返回查询结果
                    Response.End();
                }
            }
            #endregion


            try
            {

                SQLHelper mysqlh = new SQLHelper();
                //后台获取用户名和密码            
                string User_count = DecodeBase64(Request.Params.Get("phians_name1"));
                string upassword = DecodeBase64(Request.Params.Get("phians_pwd1"));
                string upassword1 = EncryptDES(upassword, "hxw_2016");
                string upassword2 = DecryptDES(upassword, "hxw_2016");

                string User_count_state = "正在使用中";
                //防止SQl数据注入
                int i = filterSql(User_count);
                int n = filterSql(upassword);

                if (i == 0 && n == 0)
                {
                    SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@User_count",SqlDbType.NVarChar,50),
                new SqlParameter("@User_pwd",SqlDbType.NVarChar,200),
                new SqlParameter("@User_count_state",SqlDbType.NVarChar,50),
                
            };
                    parms[0].Value = User_count;
                    parms[1].Value = upassword1;
                    parms[2].Value = User_count_state;
                    int count = 0;
                    try
                    {
                        SqlDataReader charges_dr = SQLHelper.ExecuteReader(CommandType.StoredProcedure, "dbo.check_user", parms);
                        while (charges_dr.Read())
                        {

                            count = 1;
                        }
                        charges_dr.Close();
                    }
                    catch { }

                    if (count > 0)
                    {

                        //string strUserId = User_count;
                        ////添加到登录列表
                        //ArrayList list = Application.Get("GLOBAL_USER_LIST") as ArrayList;
                        //if (list == null)
                        //{
                        //    list = new ArrayList();
                        //}
                        //for (int j = 0; j < list.Count; j++)
                        //{
                        //    if (strUserId == (list[j] as string))
                        //    {
                        //        //已经登录了，提示错误信息 
                        //        list.Remove(strUserId);
                        //        Application.Add("GLOBAL_USER_LIST", list);
                        //        Session.Contents.Remove("strUserId");
                        //        Response.Write('N');//返回查询结果
                        //        Response.End();
                        //        return;
                        //    }

                        //}
                        //list.Add(strUserId);
                        //获取用户名                
                        string check_user_sql = "select User_name from  dbo.TB_user_info where User_count='" + User_count + "' ";
                        //SqlCommand cmd = new SqlCommand(check_user_sql, check_user.getConn());
                        //SqlDataReader check_user_dr = cmd.ExecuteReader();
                        SqlDataReader check_user_dr = SQLHelper.ExecuteReader(CommandType.Text, check_user_sql);
                        string username = "";
                        while (check_user_dr.Read())
                        {
                            // 读取列值并输出到Label中    
                            username = check_user_dr["User_name"].ToString();

                        }
                        check_user_dr.Close();
                        //Application.Add("GLOBAL_USER_LIST", list);
                        Session["loginAccount"] = User_count.Trim();
                        Session["login_username"] = username.Trim();

                        //消息使用的Cookie
                        Response.Cookies.Add(new HttpCookie("SignalRID") { Value = User_count.Trim() });


                        //操作记录
                        Operation_log.operation_log_(User_count.Trim(), username.Trim(), "登录", "登录系统");
                        //  login_log(User_count.Trim(), username.Trim(), operation_date, operation_ip);
                        // Response.Write("location.href = '/mainform/mainform.aspx'");

                        Response.Write('Y');//返回查询结果


                    }
                    else
                    {
                        Response.Write('F');//返回查询结果


                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);//返回查询结果
            }
            finally
            {


                Response.End();

            }

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

        /// <summary> 
        /// DES解密字符串 c
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
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
        //数据库注入判断
        public int filterSql(string sSql)
        {
            int srcLen, decLen = 0;
            sSql = sSql.ToLower().Trim();
            srcLen = sSql.Length;
            sSql = sSql.Replace("exec", "");
            sSql = sSql.Replace("delete", "");
            sSql = sSql.Replace("master", "");
            sSql = sSql.Replace("truncate", "");
            sSql = sSql.Replace("declare", "");
            sSql = sSql.Replace("create", "");
            sSql = sSql.Replace("xp_", "no");
            decLen = sSql.Length;
            if (srcLen == decLen) return 0; else return 1;
        }
    }
}