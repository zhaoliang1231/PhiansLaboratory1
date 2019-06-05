using Phians_Entity;
using Phians_Entity.Common;
using Phians_Entity.LosslessReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phans_DAL_INTERFACE
{
    public interface IReportEditDAL
    {

        #region 获取报告编制列表
        /// <summary>
        /// 获取报告编制列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="history_flag">历史信息or待编制信息</param>
        /// <param name="Inspection_personnel">登录账号</param>
        /// <param name="ReportEditStatus">报告编制状态</param>
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_report_title> LoadReportEditList(int pageIndex, int pageSize, out int totalRecord, string search, string key, string history_flag, string Inspection_personnel, int ReportEditStatus);

        #endregion

        #region 页面字段显示List
        /// <summary>
        /// 页面字段显示List
        /// </summary>
        /// <param name="PageId">页面id</param>
        /// <returns></returns>
        List<TB_PageShowCustom> loadPageShowSetting(string PageId);

        #endregion

        #region 模板下拉框
        /// <summary>
        /// 模板下拉框
        /// </summary>
        /// <returns></returns>
        List<LosslessComboboxEntityss> LoadTemplateCombobox();

        #endregion

        #region 添加报告信息

        #region 拷贝报告
        /// <summary>
        /// 写入报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="flag">判断是否为复制的报告 为“1”则复制报告 </param>
        /// <returns>结果类</returns>
        string CopyReport(int id);

        #endregion

        #region 添加报告信息
        /// <summary>
        /// 添加报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="flag">判断是否为复制的报告 为“1”则复制报告 </param>
        /// <returns>结果类</returns>
        ReturnDALResult AddReportBaseInfo(TB_NDT_report_title model, int Finish, Guid LogPersonnel);

        #endregion

        #endregion

        #region ---复制报告

        #region 获取复制报告列表
        /// </summary>
        /// 获取复制报告列表
        /// </summary>
        /// <returns></returns>
        List<TB_NDT_report_title> loadReportCopy(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 获取复制报告列表
        /// </summary>
        /// 获取复制报告列表
        /// </summary>
        /// <returns></returns>
        List<TB_NDT_report_title> ReportCopyShow(TPageModel PageModel);

        #endregion

        #endregion

        #region 修改报告信息
        /// <summary>
        /// 修改报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="LogPersonnel">操作人 </param>
        /// <returns>结果类</returns>
        ReturnDALResult EditReportBaseInfo(TB_NDT_report_title model, Guid LogPersonnel);

        #endregion

        #region 添加检测数据
        /// <summary>
        /// 添加检测数据
        /// </summary>
        /// <param name="model">检测数据实体数据</param>
        /// <param name="report_num">报告编号</param>
        /// <param name="report_name">报告名称</param>
        /// <param name="date">添加日期</param>
        /// <param name="LogPersonnel">添加人</param>
        /// <param name="equipment_id">设备id</param>
        /// <param name="equipment_name">设备名称</param>
        /// <param name="equipment_name_R">label名称s</param>
        /// <returns></returns>
        ReturnDALResult AddTextData(TB_NDT_test_probereport_data model, string report_num, string report_name, DateTime date, Guid LogPersonnel, string equipment_id, string equipment_name, string equipment_name_R);

        #endregion

        #region 删除信息
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">删除的id</param>
        /// <param name="report_num">报告编号</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <returns></returns>
        ReturnDALResult DataDel(int id, string report_num, Guid LogPersonnel);

        #endregion

        #region 获取设备
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <returns></returns>
        List<ComboboxEntity_ss> GetEquipmentInfo();
        #endregion

        #region 获取报告设备
        /// <summary>
        /// 获取报告设备
        /// </summary>
        /// <returns></returns>
        List<TB_NDT_test_equipment> GetReportEquipmentInfo(int report_id);
        #endregion

        #region --探头库

        #region 获取探头库
        /// <summary>
        /// 获取探头库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="Probe_state">探头状态</param>
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_probe_library> GetProbeTest(int pageIndex, int pageSize, out int totalRecord, string search, string key, int Probe_state);
        #endregion

        #region 获取已经添加获取探头库
        /// <summary>
        /// 获取已经添加获取探头库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="ReportId">报告ID</param>
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_test_probe> GetRrportProbe(int pageIndex, int pageSize, out int totalRecord, string search, string key, int ReportId);
        #endregion

        #region 添加探头报报告
        /// <summary>
        /// 添加探头报报告
        /// </summary>
        /// <param name="ReportId">报告ID</param>
        /// <param name="probe_id">探头ID</param>
        /// <returns></returns>
        ReturnDALResult add_Probe_test(string ReportId, string probe_id);
        #endregion

        #region 删除已经添加探头
        /// <summary>
        /// 删除已经添加探头
        /// </summary>
        /// <param name="id">已经选择探头标识</param>     
        /// <returns></returns>
        ReturnDALResult Delete_Probe_test(string id);

        #endregion

        #endregion

        #region 试块库

        #region 获取试块库
        /// <summary>
        /// 获取试块库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="state">试块状态</param>
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_TestBlockLibrary> GetTestBlockLibrary(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 获取已经添加获取试块库
        /// <summary>
        /// 获取已经添加获取试块库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="ReportId">报告ID</param>
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_TestTestBlock> GetTestTestBlock(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 添加试块到报告
        /// <summary>
        /// 添加探头报报告
        /// </summary>
        /// <param name="ReportId">报告ID</param>
        /// <param name="CalBlockID">试块ID</param>
        /// <param name="ProbeID">探头ID</param>
        /// <returns></returns>
        ReturnDALResult Add_TestTestBlock(string CalBlockID, string ProbeID, string ReportID);
        #endregion

        #region 删除已经添加探头
        /// <summary>
        /// 删除已经添加探头
        /// </summary>
        /// <param name="id">已经选择探头标识</param>     
        /// <returns></returns>
        ReturnDALResult Delete_TestTestBlock(string id);
        #endregion
        #endregion

        #region 获取报告表头
        /// <summary>
        /// 获取报告表头
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        TB_NDT_report_title Getreport_title(int id);
        #endregion

        #region 获取探头数据
        /// <summary>
        /// 获取探头数据
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        TB_NDT_test_probereport_data Getreport_probe(int id);
        #endregion

        #region 获取探头
        /// <summary>
        /// 获取探头
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_test_probe> GetReportProbeLibrary(int id);
        #endregion

        #region 获取试验试块
        /// <summary>
        /// 获取试验试块
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        List<TB_NDT_TestTestBlock> GetReportTestTestBlock(int id);
        #endregion

        #region 保存报告Url
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">报告id</param>
        /// <param name="ReportUrl">报告地址</param>
        /// <returns></returns>
        ReturnDALResult SaveReportUrl(int id, string ReportUrl);
        #endregion

        #region 保存报告附件
        /// <summary>
        /// 保存报告附件
        /// </summary>
        /// <param name="TB_NDT_report_accessory">报告实体内容</param>     
        /// <returns></returns>
        ReturnDALResult SaveReportAccessory(string report_num,Guid LogPersonnel,TB_NDT_report_accessory model);
        #endregion

        #region 获取附件列表
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="report_id">报告id</param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_NDT_report_accessory> GetReportAccessoryList(TPageModel PageModel, int report_id, out int totalRecord);
        #endregion

        #region 删除报告附件
        /// <summary>
        /// 删除报告附件
        /// </summary>
        /// <param name="AccessoryID">附件ID</param>     
        /// <returns></returns>
        ReturnDALResult DelAccessory(int AccessoryID, string report_num, Guid LogPersonnel);
        #endregion


        #region --提交审核报告

        #region 获取用户信息，判断签名是否存在
        /// <summary>
        /// 获取用户信息，判断签名是否存在
        /// </summary>
        /// <param name="login_user">用户</param>     
        /// <returns></returns>
        TB_user_info getDetection(string login_user);
        #endregion

        #region 获取报告信息信息
        /// <summary>
        /// 获取报告信息信息
        /// </summary>
        /// <param name="id">报告id</param>     
        /// <returns></returns>
        TB_NDT_report_title getNDTReportInfo(int id);
        #endregion

        #region 报告审核
        /// <summary>
        /// 报告审核
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="login_user"></param>
        /// <returns></returns>
        ReturnDALResult SubmitEditReport(TB_NDT_report_title model, Guid LogPersonnel, string login_user, string new_report_url, TB_NDT_RevisionsRecord TB_NDT_RevisionsRecord, TB_ProcessRecord TB_ProcessRecord,int EditState);
        #endregion

        #endregion

        #region 查看退回原因
        /// <summary>
        /// 查看退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_NDT_error_log> LoadErrorInfo(TPageModel PageModel, out int totalRecord);

        #endregion

        #region 查看修改记录
        /// <summary>
        /// 查看修改记录
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        List<TB_NDT_RevisionsRecord> ReadRecord(TPageModel PageModel, out int totalRecord);
        #endregion

        #region 获取模板信息
        TB_TemplateFile LoadTemplateFile(int id);

        #endregion

        #region 将报告状态更改成已开始
        ReturnDALResult ReportCondition(TB_NDT_report_title model);

        #endregion

        #region 编制人员自己退回未开始审核报告
        /// <summary>
        /// 编制人员自己退回未开始审核报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        ReturnDALResult TakeBackEditReport(TB_NDT_report_title model, Guid LogPersonnel, TB_ProcessRecord TB_ProcessRecord);

        #endregion
    }
}
