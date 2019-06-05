using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface ITestBlockLibraryDAL
    {
        #region  试块库列表
        List<TB_NDT_TestBlockLibrary> load_TestBlockLibrary(TPageModel PageModel, out int totalRecord);
        #endregion

        #region  添加试块
        ReturnDALResult Add_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel);
        #endregion

        #region  修改试块
        ReturnDALResult Edit_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel);
        #endregion

        #region  删除试块
        /// <summary>
        /// 删除试块
        /// </summary>
        /// <param name="model">探头实体</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <returns></returns>
        ReturnDALResult Del_TestBlockLibrary(TB_NDT_TestBlockLibrary model, Guid LogPersonnel);
        #endregion

        #region 批量导入试块
        /// <summary>
        /// 批量导入试块
        /// </summary>
        /// <param name="listmodel">导入的数据model</param>
        /// <param name="OperatePerson">操作人</param>
        /// <returns></returns>
        ReturnDALResult importTestBlockLibrary(List<TB_NDT_TestBlockLibrary> listmodel, Guid OperatePerson);
        #endregion
    }
}
