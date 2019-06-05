using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhiansCommon.Enum
{
    public enum TB_ProcessRecordNodeIdEnum
    {
        #region 编制or审核人自己退回报告

        /// <summary>
        /// 编制取回审核报告--编制人员自己拉回审核报告到报告编制
        /// </summary>
        EditTakeBack = 17,

        /// <summary>
        /// 审核取回签发报告--审核人员自己拉回签发报告到报告审核
        /// </summary>
        ReviewtTakeBack = 18,

        #endregion

        /// <summary>
        /// 初始报告编制--提交到--报告审核
        /// </summary>
        EditToReview = 0,

        /// <summary>
        /// 再次报告编制--提交到--报告审核
        /// </summary>
        AgainEditToReview = 16,

        /// <summary>
        /// 报告审核--提交到--报告签发
        /// </summary>
        ReviewToIssue = 1,

        /// <summary>
        /// 报告审核--退回到--报告编制
        /// </summary>
        ReviewToEdit = 3,

        /// <summary>
        /// 报告审核--提交到到--报告管理
        /// </summary>
        ReviewToManage = 13,

        /// <summary>
        /// 报告签发--退回到--报告编制
        /// </summary>
        IssueToEdit = 4,

        /// <summary>
        /// 报告签发--提交到--报告管理
        /// </summary>
        IssueToManage = 5,

        #region 异常修改报告流程

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


        #endregion 

        #region 异常报废报告流程
        /// <summary>
        /// 报告管理--提交到--报废报告申请
        /// </summary>
        ManageToBFApply = 15,

        /// <summary>
        /// 报废报告申请--退回到--报告管理
        /// </summary>
        BFApplyToManage = 14,

        /// <summary>
        /// 报废报告申请--提交到--报废完成（报告报废流程）
        /// </summary>
        BFApplyToFinish = 12

        #endregion 


    }
}
