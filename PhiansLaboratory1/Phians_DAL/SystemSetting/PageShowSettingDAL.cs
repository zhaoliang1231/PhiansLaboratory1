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
    class PageShowSettingDAL : IPageShowSettingDAL
    {
        #region  加载表的所有字段信息
        public List<TB_PageShowCustom> loadInfo(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;

            sql.Append("select * from dbo.TB_PageShowCustom where 1=1");
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and PageId= " + PageModel.SearchList_[0].Key + "");
            }
            sql.OrderBy("FieldSort");
            //if (!string.IsNullOrEmpty(PageModel.SortName) || !string.IsNullOrEmpty(PageModel.SortOrder))
            //{
            //    sql.OrderBy(PageModel.SortName + " " + PageModel.SortOrder);
            //}

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_PageShowCustom>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 修改字段显示信息

        public ReturnDALResult EditInfo(List<TB_PageShowCustom> model, Guid LogPersonnel)
        {
            string Titles = "";
            foreach (var item in model)
            {
                if (Titles=="")
                    Titles = item.id.ToString();
                else
                    Titles = Titles + "," + item.id.ToString();

            }
            
            ReturnDALResult ResultModule = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PageShowCustom)).Split('%');// "修改设备";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + Titles;
            string date = DateTime.Now.ToString();



            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();

                    foreach (var item in model)
                    {
                        //判断是否存在
                        var exist = PetaPoco.Sql.Builder;
                        exist.Append("select * from TB_PageShowCustom where id =@0 ", item.id);
                        TB_PageShowCustom model_ = db.SingleOrDefault<TB_PageShowCustom>(exist);
                        if (model_ == null)
                        {

                            db.AbortTransaction();
                            ResultModule.returncontent = "字段不存在";
                            ResultModule.Success = 0;
                            return ResultModule;
                        }

                        if (model.Count == 1)
                        {
                            string[] Updatecolumns = { "title", "FieldSort", "hidden", "Sortable", "Operator", "OperateDate", "Remark" };
                            db.Update(item, Updatecolumns);

                        }
                        else
                        {
                            string[] Updatecolumns = { "hidden", "Sortable", "Operator", "OperateDate", "Remark" };
                            db.Update(item, Updatecolumns);
                        }

                    }

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


    }
}
