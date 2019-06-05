using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace Phians_DAL
{
    public class UserDemoDal
    {
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>单个用户实体</returns>
        public UserDemo GetUserByKey(int id)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.SingleOrDefault<UserDemo>(id);
            }
        }

        /// <summary>
        /// 封装参数类型获取实体
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userName">姓名</param>
        /// <returns>首条用户实体</returns>
        public UserDemo GetUserByParam(int id, string userName)
        {
            string sqlStr = "select * from userdemo where id = @0 and username = @1";//可简化为"where id = @0 and username = @1"

            Sql sql = Sql.Builder.Append(sqlStr, new object[] { id, userName });

            using (var db = DbInstance.CreateDataBase())
            {
                return db.FirstOrDefault<UserDemo>(sql);
            }
        }

        /// <summary>
        /// 封装参数类型获取实体 用法2
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userName">姓名</param>
        /// <returns>首条用户实体</returns>
        public UserDemo GetUserByParam2(int id, string userName)
        {
            string sqlStr = "select * from userdemo where id = @0 and username = @1";//可简化为"where id = @0 and username = @1"

            using (var db = DbInstance.CreateDataBase())
            {
                return db.FirstOrDefault<UserDemo>(sqlStr, new object[] { id, userName });
            }
        }

        /// <summary>
        /// 拼接参数类型获取实体 
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userName">姓名</param>
        /// <returns>首条用户实体</returns>
        public UserDemo GetUserByStrParam(int id, string userName)
        {
            string sqlStr = "select * from userdemo where id = " + id + " and username = '" + userName + "'";//可简化为"where id = " + id + " and username = '" + userName + "'"

            using (var db = DbInstance.CreateDataBase())
            {
                return db.FirstOrDefault<UserDemo>(sqlStr);
            }
        }

        /// <summary>
        /// 用户新增
        /// </summary>
        /// <param name="model">用户实体</param>
        /// <returns>是否成功bool</returns>
        public bool UserInsert(UserDemo model)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return Convert.ToInt32(db.Insert(model)) > 0;//db.Insert返回的是主键 因为我的是整型所以判断大于0是否插入成功
            }
        }

        /// <summary>
        /// 用户新增 可以用原生的insert 能用实体就实体吧
        /// </summary>
        /// <param name="userName">姓名</param>
        /// <param name="phone">电弧</param>
        /// <param name="userCount">账号</param>
        /// <returns>是否成功bool</returns>
        public bool UserInsert(string userName, string phone, string userCount)
        {
            string insertSql = "insert into userdemo (userName,phone,usercount) values(@0,@1,@2)";//也可以拼接sql

            Sql sql = Sql.Builder.Append(insertSql, new object[] { userName, phone, userCount });//拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                return db.Execute(sql) > 0;//db.Execute是执行sql返回受影响行数 
            }
        }


        /// <summary>
        /// 用户修改
        /// </summary>
        /// <param name="model">用户实体</param>
        /// <returns>是否成功bool</returns>
        public bool UserUpdate(UserDemo model)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Update(model) > 0;//db.Update返回受影响行数
            }
        }

        /// <summary>
        /// 执行sql 不需要返回内容 返回成功与否
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userName">姓名</param>
        /// <param name="phone">电弧</param>
        /// <param name="userCount">账号</param>
        /// <returns>是否成功bool</returns>
        public bool UserUpdate(int id, string userName, string phone, string userCount)
        {
            string updateSql = "update userdemo set username =@0,phone =@1,usercount=@2 where id = @3";//也可以拼接sql

            Sql sql = Sql.Builder.Append(updateSql, new object[] { userName, phone, userCount, id });//拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                return db.Execute(sql) > 0;//db.Execute是执行sql返回受影响行数 
            }
        }
        
        /// <summary>
        /// 删除 注意这里是物理删除 逻辑删除请调用db.execute 执行update 语句
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool UserDelete(int id)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Delete<UserDemo>(id) > 0;//db.Delete是执行sql返回受影响行数 //也可以写成不是主键删除 可以写成 db.Delete<UserDemo>( "where username = @0", new object[] { userName})
            }
        }


        /// <summary>
        /// 获取表里面的某一个属性的记录
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>username string</returns>
        public string GetUserName(int id)
        {
            string sqlStr = "select username from userdemo where id =@0";//也可以拼接sql

            Sql sql = Sql.Builder.Append(sqlStr, new object[] { id });//拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                return db.ExecuteScalar<string>(sql);//db.ExecuteScalar 后的<> 根据你需要的类型获取选择 现在是string的
            }
        }


        /// <summary>
        /// 获取所有用户信息无分页
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>列表</returns>
        public List<UserDemo> GetUserList(string userName)
        {
            string sqlStr = "select * from userdemo where username like @0";//可以简化为 where username like @0   这里也可以拼接sql
            Sql sql = Sql.Builder.Append(sqlStr, new object[] { userName });////拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                return db.Fetch<UserDemo>(sql);//db.Fetch返回所有内容 比如下拉框列表<> 里也是int string 等的列表
            }
        }

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="key">关键字</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<UserDemo> GetPageUserList(int pageIndex, int pageSize, string key, out int totalRecord)
        {
            string sqlStr = "select * from userdemo where username like @0";//可以简化为 where username like @0   这里也可以拼接sql
            Sql sql = Sql.Builder.Append(sqlStr, new object[] { key });////拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<UserDemo>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }


        /// <summary>
        /// 分页获取用户列表 关联其他表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="key">关键字</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<UserDemo> GetPageUserList2(int pageIndex, int pageSize, string key, out int totalRecord)
        {
            string sqlStr = "select a.*,b.GroupName from userdemo a left join usergroup b on a.groupid = b.id where username like @0";//这里也可以拼接sql 要在UserDemo 实体里面加上其他表的字段GroupName
            Sql sql = Sql.Builder.Append(sqlStr, new object[] { key });////拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<UserDemo>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }


        /// <summary>
        /// 使用事务执行 demo
        /// </summary>
        /// <returns></returns>
        public bool ExcuteSqlUseTrans(UserDemo model)
        {
            bool flag = false;
            string updateSql = " update userdemo set username = '000' where id =@0";
            string insertSql = "insert into systemlog (content) values ('插入用户')";
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    int id = Convert.ToInt32(db.Insert(model));
                    db.Execute(updateSql, new object[] {id});
                    db.Execute(insertSql);
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            return flag;
        }


    }
}
