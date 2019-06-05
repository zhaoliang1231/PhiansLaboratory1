using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phans_DAL_INTERFACE;
using System.Data;
using System.Data.SqlClient;
using PhiansCommon;
using PetaPoco;
using Phians_Entity;

namespace Phians_DAL
{
    public class MainformDAL:IMainformDAL
    {

        public List<TB_FunctionalModule> GetMenuData(Guid UserId)
        {

            var sql = PetaPoco.Sql.Builder;
            if (UserId.ToString().ToLower() == "8cff8e9f-f539-41c9-80ce-06a97f481390")
            {

                sql.Append("SELECT distinct(f.id), f.PageId, f.ParentId, f.NodeType, f.IconCls, f.U_url, f.ModuleName, f.SortNum, f.Remarks, f.PermissionFlag, f.isParent ");
                sql.Append(" FROM TB_FunctionalModule f ");
                sql.Append(" where 1=1 and NodeType !=3");
                sql.Append(" order by f.SortNum  ");
            }
            else {
                sql.Append("SELECT distinct(f.id), f.PageId, f.ParentId, f.NodeType, f.IconCls, f.U_url, f.ModuleName, f.SortNum, f.Remarks, f.PermissionFlag, f.isParent ");
                sql.Append(" FROM TB_FunctionalModule f ");
                sql.Append(" LEFT JOIN TB_PageAuthorization p ON f.PageId = p.PageId ");
                sql.Append(" LEFT JOIN TB_groupAuthorization g ON g.UserId = p.UserId ");
                sql.Append(" WHERE f.NodeType !=3 AND (p.UserId = @0 ", UserId);
                sql.Append(" OR p.GroupId IN ( SELECT GroupId FROM TB_groupAuthorization WHERE UserId = @0))", UserId);
                sql.Append(" order by f.SortNum ");
            }
          

            using (var db = DbInstance.CreateDataBase())
            {
                //返回单条数据model
                List<TB_FunctionalModule> model = db.Fetch<TB_FunctionalModule>(sql);
                return model;
            }
          
            //DataTable ds = SqlHelper.GetDataTable(SqlHelper.connStr, CommandType.Text, strSql.ToString(), paras);

            //return ds;
        }


    }
}
