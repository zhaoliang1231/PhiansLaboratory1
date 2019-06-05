using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ErrorSampleInvalidStatusEnum
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Processing = 0,

        /// <summary>
        /// 待机加
        /// </summary>
        Machining = 1,

        /// <summary>
        /// 待接收
        /// </summary>
        //Accept = 2,

        /// <summary>
        /// 完成
        /// </summary>
        Finish = 3
    }
}