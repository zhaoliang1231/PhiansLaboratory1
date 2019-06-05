using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    public class MessageDAL:IMessageDAL
    {
        /// <summary>
        /// 分页获取未读消息
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_Message> GetUnReadMessage(int pageIndex, int pageSize, out int totalRecord, Guid userId, int flag, string search, string key)
        {
            var sql = PetaPoco.Sql.Builder;
            //sql.Append("SELECT * FROM ( select tu.UserName as UserName,TM.*,TU2.UserName as PushPersonname from dbo.TB_Message as TM left join dbo.TB_UserInfo as TU on TM.UserId=TU.UserId left join dbo.TB_UserInfo as TU2 on TM.PushPersonnel=TU2.UserId) as ME WHERE ME.UserId=@0", userId);
          
            sql.Append("  select tu.UserName as UserName,TM.*,TU2.UserName as PushPersonname from dbo.TB_Message as TM");
            sql.Append("  left join dbo.TB_UserInfo as TU on TM.UserId=TU.UserId");
            sql.Append("  left join dbo.TB_UserInfo as TU2 on TM.PushPersonnel=TU2.UserId");
            sql.Append("  WHERE TM.UserId=@0" , userId );

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
                {
                    if (search == "UserName")
                        sql.Append(" and TU2.UserName like @0", "%" + key + "%");
                    else
                    {
                        sql.Append(" and TM." + search + " like @0", "%" + key + "%");
                    }
                }
            }
            if (flag == 0)//获取ConfirmTime（确认时间为空）未确认消息
            {
                sql.Append(" and TM.ConfirmTime is NULL");
                sql.Append("order by  TM.CreateTime desc");
            }

            if (flag == 1)//获取已读消息
            {
                sql.Append(" and TM.ConfirmTime is not NULL");
                sql.Append("order by  TM.ConfirmTime desc");
            }
            

            
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_Message>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }


        public bool EditMessage(string ids)
        {
            //dynamic table = new ExpandoObject();
            //table.id = message.id;
            //table.ConfirmTime = message.ConfirmTime;
            string time=DateTime.Now.ToString();
            string sql = "update TB_Message set ConfirmTime='" + time + "' where id in(" + ids + ")";
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.Execute(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        //确认全部消息
        public bool EditAllMessage(Guid userId)
        {
            //dynamic table = new ExpandoObject();
            //table.id = message.id;
            //table.ConfirmTime = message.ConfirmTime;
            string time = DateTime.Now.ToString();
            string sql = "update TB_Message set ConfirmTime='" + time + "' where ConfirmTime is NULL and UserId='" + userId + "'";
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.Execute(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
