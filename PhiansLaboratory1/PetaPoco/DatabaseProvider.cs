using System.Collections.Generic;

namespace PetaPoco
{
    /// <summary>
    /// <Remark>描述：数据库提供者，封装数据库相关操作</Remark>
    /// </summary>
    public class DBProvider
    {
        /// <summary>
        /// Remark：获取List列表系列重载方法
        /// </summary>
        #region 获取分页系列重载方法

        public static List<T> GetEntitiesByPager<T>(int pageIndex, int pageSize, string sql, out int recordCount)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<T>(pageIndex, pageSize, sql);
                recordCount = (int)result.TotalItems;

                return result.Items;
            }
        }

        public static List<T> GetEntitiesByPager<T>(int pageIndex, int pageSize, Sql sql, out int recordCount)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<T>(pageIndex, pageSize, sql);
                recordCount = (int)result.TotalItems;

                return result.Items;
            }
        }

        public static List<T> GetEntitiesByPager<T>(int pageIndex, int pageSize, string sql, out int recordCount, params object[] args)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<T>(pageIndex, pageSize, sql, args);
                recordCount = (int)result.TotalItems;

                return result.Items;
            }
        }
        #endregion

        /// <summary>
        /// Remark：获取List列表系列重载方法
        /// </summary>
        #region 获取列表系列重载方法
        public static List<T> Fetch<T>(Sql sql)
        {
            using (var db = DbInstance.CreateDataBase())
            {

                return db.Fetch<T>(sql);
            }
        }

        public static List<T> Fetch<T>(string sql, params object[] args)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Fetch<T>(sql, args);
            }
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, Sql sql)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Fetch<T>(page, itemsPerPage, sql);
            }
        }

        public static List<T> Fetch<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Fetch<T>(page, itemsPerPage, sql, args);
            }
        }
        #endregion

        /// <summary>
        /// Remark：获取IEnumerable列表系列重载方法
        /// </summary>
        #region 获取列表系列重载方法
        public static IEnumerable<T> Query<T>(Sql sql)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Query<T>(sql);
            }
        }

        public static IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Query<T>(sql, args);
            }
        }
        #endregion

        /// <summary>
        /// Remark：获取对象实体系列重载方法
        /// </summary>
        #region 获取对象实体系列重载方法
        public static T SingleOrDefault<T>(Sql sql)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.SingleOrDefault<T>(sql);
            }
        }

        public static T SingleOrDefault<T>(object primaryKey)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.SingleOrDefault<T>(primaryKey);
            }
        }

        public static T SingleOrDefault<T>(string sql, params object[] args)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.SingleOrDefault<T>(sql, args);
            }
        }
        #endregion


        /// <summary>
        /// Remark：插入对象实体系列重载方法
        /// </summary>
        #region 数据新增方法
        public static object Insert(object obj)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Insert(obj);
            }
        }

        public static object Insert(string tableName, string primaryKeyName, bool autoIncrement, object obj)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Insert(tableName, primaryKeyName, obj);
            }
        }
        #endregion


        /// <summary>
        /// Remark：插入对象实体系列重载方法
        /// </summary>
        #region 数据新增方法
        public static int Update(object obj)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Update(obj);
            }
        }

        public static object Update(string tableName, string primaryKey, bool autoIncrement, object obj)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                return db.Insert(tableName, primaryKey, obj);
            }
        }
        #endregion

    }
}
