using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum Sample_machiningStatusEnum
    {
        /// <summary>
        /// 未分配
        /// </summary>
        Non_assigned = 0,
        /// <summary>
        /// 未机加
        /// </summary>
        Machining = 1,

        /// <summary>
        /// 机加完成
        /// </summary>
        Finish = 2
    }
}