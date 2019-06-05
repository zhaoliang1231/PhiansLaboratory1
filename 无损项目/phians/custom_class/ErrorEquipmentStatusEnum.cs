using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ErrorEquipmentStatusEnum
    {
        /// <summary>
        /// 在用
        /// </summary>
        ZY = 0,

        /// <summary>
        /// 停用
        /// </summary>
        TY = 1,

        /// <summary>
        /// 报废
        /// </summary>
        BF = 2,

        /// <summary>
        /// 封存
        /// </summary>
        FC = 3,

        /// <summary>
        /// 丢失
        /// </summary>
        DS = 4
    }
}