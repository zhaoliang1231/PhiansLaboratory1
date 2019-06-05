using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using PetaPoco;

namespace Phians_DAL
{
    public class OperationLogDAL : IOperationLogDAL
    {
        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="key">关键字</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<OperationLog> GetPageList(int pageIndex, int pageSize, out int totalRecord, string search, string key)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from (select UI.UserName as UserName, OL.* from dbo.TB_OperationLog as OL left join dbo.TB_UserInfo UI on OL.UserId=UI.UserId) as a ");
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                if (search == "OperationDate")   //搜索日期
                    sql.Append("where convert(varchar," + search + ",120) like '%" + key + "%'");
                else
                    sql.Append("where " + search + " like '%" + key + "%'");
            }
            sql.Append("order by OperationDate desc");
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<OperationLog>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        public int Add(OperationLog model)
        {
            int? primaryKey;
            using (var db = new Database())
            {
                primaryKey = db.Insert(model) as int?;
            }

            return primaryKey.HasValue ? primaryKey.Value : 0;
        }
    }
}
