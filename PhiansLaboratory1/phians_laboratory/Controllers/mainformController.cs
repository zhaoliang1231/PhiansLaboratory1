using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Phians_BLL;
using PhiansCommon;
using phians_laboratory.custom_class;
using phians_laboratory.custom_class.ActionFilters;
using Phians_Entity;
using Newtonsoft.Json;
using System.Text;
using Phians_Entity.Common;
using Microsoft.AspNet.SignalR;
namespace phians_laboratory.Controllers
{
    public class mainformController : BaseController
    {
        MainfromBLL MBLL = new MainfromBLL();
        //
        // GET: /mainform/
        OperationUserBLL _userBll = new OperationUserBLL();

     
        public ActionResult Index()
        {
            string UserName = DES_.StringDecrypt(CookiesHelper.GetCookieValue("UserName").ToString());
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
           
          
            //判断是否已经登陆过
            //if (string.IsNullOrEmpty(SessionName))
            //{
                

            //    Response.Redirect("/login/index");
            //    return View();
            //}
            //else
            //{
            TB_UserInfo model = _userBll.GetUserByParam(UserId);

            ViewData["login_username"] = UserName;
            ViewData["login_usercount"] = model.UserId;

            if (!System.IO.File.Exists(Server.MapPath(model.Portrait)))
            {
                ViewData["Portrait"] = "/image/userHead.png";//默认头像

            }
            else
            {
                ViewData["Portrait"] = model.Portrait;

            }
                string now_date = DateTime.Today.ToString("yyyy");
                ViewData["root_context"] = " @2009-" + now_date + " 广东清大菲恩工业数据技术有限公司";
                return View();
            //}
        }
        // GET: /mainform/
  
        public ActionResult main()
        {
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
         // //string tabletitlename="test";
         // string tabletitle = " <thead class=\"thin-border-bottom\"><tr><th><i class=\"ace-icon fa fa-caret-right blue\"></i>" + "待办事项" + "</th><th><i class=\"ace-icon fa fa-caret-right blue\"></i>" + "链接" + "</th><th class=\"hidden-480\"><i class=\"ace-icon fa fa-caret-right blue\"></i>" + "待办数量" + "</tr></thead>";
         // ViewBag.testHtml = tabletitle;

         // string contentshow = "";
         //// string content = "content1";

         // List<TB_PendingTransaction> PendingTransactionList = new OperationUserBLL().GetPendingTransaction(UserId);

         // for (int i = 0; i < PendingTransactionList.Count; i++)
         // {

         //     contentshow += "<tr><td>" + PendingTransactionList[i].TransactionName + "</td><td> <b class=\"blue\"><a onclick=\" window.parent.addTabs({ id: \'" + PendingTransactionList[i].PageId + "\', icon: \'menu-icon fa fa-glass\', title: \'" + PendingTransactionList[i].TransactionName + "\', url: \'" + PendingTransactionList[i].TransactionUrl + "\', close: true });\">View</a></b></td><td class=\"hidden-480\"><span class=\"label label-success arrowed-in arrowed-in-right\">" + PendingTransactionList[i].TransactionCount + "</span></td> </tr>";
         // }
         // ViewBag.contentshowHtml = contentshow;

          #region 未读消息
          int totalRecord;
          List<TB_Message> list = new MessageBLL().GetUnReadMessage(1, 5, out totalRecord, UserId, 0, "", "");
          string messge = "";
          for (int i = 0; i < list.Count; i++) {

              messge += " <div class=\"itemdiv dialogdiv\"><div class=\"body\"><div class=\"time\"><i class=\"ace-icon fa fa-clock-o\"></i><span class=\"green\">" + list[i].CreateTime + "</span></div><div class=\"name\"><a href=\"javascript:;\">" + list[i].PushPersonname + "</a></div><div class=\"text\">" + list[i].Message + "</div><div class=\"tools\"><a href=\"javascript:;\" class=\"btn btn-minier btn-info\"><i class=\"icon-only ace-icon fa fa-share\"></i></a></div></div></div>";
          }

          ViewBag.messgeHtml = messge;
          #endregion

          //#region 待审核报告
          //ReportReviewBLL ReportReviewBLL = new ReportReviewBLL();
          //TPageModel PageModel = new TPageModel();
          //PageModel.PageIndex = 1;
          //PageModel.Pagesize = 5;
          ////Guid UserId = new Guid(Session["UserId"].ToString());

          //List<SearchList> SearchList = new List<SearchList>();
          ////下拉框内容
          //SearchList.Add(new SearchList
          //{
          //    Search = "",
          //    Key = ""
          //});
          ////MTR状态　审核状态
          //SearchList.Add(new SearchList { Search = "MTRState", Key = Convert.ToInt32(MTRStateEnum.ReviewReport) });
          //SearchList.Add(new SearchList { Search = "CheckedBy", Key = UserId });
          //PageModel.SearchList_ = SearchList;

          //int totalRecord2;
          //List<TB_MTRInfo> list_model = ReportReviewBLL.GetReviewMTRInfo(PageModel, out totalRecord2);


          //string Reporttitle = " <thead class=\"thin-border-bottom\"><tr><th><i class=\"ace-icon fa fa-caret-right blue\"></i>" + "MTRNO" + "</th><th><i class=\"ace-icon fa fa-caret-right blue\"></i>" + "BU" + "</th><th class=\"hidden-480\"><i class=\"ace-icon fa fa-caret-right blue\"></i>" + "TestItem" + "</tr></thead>";
          //ViewBag.ReporttitleHtml = Reporttitle;

          //string ReportTitleContent = "";

          //for (int i = 0; i < list_model.Count; i++)
          //{

          //    ReportTitleContent += "<tr><td>" + list_model[i].MTRNO + "</td><td> <b class=\"blue\">" + list_model[i].BU + "</b></td><td class=\"hidden-480\"><span class=\"label label-success arrowed-in arrowed-in-right\">" + list_model[i].TestItem + "</span></td> </tr>";
          //}
          //ViewBag.ReportshowHtml = ReportTitleContent;
          //#endregion
          //#region 待签发报告
          //TPageModel PageModel2 = new TPageModel();
          //PageModel2.PageIndex = 1;
          //PageModel2.Pagesize = 5;
          ////Guid UserId = new Guid(Session["UserId"].ToString());

          //List<SearchList> SearchList2 = new List<SearchList>();
          ////下拉框内容
          //SearchList2.Add(new SearchList
          //{
          //    Search = "",
          //    Key = ""
          //});
          ////MTR状态　审核状态
          //SearchList2.Add(new SearchList { Search = "MTRState", Key = Convert.ToInt32(MTRStateEnum.IssueReport) });
          //SearchList2.Add(new SearchList { Search = "ApprovedBy", Key = UserId });
          //PageModel2.SearchList_ = SearchList2;

          //int totalRecord3;
          //List<TB_MTRInfo> list_model2 = ReportReviewBLL.GetReviewMTRInfo(PageModel2, out totalRecord3);


          //string ReportissuedContent = "";
          //for (int i = 0; i < list_model2.Count; i++)
          //{

          //    ReportissuedContent += "<tr><td>" + list_model2[i].MTRNO + "</td><td> <b class=\"blue\">" + list_model2[i].BU + "</b></td><td class=\"hidden-480\"><span class=\"label label-success arrowed-in arrowed-in-right\">" + list_model2[i].TestItem + "</span></td> </tr>";
          //}
          //ViewBag.ReportissuedContentHtml = ReportissuedContent;
          //#endregion
          return View();

        }
        #region 退出系统
        public  ActionResult loginout()
        {
            try
            {
                //Session["loginAccount"] = null;
                CookiesHelper.AddCookie("UserId", System.DateTime.Now.AddDays(-1));
                Response.Redirect("/Login");
              

                return View();  
            }
            catch (Exception)
            {


                return Content("F");
            }

        }

        #endregion

        #region 获取用户菜单
        public ActionResult GetMenuData()
        {
            Guid UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
         
            //return Content(MBLL.GetMenutree(UserId));

            try
            {
                //链接
                string Loadpage = MBLL.GetMenutree(UserId);
                if (Loadpage != null)
                {
                    return Content(Loadpage);
                }
                else
                {
                    return Content("F");
                }
            }
            catch (Exception e)
            {
                return Content("异常");
            }
        }
        #endregion

        #region 修改用户密码
        //[Authentication]
        public string UpdateUserPwd(FormCollection collection)
        {
            TB_UserInfo model = new TB_UserInfo();
            model.UserId = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
            model.UserPwd = DES_.stringEncrypt(DecodeBase64(Encoding.UTF8, collection["UserPwd"]));
            string Original_password = DES_.stringEncrypt(DecodeBase64(Encoding.UTF8, Convert.ToString(collection["Original_password"])));
            if (string.IsNullOrEmpty(model.UserPwd)) {

                return JsonConvert.SerializeObject(new ExecuteResult(false, "密码为空"));//返回执行结果 ，添加成功可以换成需要的内容
            }

            try
            {
                int flag = new OperationUserBLL().ChangePassword(model, Original_password);
                if (flag==2)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "修改成功"));//返回执行结果 ，添加成功可以换成需要的内容

                }
                if (flag == 1) {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "原密码错误"));//返回执行结果 ，添加成功可以换成需要的内容
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "修改失败"));//返回执行结果 ，添加成功可以换成需要的内容
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "修改失败"));
            }


        }
        #endregion

        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }
}
