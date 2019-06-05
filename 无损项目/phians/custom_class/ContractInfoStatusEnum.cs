using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ContractInfoStatusEnum
    {
        /// <summary>
        /// 未提交
        /// </summary>
        Unsubmit = 0,

        /// <summary>
        /// 接收退回
        /// </summary>
        Receive_return = 1,

        /// <summary>
        /// 审核退回
        /// </summary>
        Audit_return = 2,

        /// <summary>
        /// 待审核
        /// </summary>
        Audit = 3,

        /// <summary>
        /// 待接收
        /// </summary>
        Receive = 4,

        /// <summary>
        /// 报告签发
        /// </summary>
        Report_issue = 5,

        /// <summary>
        /// 完成
        /// </summary>
        Finish = 6,
        /// <summary>
        /// 报告审核
        /// </summary>
        /// 
        Report_Audit = 7,
        /// <summary>
        /// 报告审核退回
        /// </summary>
        Audit_return_R = 8,
       /// <summary>
       /// 报告签发
       /// </summary>
           issue_return_R = 9,

        ///// <summary>
        ///// 机加
        ///// </summary>
        //Machining = 7

    }
}