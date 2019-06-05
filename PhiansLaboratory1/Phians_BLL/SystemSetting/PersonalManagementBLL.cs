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
    public class PersonalManagementBLL
    {
        IPersonalManagementDAL dal = DALFactory.GetPersonalManagement();

        #region 加载部门树
        public string GetDepartmentList(string ParentId)
        {
            if (string.IsNullOrEmpty(ParentId))
            {
                ParentId = "8cff8e9f-f539-41c9-80ce-06a97f481390";
            }
            DataTable new_table = ListToDataTable.ListToDataTable_(dal.GetDepartmentList());

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

        #region >>>>>>人员操作

        #region 加载人员信息
        public List<TB_UserInfo> GetDepPerList(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetDepPerList(PageModel, out totalRecord);
        }
        #endregion

        #region 添加人员
        public ReturnDALResult AddPersonnel(TB_UserInfo model, TB_groupAuthorization TB_groupAuthorization, TB_User_department departmentmodel)
        {
            return dal.AddPersonnel(model, TB_groupAuthorization, departmentmodel);
        }
        #endregion

        #region 修改人员信息
        public bool EditPersonnel(TB_UserInfo model, dynamic TempTable)
        {
            return dal.EditPersonnel(model, TempTable);
        }
        #endregion

        #region 人员停用/启用
        public bool DelPersonnel(Guid UserId, string UserName, Guid CreatePersonnel, int flag)
        {
            return dal.DelPersonnel(UserId, UserName, CreatePersonnel, flag);
        }
        #endregion

        #region 人员重置密码
        public bool ResetPerPwd(Guid UserId, string UserName, Guid CreatePersonnel, string User_pwd)
        {
            return dal.ResetPerPwd(UserId, UserName, CreatePersonnel, User_pwd);
        }
        #endregion

        #region 上传签名

        /// <summary>
        /// 上传签名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUserImg(dynamic model)
        {
            return dal.UpdateUserImg(model);
        }
        #endregion

        #region 上传头像

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UploadUserPortrait(dynamic model)
        {
            return dal.UploadUserPortrait(model);
        }
        #endregion


        #region  删除人员
        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="Model">用户model</param>
        /// <returns></returns>
        public ReturnDALResult DeletePerson(TB_UserInfo Model) {
            return dal.DeletePerson(Model);
        }
        #endregion

        #endregion

        #region >>>>>>组操作

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
            string json = newTree.GetTreeJsonByTable(new_table, "id", "GroupId", "GroupName", "GroupParentId", GroupParentId, "state_").ToString();
            return json;
        }
        #endregion

        #region 组信息加载——回显
        public TB_group LoadGroupInfo(Guid GroupId)
        {
            return dal.LoadGroupInfo(GroupId);
        }
        #endregion

        #region 添加组
        public TB_group AddGroup(TB_group model)
        {
            return dal.AddGroup(model);
        }
        #endregion

        #region 修改组信息
        public bool EditGroup(TB_group model, Guid CreatePersonnel)
        {
            return dal.EditGroup(model, CreatePersonnel);
        }
        #endregion

        #region 删除组
        public bool DelGroup(Guid GroupId, string GroupName, Guid CreatePersonnel)
        {
            return dal.DelGroup(GroupId, GroupName, CreatePersonnel);
        }
        #endregion

        #endregion

        #region >>>>>>人员-组操作

        #region 获取组里的人
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupId">组id</param>
        /// <param name="model">gird传参</param>
        /// <param name="totalRecord">返回总数</param>
        /// <returns>返回组list人员表</returns>
        public List<TB_UserInfo> GetPernonnelGroupBLL(Guid GroupId, TPageModel model, out int totalRecord)
        {

            return dal.GetPernonnelGroup(GroupId, model, out totalRecord);
        }
        #endregion

        #region 加载人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="PageModel">条件model</param>     
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_UserInfo> GetPersonnelListBll(TPageModel PageModel, out int totalRecord) {
            return dal.GetPersonnelList(PageModel, out totalRecord);
        }
        #endregion

        #region 显示人员授权组
        public List<TB_group> GetPerGroupTree(Guid UserId, int pageIndex, int pageSize, out int totalRecord)
        {
            return dal.GetPerGroupTree(UserId, pageIndex, pageSize, out totalRecord);
        }
        #endregion

        #region 添加人员到组
        public string AddPerToGroup(TB_groupAuthorization model, dynamic TempTable)
        {
            return dal.AddPerToGroup(model, TempTable);
        }
        #endregion

        #region 从组删除人员
        public bool DelPerToGroup(int id, string UserName, string GroupName, Guid CreatePersonnel)
        {
            return dal.DelPerToGroup(id, UserName, GroupName, CreatePersonnel);
        }
        #endregion

        #endregion

        #region 证书操作

        #region 添加证书
        /// <summary>
        /// 添加证书
        /// </summary>
        /// <param name="Certificate_model">添加证书实体</param>
        /// <param name="UserId">操作人</param>
        /// <param name="errortype">错误类型</param>
        /// <returns></returns>
        public bool AddCertificateData(TB_CertificateManagement Certificate_model, Guid UserId, out int errortype)
        {
            return dal.AddCertificateData(Certificate_model, UserId, out errortype);
        }
        #endregion

        #region 获取证书类别
        /// <summary>
        /// 获取证书类别
        /// </summary>
        /// <returns></returns>
        public List<ComboboxEntity> GetDictionaryList()
        {
            return dal.GetDictionaryList();
        }
        #endregion

        #region 获取证书列表
        /// <summary>
        /// 获取证书列表
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public List<TB_CertificateManagement> GetUserCertificate(int pageIndex, int pageSize, Guid UserId, out int totalRecord)
        {
            return dal.GetUserCertificate(pageIndex, pageSize, UserId, out totalRecord);
        }
        #endregion

        #region 获取证书附件列表
        /// <summary>
        /// 获取证书附件列表
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_CertificateAppendix> GetCertificateAppendixList(Phians_Entity.Common.TPageModel PageModel, out int totalRecord)
        {
            return dal.GetCertificateAppendixList(PageModel, out totalRecord);
        }
        #endregion

        #region 添加证书附件
        /// <summary>
        /// 添加证书附件
        /// </summary>
        /// <param name="model">添加实体</param>
        /// <param name="errortype"></param>
        /// <returns></returns>
        public bool AddCertificateFile(TB_CertificateAppendix model, out int errortype)
        {
            return dal.AddCertificateFile(model, out  errortype);
        }
        #endregion

        #region 删除证书
        public bool DelCertificateAppendix(TB_CertificateManagement model, Guid UserId)
        {
            return dal.DelCertificateAppendix(model,UserId);
        }
        #endregion

        #region 删除证书附件
        public bool DelFileManagement(TB_CertificateAppendix model)
        {
            return dal.DelFileManagement(model);
        }
        #endregion
        
        #endregion

        #region 加载停用人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_UserInfo> GetCancelPerson(TPageModel PageModel, out int totalRecord) {

            return dal.GetCancelPerson(PageModel, out  totalRecord);
        }
        #endregion
    }
}
