using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum TB_ProcessRecordNodeIdEnum
    {
        /// <summary>
        /// 报告编制--提交到--报告审核
        /// </summary>
        EditToReview = 0,

        /// <summary>
        /// 报告审核--提交到--报告签发
        /// </summary>
        ReviewToIssue = 1,

        /// <summary>
        /// 报告审核--退回到--报告编制
        /// </summary>
        ReviewToEdit = 3,

        /// <summary>
        /// 报告签发--退回到--报告编制
        /// </summary>
        IssueToEdit = 4,

        /// <summary>
        /// 报告签发--提交到--报告管理
        /// </summary>
        IssueToManage = 5,

        #region 异常报告流程

        /// <summary>
        /// 报告管理--提交到--异常报告申请
        /// </summary>
        ManageToApply = 6,

        /// <summary>
        /// 异常报告申请--退回到--报告管理
        /// </summary>
        ApplyToManage = 7,

        /// <summary>
        /// 异常报告申请--提交到--异常报告编制
        /// </summary>
        ApplyToAbnEdit = 8,

        /// <summary>
        /// 异常报告编制--提交到--异常报告审核
        /// </summary>
        AbnEditToAbnReview = 9,

        /// <summary>
        /// 异常报告审核--退回到--异常报告编制
        /// </summary>
        AbnReviewToAbnEdit = 10,

        /// <summary>
        /// 异常报告审核--提交到--异常报告管理
        /// </summary>
        AbnReviewToAbnManage = 11,

        /// <summary>
        /// 异常报告申请--提交到--异常报告管理（报告报废流程）
        /// </summary>
        BFApplyToManage = 12

        #endregion 



    }
}