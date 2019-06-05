using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IMessageDAL
    {
        /// <summary>
        /// 获取未读消息列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        List<TB_Message> GetUnReadMessage(int pageIndex, int pageSize, out int totalRecord, Guid userId, int flag, string key, string key1);

        ///// 获取已读消息列表
        //List<TB_Message> GetReadMessage(int pageIndex, int pageSize, out int totalRecord, Guid userId);
        ////修改消息状态
        bool EditMessage(string ids);

        ////修改全部消息状态
        bool EditAllMessage(Guid userId);
       
    }
}
