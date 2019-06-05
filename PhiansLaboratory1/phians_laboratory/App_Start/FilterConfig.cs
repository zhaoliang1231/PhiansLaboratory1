using phians_laboratory.App_Start;
using phians_laboratory.custom_class.ActionFilters;
using System.Web;
using System.Web.Mvc;

namespace phians_laboratory
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
            //错误的处理路由
            filters.Add(new BaseHandleErrorAttribute());
            //错误日志记录
            filters.Add(new StatisticsTrackerAttribute());
        }
    }
}