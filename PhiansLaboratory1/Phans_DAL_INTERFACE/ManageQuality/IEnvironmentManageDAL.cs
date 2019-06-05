using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE.ManageQuality
{
    public interface IEnvironmentManageDAL
    {
        #region 加载树
        List<TB_DictionaryManagement> LoadPageTree(Guid id);
        #endregion

        #region 加载文件类型
        List<TB_FileManagement> GetFileTypeList();
        #endregion

        #region 获取文件列表
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <param name="file_type">获取文件类型</param>
        /// <returns></returns>
        List<TB_FileManagement> GetFileManagement(int pageIndex, int pageSize, out int totalRecord,string file_type, string search,string key,string sortName,string sortOrder);
        #endregion

        #region 添加树节点
        bool AddFileManagement(TB_FileManagement model, out int errortype);
        #endregion

        #region 修改文件
        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="model">文件对象</param>
        /// <param name="loginer">操作人</param>
        /// <param name="FileUrl">文件路径</param>
        /// <param name="errortype">错误类型</param>
        /// <returns></returns>
        bool UpdateFileManagement(TB_FileManagement model, Guid loginer,string FileUrl, out int errortype);
        #endregion

        #region 删除文件
        bool DelFileManagement(TB_FileManagement model,Guid loginer);
        #endregion

        #region 审核文件
        bool AuditFileManagement(TB_FileManagement model, out int state, string FileUrl);
        #endregion

        #region 签发文件
        bool IssuedFileManagement(TB_FileManagement model, out int state, string FileUrl);
        #endregion

        #region 导出列表
        List<TB_FileManagement> export(string search, string key, int type, string ids);
        #endregion
        List<TB_FileManagement> GetFile(int id, string pageName);

        #region 获取文件信息
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="pageName"></param>
        /// <returns></returns>
         TB_FileManagement GetFileModel(int id);
        #endregion


          #region 获取文件信息By FileID
        /// <summary>
        /// 获取文件信息By FileID
        /// </summary>
        /// <param name="FileID">文件FileID</param>
        
        /// <returns></returns>
         TB_FileManagement GetFileModelByFileID(Guid FileID);
         #endregion
    }
}
