using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    public class ResultStatistics : CommonStatistics
    {
        /// <summary>
        /// 通过的的数量
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// Fail的数量
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// na的数量
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int NACount { get; set; }
    }
}
