using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IMainformDAL
    {
        //加载页面树
        List<TB_FunctionalModule> GetMenuData(Guid UserId);

      

    }
}
