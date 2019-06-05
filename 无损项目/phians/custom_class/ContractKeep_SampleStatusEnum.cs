using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public enum ContractKeep_SampleStatusEnum
    {
        /// <summary>
        /// 不留样/可领取
        /// </summary>
        Receive = 0,

        /// <summary>
        /// 留样
        /// </summary>
        Keep_Sample = 1,

        /// <summary>
        /// 损坏
        /// </summary>
        Damage_Sample = 2,

        /// <summary>
        /// 至试验完成
        /// </summary>
        test_finish = 3

    }
}