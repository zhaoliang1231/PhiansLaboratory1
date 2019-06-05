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
    class ProjectReviewDAL : IProjectReviewDAL
    {

        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_TemplateFile> LoadReportTemplate(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select tf.*,ui.UserName as AddPersonnel_n,TB_DM.Project_name as FileType_n from dbo.TB_TemplateFile tf");
            sql.Append(" left join dbo.TB_UserInfo ui on tf.AddPersonnel=ui.UserCount ");
            sql.Append(" left join dbo.TB_DictionaryManagement TB_DM on tf.FileType=TB_DM.id ");
            sql.Append(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
            }

            //状态为在用
            sql.Append(" and tf." + PageModel.SearchList_[1].Search + " = @0 ", PageModel.SearchList_[1].Key);

            sql.OrderBy(" tf.id desc ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_TemplateFile>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
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
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddTemplate)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.FileNum;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    var selectsql = PetaPoco.Sql.Builder;
                    selectsql.Append(" select * from TB_TemplateFile where FileNum = @0 and state=1", model.FileNum);//在用文件的编号是否存在

                    TB_TemplateFile TB_TemplateFile=db.FirstOrDefault<TB_TemplateFile>(selectsql);

                    if (TB_TemplateFile != null) 
                    {
                        ResultModel.Success = 0;
                        ResultModel.returncontent = "文件编号重复！";
                        return ResultModel;
                    }

                    db.BeginTransaction();

                    db.Insert(model);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "添加成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    ResultModel.Success = 0;
                    ResultModel.returncontent = E.ToString();
                    throw;

                    //throw;
                }

            }

            return ResultModel;

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
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditTemplate)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.FileNum;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    string[] updatefiled = { "FileName", "ReviewLevel", "Remark", "FileNum", "FileType" };
                    db.Update(model, updatefiled);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "修改成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    ResultModel.Success = 0;
                    ResultModel.returncontent = E.ToString();
                    throw;

                    //throw;
                }

            }

            return ResultModel;

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
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelTemplate)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.FileNum;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    #region 判断文件是否可删除

                    if (model.ID < 28) //id小于28的都不可删除
                    {
                        ResultModel.Success = 0;
                        ResultModel.returncontent = "该模板不能删除！";
                        return ResultModel;
                    }

                    #endregion
                    var selectsql = PetaPoco.Sql.Builder;
                    selectsql.Append(" select * from TB_PersonnelQualification where TemplateID = @0", model.ID);//在用文件的编号是否存在

                    TB_PersonnelQualification TB_TemplateFile = db.FirstOrDefault<TB_PersonnelQualification>(selectsql);
                    if (TB_TemplateFile!=null) //不为空则不能删除
                    {
                        ResultModel.Success = 0;
                        ResultModel.returncontent = "该模板已分配权限不能删除！";
                        return ResultModel;
                    }
                    string[] updatefiled = { "state"};
                    db.Update(model, updatefiled);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "删除成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    ResultModel.Success = 0;
                    ResultModel.returncontent = E.ToString();
                    throw;
                }

            }

            return ResultModel;
        }

        #endregion

    }
}
