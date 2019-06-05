using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhiansCommon.Enum
{
    public enum NDT_RevisionsRecordEnum
    {
        /// <summary>
        /// 报告编辑提交
        /// </summary>
        EditSubmit = 1,

        ///// <summary>
        ///// 报告审核提交
        ///// </summary>
        //ReviewSubmit = 2,

        ///// <summary>
        ///// 报告签发提交
        ///// </summary>
        //IssusSubmit = 3,

        ///// <summary>
        ///// 报告审核退回
        ///// </summary>
        //ReviewReturn = 4,

        ///// <summary>
        ///// 报告签发退回
        ///// </summary>
        //IssusReturn = 5,

        /// <summary>
        /// 报告审核更改 -- 版本控制
        /// </summary>
        ReviewUpdate = 6,

        /// <summary>
        /// 报告签发更改 -- 版本控制
        /// </summary>
        IssusUpdate = 7

    }
}
