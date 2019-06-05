using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IOfficeOperateDAL
    {
        #region 获取文件

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="TableName">操作表名称</param>
        /// <param name="ColumnName"> 路径的列名</param>
        /// <returns></returns>
        CommonFileModel GetFile(TFileModel FileModel);
        #endregion

        #region 保存版本word信息
        /// <summary>
        /// 保存版本word信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <returns>结果类</returns>
        ReturnDALResult SaveRevisionsRecord(TB_NDT_RevisionsRecord model, PersonnelQualificationModel PQModel);

        #endregion

        #region 获取历史保存痕迹报告信息
        /// <summary>
        /// 获取历史保存痕迹报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <returns>结果类</returns>
        TB_NDT_RevisionsRecord GetWordHistory(int id);

        #endregion

        #region 添加报告模板修改日志
        /// <summary>
        /// 添加报告模板修改日志
        /// </summary>
        /// <param name="FileNum">模板编号</param>
        /// <returns>结果类</returns>
        ReturnDALResult SaveOperationLog(string FileNum, Guid Operator);

        #endregion

        #region 获取异常报告信息
        /// <summary>
        /// 获取异常报告信息
        /// </summary>
        /// <param name="id">异常报告id</param>
        /// <returns>结果类</returns>
        TB_NDT_error_Certificate LoadErrorReport(int id);

        #endregion

    }
}
