using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL.Enum
{
    /// <summary>
    /// 异常报告状态
    /// </summary>
    public enum ReportabnormityEnum
    {
        /// <summary>
        /// 异常申请审核
        /// </summary>
        ReportApplicationReview = 0,
        /// <summary>
        /// 异常申请退回
        /// </summary>
        ReportApplicationReviewBack = 1,
        /// <summary>
        /// 异常编辑
        /// </summary>
        ReportEdit = 2,
          /// <summary>
        /// 异常审核退回
        /// </summary>
        ReportReviewBack = 3,
        /// <summary>
        /// 异常签发退回
        /// </summary>
        ReportIssueBack = 4,
        /// <summary>
        /// 异常审核
        /// </summary>
        ReportReview= 5,
        /// <summary>
        /// 异常签发
        /// </summary>
        ReportIssue = 6,
        /// <summary>
        /// 完成
        /// </summary>
        ApplicationFinish = 7,
        

    }
}
