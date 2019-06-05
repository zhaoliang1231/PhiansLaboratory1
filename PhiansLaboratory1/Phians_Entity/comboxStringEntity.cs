using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 下拉框实体字符串
    /// </summary>
    class comboxStringEntity
    {

        [Column]
        ///<summary>
        /// value
        /// </summary>		
        public string Value
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Text
        /// </summary>		
        public string Text
        {
            get;
            set;
        }
    }
}
