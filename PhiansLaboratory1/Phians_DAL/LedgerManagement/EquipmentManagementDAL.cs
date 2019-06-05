using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
using Phans_DAL_INTERFACE.LedgerManagement;
using PetaPoco;
using System.Dynamic;
using Phians_Entity.Common;
using System.IO;
using PhiansCommon;

namespace Phians_DAL
{
    public class EquipmentManagementDAL : IEquipmentManagementDAL
    {
        #region  设备列表
        public List<TB_NDT_equipment_library> GetEquipmentList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;

            sql.Append("select * from dbo.TB_NDT_equipment_library where 1=1");
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
            }
            if (!string.IsNullOrEmpty(PageModel.SortName) || !string.IsNullOrEmpty(PageModel.SortOrder))
            {
                sql.OrderBy(PageModel.SortName + " " + PageModel.SortOrder);
            }

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_equipment_library>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 新增设备
        public ReturnDALResult AddEquipment(TB_NDT_equipment_library model, Guid LogPersonnel)
        {
            ReturnDALResult resultmodel = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddEquipment)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.equipment_nem;
            string date = DateTime.Now.ToString();
            //判断设备编号是否存在
            var exist = PetaPoco.Sql.Builder;
            exist.Append("select * from TB_NDT_equipment_library where equipment_num=@0 ", model.equipment_num);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //判断设备编号是否存在
                    if (db.FirstOrDefault<TB_NDT_equipment_library>(exist) != null)
                    {
                        db.CompleteTransaction();
                        resultmodel.Success=2;  //2表示该设备编号已存在
                        resultmodel.returncontent = "设备编号已存在";
                        return resultmodel;
                    }
                    db.Insert(model);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    db.CompleteTransaction();
                    resultmodel.Success = 1;
                }
                catch
                {
                    db.AbortTransaction();
                    throw;
                }
            }
            return resultmodel;
        }
        #endregion

        #region 修改设备
        public ReturnDALResult EditEquipment(dynamic model, Guid LogPersonnel)
        {
            ReturnDALResult resultmodel = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditEquipment)).Split('%');// "修改设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.equipment_nem;
            string date = DateTime.Now.ToString();

            //dynamic table = new ExpandoObject();
            //table.id = model.id;
            //table.equipment_nem = model.equipment_nem;
            //table.equipment_Type=model.equipment_Type;
            //table.range_=model.range_;
            //table.Manufacture=model.Manufacture; 
            //table.effective=model.effective;
            //table.E_state=model.E_state;
            //table.Remarks=model.Remarks;

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //修改设备资料
                    object obj = null;
                    obj = db.Update("TB_NDT_equipment_library", "id", model);
                    if (obj == null)
                    {
                        db.CompleteTransaction();
                        resultmodel.Success = 2;
                        return resultmodel;
                    }

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    resultmodel.Success = 1;
                }
                catch
                {
                    db.AbortTransaction();
                    throw;
                }
            }
            return resultmodel;
        }
        #endregion

        #region 删除设备
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="EquipmentCode">设备编号</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <param name="EquipmentName">设备名字</param>
        /// <param name="Address">仓库位置ID</param>
        /// <returns></returns>
        public ReturnDALResult DelEquipment(string id, Guid LogPersonnel, string equipment_nem)
        {
            ReturnDALResult resultmodel = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelEquipment)).Split('%');// "删除设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + equipment_nem;
            //var sql = PetaPoco.Sql.Builder;
            //sql.Append("update TB_EquipmentInfo set StatusFlag=1 where EquipmentCode =@0", EquipmentCode);//逻辑删除

            string[] id_s = id.Split(',');
            
            using (var db = DbInstance.CreateDataBase())
            {
               
                try
                {
                    db.BeginTransaction();
                    for (int i = 0; i < id_s.Count(); i++)
                    {
                        int num = db.Delete<TB_NDT_equipment_library>("where id =@0", id_s[i]);
                    }

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    db.CompleteTransaction();
                    resultmodel.Success = 1;
                }
                catch
                {
                    db.AbortTransaction();
                    throw;
                }
            }
            return resultmodel;
        }
        #endregion
       
    }
}
