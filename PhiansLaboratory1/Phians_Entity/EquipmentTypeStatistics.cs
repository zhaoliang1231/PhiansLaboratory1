using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    public class EquipmentTypeStatistics
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }
        ///// <summary>
        ///// 保养天数
        ///// </summary>
        //public float MaintenanceDays { get; set; }
        ///// <summary>
        ///// 保养率
        ///// </summary>
        //public float MaintenanceUseRatio { get; set; }
        /// <summary>
        /// 维修天数
        /// </summary>
        public float FixDays { get; set; }
        /// <summary>
        /// 维修率
        /// </summary>
        public float FixUseRatio { get; set; }
        /// <summary>
        /// 安装天数
        /// </summary>
        public float InstallationDays { get; set; }
        /// <summary>
        /// 安装率
        /// </summary>
        public float InstallationUseRatio { get; set; }
        /// <summary>
        /// 测试天数
        /// </summary>
        public float TestDays { get; set; }
        /// <summary>
        /// 测试率
        /// </summary>
        public float TestUseRatio { get; set; }
        /// <summary>
        /// 非测试天数
        /// </summary>
        public float NonTestDay { get; set; }
        /// <summary>
        /// 非测试率
        /// </summary>
        public float NonTestUseRatio { get; set; }


    }

}
