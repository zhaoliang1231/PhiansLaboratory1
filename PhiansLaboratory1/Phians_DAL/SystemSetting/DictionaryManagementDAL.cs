using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class DictionaryManagementDAL : IDictionaryManagementDAL
    {
        #region 获取树节点
        public List<TB_DictionaryManagement> LoadPageTree(Guid Parent_id)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_DictionaryManagement where id=@0", Parent_id);
            sql.Append(" or Parent_id =@0", Parent_id);
            sql.Append(" order by Sort_num");

            //sql.Append(" or Parent_id in (select id from TB_DictionaryManagement where Parent_id=@0)", PageId);
            //sql.Append(" where Parent_Id=@0  ", PageId);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回多条数据model
                List<TB_DictionaryManagement> model = db.Fetch<TB_DictionaryManagement>(sql);
                return model;
            }
        }
        #endregion

        #region 获取字典列表
        public List<TB_DictionaryManagement> LoadDicitionaryData(int pageIndex, int pageSize, out int totalRecord, Guid nodeid, string search, string key)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_DictionaryManagement where Parent_id =@0", nodeid);
            if (!string.IsNullOrEmpty(search))
            {
                if (search == "FilePersonnel")
                    sql.Append(" and b.UserName like '%" + key + "%'");
                else if (search == "AuditPersonnel")
                    sql.Append(" and c.UserName like '%" + key + "%'");
                else if (search == "IssuePersonnel")
                    sql.Append(" and d.UserName like '%" + key + "%'");
                else
                    sql.Append(" and " + search + " like '%" + key + "%'");
            }
            sql.Append("order by Sort_num  ");
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_DictionaryManagement>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }

        }
        #endregion

        #region 添加字典列表
        public bool AddPageTree(TB_DictionaryManagement model, Guid UserId, out Guid NodeId)
        {

            //string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddFileManagement);//"添加文件";
            //string OperationInfo = "添加字典类型：" + model.Project_name;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddDictionary)).Split('%');// "添加人员";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.Project_name;

            bool flag = false;
            NodeId = new Guid();
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * ");
                    sql.Append(" from TB_DictionaryManagement where Parent_id=@0", model.Parent_id);
                    sql.Append(" and Project_name=@0", model.Project_name);
                    //sql.Append(" where Parent_Id=@0  ", PageId);

                   
                    //返回多条数据model
                    List<TB_DictionaryManagement> modelDouble = db.Fetch<TB_DictionaryManagement>(sql);
                    if (modelDouble.Count > 0)
                    {
                        flag = false;
                    }
                    else
                    {

                        NodeId = db.GetGuid(db.Insert(model));

                        //系统日志
                        string operation_log_sql = CommonDAL.operation_log_(UserId, OperationType, OperationInfo);
                        int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                        db.CompleteTransaction();
                        flag = true;
                    }

                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            return flag;
        }
        #endregion

        #region 删除字典列表
        public bool DelDicitionaryData(TB_DictionaryManagement model, Guid UserId)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelDictionary)).Split('%');// "添加人员";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.Project_name;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    //删除信息
                    db.Delete<TB_DictionaryManagement>(model.id);
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(UserId, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }
        #endregion

        #region 修改字典
        public bool EditDictionary(TB_DictionaryManagement model, Guid UserId, out int errorType)
        {
            dynamic table = new ExpandoObject();

            table.Parent_id = model.Parent_id;
            table.NodeType = "2";
            table.Project_name = model.Project_name;
            table.remarks = model.remarks;
            table.Project_value = model.Project_value;
            table.Sort_num = model.Sort_num;
            table.id = model.id;

            //string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditDepartment);//"修改部门信息";
            //string OperationInfo = "修改字典信息：" + Dictionary.Project_name;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditDictionary)).Split('%');// "添加人员";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.Project_name;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * ");
                    sql.Append(" from TB_DictionaryManagement where Parent_id!=@0", model.Parent_id);
                    sql.Append(" and Project_name=@0", model.Project_name);
                    //sql.Append(" where Parent_Id=@0  ", PageId);


                    //返回多条数据model
                    List<TB_DictionaryManagement> modelDouble = db.Fetch<TB_DictionaryManagement>(sql);
                    if (modelDouble.Count > 0)
                    {
                        flag = false;
                        errorType = 2;
                    }
                    else {
                        string NodeId = db.Update("TB_DictionaryManagement", "id", table).ToString();
                        //系统日志
                        string operation_log_sql = CommonDAL.operation_log_(UserId, OperationType, OperationInfo);
                        int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                        flag = true;
                        errorType = 1;
                    }

                    //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table2));
                    db.CompleteTransaction();
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                    errorType = 3;
                }
                return flag;
            }


        }

        #endregion

        #region 启用停用字典
        public bool EditDictionaryState(TB_DictionaryManagement model, Guid UserId)
        {
            dynamic table = new ExpandoObject();
            table.Parent_id = model.Parent_id;
            table.id = model.id;
            table.DictionaryState = model.DictionaryState;

            //string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditDepartment);//"修改部门信息";
            //string OperationInfo = "修改字典信息：" + Dictionary.Project_name;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditDictionary)).Split('%');// "添加人员";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.Project_name;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    string NodeId = db.Update("TB_DictionaryManagement", "id", table).ToString();
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(UserId, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table2));
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }


        }

        #endregion
    }
}
