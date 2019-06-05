using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
    public class PageShowSettingBLL
    {
        IPageShowSettingDAL dal = DALFactory.GetPageShowSetting();

        #region  加载表的所有字段信息
        public List<TB_PageShowCustom> loadInfo(TPageModel PageModel, out int totalRecord)
        {
            return dal.loadInfo(PageModel, out  totalRecord);
        }
        #endregion

        #region 修改字段显示信息
        public ReturnDALResult EditInfo(List<TB_PageShowCustom> model, Guid LogPersonnel)
        {
            return dal.EditInfo(model, LogPersonnel);
        }
        #endregion
    }
}
