using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhiansCommon
{
    public  class StringOperation
    {
        /// <summary>
        /// 判断是否是时间字符
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDateString(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
