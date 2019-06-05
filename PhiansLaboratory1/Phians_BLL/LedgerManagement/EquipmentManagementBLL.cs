using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
using Phans_DAL_INTERFACE.LedgerManagement;
using Phans_DAL_DALFactory;
using Windows.Excel;
using System.IO;
using Phians_Entity.Common;
using PhiansCommon.ExcelOperate;
using System.Data;
using PhiansCommon;

namespace Phians_BLL
{
    public class EquipmentManagementBLL
    {
        IEquipmentManagementDAL dal = DALFactory.GetLedgerManagement();

        #region  加载设备列表
        public List<TB_NDT_equipment_library> GetEquipmentList(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetEquipmentList( PageModel, out  totalRecord);
        }
        #endregion

       
        #region 新增设备
        public ReturnDALResult AddEquipment(TB_NDT_equipment_library model, Guid LogPersonnel)
        {
            ReturnDALResult flag = dal.AddEquipment(model, LogPersonnel);
            return flag;
        }
        #endregion

        #region 修改设备
        public ReturnDALResult EditEquipment(dynamic model, Guid LogPersonnel)
        {
            return dal.EditEquipment(model, LogPersonnel);
        }
        #endregion

        #region 删除设备
        public ReturnDALResult DelEquipment(string id, Guid LogPersonnel, string equipment_nem)
        {
            return dal.DelEquipment(id, LogPersonnel, equipment_nem);
        }       
        #endregion    
    }
}
