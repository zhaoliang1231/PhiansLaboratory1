using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
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
        Finish = 4 ,

        /// <summary>
        /// 异常申请
        /// </summary>
        Abnormal = 5,

        /// <summary>
        /// 报废报告
        /// </summary>
        Scrap = 6

    }
}

// int state_1 = (int)LosslessReportStatusEnum.Scrap;
// and state_!=" + state_1 + "
//TB_NDT_test_probereport_data