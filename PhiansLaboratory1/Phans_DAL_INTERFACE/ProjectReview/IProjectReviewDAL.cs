using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IProjectReviewDAL
    {
        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_TemplateFile> LoadReportTemplate(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 添加模板文件
        /// <summary>
        /// 添加模板文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        ReturnDALResult ReportTemplateAdd(TB_TemplateFile model, Guid LogPersonnel);

        #endregion

        #region 修改模板文件
        /// <summary>
        /// 修改模板文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        ReturnDALResult ReportTemplateEdit(TB_TemplateFile model, Guid LogPersonnel);

        #endregion

        #region 删除模板文件
        /// <summary>
        /// 删除模板文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        ReturnDALResult ReportTemplateDel(TB_TemplateFile model, Guid LogPersonnel);

        #endregion
    }
}
