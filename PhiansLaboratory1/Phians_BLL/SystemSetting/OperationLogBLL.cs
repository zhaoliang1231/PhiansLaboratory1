using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
   public class OperationLogBLL
    {
       IOperationLogDAL dal = DALFactory.GetLog();

       /// <summary>
       /// 用户登录
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="totalRecord"></param>
       /// <param name="search"></param>
       /// <param name="key"></param>
       /// <returns></returns>
       public List<OperationLog> GetPageList(int pageIndex, int pageSize,  out int totalRecord,string search,string key)
        {
            return dal.GetPageList(pageIndex, pageSize, out totalRecord,search, key);
        }


    }
}
