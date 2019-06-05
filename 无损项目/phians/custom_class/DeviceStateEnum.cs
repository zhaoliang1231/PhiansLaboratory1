using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum DeviceStateEnum
    {
        /// <summary>
        /// 在用
        /// </summary>
        ZY = 1,

        /// <summary>
        /// 停用
        /// </summary>
        TY = 2,

        /// <summary>
        /// 作废
        /// </summary>
        ZF = 3,

        /// <summary>
        /// 封存
        /// </summary>
        FC = 4,

        /// <summary>
        /// 删除
        /// </summary>
        SC = 5
    }
}