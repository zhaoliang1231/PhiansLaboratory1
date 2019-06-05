using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phians_laboratory.Controllers
{
    public class PdfOperateController : Controller
    {
        //
        // GET: /PdfOperate/

        public ActionResult PdfViewer( )
        {

            string FileRrl = Convert.ToString(Request.Params.Get("FileRrl"));
            FileRrl = HttpUtility.UrlDecode(FileRrl);  //utf-8 解码
            ViewData["FileRrl"] = FileRrl;

            return View();
         
        }

    }
}
