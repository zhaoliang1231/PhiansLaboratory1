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
    public class UserBLL
    {

        IUser_infoDAL dal = DALFactory.GetUserDAL();

        ///// <summary>
        ///// 用户登录
        ///// </summary>
        public TB_UserInfo UserLogin(string loginId, string loginPwd)
        {
            return dal.UserLogin(loginId, loginPwd);
        }
    }

}
