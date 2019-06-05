using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
namespace Phans_DAL_INTERFACE
{
     public interface IUser_infoDAL
    { 
        /// <summary>
        /// 用户登录
        /// </summary>
         TB_UserInfo UserLogin(string loginId, string loginPwd);
       

    }
}
