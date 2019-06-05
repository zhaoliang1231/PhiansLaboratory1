using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phians_laboratory.Controllers
{
    public class WebCamController : Controller
    {
        //
        // GET: /WebCam/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestRecord()
        {
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
        public ActionResult html5cam()
        {
            return View();
        }
        [HttpPost]
        #region 保存图片
        public ActionResult Upload(string image, FormCollection collection)
        {
            string mtrno = collection["mtrno"];
            image = image.Substring("data:image/png;base64,".Length);
            var buffer = Convert.FromBase64String(image);
            // TODO: I am saving the image on the hard disk but
            // you could do whatever processing you want with it
            System.IO.File.WriteAllBytes(Server.MapPath("~/app_data/capture.png"), buffer);
            return Json(new { success = true });
        }
        #endregion
    }
}
