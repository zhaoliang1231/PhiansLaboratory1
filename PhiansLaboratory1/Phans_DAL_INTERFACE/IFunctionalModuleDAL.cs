using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;

namespace Phans_DAL_INTERFACE
{
    public interface IFunctionalModuleDAL
    {
        //加载页面
        List<TB_FunctionalModule> FunctionalModuleLoad(string PageId);
        //获取页面信息
        TB_FunctionalModule FunctionalModuleLoadPageInfo(Guid PageId);
        //模块信息加载
        List<TB_FunctionalModule> LoadModuleInfo(Guid PageId, int pageIndex, int pageSize, out int totalRecord);
        //添加页面
        Guid FunctionalModuleAdd(TB_FunctionalModule FunctionalModule);
        //修改页面
        bool FunctionalModuleEdit(TB_FunctionalModule FunctionalModule);
        //删除页面
        bool FunctionalModuleDel(Guid PageId);
    }
}
