using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    public class ReportStatisticsRate
    {
        public string Name { get; set; }

        /// <summary>
        /// 一次通过的数量
        /// </summary>
        public int OncePassCount { get; set; }

        /// <summary>
        /// 最终无问题数量
        /// </summary>
        public int NoErrorCount { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
