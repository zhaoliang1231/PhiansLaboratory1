using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using PetaPoco;
using System.Dynamic;
using PhiansCommon;
using System.Net;
using System.Net.Sockets;
using Phians_Entity.Common;

namespace Phians_DAL
{
    class OrganizationDAL : IOrganizationDAL
    {
        #region 加载列表
        public List<TB_Organization> GetDepartmentManagementList(string ParentId)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_Organization ");

            using (var db = DbInstance.CreateDataBase())
            {
                //返回多条数据model
                List<TB_Organization> model = db.Fetch<TB_Organization>(sql);
                return model;
            }
        }
        #endregion

        #region 模块信息加载
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_Organization> LoadDepartmentModuleInfo(Guid NodeId, int pageIndex, int pageSize, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from (select * from TB_Organization where ParentId=@0 ) as a ", NodeId);


            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_Organization>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 获取页面信息
        public TB_Organization LoadDepartmentInfo(Guid NodeId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_Organization ");
            sql.Append(" where NodeId=@0  ", NodeId);

            using (var db = DbInstance.CreateDataBase())
            {
                //model
                TB_Organization model = db.SingleOrDefault<TB_Organization>(sql);
                return model;
            }


        }
        #endregion

        #region 添加部门
        public TB_Organization AddDepartment(TB_Organization Organization)
        {

            string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddDepartment);//"添加部门";
            string OperationInfo = "添加部门：" + Organization.NodeName;

            TB_Organization TB_Organization = new TB_Organization();     
    
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    string NodeId = db.Insert(Organization).ToString();

                    var sql = PetaPoco.Sql.Builder;

                    sql.Append(" select * from TB_Organization where NodeId=@0 ", NodeId);

                    TB_Organization = db.FirstOrDefault<TB_Organization>(sql);

                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(Organization.CreatePersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();

                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            return TB_Organization;

        }
    
        #endregion

        #region 修改部门
        public bool EditDepartment(TB_Organization Organization)
        {
            dynamic table = new ExpandoObject();

            table.NodeId = Organization.NodeId;
            //table.ParentId = Organization.ParentId;
            table.NodeName = Organization.NodeName;
            table.OrganizationCode = Organization.OrganizationCode;
            table.remarks = Organization.remarks;
            table.NodeType = Organization.NodeType;
            table.Address = Organization.Address;
            table.Phone = Organization.Phone;
            table.PostCode = Organization.PostCode;
            table.SortNum = Organization.SortNum;

            //string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditDepartment);//"修改部门信息";
            //string OperationInfo = "修改部门信息：" + Organization.NodeName;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditDepartment)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + Organization.NodeName;

             bool flag = false;
             using (var db = DbInstance.CreateDataBase())
            {
                 try
                {
                    db.BeginTransaction();
                    string NodeId = db.Update("TB_Organization", "NodeId", table).ToString();
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(Organization.CreatePersonnel, OperationType, OperationInfo);
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

        #region 删除部门
        public bool DelDepartment(Guid NodeId, string NodeName, Guid CreatePersonnel)
        {

            //string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelDepartment);//"删除部门";
            //string OperationInfo = "删除部门：" + NodeName;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelDepartment)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + NodeName;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    string num = db.Delete<TB_Organization>(NodeId).ToString();
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(CreatePersonnel, OperationType, OperationInfo);
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
        

    }
}
