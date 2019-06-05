using Phians_Entity;
using Phians_Entity.Common;
using Phians_Entity.LosslessReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IReportManagementDAL
    {
        #region 加载报告管理信息
        /// <summary>
        /// 加载报告管理信息
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_NDT_report_title> LoadReportManageList(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 添加线下报告信息
        /// <summary>
        /// 添加线下报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        ReturnDALResult AddUnderLineReportInfo(TB_NDT_report_title model, Guid LogPersonnel);

        #endregion

        #region 异常报告申请
        /// <summary>
        /// 异常报告申请
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_NDT_error_Certificate"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        ReturnDALResult SubmitAbnormalReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, TB_NDT_error_Certificate error_Certificate, Guid LogPersonnel);

        #endregion


        #region 批量下载

        #region 选择下载
        /// <summary>
        /// 选择下载
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<ReportManagementDownload> Choosedownload(string ids);

        #endregion

        #region 搜索下载
        /// <summary>
        /// 搜索下载
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<ReportManagementDownload> Searchdownload(dynamic model);

        #endregion

        #endregion
        #region 根据列导出报告
        /// <summary>
        /// 根据列导出报告
        /// </summary>
        /// <param name="search">搜索条件</param>
        /// <param name="key">搜索值</param>
        /// <param name="tempFilePath">导出路径</param>
        /// <param name="type">导出类型</param>
        /// <param name="ids">报告id集合</param>
        /// <param name="columns">列集合</param>
        /// <returns></returns>
        System.Data.DataTable Report_ExportExcl(string search3, string key3, string search2, string key2, string search1, string key1, string search, string key, int type, string ids, string columns);
        #endregion

        #region 页面字段显示List
        /// <summary>
        /// 页面字段显示List
        /// </summary>
        /// <param name="PageId">页面id</param>
        /// <returns></returns>
        List<TB_PageShowCustom> loadPageShowSetting(string PageId);

        #endregion

        #region 获取报告信息
        /// <summary>
        /// 获取报告信息
        /// </summary>
        /// <param name="id">报告id</param>
        /// <returns></returns>
        TB_NDT_report_title LoadReportInfo(int id);

        #endregion
    }
}
