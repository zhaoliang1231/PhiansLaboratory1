using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IPersonalManagementDAL
    {
        //加载部门树
        List<TB_Organization> GetDepartmentList();

        #region 人员操作
        //加载人员信息
        List<TB_UserInfo> GetDepPerList(TPageModel PageModel, out int totalRecord);
        //添加人员
        ReturnDALResult AddPersonnel(TB_UserInfo model, TB_groupAuthorization TB_groupAuthorization, TB_User_department departmentmodel);
        //修改人员信息
        bool EditPersonnel(TB_UserInfo model, dynamic TempTable);
        //人员停用/启用
        bool DelPersonnel(Guid UserId, string UserName, Guid CreatePersonnel, int flag);
        //人员重置密码
        bool ResetPerPwd(Guid UserId, string UserName, Guid CreatePersonnel, string User_pwd);
        ///更改签名 
        bool UpdateUserImg(dynamic model);

        ///上传头像 
        bool UploadUserPortrait(dynamic model);

        #endregion

        #region 组操作
        //加载组树
        List<TB_group> GetGroupTree(string GroupParentId);
        //组信息加载——回显
        TB_group LoadGroupInfo(Guid GroupId);
        //添加组
        TB_group AddGroup(TB_group model);
        //修改组信息
        bool EditGroup(TB_group model, Guid CreatePersonnel);
        //删除组
        bool DelGroup(Guid GroupId, string GroupName, Guid CreatePersonnel);
        #endregion

        #region 人员-组操作


        #region 加载人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="PageModel">条件model</param>     
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        List<TB_UserInfo> GetPersonnelList(TPageModel PageModel, out int totalRecord);
        #endregion
        //显示人员授权组
        List<TB_group> GetPerGroupTree(Guid UserId, int pageIndex, int pageSize, out int totalRecord);
        //添加人员到组
        string AddPerToGroup(TB_groupAuthorization model, dynamic TempTable);
        //从组删除人员
        bool DelPerToGroup(int id, string UserName, string GroupName, Guid CreatePersonnel);
        #endregion

        #region 证书操作
        bool AddCertificateData(TB_CertificateManagement Certificate_model, Guid UserId, out int errortype);

        List<ComboboxEntity> GetDictionaryList();
        #endregion

        List<TB_CertificateManagement> GetUserCertificate(int pageIndex, int pageSize, Guid UserId, out int totalRecord);

        List<TB_CertificateAppendix> GetCertificateAppendixList(Phians_Entity.Common.TPageModel PageModel, out int totalRecord);

        bool AddCertificateFile(TB_CertificateAppendix model, out int errortype);

        bool DelFileManagement(TB_CertificateAppendix model);

        bool DelCertificateAppendix(TB_CertificateManagement model, Guid UserId);

        #region 显示组授权人员lsit

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupId">组id</param>
        /// <param name="model">model传参</param>
        /// <param name="totalRecord">返回总数</param>
        /// <returns></returns>
        List<TB_UserInfo> GetPernonnelGroup(Guid GroupId, TPageModel model, out int totalRecord);
        #endregion

        #region 加载停用人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        List<TB_UserInfo> GetCancelPerson(TPageModel PageModel, out int totalRecord);
        #endregion

        #region  删除人员
        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="Model">用户model</param>
        /// <returns></returns>
        ReturnDALResult DeletePerson(TB_UserInfo Model);
        #endregion
    }
}
