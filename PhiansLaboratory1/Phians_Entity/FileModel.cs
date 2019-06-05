using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    /// 文件读取传参数model
    /// </summary>
    public class TFileModel
    {
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 操作表的名字
        /// </summary>
        public string Tablename
        {
            get;
            set;
        }
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 表主键
        /// </summary>
        public dynamic PrimaryKey
        {

            get;
            set;
        }
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 表主键
        /// </summary>
        public dynamic PrimaryValue
        {

            get;
            set;
        }
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 文件url对应列名
        /// </summary>
        public string FileUrlColumnName
        {
            get;
            set;
        }
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 文件格式对应列名
        /// </summary>
        public string FileFormatColumnName
        {
            get;
            set;
        }
        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        ///  主键类型
        /// </summary>
        public string PrimaryType
        {
            get;
            set;
        }
      
    }
}


