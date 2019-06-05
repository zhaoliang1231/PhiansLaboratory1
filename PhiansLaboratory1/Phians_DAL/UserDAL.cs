using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System.Data.SqlClient;
using System.Data;
using PhiansCommon;
namespace Phians_DAL
{

    class UserDAL : IUser_infoDAL
    {
         
        PetaPoco.Database db = new PetaPoco.Database();
        //-----------------------------------------------------------------------------------------------------
        #region 用户登录获取
        public TB_UserInfo UserLogin(string UserId, string loginPwd)
        {


            var sql = PetaPoco.Sql.Builder;
            sql.Append("select id, Email, Postcode, QQ, Address, Signature, Remarks, sort_num, CreateDate, CreatePersonnel, CountState, UserId, UserCount, UserPwd, UserName, UserNsex, Tel, Phone, Fax  ");
            sql.Append("  from TB_UserInfo ");
            sql.Append(" where UserCount=@0", UserId);
            sql.Append(" and UserPwd=@0", loginPwd);
            //返回单条数据model
            TB_UserInfo model = db.SingleOrDefault<TB_UserInfo>(sql);
            //DataTable nn = db.ExecuteDataTable(sql);
            List<TB_UserInfo> hh = db.Fetch<TB_UserInfo>(sql);
            return model;
         

        }
        #endregion


    }
}


