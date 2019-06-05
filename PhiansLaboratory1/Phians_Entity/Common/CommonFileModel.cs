using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity.Common
{
    /// <summary>
    /// 通用文件返回文件信息
    /// </summary>
   public class CommonFileModel
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 文件 格式
        /// </summary>
        public string FileFormat
        {
            get;
            set;
        }
       
    }
}
