using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.test
{
    public partial class test_word_view : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             // 设置PageOffice组件服务页面       
        PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
        PageOfficeCtrl1.Caption = "演示：文件在线安全浏览";
        PageOfficeCtrl1.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
        PageOfficeCtrl1.AllowCopy = false;//禁止拷贝
        PageOfficeCtrl1.Menubar = false;
        PageOfficeCtrl1.OfficeToolbars = false;
        PageOfficeCtrl1.CustomToolbar = false;
        // 设置保存文件页面     
        // 打开文档
        PageOfficeCtrl1.WebOpen(Server.MapPath("/upload_Folder/contract_word/检测报告普通版--po_书签.doc"), PageOffice.OpenModeType.docNormalEdit, "Tom");
    
        }
    }
}