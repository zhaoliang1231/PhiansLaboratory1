using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ReportDistributionReceive
    {
        /// <summary>
        /// 未分配
        /// </summary>
        Distribution = 0,
        /// <summary>
        /// 已分配,待接收
        /// </summary>
        Receive = 1,

        /// <summary>
        /// 已接收，完成
        /// </summary>
        Finish = 2
    }
}