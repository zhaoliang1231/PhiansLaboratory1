using Newtonsoft.Json;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace phians_laboratory.Controllers
{
    public class GetAuthorityController : Controller
    {
        //
        // GET: /GetAuthority/
        #region  无权访问返回视图
        public ActionResult AuthorityView()
        {
            return View();
        }
        #endregion
        #region  无权访问返回信息
        public string Authorityshow()
        {
            return JsonConvert.SerializeObject(new ExecuteResult(false, "无权操作！"));
        }
        #endregion
        #region  无权访问返回信息
        public string UnLogin()
        {
            return JsonConvert.SerializeObject(new ExecuteResult(false, "Need to login again"));
        }
        #endregion



    }
}
