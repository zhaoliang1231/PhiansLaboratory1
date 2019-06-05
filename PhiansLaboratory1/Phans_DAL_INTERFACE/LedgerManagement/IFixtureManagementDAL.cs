using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE.LedgerManagement
{
    public interface IFixtureManagementDAL
    {
        #region  探头列表
        List<TB_NDT_probe_library> GetProbeList(TPageModel PageModel, out int totalRecord);
        #endregion

        #region  添加探头
        ReturnDALResult Probe_add(TB_NDT_probe_library model, Guid LogPersonnel);
        #endregion

        #region  修改探头
        ReturnDALResult Probe_edit(TB_NDT_probe_library model, Guid LogPersonnel);
        #endregion

        #region  删除探头
      /// <summary>
        /// 删除探头
      /// </summary>
      /// <param name="model">探头实体</param>
      /// <param name="LogPersonnel">操作人</param>
      /// <returns></returns>
        ReturnDALResult Probe_delete(TB_NDT_probe_library model, Guid LogPersonnel);
        #endregion


        #region 批量导入探头
        /// <summary>
        /// 批量导入探头
        /// </summary>
        /// <param name="listmodel">导入的数据model</param>
        /// <param name="OperatePerson">操作人</param>
        /// <returns></returns>
        ReturnDALResult importProbe(List<TB_NDT_probe_library> listmodel, Guid OperatePerson);
        #endregion
    }
}
