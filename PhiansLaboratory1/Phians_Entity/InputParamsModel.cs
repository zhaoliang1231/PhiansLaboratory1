using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    /// <summary>
    /// 接口参数model
    /// </summary>
   public class InputParamsModel
    {

        /// <summary>
        /// 测试时间开始时间
        /// </summary>
        public string TestStartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 测试结束时间
        /// </summary>
        public string TestEndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator
        {
            get;
            set;
        }
        /// <summary>
        /// 结论
        /// </summary>
        public string Conclusion
        {
            get;
            set;
        }

        /// <summary>
        /// 任务id  子任务
        /// </summary>
        public string TaskID
        {
            get;
            set;
        }
        /// <summary>
        /// 房间名
        /// </summary>
        public string RoomName
        {
            get;
            set;
        }
        /// <summary>
        /// 检测结果
        /// </summary>
        public string TestResult
        {
            get;
            set;
        }
    }
}
