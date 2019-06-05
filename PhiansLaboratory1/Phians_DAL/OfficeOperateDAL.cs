using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    public  class OfficeOperateDAL : IOfficeOperateDAL
    {

        #region 获取文件
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="TableName">操作表名称</param>
        /// <param name="ColumnName"> 路径的列名</param>
        /// <returns></returns>
        public CommonFileModel GetFile(TFileModel FileModel)
        {
            var sql = PetaPoco.Sql.Builder;

            switch (FileModel.PrimaryType)
            {
                case "Int": FileModel.PrimaryValue = Convert.ToInt32(FileModel.PrimaryValue); break;
                case "String": FileModel.PrimaryValue = Convert.ToString(FileModel.PrimaryValue); break;
                case "Guid": FileModel.PrimaryValue = new Guid(FileModel.PrimaryValue); break;

            }

            sql.Append("select " + FileModel.FileUrlColumnName + " as FileUrl ," + FileModel.FileFormatColumnName + " as FileFormat  from  " + FileModel.Tablename + "  where " + FileModel.PrimaryKey + " =@0", FileModel.PrimaryValue);
            //sql.Append(" where Parent_Id=@0  ", PageId);

            ////判断如果表格为版本控制表  则应该判断该记录类型 是否为异常报告备份
            //if (FileModel.Tablename == "TB_TotalReportVersion")
            //{
            //    sql.Append(" and OperateType = 2 ");
            //}
            try
            {
                using (var db = DbInstance.CreateDataBase())
                {
                    //PetaPoco框架自带分页
                    CommonFileModel model = db.SingleOrDefault<CommonFileModel>(sql);
                    return model;
                }
            }
            catch (Exception E)
            {

                throw;
            }

        }

        #endregion

        #region 保存版本word信息
        /// <summary>
        /// 保存版本word信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <returns>结果类</returns>
        public ReturnDALResult SaveRevisionsRecord(TB_NDT_RevisionsRecord model, PersonnelQualificationModel PQModel)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            using (var db = DbInstance.CreateDataBase())
            {
                //获取报告编制提交的时间
                var SelectPersonInfo = PetaPoco.Sql.Builder;
                SelectPersonInfo.Append("select * from TB_PersonnelQualification where UserId=@0 and TemplateID=@1 and AuthorizationType=@2", PQModel.UserId, PQModel.TemplateID, PQModel.AuthorizationType);//获取授权信息 人员、模板、授权类型
                TB_PersonnelQualification QualificationInfo = db.FirstOrDefault<TB_PersonnelQualification>(SelectPersonInfo);

                if (QualificationInfo == null) 
                {
                    ResultModel.Success = 0;
                    ResultModel.returncontent = "该报告需要有资质的人员才可操作！";
                }

                db.Insert(model);
                ResultModel.Success = 1;

            }
            return ResultModel;

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
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_RevisionsRecord where id=@0 ", id);

                TB_NDT_RevisionsRecord result = db.FirstOrDefault<TB_NDT_RevisionsRecord>(sql);
                return result;
            }
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
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.SaveOperationLog)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + FileNum;

            using (var db = DbInstance.CreateDataBase())
            {
                //添加日志
                string operation_log_sql = CommonDAL.operation_log_(Operator, OperationType, OperationInfo);
                int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                ResultModel.Success = 1;

            }
            return ResultModel;

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
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_error_Certificate where id=@0 ", id);

                TB_NDT_error_Certificate result = db.FirstOrDefault<TB_NDT_error_Certificate>(sql);
                return result;
            }
        }

        #endregion
    }
}
