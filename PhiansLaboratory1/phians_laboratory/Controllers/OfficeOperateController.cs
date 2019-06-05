using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Phians_BLL;
using Phians_Entity;
using phians_laboratory.custom_class;
using PhiansCommon;
using System.Configuration;
using System.Collections.Specialized;
using Phians_Entity.Common;
using PhiansCommon.Enum;

namespace phians_laboratory.Controllers
{
    public class OfficeOperateController : Controller
    {
        //
        // GET: /OfficeOperate/
        public ActionResult Index()
        {
            return View();
        }

        #region 报告预览打印
        /// <summary>
        /// 报告预览打印
        /// </summary>
        /// <returns></returns>

        public ActionResult OfficePrintOrView()
        {

            try
            {
                //string UserCount_ = DES_.StringDecrypt(Request.Params.Get("UserCount_"));//GetCookie用户名

                HttpCookie UserCount = CookiesHelper.GetCookie("UserCount");
                string UserCount_ = "";//GetCookie用户名              
                UserCount_ = DES_.StringDecrypt(UserCount.Value);
                string OperateType = Request.Params.Get("OperateType");//操作类型 Report or Template or ErrorReport
                int id = Convert.ToInt32(Request.Params.Get("id"));//报告id  or 模板id 

                if (UserCount_ == null)
                {
                    ViewBag.EditorHtml = "<h1><strong><span style=color:#E53333;>提示：登录失效，请重新登录！</span></strong> </h1>";
                    return View();
                }

                string fileUrl = "";
                string format = "";
                
                try
                {

                    if (OperateType == "Report")
                    {
                        TB_NDT_report_title model = new ReportEditBLL().Getreport_title(id);//获取报告信息
                        fileUrl = model.report_url;
                        format = model.report_format;
                    }
                    else if (OperateType == "Template")
                    {
                        TB_TemplateFile model = new ReportEditBLL().LoadTemplateFile(id);//获取报告信息
                        fileUrl = model.FileUrl;
                        format = model.FileFormat;
                    }
                    else if (OperateType == "ErrorReport")
                    {
                        TB_NDT_error_Certificate model = new GetFileBLL().LoadErrorReport(id);//获取报告信息
                        fileUrl = model.File_url;
                        format = model.File_format;
                    }


                }
                catch (Exception E)
                {
                    ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">" + E.ToString() + "</span></strong> </h1>";
                    return View();

                }

                //pdf阅读
                if (format.ToLower() == ".pdf")
                {

                    string pathurl = HttpUtility.UrlEncode(fileUrl);  //utf-8 编码
                //    HttpUtility.UrlDecode(text);  //utf-8 解码

                    ViewBag.EditorHtml = "<iframe src=\"/PdfOperate/PdfViewer?FileRrl=" + pathurl + "\"  style=\"width:100%;height:1500;\" id=\"KindeditorIframe\"></iframe>";


                    return View();
                }


                if (!string.IsNullOrEmpty(fileUrl))
                {
                    //判断文件是否存在
                    if (!System.IO.File.Exists(Server.MapPath(fileUrl)))
                    {
                        ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                        return View();

                    }
                  

                }
                else
                {

                    ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                    return View();

                }


                Page page = new Page();
                string controlOutput = string.Empty;
                PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
                pc.ServerPage = "/pageoffice/server.aspx";
                pc.Menubar = false; //隐藏菜单栏        
                // pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
                pc.Caption = "检测报告预览和打印";
                //pc.AddCustomToolButton("保存", "Save", 1);
                pc.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);
                pc.AddCustomToolButton("全屏", "SetFullScreen", 4);
                pc.AddCustomToolButton("打印", "Print", 4);
                pc.AddCustomToolButton("关闭", "Close", 11);
                pc.WebOpen(fileUrl, PageOffice.OpenModeType.docReadOnly, UserCount_);//参数一：文档路径；参数二：文档打开模式；参数三：操作当前文档的用户名。一般来说，UserName 应该采用登录到您的Web应用程序的当前用户的名称
                pc.ID = "PageOfficeCtrl1";
                page.Controls.Add(pc);

                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        Server.Execute(page, htw, false); controlOutput = sb.ToString();
                    }
                }
                ViewBag.EditorHtml = controlOutput;//控件字符串                       
                return View();
            }
            catch (Exception E)
            {
          
                ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：" + E.ToString() + "</span></strong> </h1>";                              
                return View();

            }
        }

        #endregion

        #region 报告可编辑保存
        /// <summary>
        /// word编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult WordEdit()
        {
            string UserCount_ = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
            if (UserCount_ == null)
            {
                ViewBag.EditorHtml = "<h1><strong><span style=color:#E53333;>提示：登录失效，请重新登录！</span></strong> </h1>";
                return View();
            }

            int id = Convert.ToInt32(Request.Params.Get("id"));//报告id  or 模板id

            //保存类型  1）Lossless_report_：报告编辑保存；2）Lossless_report_Error：报告异常页面保存；3）certificate_Template：报告模板保存
            string save_type = Request.Params.Get("save_type");

            string OperateType = Request.Params.Get("OperateType");//操作类型 Report or Template or ErrorReport

            string Condition = Convert.ToString(Request.Params.Get("Condition"));//修改报告状态 只有在可保存的报告需要 

            string fileUrl = "";
            int FileNum = 0;//模板文件编号，用于写日志

            try
            {
                if (OperateType == "Report")
                {
                    TB_NDT_report_title model = new ReportEditBLL().Getreport_title(id);//获取报告信息
                    fileUrl = model.report_url;
                    FileNum = model.id;

                    if (Condition == "true") 
                    {
                        model.Condition = 1;
                        ReturnDALResult ReturnDALResult = new ReportEditBLL().ReportCondition(model);//将报告状态更改成已开始

                        if (ReturnDALResult.Success==0) 
                        {
                            ViewBag.EditorHtml = "<h1><strong><span style=color:#E53333;>提示：写入状态失败</span></strong> </h1>";
                            return View();

                        }
                    }
                }
                else if (OperateType == "Template")
                {
                    TB_TemplateFile model = new ReportEditBLL().LoadTemplateFile(id);//获取报告信息
                    fileUrl = model.FileUrl;
                    FileNum = model.ID;

                }
                else if (OperateType == "ErrorReport")
                {
                    TB_NDT_error_Certificate model = new GetFileBLL().LoadErrorReport(id);//获取报告信息
                    fileUrl = model.File_url;
                    FileNum = model.id;

                }
            }
            catch (Exception E)
            {
                ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">" + E.ToString() + "</span></strong> </h1>";
                return View();

            }

            Page page = new Page();
            string controlOutput = string.Empty;
            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.Menubar = false; //隐藏菜单栏        
            // pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            pc.Caption = "检测报告编辑";
            pc.AddCustomToolButton("保存", "Save", 1);
            pc.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);
            pc.AddCustomToolButton("全屏", "SetFullScreen", 4);
            pc.AddCustomToolButton("关闭", "Close", 11);

            pc.SaveFilePage = "/OfficeOperate/SaveDoc?save_type=" + save_type + "&save_temp=" + fileUrl + "&FileNum=" + FileNum;//报告保存

            if (!string.IsNullOrEmpty(fileUrl))
            {
                //判断文件是否存在
                if (!System.IO.File.Exists(Server.MapPath(fileUrl)))
                {
                    ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                    return View();

                }
                else
                {
                    pc.WebOpen(fileUrl, PageOffice.OpenModeType.docNormalEdit, UserCount_);//参数一：文档路径；参数二：文档打开模式；参数三：操作当前文档的用户名。一般来说，UserName 应该采用登录到您的Web应用程序的当前用户的名称

                }

            }
            else
            {

                ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                return View();

            }
            pc.ID = "PageOfficeCtrl1";
            page.Controls.Add(pc);

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false); controlOutput = sb.ToString();
                }
            }
            ViewBag.EditorHtml = controlOutput;//控件字符串                       
            return View();
        }

        #endregion

        #region 注释方法
        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(string queryString)
        {
            return GetQueryString(queryString, null, true);
        }
        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(string queryString, Encoding encoding, bool isEncoded)
        {
            queryString = queryString.Replace("?", "");
            NameValueCollection result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    if (isEncoded)
                    {
                        result[MyUrlDeCode(key, encoding)] = MyUrlDeCode(value, encoding);
                    }
                    else
                    {
                        result[key] = value;
                    }
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 解码URL.
        /// </summary>
        /// <param name="encoding">null为自动选择编码</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MyUrlDeCode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码                     
                string code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //将已经解码的字符再次进行编码.
                string encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }

        #endregion

        #region 报告只读打开
        /// <summary>
        /// 只读模式打开报告
        /// </summary>
        /// <returns></returns>
        public ActionResult WordRead()
        {
            //string UserCount_ = DES_.StringDecrypt(Request.Params.Get("UserCount_"));//GetCookie用户名
            HttpCookie UserCount = CookiesHelper.GetCookie("UserCount");
            string UserCount_ = "";//GetCookie用户名
            //string url = Request.Url.ToString();
            //Uri uri = new Uri(url);
            //string queryString = uri.Query;
            //NameValueCollection col = GetQueryString(queryString);
            //string searchKey = col["UserCount_"];

            UserCount_ = DES_.StringDecrypt(UserCount.Value);


            string OperateType = Request.Params.Get("OperateType");//操作类型 Report or Template or ErrorReport
            int id = Convert.ToInt32(Request.Params.Get("id"));//报告id  or 模板id 

            if (UserCount_ == null)
            {
                ViewBag.EditorHtml = "<h1><strong><span style=color:#E53333;>提示：登录失效，请重新登录！</span></strong> </h1>";
                return View();
            }

            Page page = new Page();
            string controlOutput = string.Empty;
            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.Menubar = false; //隐藏菜单栏        
            // pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            pc.Caption = "检测报告编辑";
            //pc.AddCustomToolButton("保存", "Save", 1);
            pc.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);
            pc.AddCustomToolButton("全屏", "SetFullScreen", 4);
            pc.AddCustomToolButton("关闭", "Close", 11);
            string fileUrl = "";

            string format = "";
            try
            {

                if (OperateType == "Report")
                {
                    TB_NDT_report_title model = new ReportEditBLL().Getreport_title(id);//获取报告信息
                    fileUrl = model.report_url;
                    format = model.report_format;
                }
                else if (OperateType == "Template")
                {
                    TB_TemplateFile model = new ReportEditBLL().LoadTemplateFile(id);//获取报告信息
                    fileUrl = model.FileUrl;
                    format = model.FileFormat;

                }
                else if (OperateType == "ErrorReport")
                {
                    TB_NDT_error_Certificate model = new GetFileBLL().LoadErrorReport(id);//获取报告信息
                    fileUrl = model.File_url;
                    format = model.File_format;

                }

            }
            catch (Exception E)
            {
                ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">" + E.ToString() + "</span></strong> </h1>";
                return View();

            }


            if (!string.IsNullOrEmpty(fileUrl))
            {
                //判断文件是否存在
                if (!System.IO.File.Exists(Server.MapPath(fileUrl)))
                {
                    ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                    return View();

                }
                else
                {
                    pc.WebOpen(fileUrl, PageOffice.OpenModeType.docReadOnly, UserCount_);//参数一：文档路径；参数二：文档打开模式；参数三：操作当前文档的用户名。一般来说，UserName 应该采用登录到您的Web应用程序的当前用户的名称

                }

            }
            else
            {

                ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                return View();

            }

            if (format.ToLower() == ".pdf") {


                ViewBag.EditorHtml = "<iframe src=\"/PdfOperate/PdfViewer?FileRrl=\"" + fileUrl + " style=\"width:100%;height:390px;\" id=\"KindeditorIframe\"></iframe>";


                return View();                       
            }


            pc.ID = "PageOfficeCtrl1";
            page.Controls.Add(pc);

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false); controlOutput = sb.ToString();
                }
            }
            ViewBag.EditorHtml = controlOutput;//控件字符串                       
            return View();
        }


        #endregion

        #region 痕迹模式只读（显示历史痕迹信息）
        /// <summary>
        /// 痕迹模式打开报告
        /// </summary>
        /// <returns></returns>
        public ActionResult WordRevisionRead()
        {
            //string UserCount_ = DES_.StringDecrypt(Request.Params.Get("UserCount_"));//GetCookie用户名

            HttpCookie UserCount = CookiesHelper.GetCookie("UserCount");
            string UserCount_ = "";//GetCookie用户名
            //string url = Request.Url.ToString();
            //Uri uri = new Uri(url);
            //string queryString = uri.Query;
            //NameValueCollection col = GetQueryString(queryString);
            //string searchKey = col["UserCount_"];

            UserCount_ = DES_.StringDecrypt(UserCount.Value);


            //string save_type = Request.Params.Get("save_type");//保存类型
            int ReturnNode = Convert.ToInt32(Request.Params.Get("ReturnNode"));//保存类型  --6 报告审核更改 ， 7 报告签发更改
            int id = Convert.ToInt32(Request.Params.Get("id"));//痕迹id

            if (UserCount_ == null)
            {
                ViewBag.EditorHtml = "<h1><strong><span style=color:#E53333;>提示：登录失效，请重新登录！</span></strong> </h1>";
                return View();
            }

            TB_NDT_RevisionsRecord model = new GetFileBLL().GetWordHistory(id);//获取报告信息

            Page page = new Page();
            string controlOutput = string.Empty;
            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.Menubar = false; //隐藏菜单栏        
            pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            pc.Caption = "office 文件阅读";
            //pc.AddCustomToolButton("保存", "Save", 1);
            pc.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);
            //pc.AddCustomToolButton("保存", "Save", 1);
            pc.OfficeToolbars = false;
            pc.AddCustomToolButton("全屏", "SetFullScreen", 4);
            pc.AddCustomToolButton("关闭", "Close", 11);
            if (model != null)
            {
                //判断文件是否存在
                if (!System.IO.File.Exists(Server.MapPath(model.report_url)))
                {
                    ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                    return View();

                }
                else
                {
                    pc.WebOpen(model.report_url, PageOffice.OpenModeType.docRevisionOnly, UserCount_);//参数一：文档路径；参数二：文档打开模式；参数三：操作当前文档的用户名。一般来说，UserName 应该采用登录到您的Web应用程序的当前用户的名称
                    //pc.TimeSlice = 40;
                }

            }
            else
            {

                ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                return View();

            }
            pc.ID = "PageOfficeCtrl1";
            page.Controls.Add(pc);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false); controlOutput = sb.ToString();
                }
            }
            ViewBag.EditorHtml = controlOutput;//控件字符串                       
            return View();
        }

        #endregion

        #region 痕迹模式编辑
        /// <summary>
        /// 痕迹模式打开报告
        /// </summary>
        /// <returns></returns>
        public ActionResult WordRevision()
        {
            //string UserCount_ = DES_.StringDecrypt(Request.Params.Get("UserCount_"));//GetCookie用户名

            HttpCookie UserCount = CookiesHelper.GetCookie("UserCount");

            //string UserCount_ = "";//GetCookie用户名
            //string url = Request.Url.ToString();
            //Uri uri = new Uri(url);
            //string queryString = uri.Query;
            //NameValueCollection col = GetQueryString(queryString);
            //string searchKey = col["UserCount_"];

            string UserCount_ = DES_.StringDecrypt(UserCount.Value);
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人

            if (UserCount_ == null)
            {
                ViewBag.EditorHtml = "<h1><strong><span style=color:#E53333;>提示：登录失效，请重新登录！</span></strong> </h1>";
                return View();
            }
            //string save_type = Request.Params.Get("save_type");//保存类型
            int ReturnNode = Convert.ToInt32(Request.Params.Get("ReturnNode"));//保存类型  --6 报告审核更改 ， 7 报告签发更改
            int id = Convert.ToInt32(Request.Params.Get("id"));//报告id

            string Condition = Convert.ToString(Request.Params.Get("Condition"));//修改报告状态 只有在可保存的报告需要 
            string TmId = Convert.ToString(Request.Params.Get("TmId"));//模板id 


            TB_NDT_report_title model = new ReportEditBLL().Getreport_title(id);//获取报告信息

            if (Condition == "true")
            {
                model.Condition = 1;
                ReturnDALResult ReturnDALResult = new ReportEditBLL().ReportCondition(model);//将报告状态更改成已开始

                if (ReturnDALResult.Success == 0)
                {
                    ViewBag.EditorHtml = "<h1><strong><span style=color:#E53333;>提示：写入状态失败</span></strong> </h1>";
                    return View();

                }
            }

            Page page = new Page();
            string controlOutput = string.Empty;
            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.Menubar = false; //隐藏菜单栏        
            // pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            pc.Caption = "office 文件阅读";
            //pc.AddCustomToolButton("保存", "Save", 1);
            pc.AddCustomToolButton("隐藏工具栏", "view_office_toolbar", 1);
            pc.AddCustomToolButton("保存", "Save", 1);
            pc.OfficeToolbars = false;
            pc.AddCustomToolButton("全屏", "SetFullScreen", 4);
            pc.AddCustomToolButton("关闭", "Close", 11);
            pc.SaveFilePage = "/OfficeOperate/SaveDocRevision?save_temp=" + model.report_url + "&ReturnNode=" + ReturnNode + "&report_id=" + id + "&addpersonnel=" + UserCount_ + "&UserId=" + UserId + "&TmId=" + TmId;//报告保存
            if (model != null)
            {
                //判断文件是否存在
                if (!System.IO.File.Exists(Server.MapPath(model.report_url)))
                {
                    ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                    return View();

                }
                else
                {
                    pc.WebOpen(model.report_url, PageOffice.OpenModeType.docRevisionOnly, UserCount_);//参数一：文档路径；参数二：文档打开模式；参数三：操作当前文档的用户名。一般来说，UserName 应该采用登录到您的Web应用程序的当前用户的名称
                    pc.TimeSlice = 40;
                }

            }
            else
            {

                ViewBag.EditorHtml = "<h1><strong><span style=" + "color:#E53333;" + ">提示：文件不存在！</span></strong> </h1>";
                return View();

            }
            pc.ID = "PageOfficeCtrl1";
            page.Controls.Add(pc);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false); controlOutput = sb.ToString();
                }
            }
            ViewBag.EditorHtml = controlOutput;//控件字符串                       
            return View();
        }

        #endregion

        #region 报告保存
        /// <summary>
        /// 保存Word文件
        /// </summary>
        /// <param name="save_type">保存类型</param>
        public void SaveDoc(string save_type, string save_temp, string FileNum)
        {
            Guid Operator = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

            #region 保存文件

            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            int szie = fs.FileSize;

            if (szie > 0)
            {
                switch (save_type)
                {
                    case "Lossless_report_"://报告保存
                        save_temp = ConfigurationManager.AppSettings["Lossless_report_"];
                        fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                        fs.Close();
                        break;
                    case "Lossless_report_Error"://异常报告保存
                        save_temp = ConfigurationManager.AppSettings["Lossless_report_certificate_E"];
                        fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                        fs.Close();
                        break;
                    case "certificate_Template"://模板保存
                        save_temp = ConfigurationManager.AppSettings["Lossless_report"];
                        ReturnDALResult ReturnDALResult = new GetFileBLL().SaveOperationLog(FileNum,Operator);//新增
                        if (ReturnDALResult.Success == 1) 
                        {
                            fs.SaveToFile(Server.MapPath(save_temp) + fs.FileName);
                            fs.Close();
                        }
                        break;
                }
            }

            #endregion


        }

        #endregion

        #region 版本控制报告保存
        /// <summary>
        /// 版本控制报告保存
        /// </summary>
        /// <param name="save_type">保存类型</param>
        public string SaveDocRevision(string save_temp, int ReturnNode, int report_id, string addpersonnel, Guid UserId, int TmId)
        {
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            int szie = fs.FileSize;

            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string flag="";

            #region 人员资质判断信息

            PersonnelQualificationModel PQModel = new PersonnelQualificationModel();

            if (ReturnNode == (int)NDT_RevisionsRecordEnum.ReviewUpdate)
            {
                PQModel.AuthorizationType = 0;
            }
            else if (ReturnNode == (int)NDT_RevisionsRecordEnum.IssusUpdate)
            {
                PQModel.AuthorizationType = 1;
            }

            PQModel.UserId = UserId;//人员guid
            PQModel.TemplateID = TmId;//模板id

            #endregion

            if (szie > 0)
            {
                string TempUrl = ConfigurationManager.AppSettings["Lossless_report_"];
                string report_url = TempUrl + Guid.NewGuid() + ".doc";

                #region 保存版本word信息
                TB_NDT_RevisionsRecord model = new TB_NDT_RevisionsRecord();
                model.add_date = DateTime.Now;
                model.addpersonnel = addpersonnel;
                model.report_id = report_id;
                model.ReturnNode = ReturnNode;
                model.report_url = report_url;

                ReturnDALResult = new GetFileBLL().SaveRevisionsRecord(model, PQModel);//新增

                #endregion
                if (ReturnDALResult.Success == 1)
                {
                    fs.SaveToFile(Server.MapPath(report_url));//保存版本报告
                    fs.Close();
                    flag="1";
                } 
                if (ReturnDALResult.Success == 1)
                {
                  flag="0";
                }

            }
            return flag;

        }

        #endregion

    }
}
