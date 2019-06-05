using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    public class HeadCountStatistics
    {
        /// <summary>
        /// 统计名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// staff数量
        /// </summary>
        public int StaffCount { get; set; }

        /// <summary>
        /// idl数量
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int IDLCount { get; set; }
    }
}
