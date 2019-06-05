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
    public class GetFileBLL
    {
        IOfficeOperateDAL dal = DALFactory.GetFile();

        public CommonFileModel GetFile(TFileModel FileModel)
        {
            CommonFileModel model = dal.GetFile(FileModel);
            return model;
        }

        #region 保存版本word信息
        /// <summary>
        /// 保存版本word信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <returns>结果类</returns>
        public ReturnDALResult SaveRevisionsRecord(TB_NDT_RevisionsRecord model, PersonnelQualificationModel PQModel)
        {
            return dal.SaveRevisionsRecord(model, PQModel);

        }

        #endregion

        #region 获取历史保存痕迹报告信息
        /// <summary>
        /// 获取历史保存痕迹报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <returns>结果类</returns>
        public TB_NDT_RevisionsRecord GetWordHistory(int id)
        {
           return dal.GetWordHistory(id);

        }

        #endregion

        #region 添加报告模板修改日志
        /// <summary>
        /// 添加报告模板修改日志
        /// </summary>
        /// <param name="FileNum">模板编号</param>
        /// <returns>结果类</returns>
        public ReturnDALResult SaveOperationLog(string FileNum, Guid Operator)
        {
            return dal.SaveOperationLog(FileNum, Operator);

        }

        #endregion

        #region 获取异常报告信息
        /// <summary>
        /// 获取异常报告信息
        /// </summary>
        /// <param name="id">异常报告id</param>
        /// <returns>结果类</returns>
        public TB_NDT_error_Certificate LoadErrorReport(int id)
        {
            return dal.LoadErrorReport(id);

        }

        #endregion


    }
}
