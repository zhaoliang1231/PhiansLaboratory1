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
    public class QualificationManagementBLL
    {
        IQualificationManagementDAL dal = DALFactory.GetQualificationManagement();

        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_TemplateFile> LoadReportList(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadReportList(PageModel, out totalRecord);
        }
        #endregion

        #region 加载人员信息
        public List<TB_UserInfo> GetAllUserList(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetAllUserList(PageModel, out totalRecord);
        }
        #endregion


        #region 加载已授权人员信息
        public List<TB_PersonnelQualification> GetQualificationUserList(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetQualificationUserList(PageModel, out totalRecord);
        }
        #endregion

        #region 添加模板权限
        /// <summary>
        /// 添加模板权限
        /// </summary>
        /// <param name="model">添加实体</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <returns></returns>
        public ReturnDALResult AddQualificationPerson(TB_PersonnelQualification model, Guid LogPersonnel)
        {
            return dal.AddQualificationPerson(model, LogPersonnel);
        }
        #endregion

        #region 删除模板权限
        /// <summary>
        /// 添加模板权限
        /// </summary>
        /// <param name="model">删除实体</param>
        /// <param name="UserId">操作人</param>
        /// <returns></returns>
        public ReturnDALResult DelQualificationPerson(TB_PersonnelQualification model, Guid UserId)
        {
            return dal.DelQualificationPerson(model, UserId);
        }
        #endregion
    }
}
