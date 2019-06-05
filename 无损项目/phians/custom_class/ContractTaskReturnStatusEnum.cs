using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ContractTaskReturnStatusEnum
    {
        /// <summary>
        /// 未审核
        /// </summary>
        NoReview = 0,

        /// <summary>
        /// 审核通过
        /// </summary>
        Review = 1,

        /// <summary>
        /// 审核退回
        /// </summary>
        Review_return = 2

    }
}