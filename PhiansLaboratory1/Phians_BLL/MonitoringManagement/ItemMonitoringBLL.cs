using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
    public class ItemMonitoringBLL
    {
        IItemMonitoringDAL dal = DALFactory.GetItemMonitoring();

        #region 获取报告列表
        /// <summary>
        /// 加载获取报告列表
        /// </summary>
        /// <param name="PageModel">加载的行数页数、搜索内容、MTR单号</param>
        /// <param name="totalRecord">totalRecord</param>
        /// <returns>项目信息表</returns>
        public List<TB_NDT_report_title> load_list(TPageModel PageModel, out int totalRecord)
        {
            return dal.load_list(PageModel, out totalRecord);
        }

        #endregion

        #region 加载错误报告
        public List<TB_NDT_error_Certificate> load_Errorlist(TPageModel PageModel, out int totalRecord)
        {
            return dal.load_Errorlist(PageModel, out totalRecord);
        }
        #endregion
    }
}
