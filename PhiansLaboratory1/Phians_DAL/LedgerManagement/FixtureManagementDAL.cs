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
using PhiansCommon;
using Phans_DAL_INTERFACE;


namespace Phians_DAL
{
    /// <summary>
    /// 探头库
    /// </summary>
    public class FixtureManagementDAL : IFixtureManagementDAL
    {
        #region  探头库列表
        public List<TB_NDT_probe_library> GetProbeList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;

            sql.Append("select * from dbo.TB_NDT_probe_library where 1=1");
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
                var result = db.Page<TB_NDT_probe_library>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 新增探头
        public ReturnDALResult Probe_add(TB_NDT_probe_library model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModule = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.Probe_add)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.Probe_name;
            string date = DateTime.Now.ToString();
            //判断设备编号是否存在
            var exist = PetaPoco.Sql.Builder;
            exist.Append("select * from TB_NDT_probe_library where Probe_num=@0 ", model.Probe_num);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //判断设备编号是否存在
                    if (db.FirstOrDefault<TB_NDT_probe_library>(exist) != null)
                    {
                        db.AbortTransaction();
                        ResultModule.Success = 2;
                        ResultModule.returncontent = "任务不存在";
                        return ResultModule;
                    }
                    db.Insert(model);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModule.Success = 1;
                }
                catch
                {
                    db.AbortTransaction();
                }
            }
            return ResultModule;
        }
        #endregion

        #region 修改探头
        public ReturnDALResult Probe_edit(TB_NDT_probe_library model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModule = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.Probe_edit)).Split('%');// "修改设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.Probe_name;
            string date = DateTime.Now.ToString();

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //修改设备资料
                    object obj = null;

                    //判断设备编号是否存在
                    var exist = PetaPoco.Sql.Builder;
                    exist.Append("select * from TB_NDT_probe_library where Probe_num=@0 and id !=@1 ", model.Probe_num, model.id);
                    TB_NDT_probe_library model_ = db.SingleOrDefault<TB_NDT_probe_library>(exist);
                    if (model_ != null)
                    {

                        db.AbortTransaction();
                        ResultModule.returncontent = "探头编号重复";
                        ResultModule.Success = 0;
                        return ResultModule;
                    }
                    var exist2 = PetaPoco.Sql.Builder;
                    exist2.Append("select * from TB_NDT_probe_library where id !=@0", model.id);
                   List< TB_NDT_probe_library> model_2 = db.Fetch<TB_NDT_probe_library>(exist2);
                    if (model_2 == null)
                    {

                        db.AbortTransaction();
                        ResultModule.returncontent = "任务不存在";
                        ResultModule.Success = 0;
                        return ResultModule;
                    }
                    string[] Updatecolumns = { "Probe_name", "Probe_num", "Probe_type", "Probe_size", "Probe_frequency", "Coil_Size", "Probe_Length", "Cable_Length", "Mode_L", "Chip_size", "Angle", "Nom_Angle", "Shoe", "Probe_state", "remarks", "Probe_Manufacture", "Mode_T"};
                    obj = db.Update( model,Updatecolumns);
             //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModule.Success = 1;
                }
                catch
                {
                    db.AbortTransaction();
                    throw;
                }
            }
            return ResultModule;
        }
        #endregion

        #region 删除探头
        /// <summary>
        /// 删除探头
        /// </summary>
        /// <param name="Id">探头id</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <param name="FixtureName">夹具名称</param>
        /// <param name="Address">仓库位置</param>
        /// <returns></returns>
        public ReturnDALResult Probe_delete(TB_NDT_probe_library model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModule = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.Probe_delete)).Split('%');// "删除设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.Probe_name;

            //string[] FixtureCode_s = FixtureCode.Split(',');
            //string[] Address_s = Address.Split(',');
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    var slectprobe = Sql.Builder;
                    //检查记录是否存在
                    slectprobe.Append("select *from TB_NDT_probe_library where id =@0",model.id);

                    TB_NDT_probe_library model_ = db.SingleOrDefault<TB_NDT_probe_library>(slectprobe);
                    if (model_ == null)
                    {

                        db.AbortTransaction();
                        ResultModule.returncontent = "探头不存在";
                        ResultModule.Success = 0;
                        return ResultModule;
                    }
                    else {

                        int num = db.Delete(model_);
                    }
             
                        
                    //}
                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    db.CompleteTransaction();
                    ResultModule.Success = 1;
                }
                catch(Exception e)
                {
                    db.AbortTransaction();
                    ResultModule.returncontent = e.ToString();
                    ResultModule.Success = 2;
                    throw;
                }
            }
            return ResultModule;
        }
        #endregion





        #region 导入探头
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listmodel">导入的数据model</param>
        /// <param name="OperatePerson">操作人</param>
        /// <returns></returns>

        public ReturnDALResult importProbe(List<TB_NDT_probe_library> listmodel, Guid OperatePerson)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.Probe_add)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + "批量导入探头";
            string date = DateTime.Now.ToString();
            ReturnDALResult ReturnDALResult = new ReturnDALResult();

            List<string> unimportlist = new List<string>();
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //判断设备编号是否存在
                    foreach (var item in listmodel)
                    {
                        var exist = PetaPoco.Sql.Builder;
                        exist.Append("select * from TB_NDT_probe_library where Probe_num=@0 ", item.Probe_num.Trim());
                        //去除同号
                        if (db.FirstOrDefault<TB_NDT_probe_library>(exist) != null)
                        {
                            unimportlist.Add(item.Probe_num);
                            ReturnDALResult.returncontent += item.Probe_num + ",";
                        }
                        else
                        {

                            db.Insert(item);
                        }

                    }
                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(OperatePerson, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    db.CompleteTransaction();
                    ReturnDALResult.Success = 1;

                }
                catch (Exception e)
                {

                    db.AbortTransaction();
                    ReturnDALResult.Success = 2;
                    throw;
                }
            }
            return ReturnDALResult;
        }
        #endregion
    }
}
