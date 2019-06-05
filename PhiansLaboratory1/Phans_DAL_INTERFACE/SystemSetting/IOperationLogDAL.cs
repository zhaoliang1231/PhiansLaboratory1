using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
namespace Phans_DAL_INTERFACE
{
    public interface IOperationLogDAL
    {
       
        /// <summary>
        /// 加载日志列表
        /// </summary>
        /// <returns></returns>
        List<OperationLog> GetPageList(int pageIndex, int pageSize, out int totalRecord, string search, string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(OperationLog model);

    }
}
