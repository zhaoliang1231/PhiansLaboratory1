using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PhiansCommon
{
  public  class DataTableToList
    {

        /// <summary>        
        /// DataSetToList        
        /// </summary>         
        /// <typeparam name="T">转换类型</typeparam>        
        /// <param name="dataSet">数据源</param>        
        /// <param name="tableIndex">需要转换表的索引</param>       
        /// /// <returns>泛型集合</returns>
        public static List<T> DataSetToList<T>(DataSet dataset, int tableIndex)
        {
            //确认参数有效
            if (dataset == null || dataset.Tables.Count <= 0 || tableIndex < 0)
            {
                return null;
            }

            DataTable dt = dataset.Tables[tableIndex];

            List<T> list = new List<T>();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                T _t = Activator.CreateInstance<T>();

                //获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();

                //属性和名称相同时则赋值
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                info.SetValue(_t, dt.Rows[i][j], null);
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }

                            break;
                        }
                    }
                }

                list.Add(_t);
            }

            return list;
        }


        /// <summary>  
        /// DataTable转化为List集合  
        /// </summary>  
        /// <typeparam name="T">实体对象</typeparam>  
        /// <param name="dt">datatable表</param>  
        /// <param name="isStoreDB">是否存入数据库datetime字段，date字段没事，取出不用判断</param>  
        /// <returns>返回list集合</returns>  
        public static List<T> TableToList<T>(DataTable dt, bool isStoreDB = true)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            //List<string> listColums = new List<string>();  
            PropertyInfo[] pArray = type.GetProperties(); //集合属性数组  
            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>(); //新建对象实例   
                foreach (PropertyInfo p in pArray)
                {
                    if (!dt.Columns.Contains(p.Name) || row[p.Name] == null || row[p.Name] == DBNull.Value)
                    {
                        continue;  //DataTable列中不存在集合属性或者字段内容为空则，跳出循环，进行下个循环     
                    }
                    if (isStoreDB && p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                   
                    try
                    {
                        string dd = p.Name;
                        if (dd == "DueDate")
                        {
                            Console.Write(p.PropertyType);
                        }
                            var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为集合字段类型
                              p.SetValue(entity, obj, null);
                       
                          
                       
                    }
                    catch (Exception e)
                    {
                        // throw;  
                    }
                    //if (row[p.Name].GetType() == p.PropertyType)  
                    //{  
                    //    p.SetValue(entity, row[p.Name], null); //如果不考虑类型异常，foreach下面只要这一句就行  
                    //}                      
                    //object obj = null;  
                    //if (ConvertType(row[p.Name], p.PropertyType,isStoreDB, out obj))  
                    //{                                          
                    //    p.SetValue(entity, obj, null);  
                    //}                  
                }
                list.Add(entity);
            }
            return list;
        }  



       
    }

}
    
