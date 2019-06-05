using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum StandardLibraryStatusEnum
    {
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 0 ,

        /// <summary>
        /// 审核不通过
        /// </summary>
        Audit_not_through = 1,

        /// <summary>
        /// 审核
        /// </summary>
        Audit = 2,

        /// <summary>
        /// 完成
        /// </summary>
        Finish = 3
    }
}