using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum LosslessReport_EditApplyEnum
    {
        /// <summary>
        /// 报废申请审核
        /// </summary>
        BFSH = 0,

        /// <summary>
        /// 报废申请审核退回
        /// </summary>
        BFTH = 7,

        /// <summary>
        /// 报废申请完成
        /// </summary>
        BFWC = 8,

        /// <summary>
        /// 异常申请审核
        /// </summary>
        SQSH = 1,

        /// <summary>
        /// 异常申请退回
        /// </summary>
        SQTH = 2,

        /// <summary>
        /// 异常编制
        /// </summary>
        YCBZ = 3,

        /// <summary>
        /// 异常编制审核
        /// </summary>
        YCSH = 4,

        /// <summary>
        /// 异常编制审核退回
        /// </summary>
        YCTH = 5,

        /// <summary>
        /// 异常编制完成
        /// </summary>
        YCWC = 6

       
    }
}