using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ErrorReportStatusEnum
    {
        /// <summary>
        /// 退回
        /// </summary>
        Return = 0,

        /// <summary>
        /// 申请
        /// </summary>
        Apply = 1,

        /// <summary>
        /// 修改编辑
        /// </summary>
        ModifyEdit = 2,

        /// <summary>
        /// 修改审核
        /// </summary>
        ModifyAudit = 3,

        /// <summary>
        /// 完成
        /// </summary>
        Finish = 4
    }
}