using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace phians.custom_class
    
{
    public enum ReportProcessStatusEnum
    {
        /// <summary>
        /// 默认初始状态
        /// </summary>
        Default = 0,

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
        /// 报告整理
        /// </summary>
       arrange=5,
        /// <summary>
        /// 审核退回
        /// </summary>
       Audit_return=6,
        /// <summary>
        /// 签发退回
        /// </summary>
       Issue_rturn=7,

    }
}