using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IReportApprovalDAL
    {
        #region 加载报告签发
        /// <summary>
        /// 加载报告签发
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_NDT_report_title> LoadReportIssueList(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 退回报告编制
        /// <summary>
        /// 退回报告编制
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_NDT_error_log"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        ReturnDALResult BackIssueReport(TB_NDT_report_title model, Guid LogPersonnel, List<TB_NDT_error_log> TB_NDT_error_log, TB_ProcessRecord TB_ProcessRecord, int IssueState);

        #endregion

        #region 提交签发报告
        /// <summary>
        /// 提交签发报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        ReturnDALResult SubmitIssueReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel, int IssueState);
        #endregion


    }
}
