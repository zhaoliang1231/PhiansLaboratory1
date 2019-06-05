using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
using Phians_Entity.Common;

namespace Phans_DAL_INTERFACE
{
    public interface IOrganizationDAL
    {
        //加载部门管理界面
        List<TB_Organization> GetDepartmentManagementList(string ParentId);
        //部门模块信息加载
        List<TB_Organization> LoadDepartmentModuleInfo(Guid NodeId, int pageIndex, int pageSize, out int totalRecord);
        //加载部门信息
        TB_Organization LoadDepartmentInfo(Guid NodeId);
        //添加部门
        TB_Organization AddDepartment(TB_Organization model);
        //修改部门信息
        bool EditDepartment(TB_Organization model);
        //删除部门信息
        bool DelDepartment(Guid NodeId, string NodeName, Guid CreatePersonnel);
    }
}
