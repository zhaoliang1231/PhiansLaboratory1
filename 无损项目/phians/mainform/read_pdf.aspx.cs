using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    public partial class read_pdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //PDFCtrl1.ServerPage = "/pageoffice/server.aspx";
            //string url = Request["url"];
            //PDFCtrl1.Theme = PageOffice.ThemeType.Office2007;
            //PDFCtrl1.WebOpen(url);
            ////PDFCtrl1.WebOpen(Server.MapPath(url));
        }
        protected void PDFCtrl1_Load(object sender, EventArgs e)
        {
            string url = Request["url"];
            PDFCtrl1.Titlebar = false; //隐藏标题栏
            PDFCtrl1.Menubar = false; //隐藏菜单栏
            PDFCtrl1.CustomToolbar = false; //隐藏自定义工具栏
            // 按键说明：光标键、Home、End、PageUp、PageDown可用来移动或翻页；数字键盘+、-用来放大缩小；数字键盘/、*用来旋转页面。
            PDFCtrl1.ServerPage ="/pageoffice/server.aspx";
            PDFCtrl1.Theme = PageOffice.ThemeType.Office2007;
            //PDFCtrl1.TitlebarColor = Color.Green;
            //PDFCtrl1.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            //PDFCtrl1.AddCustomToolButton("打印", "Print()", 6);
            //PDFCtrl1.AddCustomToolButton("-", "", 0);
            //PDFCtrl1.AddCustomToolButton("显示/隐藏书签", "SwitchBKMK()", 0);
            //PDFCtrl1.AddCustomToolButton("实际大小", "SetPageReal()", 16);
            //PDFCtrl1.AddCustomToolButton("适合页面", "SetPageFit()", 17);
            //PDFCtrl1.AddCustomToolButton("适合宽度", "SetPageWidth()", 18);
            //PDFCtrl1.AddCustomToolButton("-", "", 0);
            //PDFCtrl1.AddCustomToolButton("首页", "FirstPage()", 8);
            //PDFCtrl1.AddCustomToolButton("上一页", "PreviousPage()", 9);
            //PDFCtrl1.AddCustomToolButton("下一页", "NextPage()", 10);
            //PDFCtrl1.AddCustomToolButton("尾页", "LastPage()", 11);
            //PDFCtrl1.AddCustomToolButton("-", "", 0);
            //PDFCtrl1.AddCustomToolButton("左转", "RotateLeft()", 12);
            //PDFCtrl1.AddCustomToolButton("右转", "RotateRight()", 13);
            //PDFCtrl1.AddCustomToolButton("-", "", 0);
            //PDFCtrl1.AddCustomToolButton("放大", "ZoomIn()", 14);
            //PDFCtrl1.AddCustomToolButton("缩小", "ZoomOut()", 15);
            //PDFCtrl1.AddCustomToolButton("-", "", 0);
            //PDFCtrl1.AddCustomToolButton("全屏", "SwitchFullScreen()", 4); 
            //PDFCtrl1.AllowCopy = false;

            PDFCtrl1.WebOpen(url);
        }
    }
}