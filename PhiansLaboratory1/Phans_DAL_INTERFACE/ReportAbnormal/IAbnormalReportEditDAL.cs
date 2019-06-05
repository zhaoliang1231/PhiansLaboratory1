using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity.Common;

namespace Phans_DAL_INTERFACE
{
    public interface IAbnormalReportEditDAL
    {

        #region 异常报告编制
        ReturnDALResult SubmitAbnormalReportReview(TB_NDT_error_Certificate Model,TB_ProcessRecord TB_ProcessRecord, TB_NDT_report_title reportModule, Guid Operator, string PageType);
  
        #endregion

        #region 加载异常申请信息
        /// <summary>
        /// 加载异常申请信息
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="PageModel">页面传参</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="currentState">当前任务状态</param>)
        /// <returns>返回项目信息实体集</returns>
        /// 
        List<TB_NDT_error_Certificate> GetUnusualCertificateList(TPageModel PageModel, out int totalRecord);
        #endregion


        #region 退回编制
        ReturnDALResult BackAbnormalReviewReport(TB_NDT_error_Certificate module,TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel);
        #endregion
    }
}
