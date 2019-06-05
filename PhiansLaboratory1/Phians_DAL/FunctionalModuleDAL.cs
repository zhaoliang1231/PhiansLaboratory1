using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System.Data.SqlClient;
using System.Data;
using PhiansCommon;
using System.Dynamic;
using PetaPoco;

namespace Phians_DAL
{
    class FunctionalModuleDAL : IFunctionalModuleDAL
    {

        //-----------------------------------------------------------------------------------------------------
        #region 加载页面
        public List<TB_FunctionalModule> FunctionalModuleLoad(string PageId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_FunctionalModule ");
            sql.Append(" order by SortNum ");
            //sql.Append(" where ParentId=@0  ", PageId);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                List<TB_FunctionalModule> model = db.Fetch<TB_FunctionalModule>(sql);
                return model;
            }


        }
        #endregion

        #region 获取页面信息
        public TB_FunctionalModule FunctionalModuleLoadPageInfo(Guid PageId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_FunctionalModule ");
            sql.Append(" where PageId=@0  ", PageId);
            sql.Append(" order by SortNum ");

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                TB_FunctionalModule model = db.SingleOrDefault<TB_FunctionalModule>(sql);
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
        public List<TB_FunctionalModule> LoadModuleInfo(Guid PageId, int pageIndex, int pageSize, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from (select * from TB_FunctionalModule where ParentId=@0 ) as a order by SortNum ", PageId);


            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_FunctionalModule>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion


        #region 添加页面
        public Guid FunctionalModuleAdd(TB_FunctionalModule FunctionalModule)
        {

            #region
            //dynamic table = new ExpandoObject();

            //table.ParentId = FunctionalModule.ParentId;
            //table.NodeType = FunctionalModule.NodeType;
            //table.IconCls = FunctionalModule.IconCls;
            //table.U_url = FunctionalModule.U_url;
            //table.ModuleName = FunctionalModule.ModuleName;
            //table.SortNum = FunctionalModule.SortNum;
            //table.Remarks = FunctionalModule.Remarks;
            //table.PermissionFlag = FunctionalModule.PermissionFlag;


            //dynamic table2 = new ExpandoObject();
            //table2.isParent = true;

            //var update = PetaPoco.Sql.Builder;
            //update.Append("update ");
            //update.Append(" TB_FunctionalModule ");
            //update.Append(" set ");
            //update.Append(" isParent=true ");
            //update.Append(" where PageId='" + table.PageId + "'");

            //var sql = PetaPoco.Sql.Builder;
            //sql.Append("insert into ");
            //sql.Append(" TB_FunctionalModule ");
            //sql.Append(" (PageId, ParentId, NodeType, IconCls, U_url, ModuleName, SortNum, Remarks, PermissionFlag, isParent) ");
            //sql.Append(" values ");
            //sql.Append(" ('" + table.PageIds + "','" + table.PageId + "','" + table.NodeType + "','" + table.IconCls + "','" + table.U_url + "','" + table.ModuleName + "','" + table.SortNum + "','" + table.Remarks + "','" + table.PermissionFlag + "','" + table.isParent + "') ");

            //string insertSql = " insert into TB_FunctionalModule (PageId, ParentId, NodeType, IconCls, U_url, ModuleName, SortNum, Remarks, PermissionFlag) values ('" + FunctionalModule.PageId + "','" + FunctionalModule.ParentId + "','" + FunctionalModule.NodeType + "','" + FunctionalModule.IconCls + "','" + FunctionalModule.U_url + "','" + FunctionalModule.ModuleName + "','" + FunctionalModule.SortNum + "','" + FunctionalModule.Remarks + "','" + FunctionalModule.PermissionFlag + "')";
            #endregion

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                Guid guid = db.GetGuid(db.Insert(FunctionalModule));
                return guid;

            }

        }

        #endregion

        #region 修改页面
        public bool FunctionalModuleEdit(TB_FunctionalModule FunctionalModule)
        {
            dynamic table = new ExpandoObject();

            table.PageId = FunctionalModule.PageId;
            table.NodeType = FunctionalModule.NodeType;
            table.IconCls = FunctionalModule.IconCls;
            table.U_url = FunctionalModule.U_url;
            table.ModuleName = FunctionalModule.ModuleName;
            table.SortNum = FunctionalModule.SortNum;
            table.Remarks = FunctionalModule.Remarks;
            table.PermissionFlag = FunctionalModule.PermissionFlag;
            // table.isParent = FunctionalModule.isParent;

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                object obj = null;
                obj = db.Update("TB_FunctionalModule", "PageId", table);
                return (obj != null) ? true : false;
            }


        }

        #endregion

        #region 删除页面
        public bool FunctionalModuleDel(Guid PageId)
        {
            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    RemoveNode(PageId, db);//递归删除子页面信息

                    db.Delete<TB_FunctionalModule>(PageId);//删除页面信息

                    db.CompleteTransaction();
                    flag = true;

                }
                catch (Exception)
                {
                    db.AbortTransaction();
                    throw;
                }
            }

            return flag;

        }

        /// <summary>
        /// 删除子页面（递归）
        /// </summary>
        /// <param name="PageId">页面ID</param>
        public void RemoveNode(Guid PageId, Database db)
        {

            try
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append(" select * from TB_FunctionalModule where ParentId=@0", PageId);
                List<TB_FunctionalModule> TB_FunctionalModule = db.Fetch<TB_FunctionalModule>(sql);

                if (TB_FunctionalModule.Count != 0)
                {
                    foreach (var item in TB_FunctionalModule)
                    {
                        Guid ParentId = item.PageId;//获取页面id
                        DeleteNode(ParentId, db);//递归删除信息

                        db.Delete(item);//删除页面信息
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 递归删除信息
        /// </summary>
        /// <param name="ParentId">父id</param>
        public void DeleteNode(Guid ParentId, Database db)
        {
            RemoveNode(ParentId, db);//递归删除信息
        }

        #endregion

    }

}
