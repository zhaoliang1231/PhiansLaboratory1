using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform
{
    public partial class Revisionsshow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string report_url = Request.Params.Get("report_url");//待打开文件路径
            string save_type = Request.QueryString["save_type"];//保存类型
            string report_id = Request.QueryString["report_id"];
            string ReturnNode = Request.QueryString["ReturnNode"];
            string flag = Request.QueryString["flag"];    //判断是否显示保存按钮
            string add_date = DateTime.Now.ToLocalTime().ToString();                    //时间

            string SessionName = Convert.ToString(Session["login_username"]);
            string loginAccount = Convert.ToString(Session["loginAccount"]);
            PageOfficeCtrl1.Titlebar = false; //隐藏标题栏
            PageOfficeCtrl1.Menubar = true; //隐藏菜单栏
            PageOfficeCtrl1.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);

            // 设置PageOffice组件服务页面
            PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "/pageoffice/server.aspx";
            PageOfficeCtrl1.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            //判断是否显示保存按钮
            if (flag  != "1")
            {
                PageOfficeCtrl1.AddCustomToolButton("保存", "Save()", 1);
                PageOfficeCtrl1.OfficeToolbars = false;
            }
           
         
            PageOfficeCtrl1.AddCustomToolButton("关闭", "Close", 11);
            // 设置保存文件页面
            PageOfficeCtrl1.SaveFilePage = "/pageoffice/SaveFile.aspx?save_type=" + save_type + "&report_url=" + report_url + "&report_id=" + 
                report_id + "&add_date=" + add_date + "&ReturnNode=" + ReturnNode + "&addpersonnel=" + loginAccount + "&flag=" + flag;
           //设置在线编制时间20分钟
            PageOfficeCtrl1.TimeSlice = 40;
            // 打开文档
            PageOfficeCtrl1.WebOpen(Server.MapPath( report_url), PageOffice.OpenModeType.docRevisionOnly, SessionName);

        }
    }
}