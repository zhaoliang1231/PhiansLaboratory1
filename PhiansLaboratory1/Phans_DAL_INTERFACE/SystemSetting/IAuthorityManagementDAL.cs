using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IAuthorityManagementDAL
    {
       /// <summary>
        ///  加载页面树
       /// </summary>
       /// <param name="ParentId"></param>
       /// <returns></returns>
        List<TB_FunctionalModule> LoadPageTree(Guid ParentId);
        //加载页面表格树
        ///List<TB_FunctionalModule> LoadPageTbleTree(string ParentId, int pageIndex, int pageSize, out int totalRecord);
       
        /// <summary>
        /// 加载组树/
        /// </summary>
        /// <param name="GroupParentId"></param>
        /// <returns></returns>
        List<TB_group> GetGroupTree(string GroupParentId);


      /// <summary>
      ///  加载组人员信息
      /// </summary>
      /// <param name="GroupId"></param>
      /// <param name="pageIndex"></param>
      /// <param name="pageSize"></param>
      /// <param name="totalRecord"></param>
      /// <param name="search"></param>
      /// <param name="key"></param>
      /// <returns></returns>
        List<TB_UserInfo> GetGroupPerList(Guid GroupId, int pageIndex, int pageSize, out int totalRecord, string search, string key);

        /// <summary>
        /// //组/人员 授权
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="ModuleNames"></param>
        /// <param name="GroupName"></param>
        /// <param name="CreatePersonnel"></param>
        /// <param name="flag2"></param>
        /// <param name="flag3"></param>
        /// <param name="ButtonNames"></param>
        /// <returns></returns>
        bool GroupAuthority(List<TB_PageAuthorization> mode, string ModuleNames, string GroupName, Guid CreatePersonnel, bool flag2, bool flag3, string ButtonNames);

        /// <summary>
        /// 组/人员 授权回显
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        List<TB_FunctionalModule> ShowAuthorizedPage(dynamic table);

        #region <<<<<<<<<<<<<<详细权限操作

       /// <summary>
        ///  加载按钮权限表格
       /// </summary>
       /// <param name="table"></param>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="totalRecord"></param>
       /// <param name="search"></param>
       /// <param name="key"></param>
       /// <returns></returns>
        List<TB_FunctionalModule> GetButtonAuthorityList(TPageModel PageModel, out int totalRecord);

      /// <summary>
      ///  回显按钮权限表格
      /// </summary>
      /// <param name="table"></param>
      /// <param name="pageIndex"></param>
      /// <param name="pageSize"></param>
      /// <param name="totalRecord"></param>
      /// <returns></returns>
        List<TB_FunctionalModule> ShowButtonAuthorityList(dynamic table, int pageIndex, int pageSize, out int totalRecord);

        #endregion


        /// <summary>
        /// 返回系统列表
        /// </summary>
        /// <returns></returns>
        List<ComboboxEntity> GetSystemList();
    }
}
