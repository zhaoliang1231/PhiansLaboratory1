using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IAbnormalReportManagementDAL
    {
        #region 获取集合
        List<TB_NDT_error_Certificate> GetUnusualCertificateListBLL(TPageModel PageModel, out int totalRecord);
        #endregion

    }
}
