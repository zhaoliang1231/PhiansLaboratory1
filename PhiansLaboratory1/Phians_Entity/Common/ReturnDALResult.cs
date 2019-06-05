using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity.Common
{
    /// <summary>
    /// 数据层操作返回结果
    /// </summary>
   public class ReturnDALResult
    {
       /// <summary>
       /// 成功操作 0：失败；1：成功；2；异常
       /// </summary>
       public int Success { get; set; }
       /// <summary>
       ///返回主键
       /// </summary>
       public string PrimaryKey { get; set; }
       /// <summary>
       /// 返回操作内容
       /// </summary>
       public string returncontent { get; set; }


       /// <summary>
       /// 返回消息通知人
       /// </summary>
       public string MessagePerson { get; set; }

    }
}
