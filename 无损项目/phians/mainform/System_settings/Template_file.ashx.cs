using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace phians.mainform.System_settings
{
    /// <summary>
    /// Template_file1 的摘要说明
    /// </summary>
    public class Template_file1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
             string command = context.Request.Params.Get("cmd");
             switch (command)
             {
                 case "report_read": report_Read(context); break;//查看检测证书模板
                 case "report_Edit": report_Edit(context); break;//查看检测report_Edit证书模板
             }
        }
        //查看文件
        private void report_Read(HttpContext context)
        {
            string field_name = context.Request.Params.Get("field_name");
            string File_url = "";
            
            switch (field_name)
	        {
                case "热处理模板3": File_url = "/upload_Folder/Lossless_report/1.docx"; break;
                case "目视报告模板_VT_63": File_url = "/upload_Folder/Lossless_report/2.doc"; break;
                case "DT模板空白_5版": File_url = "/upload_Folder/Lossless_report/3.docx"; break;
                case "ECT-涡流检验报告模板RPV": File_url = "/upload_Folder/Lossless_report/4.doc"; break;
                case "ECT-涡流检验报告模板SG": File_url = "/upload_Folder/Lossless_report/5.doc"; break;
                case "LT-氦检漏报告模板_4版123": File_url = "/upload_Folder/Lossless_report/6.doc"; break;
                case "MT-磁轭法和触头法磁粉检验报告3版123": File_url = "/upload_Folder/Lossless_report/7.doc"; break;
                case "MT-磁粉检验报告床式": File_url = "/upload_Folder/Lossless_report/8.doc"; break;
                case "UT-超声波测厚报告": File_url = "/upload_Folder/Lossless_report/10.doc"; break;
                case "UT-超声波检验报告1-正页": File_url = "/upload_Folder/Lossless_report/11.doc"; break;
                case "水压试验报告模板21": File_url = "/upload_Folder/Lossless_report/12.doc"; break;
                case "自动超声波检验报告1-检验报告": File_url = "/upload_Folder/Lossless_report/13.doc"; break;
                case "PT-液体渗透检验报告": File_url = "/upload_Folder/Lossless_report/26.doc"; break;
                case "RT-射线检验报告1": File_url = "/upload_Folder/Lossless_report/27.doc"; break;
		        default:
                    break;
	        }
            context.Response.Write(File_url);//返回给前台页面   
            context.Response.End();
        }

        private void report_Edit(HttpContext context)
        {
            string field_name = context.Request.Params.Get("field_name");
            string File_url = "";

            switch (field_name)
            {
                case "热处理模板3": File_url = "/upload_Folder/Lossless_report/1.docx，.docx"; break;
                case "目视报告模板_VT_63": File_url = "/upload_Folder/Lossless_report/2.doc，.doc"; break;
                case "DT模板空白_5版": File_url = "/upload_Folder/Lossless_report/3.docx，.docx"; break;
                case "ECT-涡流检验报告模板RPV": File_url = "/upload_Folder/Lossless_report/4.doc，.doc"; break;
                case "ECT-涡流检验报告模板SG": File_url = "/upload_Folder/Lossless_report/5.doc，.doc"; break;
                case "LT-氦检漏报告模板_4版123": File_url = "/upload_Folder/Lossless_report/6.doc，.doc"; break;
                case "MT-磁轭法和触头法磁粉检验报告3版123": File_url = "/upload_Folder/Lossless_report/7.doc，.doc"; break;
                case "MT-磁粉检验报告床式": File_url = "/upload_Folder/Lossless_report/8.doc，.doc"; break;
                case "UT-超声波测厚报告": File_url = "/upload_Folder/Lossless_report/10.doc，.doc"; break;
                case "UT-超声波检验报告1-正页": File_url = "/upload_Folder/Lossless_report/11.doc，.doc"; break;
                case "水压试验报告模板21": File_url = "/upload_Folder/Lossless_report/12.doc，.doc"; break;
                case "自动超声波检验报告1-检验报告": File_url = "/upload_Folder/Lossless_report/13.doc，.doc"; break;
                case "PT-液体渗透检验报告": File_url = "/upload_Folder/Lossless_report/26.doc，.doc"; break;
                case "RT-射线检验报告1": File_url = "/upload_Folder/Lossless_report/27.doc，.doc"; break;
                default:
                    break;
            }
             string loginAccount = Convert.ToString(context.Session["loginAccount"]);
             string login_username = Convert.ToString(context.Session["login_username"]);
             Operation_log.operation_log_(loginAccount, login_username, "无损异常报告模板", "无损报告模板" + field_name + "修改");
            context.Response.Write(File_url);//返回给前台页面   
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