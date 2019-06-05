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
    public class MessageBLL
    {
        IMessageDAL dal = DALFactory.GetMessage();

        #region 获取消息列表
        /// <summary>
        /// 未读消息列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_Message> GetUnReadMessage(int pageIndex, int pageSize, out int totalRecord, Guid userId, int flag, string key, string key1)
        {
            return dal.GetUnReadMessage(pageIndex, pageSize, out totalRecord, userId, flag, key, key1);
        }
        //已读消息
        //public List<TB_Message> GetReadMessage(int pageIndex, int pageSize, out int totalRecord, Guid userId)
        //{
        //    return dal.GetReadMessage(pageIndex, pageSize, out totalRecord, userId);
        //}
        #endregion
        #region 修改消息状态
        public bool MessageEdit(string ids)
        {
            return dal.EditMessage( ids);
        }
        #endregion

        #region 修改全部消息状态
        public bool MessageAllEdit(Guid userId)
        {
            return dal.EditAllMessage( userId);
        }
        #endregion
    }
}
