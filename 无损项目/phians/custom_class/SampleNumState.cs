using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum SampleNumState
    {
        /// <summary>
        /// 有效，未生成
        /// </summary>
        Valid = 0,

        /// <summary>
        /// 失效
        /// </summary>
        Invalid = 1,

        /// <summary>
        /// 样品接收已生成
        /// </summary>
        sample_split = 2,

        /// <summary>
        /// 样品接收已保存/任务已经到达任务池
        /// </summary>
        sample_save = 3
    }
}