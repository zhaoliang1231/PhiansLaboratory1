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
    class QualificationManagementDAL:IQualificationManagementDAL
    {

        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_TemplateFile> LoadReportList(TPageModel PageModel, out int totalRecord)
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

        #region 加载人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_UserInfo> GetAllUserList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select UI.* ");
            sql.Append(" from TB_UserInfo UI  ");
            sql.Append(" where UI.CountState !=0  ");
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {

                sql.Append(" and UI." + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }
            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[1].Key))
            {
                //签发权限
                if (PageModel.SearchList_[1].Key=="1")
                {
                    sql.Append(" and  UI.UserId in(select UserId from  TB_groupAuthorization where  GroupId=@0)", "F967C5C6-BF07-47D0-8383-032C12E7DEE2");
                }
                
            }
            sql.OrderBy("UI." + PageModel.SortName + " " + PageModel.SortOrder);


            // sql.Append(" ORDER BY @0 @1", PageModel.SortName,PageModel.SortOrder);


            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_UserInfo>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 加载已授权人员信息
        public List<TB_PersonnelQualification> GetQualificationUserList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select TP.*,UI.UserName as UserName,UI.UserCount as UserCount  from TB_PersonnelQualification TP");
            sql.Append(" left join TB_UserInfo UI on TP.UserId=UI.UserId");
            sql.Append(" where TP.AuthorizationType=@0", PageModel.SearchList_[1].Key);//权限类型

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and UI." + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");//搜索功能
            }
            if (!string.IsNullOrEmpty(PageModel.SearchList_[2].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[2].Key))
            {
                sql.Append("and TP.TemplateID=@0", PageModel.SearchList_[2].Key);//模板ID
            }
            
            sql.OrderBy("UI." + PageModel.SortName + " " + PageModel.SortOrder);

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_PersonnelQualification>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 添加模板资质权限
        /// <summary>
        /// 添加模板权限
        /// </summary>
        /// <param name="model">添加实体</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <returns></returns>
        public ReturnDALResult AddQualificationPerson(TB_PersonnelQualification model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddQualificationPerson)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.TemplateID + AllInfo[2]+model.UserId;
            
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    var selectsql = PetaPoco.Sql.Builder;
                    selectsql.Append(" select * from TB_PersonnelQualification where AuthorizationType = @0 and UserId=@1 and TemplateID=@2", model.AuthorizationType, model.UserId,model.TemplateID);//在用文件的编号是否存在

                    TB_PersonnelQualification PersonnelQualification = db.FirstOrDefault<TB_PersonnelQualification>(selectsql);

                    if (PersonnelQualification != null)
                    {
                        ResultModel.Success = 0;
                        ResultModel.returncontent = "权限已添加！";
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

        #region 删除模板资质权限
        /// <summary>
        /// 删除模板文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult DelQualificationPerson(TB_PersonnelQualification model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelQualificationPerson)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.TemplateID + AllInfo[2] + model.UserId;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    string num = db.Delete<TB_PersonnelQualification>(model).ToString();
                    db.CompleteTransaction();

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
