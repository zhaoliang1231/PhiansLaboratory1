using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
    public class AuthorityManagementBLL
    {
        IAuthorityManagementDAL dal = DALFactory.GetAuthorityManagementDAL();

        #region 加载页面树
        public string LoadPageTree(Guid PageId)
        {
            Guid ParentId = PageId;

            DataTable new_table = ListToDataTable.ListToDataTable_(dal.LoadPageTree(ParentId));

            TreeJsonByTable newTree = new TreeJsonByTable();
            string json = newTree.GetTreeJsonByTable(new_table, "PageId", "ModuleName", "ParentId", ParentId, "NodeType", "IconCls", "U_url", "SortNum", "Remarks", "PermissionFlag", "isParent", "id").ToString();
            return json;


        }


        #endregion

        #region 加载页面表格树
        //public List<TB_FunctionalModule> LoadPageTbleTree(string ParentId, int pageIndex, int pageSize, out int totalRecord)
        //{

        //    if (string.IsNullOrEmpty(ParentId))
        //    {
        //        ParentId = "2147efda-9ce3-4f56-b24c-cf2f16cabc52";
        //    }

        //    return dal.LoadPageTbleTree(ParentId, pageIndex, pageSize, out totalRecord);

        //}


        #endregion

        #region 加载组树
        public string GetGroupTree(string GroupParentId)
        {
            if (string.IsNullOrEmpty(GroupParentId))
            {
                GroupParentId = "8cff8e9f-f539-41c9-80ce-06a97f481390";
            }
            DataTable new_table = ListToDataTable.ListToDataTable_(dal.GetGroupTree(GroupParentId));

            new_table.Columns.Add("state_", typeof(string));//表格添加一个state_字段

            foreach (DataRow dr in new_table.Rows)
            {
                int i = 0;
                foreach (DataRow drs in new_table.Rows)
                {
                    if (dr["GroupId"].ToString() == drs["GroupParentId"].ToString())
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
            string json = newTree.GetTreeJsonByTable(new_table,"id", "GroupId", "GroupName", "GroupParentId", GroupParentId, "state_").ToString();
            return json;
        }
        #endregion

        #region 加载组人员信息
        public List<TB_UserInfo> GetGroupPerList(Guid GroupId, int pageIndex, int pageSize, out int totalRecord, string search, string key)
        {
            return dal.GetGroupPerList(GroupId, pageIndex, pageSize, out totalRecord, search, key);
        }
        #endregion

        #region 组/人员 授权
        public bool GroupAuthority(List<TB_PageAuthorization> mode, string ModuleNames, string GroupName, Guid CreatePersonnel, bool flag2, bool flag3, string ButtonNames)
        {
            return dal.GroupAuthority(mode, ModuleNames, GroupName, CreatePersonnel, flag2, flag3, ButtonNames);
        }
        #endregion

        #region 组/人员 授权回显
        public string ShowAuthorizedPage(dynamic table)
        {
           // return dal.ShowAuthorizedPage(GroupId, UserId);
            Guid ParentId = table.PageId;

            DataTable new_table = ListToDataTable.ListToDataTable_(dal.ShowAuthorizedPage(table));

            TreeJsonByTable newTree = new TreeJsonByTable();
            string json = newTree.GetTreeJsonByTable(new_table, "PageId", "ModuleName", "ParentId", ParentId, "NodeType", "IconCls", "U_url", "SortNum", "Remarks", "PermissionFlag", "isParent", "id").ToString();
            return json;
        }
        #endregion

        #region <<<<<<<<<<<<<<详细权限操作

        #region 加载按钮权限表格
        public List<TB_FunctionalModule> GetButtonAuthorityList(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetButtonAuthorityList(PageModel, out totalRecord);
        }
        #endregion

        #region 回显按钮权限表格
        public List<TB_FunctionalModule> ShowButtonAuthorityList(dynamic table, int pageIndex, int pageSize, out int totalRecord)
        {
            return dal.ShowButtonAuthorityList(table, pageIndex, pageSize, out totalRecord);
        }
        #endregion

        #endregion

        #region 返回系统list
        public List<ComboboxEntity> GetSystemList() 
        {

            return dal.GetSystemList();
        }
        #endregion
    }
}
