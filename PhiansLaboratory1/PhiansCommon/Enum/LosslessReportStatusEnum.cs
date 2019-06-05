using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhiansCommon.Enum
{
   public enum LosslessReportStatusEnum
    {
        /// <summary>
        /// 报告编辑状态
        /// </summary>
        Edit = 1,

        /// <summary>
        /// 报告审核状态
        /// </summary>
        Audit = 2,

        /// <summary>
        /// 报告签发状态 
        /// </summary>
        Issue = 3,

        /// <summary>
        /// 完成状态
        /// </summary>
        Finish = 4,

        /// <summary>
        /// 异常申请
        /// </summary>
        Abnormal = 5,

        /// <summary>
        /// 报废申请
        /// </summary>
        Scrap = 6
    }
}
