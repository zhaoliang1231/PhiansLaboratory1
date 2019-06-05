using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.Data.OleDb;
 
namespace phians
{
    public class SQLHelper
    {
        //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
        public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["pubsConnectionString"].ConnectionString;//理化
        public static readonly string Lossless_reportConnectionString2 = ConfigurationManager.ConnectionStrings["pubsConnectionString2"].ConnectionString;//计量
        // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///执行一个不需要返回值的SqlCommand命令，通过指定专用的连接字符串。
        /// 使用参数数组形式提供参数列表 
        /// </summary>
        /// <remarks>
        /// 使用示例：
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空SqlCommand中的参数列表
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        ///执行一条不返回结果的SqlCommand，通过一个已经存在的数据库连接 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个现有的数据库连接</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行一条不返回结果的SqlCommand，通过一个已经存在的数据库事物处理 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个存在的 sql 事物处理</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

       
        /// <summary>
        /// 执行一条返回结果集的SqlCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        //理化的数据库
        public static SqlDataReader ExecuteReader( CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction);
            if (conn.State == ConnectionState.Closed)
            {
             conn.Open();
            }
           
            // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally {
                
            }
                
        }
        //计量的数据库
        public static SqlDataReader ExecuteReader1(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(Lossless_reportConnectionString2);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally
            {

            }

        }
        /// <summary>
        /// 执行一条返回第一条记录第一列的SqlCommand命令，通过专用的连接字符串。 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行一条返回第一条记录第一列的SqlCommand命令，通过已经存在的数据库连接。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个已经存在的数据库连接</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 缓存参数数组
        /// </summary>
        /// <param name="cacheKey">参数缓存的键值</param>
        /// <param name="cmdParms">被缓存的参数列表</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            //新建一个参数的克隆列表
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        public DataSet GetDataSet(string strfaca)
        {
           
            //SqlConnection con = getConn();
            //SqlDataAdapter adp = new SqlDataAdapter(strfaca, con);
            //DataSet ds = new DataSet();
            //adp.Fill(ds);
            //con.Close();
            //return ds;
            string str = System.Configuration.ConfigurationManager.ConnectionStrings["pubsConnectionString"].ToString();
            try
            {               
                using (SqlConnection connection = new SqlConnection(str)) {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(strfaca, connection))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        return ds;

                    }           
                }
                                                                    
            }
            catch (Exception e)
            {
               
                throw e;
            }
           
        }

        //计量
        public DataSet GetDataSet(string strfaca, string str)
        {

            //SqlConnection con = getConn();
            //SqlDataAdapter adp = new SqlDataAdapter(strfaca, con);
            //DataSet ds = new DataSet();
            //adp.Fill(ds);
            //con.Close();
            //return ds;
          
            try
            {
                using (SqlConnection connection = new SqlConnection(str))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(strfaca, connection))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        return ds;

                    }
                }

            }
            catch (Exception  e)
            {

                throw e;
            }

        }
        public SqlConnection getConn()
        {
                SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction);                                                       
                connection.Open();
                return connection;                                                 
        }

    

        public OleDbConnection getConn2()
        {

            OleDbConnection cnnxls = new OleDbConnection(Lossless_reportConnectionString2);
            cnnxls.Open();
            return cnnxls;
        } 

        public DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, CommandType.Text, null);
        }
        /// <summary> 
        /// 执行一个查询,并返回查询结果 
        /// </summary> 
        /// <param name="sql">要执行的SQL语句</param> 
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者SQL文本命令</param> 
        /// <returns>返回查询结果集</returns> 
        public DataTable ExecuteDataTable(string sql, CommandType commandType)
        {
            return ExecuteDataTable(sql, commandType, null);
        }

        public DataTable ExecuteDataTable(string sql, CommandType commandType, SqlParameter[] parameters)
        {




            DataTable data = new DataTable();//实例化DataTable，用于装载查询结果集 
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = commandType;//设置command的CommandType为指定的CommandType 
                    //如果同时传入了参数，则添加这些参数 
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    //通过包含查询SQL的SqlCommand实例来实例化SqlDataAdapter 
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(data);//填充DataTable 
                }
            }
            return data;
        } 


        /// <summary>  
        /// DataSet转换成Json格式   
        /// </summary>   
        /// <paramname="ds">DataSet</param>  
        ///<returns></returns>   
        public string Dataset2Json(DataSet ds,string strsql2,int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"total\":");
                if (total == -1)
                {
                    json.Append(dt.Rows.Count);
                }
                else
                {
                    SQLHelper sqla1 = new SQLHelper();
                    string strsql = strsql2;//按条件查询数据
                    DataSet ds1 = sqla1.GetDataSet(strsql);
                    DataTable dt1 = ds1.Tables[0];
                    int count1 = dt1.Rows.Count;
                    json.Append(count1);
                }
                json.Append(",\"rows\":[");
                json.Append(DataTable2Json(dt));
                json.Append("]}");
            }
            return json.ToString();
        }

        /// <summary>
        /// dataset 转 json
        /// </summary>
        /// <param name="ds">包含总数的dataset</param>
        /// <returns></returns>
        public string DataSetToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"total\":");
            json.Append(ds.Tables[1].Rows[0][0]);
            json.Append(",\"rows\":[");
            json.Append(DataTable2Json(ds.Tables[0]).Trim());
            json.Append("]}");
            return json.ToString();
        }
        public string Dataset2Json(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {

                json.Append("[");

                json.Append(DataTable2Json(dt));
                json.Append("]");
            }
            return json.ToString();
        }
        /// <summary>   
        /// dataTable转换成Json格式   
        /// </summary>   
        /// <paramname="dt"></param>   
        ///<returns></returns>   
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    string text_ = dt.Rows[i][j].ToString().Trim().Replace("\\", "\\\\");
                    text_ = text_.Replace("\r", "");
                    text_ = text_.Replace("\n", "");
                    text_ = text_.Replace("\"", "'");
                    //\r\n
                    jsonBuilder.Append(text_);
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }

            return jsonBuilder.ToString();
        }

        public static void ExecuteSqlTran(List<string> hxw_SQL_List, HttpContext context)
        {                                 
            using ( SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                SqlTransaction tran = connection.BeginTransaction();
                cmd.Connection = tran.Connection;
                cmd.Transaction = tran; //获取或设置将要其执行的事务                  
                try
                {
                    for (int n = 0; n < hxw_SQL_List.Count; n++)
                    {
                        string hxw_sql_str = hxw_SQL_List[n].ToString();
                        if (hxw_sql_str.Trim().Length > 1)
                        {
                            cmd.CommandText = hxw_sql_str;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                    context.Response.Write("T");
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tran.Rollback();
                    throw new Exception(E.Message);
                }
            }

        }
        public static void ExecuteSqlTran(List<string> hxw_SQL_List)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                SqlTransaction tran = connection.BeginTransaction();
                cmd.Connection = tran.Connection;
                cmd.Transaction = tran; //获取或设置将要其执行的事务                  
                try
                {
                    for (int n = 0; n < hxw_SQL_List.Count; n++)
                    {
                        string hxw_sql_str = hxw_SQL_List[n].ToString();
                        if (hxw_sql_str.Trim().Length > 1)
                        {
                            cmd.CommandText = hxw_sql_str;
                         
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tran.Rollback();
                    throw new Exception(E.Message);
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hxw_SQL_List"> list sql语句</param>
        /// <param name="commandParameters">list 参数列表</param>
        public static void ExecuteSqlTran(List<string> hxw_SQL_List, List<SqlParameter[]> commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringLocalTransaction))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                SqlTransaction tran = connection.BeginTransaction();
                cmd.Connection = tran.Connection;
                cmd.Transaction = tran; //获取或设置将要其执行的事务                  
                try
                {
                    for (int n = 0; n < hxw_SQL_List.Count; n++)
                    {
                        SqlParameter[] new_SqlParameter = commandParameters[n];
                        string hxw_sql_str = hxw_SQL_List[n].ToString();
                        if (hxw_sql_str.Trim().Length > 1)
                        {
                            cmd.CommandText = hxw_sql_str;
                            if (new_SqlParameter != null)
                            {
                                foreach (SqlParameter parm in new_SqlParameter)
                                {
                                    if (parm.SqlValue == null)
                                    {
                                      
                                        cmd.Parameters.Add(parm.ParameterName.ToString(), DBNull.Value);
                                    }
                                      
                                    else { cmd.Parameters.Add(parm); }


                                    
                                }
                            }
                            cmd.ExecuteNonQuery();
                           
                          
                        }
                    }

                    cmd.Parameters.Clear();
                    tran.Commit();

                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    tran.Rollback();
                    throw new Exception(E.Message);
                }
            }

        }
    }
}