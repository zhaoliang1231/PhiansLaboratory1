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
    public class ProjectReviewBLL
    {
        IProjectReviewDAL dal = DALFactory.GetProjectReview();

        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_TemplateFile> LoadReportTemplate(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadReportTemplate(PageModel, out totalRecord);
        }

        #endregion

        #region 添加模板文件
        /// <summary>
        /// 添加模板文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult ReportTemplateAdd(TB_TemplateFile model, Guid LogPersonnel)
        {
            return dal.ReportTemplateAdd(model, LogPersonnel);

        }

        #endregion

        #region 修改模板文件
        /// <summary>
        /// 修改模板文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult ReportTemplateEdit(TB_TemplateFile model, Guid LogPersonnel)
        {
            return dal.ReportTemplateEdit(model, LogPersonnel);

        }

        #endregion

        #region 删除模板文件
        /// <summary>
        /// 删除模板文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult ReportTemplateDel(TB_TemplateFile model, Guid LogPersonnel)
        {
            return dal.ReportTemplateDel(model, LogPersonnel);

        }

        #endregion

    }
}
