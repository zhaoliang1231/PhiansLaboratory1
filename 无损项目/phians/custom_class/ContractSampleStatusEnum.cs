using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{       //样品存放状态
    public enum ContractSampleStatusEnum
    {
        /// <summary>
        /// 待接收
        /// </summary>
        Accept = 0,

        /// <summary>
        /// 待领取
        /// </summary>
        Receive = 1,

        /// <summary>
        /// 领取完成
        /// </summary>
        Finish = 2,

        /// <summary>
        /// 失效
        /// </summary>
        Invalid = 3,
        /// <summary>
        /// 留样
        /// </summary>
        Stay =  4 ,

    }
}