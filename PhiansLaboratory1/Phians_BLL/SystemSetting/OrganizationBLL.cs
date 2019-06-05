using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System.Data;
using PhiansCommon;
using Phians_Entity.Common;

namespace Phians_BLL
{
    public class OrganizationBLL
    {
        IOrganizationDAL dal = DALFactory.GetOrganization();
        #region 加载部门管理界面
        public string GetDepartmentManagementList(string ParentId)
        {
            if (string.IsNullOrEmpty(ParentId))
            {
                ParentId = "8cff8e9f-f539-41c9-80ce-06a97f481390";
            }
            DataTable new_table = ListToDataTable.ListToDataTable_(dal.GetDepartmentManagementList(ParentId));

            new_table.Columns.Add("state_", typeof(string));//表格添加一个state_字段

            foreach (DataRow dr in new_table.Rows)
            {
                int i = 0;
                foreach (DataRow drs in new_table.Rows)
                {
                    if (dr["NodeId"].ToString() == drs["ParentId"].ToString())
                    {
                        dr["state_"] = "closed";
                        i = 1;
                        break;
                    }
                }

                if (i == 0)
                {
                    dr["state_"] = "open";
                }
            }

            TreeJsonByTable newTree = new TreeJsonByTable();
            string json = newTree.GetTreeJsonByTable(new_table, "NodeId", "NodeId", "NodeName", "ParentId", ParentId, "state_").ToString();
            return json;
        }
        #endregion

        #region 加载模块部门信息
        public List<TB_Organization> LoadDepartmentModuleInfo(Guid NodeId, int pageIndex, int pageSize, out int totalRecord)
        {
            return dal.LoadDepartmentModuleInfo(NodeId, pageIndex, pageSize, out totalRecord);
        }
        #endregion

        #region 加载部门信息
        public TB_Organization LoadDepartmentInfo(Guid NodeId)
        {
            return dal.LoadDepartmentInfo(NodeId);
        }
        #endregion

        #region 添加部门
        public TB_Organization AddDepartment(TB_Organization model)
        {
            return dal.AddDepartment(model);
        }
        #endregion

        #region 修改部门信息
        public bool EditDepartment(TB_Organization model)
        {
            return dal.EditDepartment(model);
        }
        #endregion

        #region 删除部门信息
        public bool DelDepartment(Guid NodeId, string NodeName, Guid CreatePersonnel)
        {
            return dal.DelDepartment(NodeId, NodeName, CreatePersonnel );
        }
        #endregion
    }
}
