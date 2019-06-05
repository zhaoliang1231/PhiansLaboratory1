using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity.Common
{
   
    /// <summary>
    /// 返回bll结果
    /// </summary>
    public class ReturnBLLResult
    {

        //成功操作 0：失败；1：成功；2；异常
        public int Success { get; set; }
        //返回主键
        public string PrimaryKey { get; set; }
        //返回操作内容
        public string returncontent { get; set; }
       //返回异常信息
        public string Exceptioncontent { get; set; }
    }
}
