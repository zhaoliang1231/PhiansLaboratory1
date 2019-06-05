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
    public class TestBlockLibraryDAL : ITestBlockLibraryDAL
    {
        #region  试块库列表
        public List<TB_NDT_TestBlockLibrary> load_TestBlockLibrary(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;

            sql.Append("select TT.*,ui1.UserName as CreatePerson_n from dbo.TB_NDT_TestBlockLibrary TT");
            sql.Append("left join dbo.TB_UserInfo ui1 on TT.CreatePerson=ui1.UserCount where 1=1");
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and TT." + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
            }
            if (!string.IsNullOrEmpty(PageModel.SortName) || !string.IsNullOrEmpty(PageModel.SortOrder))
            {
                sql.OrderBy(PageModel.SortName + " " + PageModel.SortOrder);
            }

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_TestBlockLibrary>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 新增试块
        public ReturnDALResult Add_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModule = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.TestBlockLibrary_add)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.CalBlock;
            string date = DateTime.Now.ToString();
            //判断设备编号是否存在
            var exist = PetaPoco.Sql.Builder;
            exist.Append("select * from TB_NDT_TestBlockLibrary where CalBlockNum=@0 ", model.CalBlockNum);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //判断设备编号是否存在
                    if (db.FirstOrDefault<TB_NDT_TestBlockLibrary>(exist) != null)
                    {
                        db.AbortTransaction();
                        ResultModule.Success = 2;
                        ResultModule.returncontent = "试块已经存在";
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

        #region 修改试块
        public ReturnDALResult Edit_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModule = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.TestBlockLibrary_edit)).Split('%');// "修改设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.CalBlock;
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
                    exist.Append("select * from TB_NDT_TestBlockLibrary where CalBlockNum=@0 and ID !=@1 ", model.CalBlockNum, model.ID);
                    TB_NDT_TestBlockLibrary model_ = db.SingleOrDefault<TB_NDT_TestBlockLibrary>(exist);
                    if (model_ != null)
                    {

                        db.AbortTransaction();
                        ResultModule.returncontent = "试块编号重复";
                        ResultModule.Success = 0;
                        return ResultModule;
                    }
                    var exist2 = PetaPoco.Sql.Builder;
                    exist2.Append("select * from TB_NDT_TestBlockLibrary where ID !=@0", model.ID);
                    List<TB_NDT_TestBlockLibrary> model_2 = db.Fetch<TB_NDT_TestBlockLibrary>(exist2);
                    if (model_2 == null)
                    {

                        db.AbortTransaction();
                        ResultModule.returncontent = "试块不存在";
                        ResultModule.Success = 0;
                        return ResultModule;
                    }
                    string[] Updatecolumns = { "CalBlock", "C_S", "InstrumentSet", "Reflector", "State", "Remarks" };
                    obj = db.Update(model, Updatecolumns);
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

        #region 删除试块
        /// <summary>
        /// 删除探头
        /// </summary>
        /// <param name="Id">探头id</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <param name="FixtureName">夹具名称</param>
        /// <param name="Address">仓库位置</param>
        /// <returns></returns>
        public ReturnDALResult Del_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel)
        {
            ReturnDALResult ResultModule = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.TestBlockLibrary_delete)).Split('%');// "删除设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.CalBlock;

            //string[] FixtureCode_s = FixtureCode.Split(',');
            //string[] Address_s = Address.Split(',');
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    var slectprobe = Sql.Builder;
                    //检查记录是否存在
                    slectprobe.Append("select *from TB_NDT_TestBlockLibrary where ID =@0", model.ID);

                    TB_NDT_TestBlockLibrary model_ = db.SingleOrDefault<TB_NDT_TestBlockLibrary>(slectprobe);
                    if (model_ == null)
                    {

                        db.AbortTransaction();
                        ResultModule.returncontent = "试块不存在";
                        ResultModule.Success = 0;
                        return ResultModule;
                    }
                    else
                    {

                        int num = db.Delete(model_);
                    }


                    //}
                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    db.CompleteTransaction();
                    ResultModule.Success = 1;
                }
                catch (Exception e)
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


        #region 批量导入试块
        /// <summary>
        /// 批量导入试块
        /// </summary>
        /// <param name="listmodel">导入的数据model</param>
        /// <param name="OperatePerson">操作人</param>
        /// <returns></returns>
        public ReturnDALResult importTestBlockLibrary(List<TB_NDT_TestBlockLibrary> listmodel, Guid OperatePerson)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.TestBlockLibrary_add)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + "批量导入试块";
            string date = DateTime.Now.ToString();
            ReturnDALResult ReturnDALResult = new ReturnDALResult();

            List<string> unimportlist = new List<string>();
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    #region 搜索编制人员的UserId

                    var UserNameSql = PetaPoco.Sql.Builder;
                    UserNameSql.Append("select * from TB_UserInfo where UserId = @0 ", OperatePerson);

                    string  UserCount = db.FirstOrDefault<TB_UserInfo>(UserNameSql).UserCount;

                    #endregion

                    //判断设备编号是否存在
                    foreach (var item in listmodel)
                    {
                        var exist = PetaPoco.Sql.Builder;
                        exist.Append("select * from TB_NDT_TestBlockLibrary where CalBlockNum=@0 ", item.CalBlockNum.Trim());
                        //去除同号
                        if (db.FirstOrDefault<TB_NDT_TestBlockLibrary>(exist) != null)
                        {
                            unimportlist.Add(item.CalBlockNum);
                            ReturnDALResult.returncontent += item.CalBlockNum + ",";
                        }
                        else
                        {
                            item.CreatePerson = UserCount;
                            item.CreateTime = DateTime.Now;
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
