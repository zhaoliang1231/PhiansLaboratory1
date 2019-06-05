using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
   public class EquipmentStatistics
    {
       /// <summary>
       /// 月份
       /// </summary>
       public string Month { get; set; }

       /// <summary>
       /// 数量
       /// </summary>
       public int EquipmentCount { get; set; }

       /// <summary>
       /// 占用的时间(秒)
       /// </summary>
       public int EquipmentTime { get; set; }

       /// <summary>
       /// 占用的时间(天)
       /// </summary>
       public float EquipmentTimeToday { get; set; }

       /// <summary>
       /// 占用率
       /// </summary>
       public float UseRatio { get; set; }

    }
}
