using Newtonsoft.Json;
using Phians_BLL;
using Phians_Entity;
using System;
using System.Web.Http;

namespace phians_laboratory.Controllers
{
    public class CheckAuthorityController : ApiController
    {
        #region 检查权限 
       /// <summary>
        /// 检查权限 
       /// </summary>
       /// <param name="UserId"> 用户id格式</param>
       /// <param name="RequestUrl">请求的连接连格式 /controller/action</param>
       /// <returns></returns>
         [HttpPost]
        public dynamic Check(CheckParam param)
        {
            bool Successflag = false;
            try
            {
                Guid newUserId = new Guid(param.UserId);
                CommonBLL CommonBLL_ = new CommonBLL();
                Successflag = CommonBLL_.GetAuthorization_BLL(newUserId, param.RequestUrl);
               if (Successflag)
                   return new ExecuteResult(Successflag, "有权限访问");
                else
                   return new ExecuteResult(Successflag, "无权限访问，请联系管理员授权！");
            }
            catch (Exception e)
            {

                return new ExecuteResult(Successflag, e.ToString());
            }
        }

        public class CheckParam
        {
            public string UserId { get; set; }

            public string RequestUrl { get; set; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TestStartDate">测试时间开始时间</param>
        /// <param name="TestEndDate">测试结束时间</param>
        /// <param name="Operator">操作人</param>
        /// <returns></returns>
        /// 
      [HttpPost]
        public dynamic testConfirmTestItem(paramsModel paramsModel)
        {

            try
            {
                Convert.ToDateTime(paramsModel.TestStartDate);
                Convert.ToDateTime(paramsModel.TestEndDate);
                return JsonConvert.SerializeObject(new ExecuteResult(true, paramsModel.Operator + "操作成功"));
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject( new ExecuteResult(false, e.ToString()));
               
            }
           
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="TestStartDate">测试时间开始时间</param>
      /// <param name="TestEndDate">测试结束时间</param>
      /// <param name="Operator">操作人</param>
      /// <returns></returns>
      /// 
      [HttpPost]
      public dynamic confirmTestItem2(string TestStartDate, string TestEndDate, string Operator)
      {

          try
          {
              Convert.ToDateTime(TestStartDate);
              Convert.ToDateTime(TestEndDate);
              return JsonConvert.SerializeObject(new ExecuteResult(true, Operator + "操作成功"));
          }
          catch (Exception e)
          {
              return JsonConvert.SerializeObject(new ExecuteResult(false, e.ToString()));

          }

      }
        
    }
    //接口参数model
   public class paramsModel {
        //测试时间开始时间
       public string TestStartDate
        {
            get;
            set;
        }
        /// <param name="TestEndDate">测试结束时间</param>
       public string TestEndDate
        {
            get;
            set;
        }
        /// <param name="Operator">操作人</param>
       public string Operator
        {
            get;
            set;
        }
        
    }
}