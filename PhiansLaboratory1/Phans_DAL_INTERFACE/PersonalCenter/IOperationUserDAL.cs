using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IOperationUserDAL
    {
        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        TB_UserInfo GetUserByParam(Guid id);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateUser(TB_UserInfo model);
        
        /// <summary>
        ///更改签名 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateUserImg(TB_UserInfo model);

        /// <summary>
        ///上传头像 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UploadUserPortrait(TB_UserInfo model);
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="model">用户信息model</param>
        /// <returns></returns>
        int ChangePassword(TB_UserInfo model, string Original_password);

        #region 获取当前用户的页面权限
        /// <summary>
        /// 获取当前用户的页面权限
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <returns></returns>
        List<TB_FunctionalModule> GetFunctionalModuleList(Guid UserId);
        #endregion

        #region 获取当前用户所有代办事项
        /// <summary>
        /// 获取当前用户所有代办事项
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <returns></returns>
        List<TB_PendingTransaction> GetPendingTransactionList(Guid UserId);
        #endregion
    }
}
