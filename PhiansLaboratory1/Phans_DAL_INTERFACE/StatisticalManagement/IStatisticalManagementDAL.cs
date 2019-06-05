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
    public interface IStatisticalManagementDAL
    {
        #region 统计管理

        #region 无损获取统计数据
        List<TB_StatisticsCount> GetStatistical(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 统计列表数据
        List<TB_NDT_report_title> Report_ArrangeList(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 获取科室组
        List<TB_group> GetGroup(Guid GroupParentId);
        #endregion

        #region 获取逾期类型
        List<TB_DictionaryManagement> GetDicitionaryData(Guid DicitionaryParentId);
        #endregion

        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_TemplateFile> GetReportTemplate();

        #endregion

        #endregion

        #region 个人工作量统计显示

        #region 个人工作量统计显示
        /// <summary>
        /// 个人工作量统计显示
        /// </summary>
        /// <param name="PageModel">搜索条件</param>
        /// <param name="totalRecord">搜索数量</param>
        /// <returns></returns>
        List<TB_NDT_report_title> LoadPersonnelTaskStatistics(TPageModel PageModel, out int totalRecord);
        #endregion

        #endregion
    }
}
