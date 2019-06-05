using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
using Phians_Entity.Common;

namespace Phans_DAL_INTERFACE.LedgerManagement
{
    public interface IEquipmentManagementDAL
    {
        #region 加载设备
        List<TB_NDT_equipment_library> GetEquipmentList(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 添加设备
        ReturnDALResult AddEquipment(TB_NDT_equipment_library model, Guid LogPersonnel);
        #endregion

        #region 修改设备
        ReturnDALResult EditEquipment(dynamic model, Guid LogPersonnel);

        #endregion

        #region 删除设备
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="EquipmentCode">设备编号</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <param name="EquipmentName">设备名字</param>
        /// <param name="Address">仓库位置ID</param>
        /// <returns></returns>
        ReturnDALResult DelEquipment(string id, Guid LogPersonnel, string equipment_nem);
        #endregion


    }
}
