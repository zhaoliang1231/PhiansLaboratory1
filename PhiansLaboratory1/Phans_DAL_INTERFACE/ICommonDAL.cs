using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity;
using Phians_Entity.LosslessReport;

namespace Phans_DAL_INTERFACE
{
    public interface ICommonDAL
    {
        #region 权限系列

        //判断模块是否存在
        int GetFunctionalModule(string U_url);
        //检查人员权限
        int GetAuthorization(Guid UserId, string U_url);
        #endregion

        #region 消息系列

        #region 注册消息
        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="model">注册消息实体</param>
        /// <returns></returns>

        bool RegisterSignalR(TB_MessageRegister model);
        #endregion

        #region 获取注册消息
        /// <summary>
        /// 获取注册消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        TB_MessageRegister getRegisterSignalRInfo(Guid UserId);
        #endregion

        #endregion

        #region 组、人员组合选择系列

        #region 室下拉框
        /// <summary>
        /// 室下拉框
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        List<ComboboxEntity> LoadRoomCombobox(dynamic TempTable);
        #endregion

        #region 组下拉框
        /// <summary>
        /// 组下拉框
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        List<LosslessComboboxEntity> LoadGroupCombobox(dynamic TempTable);
        #endregion

        #region 根据组选择人员
        /// <summary>
        /// 根据组选择人员
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        List<LosslessUserComboboxEntity> LoadPersonnelCombobox(int id);
        #endregion


        #region 加载提交CNAS人员下拉框
        /// <summary>
        /// 加载提交CNAS人员下拉框
        /// </summary>
        /// <param name="TempTable">传递信息</param>
        /// <returns></returns>
        List<ComboboxEntity> LoadCNASPersonnel(dynamic TempTable);
        #endregion

        #endregion

        #region 逾期时间下拉框
        /// <summary>
        /// 逾期时间下拉框
        /// </summary>
        /// <param name="Parent_id">逾期时间下拉框 字典父id</param>
        /// <returns></returns>
        List<ComboboxEntityString> LoadDaySetting(Guid Parent_id);

        #endregion

        #region 报告类型下拉框——报告模板
        /// <summary>
        /// 报告类型下拉框——报告模板
        /// </summary>
        /// <param name="Parent_id">报告类型下拉框 字典父id</param>
        /// <returns></returns>
        List<ComboboxEntity> LoadReportType(Guid Parent_id);
        #endregion

        #region 通用字典获取
        /// <summary>
        /// 通用字典获取  Project_value  Project_name
        /// </summary>
        /// <param name="Parent_id">字典父id</param>
        /// <returns></returns>
        List<ComboboxEntityString> GetDictionaryList(Guid Parent_id);
        #endregion



        #region 通用字典获取  id  Project_name
        /// <summary>
        ///  通用字典获取  id  Project_name
        /// </summary>
        /// <param name="Parent_id">字典父id</param>
        /// <returns></returns>
        List<ComboboxEntityString> GetDictionaryListId(Guid Parent_id);
        #endregion


        #region 通用字典获取  id　获取的字典内容
        /// <summary>
        /// 通用字典获取  id　获取的字典内容
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        TB_DictionaryManagement GetDicitionaryContent(Guid ID);
        #endregion
    }
}
