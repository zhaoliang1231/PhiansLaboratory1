using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ContractTaskStatusEnum
    {
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 0,

        /// <summary>
        /// 任务生成
        /// </summary>
        Task_generate = 1,

        /// <summary>
        /// 待领取
        /// </summary>
        Receive = 2,

        /// <summary>
        /// 待检测
        /// </summary>
        Check = 3,

        /// <summary>
        /// 校对
        /// </summary>
        Proof = 4,

        /// <summary>
        /// 报告待编辑
        /// </summary>
        ReportEdit = 5,

        /// <summary>
        /// 报告待审核
        /// </summary>
        ReportAudit = 6,

        /// <summary>
        /// 报告待审批
        /// </summary>
        ReportAppproval = 7,

        /// <summary>
        /// 完成
        /// </summary>
        Finish = 8,

        /// <summary>
        /// 任务退回
        /// </summary>
        Task_return = 9,
        /// <summary>
        /// 报告整理
        /// </summary>
        arrange = 10
    }
}