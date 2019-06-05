using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    /// <summary>
    /// 报告评审interfere
    /// </summary>
    public interface IReportReviewDAL
    {

        #region 加载报告审核
        /// <summary>
        /// 加载报告审核
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_NDT_report_title> LoadReportReviewList(TPageModel PageModel, out int totalRecord);

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
        ReturnDALResult BackReviewReport(TB_NDT_report_title model, Guid LogPersonnel, List<TB_NDT_error_log> TB_NDT_error_log, TB_ProcessRecord TB_ProcessRecord, int AuditState);

        #endregion

        #region 提交审核报告

        #region 获取报告文档版本记录信息
        /// <summary>
        /// 获取报告文档版本记录信息
        /// </summary>
        /// <param name="id">报告id</param>
        /// <returns></returns>
        TB_NDT_RevisionsRecord GetRevisionsRecord(int id);

        #endregion

        #region 提交审核报告
        /// <summary>
        /// 提交审核报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="AuditState">审核状态</param>
        /// <returns></returns>
        ReturnDALResult SubmitReviewReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel,int AuditState);
        #endregion

        #endregion

        #region --退回原因

        #region 加载全部退回原因
        /// <summary>
        /// 加载全部退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_DictionaryManagement> AllErrorInfo(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 添加退回原因
        /// <summary>
        /// 添加退回原因
        /// </summary>
        /// <param name="model"></param>
        /// <param name="report_num"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        ReturnDALResult AddErrorInfo(TB_NDT_error_log model, string report_num, Guid LogPersonnel);

        #endregion

        #region 加载已选退回原因
        /// <summary>
        /// 加载已选退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <param name="id">report_id</param>
        /// <returns></returns>
        List<TB_NDT_error_log> ReturnErrorInfo(TPageModel PageModel, out int totalRecord, int report_id);

        #endregion

        #endregion

        #region 审核人员自己退回未开始签发的报告
        /// <summary>
        /// 审核人员自己退回未开始签发的报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        ReturnDALResult TakeBackReviewReport(TB_NDT_report_title model, Guid LogPersonnel, TB_ProcessRecord TB_ProcessRecord);
        #endregion

        #region 判断人员资质
        /// <summary>
        /// 判断人员资质
        /// </summary>
        /// <param name="model">人员资质信息</param>
        /// <returns></returns>
        ReturnDALResult JudgingPersonnelQualifications(TB_PersonnelQualification model);

        #endregion
    }
}
