using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    public partial class test_records_view_word : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string save_type = Request.QueryString["save_type"];
            view_test_records.ServerPage = "/pageoffice/server.aspx";
            view_test_records.Menubar = false; //隐藏菜单栏        
            view_test_records.AddCustomToolButton("保存", "Save", 1);
            view_test_records.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);
            view_test_records.AddCustomToolButton("全屏", "SetFullScreen", 4);

            view_test_records.SaveFilePage = "/pageoffice/SaveFile.aspx?save_type=" + save_type + "";
            view_test_records.Caption = "检测记录编辑";
            //string tempPath = Server.MapPath("/upload_Folder/record_Template/65138615-8705-49e7-b41f-957aa917ef2a.doc");
            //view_test_records.WebOpen(tempPath, PageOffice.OpenModeType.docNormalEdit, "admins");
        }
    }
}