using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    public class EquipmentTypeTestStatistics
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 测试天数
        /// </summary>
        public float TestDays { get; set; }

        /// <summary>
        /// 测试利用率
        /// </summary>
        public float TestUseRatio { get; set; }
    }

}
