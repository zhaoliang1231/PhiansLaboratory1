using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    public partial class print_form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageOfficeCtrl1.ServerPage = "/pageoffice/server.aspx";
            PageOfficeCtrl1.AddCustomToolButton("打印", "Print()", 6);
            PageOfficeCtrl1.AddCustomToolButton("打印预览", "print_reports", 6);
            PageOfficeCtrl1.Titlebar = false; //隐藏标题栏
            PageOfficeCtrl1.Menubar = false; //隐藏菜单栏
            PageOfficeCtrl1.OfficeToolbars = false; //隐藏Office工具栏
            //PageOfficeCtrl1.CustomToolbar = false; //隐藏自定义工具栏
        }
    }
}