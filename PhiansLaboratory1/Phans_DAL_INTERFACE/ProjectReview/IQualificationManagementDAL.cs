using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IQualificationManagementDAL
    {
        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_TemplateFile> LoadReportList(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 加载所有人员信息
        List<TB_UserInfo> GetAllUserList(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 加载已授权人员信息
        List<TB_PersonnelQualification> GetQualificationUserList(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 添加模板权限
        /// <summary>
        /// 添加模板权限
        /// </summary>
        /// <param name="model">添加实体</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <returns></returns>
        ReturnDALResult AddQualificationPerson(TB_PersonnelQualification model, Guid LogPersonnel);
        #endregion

        #region 删除模板权限
        /// <summary>
        /// 添加模板权限
        /// </summary>
        /// <param name="model">删除实体</param>
        /// <param name="UserId">操作人</param>
        /// <returns></returns>
        ReturnDALResult DelQualificationPerson(TB_PersonnelQualification model, Guid UserId);
        #endregion
    }
}
