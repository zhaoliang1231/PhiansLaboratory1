using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    public partial class Lossless_report_word_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageOfficeCtrl1.ServerPage = "/pageoffice/server.aspx";
            PageOfficeCtrl1.Menubar = false; //隐藏菜单栏        
            PageOfficeCtrl1.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            PageOfficeCtrl1.Caption = "检测报告编辑";
            PageOfficeCtrl1.AddCustomToolButton("保存", "Save", 1);
            PageOfficeCtrl1.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);

            PageOfficeCtrl1.AddCustomToolButton("全屏", "SetFullScreen", 4);
            PageOfficeCtrl1.AddCustomToolButton("关闭", "Close", 11);
            PageOfficeCtrl1.SaveFilePage = "/pageoffice/SaveFile.aspx?save_type=Lossless_report_";
        }
    }
}