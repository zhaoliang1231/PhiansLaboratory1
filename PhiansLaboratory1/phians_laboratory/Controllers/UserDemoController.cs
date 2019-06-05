using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Phians_BLL;
using Phians_Entity;
using System.Collections.Generic;

namespace phians_laboratory.Controllers
{
    public class UserDemoController : Controller
    {
        //
        // GET: /UserDemo/

        private readonly UserDemoBLL _userDemoBll = new UserDemoBLL();

        public ActionResult Index()
        {
            return View();
        }

        #region 返回都是json
        public JsonResult AddUser(FormCollection collection)
        {
            UserDemo model = new UserDemo
            {
                UserName = collection["userName"],
                UserCount = collection["userCount"]
            };
            bool flag = _userDemoBll.AddUser(model);
            if (flag)
            {
                return Json(new ExecuteResult(true, "添加成功"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            else
            {
                return Json(new ExecuteResult(false, "添加失败"));
            }
            //简写
            //return Json(new ExecuteResult(flag, flag ? "添加成功" : "添加失败"));

        }

        public JsonResult GetUser(int id)
        {

            try
            {
                UserDemo model = _userDemoBll.GetUser(id);
                return Json(new SingleExecuteResult<UserDemo>(true, "成功", model));//返回实体的结果{"Success":true,"Messege":"成功","Data":{}}
            }
            catch (Exception e)
            {
                return Json(new SingleExecuteResult<UserDemo>(false, "失败", null));
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserList()
        {
            try
            {
                List<UserDemo> list = _userDemoBll.GetUserList();
                return Json(new ListExecuteResult<UserDemo>(true, "成功", list));//返回实体的结果{"Success":true,"Messege":"成功","Data":[]}
            }
            catch (Exception e)
            {
                return Json(new ListExecuteResult<UserDemo>(false, "失败", null));
            }
        }

        /// <summary>
        /// 获取分页的用户列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="rows">每页大小</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public JsonResult GetPageUserList(int page, int rows, int key)
        {
            try
            {
                int totalRecord;
                List<UserDemo> list = _userDemoBll.GetUserPageList(page, rows, key, out totalRecord);
                PagedResult<UserDemo> pagelist = new PagedResult<UserDemo>(totalRecord, list,true);//转换成easyui json
                return Json(pagelist);//返回easyUI json
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }
        }

        #endregion


        #region 返回json字符串

        public string AddUser2(FormCollection collection)
        {
            UserDemo model = new UserDemo
            {
                UserName = collection["userName"],
                UserCount = collection["userCount"]
            };
            bool flag = _userDemoBll.AddUser(model);
            if (flag)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(true, "添加成功"));//返回执行结果 ，添加成功可以换成需要的内容
            }
            else
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "添加失败"));
            }
            //简写
            //return Json(new ExecuteResult(flag, flag ? "添加成功" : "添加失败"));

        }

        public string GetUser2(int id)
        {

            try
            {
                UserDemo model = _userDemoBll.GetUser(id);
                return JsonConvert.SerializeObject(new SingleExecuteResult<UserDemo>(true, "成功", model));//返回实体的结果{"Success":true,"Messege":"成功","Data":{}}
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<UserDemo>(false, "失败", null));
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public string GetUserList2()
        {
            try
            {
                List<UserDemo> list = _userDemoBll.GetUserList();
                return JsonConvert.SerializeObject(new ListExecuteResult<UserDemo>(true, "成功", list));//返回实体的结果{"Success":true,"Messege":"成功","Data":[]}
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ListExecuteResult<UserDemo>(false, "失败", null));
            }
        }

        /// <summary>
        /// 获取分页的用户列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="rows">每页大小</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public string GetPageUserList2(int page, int rows, int key)
        {
            try
            {
                int totalRecord;
                List<UserDemo> list = _userDemoBll.GetUserPageList(page, rows, key, out totalRecord);
                PagedResult<UserDemo> pagelist = new PagedResult<UserDemo>(totalRecord, list,true);//转换成easyui json
                return JsonConvert.SerializeObject(pagelist);//返回easyUI json
            }
            catch (Exception e)
            {
                return "{'total':0,'rows':[]}";//异常返回无数据
            }
        }

        #endregion

    }
}
