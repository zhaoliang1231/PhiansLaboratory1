using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.pageoffice
{
    public partial class SaveFile : System.Web.UI.Page
    {
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();
        protected void Page_Load(object sender, EventArgs e)
        {

            #region 保存文件
            //保存文件类型
            string save_type = Request.QueryString["save_type"];
            string report_id = Request.QueryString["report_id"];
            string report_url = Request.QueryString["report_url"];
            string ReturnNode = Request.QueryString["ReturnNode"];
            string add_date = Request.QueryString["add_date"];
            string addpersonnel = Request.QueryString["addpersonnel"];
            string flag = Request.QueryString["flag"];
            string save_temp = "";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            int szie = fs.FileSize;

            if (szie>0) { 
            switch (save_type)
            {
               // 证书
                case "certificate":
                   save_temp = ConfigurationManager.AppSettings["word_certificate"];               
                  fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);

                  fs.Close();

                    break;
                    //记录
                case "records": 
                    save_temp = ConfigurationManager.AppSettings["test_record"];                
                    fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                    fs.Close();
                    break;
                //无损报告
                case "Lossless_report_":
                     save_temp = ConfigurationManager.AppSettings["Lossless_report_"];            
                     fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                     fs.Close();
                     break;
                //无损报告----报告审核签发页面的保存
                case "Lossless_report_1":
                    // 在编制界面打开不允许保存
                     if (flag != "1") {

                         save_temp = ConfigurationManager.AppSettings["Lossless_report_"];
                         fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                         fs.Close();
                         String select = "select top 1 id from dbo.TB_NDT_RevisionsRecord where report_id='" + report_id + "' order by id desc";
                         string id = db.ExecuteScalar(select).ToString();
                         string update = "update TB_NDT_RevisionsRecord set ReturnNode='" + ReturnNode + "', add_date='" + add_date + "',addpersonnel= '" + addpersonnel + "' where id='" + id + "' ";
                         //string insert = "insert into dbo.TB_NDT_RevisionsRecord (report_id,report_url,ReturnNode,add_date,addpersonnel) values ('" + report_id + "','" + report_url + "','" + ReturnNode + "','" + add_date + "','" + addpersonnel + "')";
                         try
                         {
                             db.BeginTransaction();
                             db.ExecuteNonQueryByTrans(update);
                             db.CommitTransacton();
                         }
                         catch (System.Exception ex)
                         {
                             db.RollbackTransaction();
                             Response.Write(ex);

                         }
                     }
               
                 
                     break;

                //无损报告
                case "sample_record":
                     save_temp = "/upload_Folder/sample_record/";
                     fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                     fs.Close();
                     break;
                //报告模板
                case "certificate_Template":
                     save_temp = "/upload_Folder/Lossless_report/";
                     fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                     fs.Close();
                     break;
            }
            }

            #endregion

        }
    }
}