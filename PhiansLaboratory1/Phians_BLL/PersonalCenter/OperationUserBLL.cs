using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
    public class OperationUserBLL
    {


        IOperationUserDAL dal = DALFactory.GetOperationUserDAL();

        #region 获取用户资料
        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public TB_UserInfo GetUserByParam(Guid id)
        {
            return dal.GetUserByParam(id);
        }
        #endregion

        #region 修改基础资料
        /// <summary>
        /// 修改基础资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(TB_UserInfo model)
        {
            return dal.UpdateUser(model);
        }
        #endregion

        #region 上传签名
        /// <summary>
        /// 上传签名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUserImg(TB_UserInfo model)
        {
            return dal.UpdateUserImg(model);
        }
        #endregion

        #region 上传头像
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UploadUserPortrait(TB_UserInfo model)
        {
            return dal.UploadUserPortrait(model);
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">用户信息model</param>
        /// <returns></returns>
        public int ChangePassword(TB_UserInfo model,string Original_password) {
            return dal.ChangePassword(model, Original_password);
        }
        #endregion


        #region 获取待处理事务
        /// <summary>
        /// 获取待处理事务
        /// </summary>
        /// <param name="UserId">当前登录用户</param>
        /// <returns></returns>
        public List<TB_PendingTransaction> GetPendingTransaction(Guid UserId)
        {
            List<TB_PendingTransaction> list = new List<TB_PendingTransaction>();
            //获取当前登录用户权限
            List<TB_FunctionalModule> FunctionalModuleList = dal.GetFunctionalModuleList(UserId);
            //获取当前用户所有代办事项
            List<TB_PendingTransaction> PendingTransactionList = dal.GetPendingTransactionList(UserId);

            //用户权限和所有代办事项对比，有则添加
            for (int i = 0; i < FunctionalModuleList.Count;i++ )
            {
                for (int j = 0; j < PendingTransactionList.Count; j++)
                {
                    if (FunctionalModuleList[i].ModuleName == PendingTransactionList[j].TransactionName) {
                        if (PendingTransactionList[j].TransactionCount!=0)
                        {
                            TB_PendingTransaction PendingTransaction = new TB_PendingTransaction();
                            PendingTransaction.TransactionCount = PendingTransactionList[j].TransactionCount;
                            PendingTransaction.TransactionName = PendingTransactionList[j].TransactionName;
                            PendingTransaction.TransactionUrl = FunctionalModuleList[i].U_url;
                            PendingTransaction.PageId = FunctionalModuleList[i].PageId.ToString();

                            list.Add(PendingTransaction);
                        }
                    
                    }
                }
                
            }
            return list;
        }
        #endregion
    }
}

