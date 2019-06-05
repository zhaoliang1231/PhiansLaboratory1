using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IPageShowSettingDAL
    {
        #region  加载表的所有字段信息
        List<TB_PageShowCustom> loadInfo(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 修改字段显示信息
        ReturnDALResult EditInfo(List<TB_PageShowCustom> model, Guid LogPersonnel);
        #endregion
    }
}
