using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    public class GetOperationType
    {
        /// <summary>
        /// 获取系统日志操作类型
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static string GetOperationTypeEnum(int enums)
        {

            string OperationType = "";

            switch (enums)
            {
                //人员操作
                case (int)OperationTypeEnum.PersonnelAdd: OperationType = "添加人员%人员名称："; break;
                case (int)OperationTypeEnum.PersonnelEdit: OperationType = "修改人员%修改人员："; break;
                case (int)OperationTypeEnum.PersonnelDisable: OperationType = "人员停用%停用人员名称："; break;
                case (int)OperationTypeEnum.PersonnelEnable: OperationType = "人员启用%启用人员名称："; break;
                case (int)OperationTypeEnum.ResetPerPwd: OperationType = "人员重置密码%人员重置密码："; break;
                case (int)OperationTypeEnum.PersonnelAddImg: OperationType = "人员上传签名%被上传签名人员："; break;
                case (int)OperationTypeEnum.UploadUserPortrait: OperationType = "人员上传头像%被上传头像人员："; break;
                case (int)OperationTypeEnum.PersonnelEditInfo: OperationType = "人员修改资料%人员修改资料："; break;
                //组操作
                case (int)OperationTypeEnum.GroupAdd: OperationType = "添加组%添加组"; break;
                case (int)OperationTypeEnum.GroupEdit: OperationType = "修改组信息%修改组信息"; break;
                case (int)OperationTypeEnum.GroupDel: OperationType = "删除组%删除组"; break;
                case (int)OperationTypeEnum.AddPerToGroup: OperationType = "添加人员到组%添加人员：%到组："; break;
                case (int)OperationTypeEnum.DelPerToGroup: OperationType = "从组删除人员"; break;
                //部门操作
                case (int)OperationTypeEnum.AddDepartment: OperationType = "添加部门%添加部门"; break;
                case (int)OperationTypeEnum.EditDepartment: OperationType = "修改部门信息%修改部门信息"; break;
                case (int)OperationTypeEnum.DelDepartment: OperationType = "删除部门%删除部门"; break;
                //授权操作
                case (int)OperationTypeEnum.GroupAuthorityPage: OperationType = "组授权页面%给组：%，授权页面："; break;
                case (int)OperationTypeEnum.GroupAuthorityButton: OperationType = "组授权按钮%给组：%页面：%授权操作："; break;
                case (int)OperationTypeEnum.PersonAuthorityPage: OperationType = "人员授权按钮%给人：%页面：%授权操作："; break;
                case (int)OperationTypeEnum.PersonAuthorityButton: OperationType = "人员授权按钮%给人：%页面：%授权操作："; break;
               //证书操作
                case (int)OperationTypeEnum.Add_Certificate: OperationType = "添加证书%添加证书"; break;
                case (int)OperationTypeEnum.Del_Certificate: OperationType = "删除证书%删除证书"; break;
                case (int)OperationTypeEnum.Add_CertificateType: OperationType = "新增证书类别%新增证书类别"; break;

                //环境文件
                case (int)OperationTypeEnum.AddFileManagement: OperationType = "添加文件%添加文件"; break;
                case (int)OperationTypeEnum.UpdateFileManagement: OperationType = "添加文件%添加文件"; break;
                case (int)OperationTypeEnum.DelFileManagement: OperationType = "删除文件%删除文件"; break;
                case (int)OperationTypeEnum.AuditFileManagement: OperationType = "审核文件%审核文件"; break;
                case (int)OperationTypeEnum.IssuedFileManagement: OperationType = "签发文件%签发文件"; break;

                //字典管理
                case (int)OperationTypeEnum.AddDictionary: OperationType = "添加字典%添加字典"; break;
                case (int)OperationTypeEnum.EditDictionary: OperationType = "修改字典%修改字典"; break;
                case (int)OperationTypeEnum.DelDictionary: OperationType = "删除字典%删除字典"; break;

                //标准物质管理
                case (int)OperationTypeEnum.AddStandardSubstance: OperationType = "添加标准物质%添加标准物质："; break;
                case (int)OperationTypeEnum.EditStandardSubstance: OperationType = "修改标准物质%修改标准物质："; break;
                case (int)OperationTypeEnum.DelStandardSubstance: OperationType = "删除标准物质%删除标准物质："; break;

                //设备管理
                case (int)OperationTypeEnum.AddEquipment: OperationType = "添加设备%添加设备："; break;
                case (int)OperationTypeEnum.EditEquipment: OperationType = "修改设备%修改设备："; break;
                case (int)OperationTypeEnum.DelEquipment: OperationType = "删除设备%删除设备："; break;
                    
                //报告操作
                case (int)OperationTypeEnum.InsertSubReport: OperationType = "载入报告%报告编号：%任务id："; break;
                case (int)OperationTypeEnum.DelReport: OperationType = "删除载入报告%任务id："; break;
                case (int)OperationTypeEnum.AddTotalReport: OperationType = "提交报告审核（生成总报告信息）%报告编号："; break;
                case (int)OperationTypeEnum.ConfirmCompletionSubReport: OperationType = "确认完成子报告%任务id："; break;

                case (int)OperationTypeEnum.SubmitReportReturn: OperationType = "退回审核报告%报告编号："; break;
                case (int)OperationTypeEnum.SubmitReportIssued: OperationType = "提交报告签发%报告编号："; break;
                case (int)OperationTypeEnum.SubmitReportFinish: OperationType = "提交报告完成%报告编号："; break;
                case (int)OperationTypeEnum.ReturnIssueReport: OperationType = "退回签发报告%报告编号："; break;
                case (int)OperationTypeEnum.InsertTotalReportVersion: OperationType = "添加报告版本%报告编号："; break;

                case (int)OperationTypeEnum.PassUnusualApply: OperationType = "报告异常申请通过%MTR单号：%异常申请id："; break;
                case (int)OperationTypeEnum.ReturnUnusualApply: OperationType = "报告异常申请退回%MTR单号：%异常申请id："; break;
                case (int)OperationTypeEnum.SubmitAbnormalReportReview: OperationType = "异常报告提交审核%MTR单号：%异常申请id："; break;
                case (int)OperationTypeEnum.PassAbnormalReportReview: OperationType = "异常报告审核通过%MTR单号：%异常申请id："; break;
                case (int)OperationTypeEnum.ReturnAbnormalReportReview: OperationType = "异常报告审核退回%MTR单号：%异常申请id："; break;
                case (int)OperationTypeEnum.AddReportAppendix: OperationType = "添加报告附件%："; break;
                case (int)OperationTypeEnum.DelReportAppendix: OperationType = "删除报告附件%："; break;

                //==========无损项目枚举begin
                #region 无损项目枚举

                #region 模板操作
                case (int)OperationTypeEnum.AddTemplate: OperationType = "添加模板%模板编号："; break;
                case (int)OperationTypeEnum.EditTemplate: OperationType = "修改模板%模板编号："; break;
                case (int)OperationTypeEnum.DelTemplate: OperationType = "删除模板%模板编号："; break;
                case (int)OperationTypeEnum.SaveOperationLog: OperationType = "在线修改模板word%模板编号："; break;

                case (int)OperationTypeEnum.AddQualificationPerson: OperationType = "添加模板资质权限%模板ID：%人员ID："; break;
                case (int)OperationTypeEnum.DelQualificationPerson: OperationType = "删除模板资质权限%模板ID：%人员ID："; break;

                    

                #endregion

                case (int)OperationTypeEnum.SaveReportAccessory: OperationType = "添加附件%报告编号："; break;
                case (int)OperationTypeEnum.DelAccessory: OperationType = "删除附件%报告编号：%;附件id："; break;
                case (int)OperationTypeEnum.PageShowCustom: OperationType = "修改报告字段显示%字段名或id："; break;

                #region 无损报告操作
                case (int)OperationTypeEnum.AddReportBaseInfo: OperationType = "添加报告%报告编号："; break;
                case (int)OperationTypeEnum.EditReportBaseInfo: OperationType = "修改报告信息%报告编号："; break;
                case (int)OperationTypeEnum.AddTextData: OperationType = "添加检测数据%报告编号："; break;
                case (int)OperationTypeEnum.DataDel: OperationType = "删除报告%报告编号："; break;
                case (int)OperationTypeEnum.SubmitEditReport: OperationType = "提交报告审核%报告编号："; break;
                case (int)OperationTypeEnum.TakeBackEditReport: OperationType = "编制人员拿回未开始审核报告%报告编号："; break;

                case (int)OperationTypeEnum.BackReviewReport: OperationType = "报告审核退回%报告编号："; break;
                case (int)OperationTypeEnum.SubmitReviewReport: OperationType = "审核报告提交%报告编号："; break;
                case (int)OperationTypeEnum.AddErrorInfo: OperationType = "添加退回原因%报告编号："; break;
                case (int)OperationTypeEnum.TakeBackReviewReport: OperationType = "审核人员拿回未开始签发报告%报告编号："; break;

                case (int)OperationTypeEnum.BackIssueReport: OperationType = "报告签发退回%报告编号："; break;
                case (int)OperationTypeEnum.SubmitIssueReport: OperationType = "提交签发报告%报告编号："; break;

                case (int)OperationTypeEnum.SubmitAbnormalReport: OperationType = "提交到异常报告申请%报告编号："; break;
                case (int)OperationTypeEnum.ErrorReportAgree: OperationType = "异常报告申请通过%报告编号："; break;
                case (int)OperationTypeEnum.ErrorReportNoAgree: OperationType = "异常报告申请拒绝%报告编号："; break;
                case (int)OperationTypeEnum.ScrapReportAgree: OperationType = "异常报告报废申请通过%报告编号："; break;
                case (int)OperationTypeEnum.ScrapReportNoAgree: OperationType = "异常报告报废申请拒绝%报告编号："; break;

                case (int)OperationTypeEnum.ErrorReportSubmitAudit: OperationType = "异常报告编制提交到审核%报告编号："; break;
                case (int)OperationTypeEnum.ErrorReportSubmitIssue: OperationType = "异常报告审核提交到签发%报告编号："; break;
                case (int)OperationTypeEnum.ErrorReportEditSendBackEdit: OperationType = "异常报告审核退回到编制%报告编号："; break;
                case (int)OperationTypeEnum.ErrorReportIssueSubmitAudit: OperationType = "异常报告签发退回到编制%报告编号："; break;
                case (int)OperationTypeEnum.ErrorReportSubmitFinish: OperationType = "异常报告完成%报告编号："; break;
                case (int)OperationTypeEnum.AddUnderLineReportInfo: OperationType = "线下报告上传%报告编号："; break;

                #endregion
               
                #region 探头操作
                case (int)OperationTypeEnum.Probe_add: OperationType = "添加探头%探头名称："; break;
                case (int)OperationTypeEnum.Probe_edit: OperationType = "修改探头%探头编号："; break;
                case (int)OperationTypeEnum.Probe_delete: OperationType = "删除探头%探头名称："; break;
                #endregion

                #region 试块操作
                case (int)OperationTypeEnum.TestBlockLibrary_add: OperationType = "添加试块%试块名称："; break;
                case (int)OperationTypeEnum.TestBlockLibrary_edit: OperationType = "修改试块%试块编号："; break;
                case (int)OperationTypeEnum.TestBlockLibrary_delete: OperationType = "删除试块%试块名称："; break;
                #endregion

                case (int)OperationTypeEnum.EditInfoPage: OperationType = "修改页面展示字段%字段名称："; break;
                    
                #endregion

                //==========无损项目枚举end

              
                default: OperationType = ""; break;
            }

            return OperationType;

        }

    }

    public enum OperationTypeEnum
    {

        //==========无损项目枚举begin
        #region 无损项目枚举

        /// <summary>
        /// 修改报告字段显示
        /// </summary>
        PageShowCustom = 324,

        #region 报告附件操作

        /// <summary>
        /// 添加附件
        /// </summary>
        SaveReportAccessory = 328,

        /// <summary>
        /// 删除附件
        /// </summary>
        DelAccessory = 329,

        #endregion

        #region 无损报告操作
        /// <summary>
        /// 写入报告信息
        /// </summary>
        AddReportBaseInfo = 300,

        /// <summary>
        /// 修改报告信息
        /// </summary>
        EditReportBaseInfo = 301,
        
        /// <summary>
        /// 添加检测数据
        /// </summary>
        AddTextData = 302,

        /// <summary>
        /// 删除报告
        /// </summary>
        DataDel = 303,

        /// <summary>
        /// 提交报告审核
        /// </summary>
        SubmitEditReport = 308,

        /// <summary>
        /// 编制人员自己退回未开始审核报告
        /// </summary>
        TakeBackEditReport = 335,

        /// <summary>
        /// 报告审核退回
        /// </summary>
        BackReviewReport = 313,

        /// <summary>
        /// 报告审核提交
        /// </summary>
        SubmitReviewReport = 314,

        /// <summary>
        /// 编制人员自己退回未开始审核报告
        /// </summary>
        TakeBackReviewReport = 336,

        /// <summary>
        /// 添加退回原因 
        /// </summary>
        AddErrorInfo = 315,

        /// <summary>
        /// 报告签发退回
        /// </summary>
        BackIssueReport = 321,

        /// <summary>
        /// 提交签发报告
        /// </summary>
        SubmitIssueReport = 322,

        /// <summary>
        /// 线下报告上传
        /// </summary>
        AddUnderLineReportInfo = 334,

        #region 异常报告操作
        /// <summary>
        ///  从报告管理提交到异常报告申请
        /// </summary>
        SubmitAbnormalReport = 323,

        /// <summary>
        ///  异常申请审核——通过
        /// </summary>
        ErrorReportAgree = 309,

        /// <summary>
        ///  异常申请审核——退回
        /// </summary>
        ErrorReportNoAgree = 310,

         /// <summary>
        ///  异常报废审核——通过
        /// </summary>
        ScrapReportAgree = 311,

        /// <summary>
        ///  异常报废审核——退回
        /// </summary>
        ScrapReportNoAgree = 312,

        /// <summary>
        ///  异常报告编制--审核
        /// </summary>
        ErrorReportSubmitAudit = 316,

        /// <summary>
        ///  异常报告审核--签发
        /// </summary>
        ErrorReportSubmitIssue = 317,

        ///// <summary>
        /////  异常报告审核--编制
        ///// </summary>
        ErrorReportEditSendBackEdit = 318,

        /// <summary>
        ///  异常报告签发--编制
        /// </summary>
        ErrorReportIssueSubmitAudit = 319,

        /// <summary>
        ///  异常报告签发--完成
        /// </summary>
        ErrorReportSubmitFinish = 320,
        #endregion

        #region 异常报告流程

        /// <summary>
        ///  异常申请审核——通过
        /// </summary>
        PassUnusualApply = 133,

        /// <summary>
        ///  异常申请审核——退回
        /// </summary>
        ReturnUnusualApply = 134,

        /// <summary>
        ///  异常报告提交审核(从编制到审核)
        /// </summary>
        SubmitAbnormalReportReview = 135,

        /// <summary>
        ///  异常报告审核通过   （从异常报告审核——异常报告完成）
        /// </summary>
        PassAbnormalReportReview = 136,

        /// <summary>
        ///  异常报告审核退回   （从异常报告审核——异常报告编制）
        /// </summary>
        ReturnAbnormalReportReview = 137,

        #endregion


        #endregion

        #region 探头操作

        /// <summary>
        /// 添加探头
        /// </summary>
        Probe_add=304,

        /// <summary>
        /// 修改探头
        /// </summary>
        Probe_edit = 305,

        /// <summary>
        /// 删除探头
        /// </summary>
        Probe_delete = 306,
        #endregion

        #region 试块库操作

        /// <summary>
        /// 添加试块
        /// </summary>
        TestBlockLibrary_add = 331,

        /// <summary>
        /// 修改试块
        /// </summary>
        TestBlockLibrary_edit = 332,

        /// <summary>
        /// 删除试块
        /// </summary>
        TestBlockLibrary_delete = 333,
        #endregion

        #region 页面设置操作
        /// <summary>
        /// 修改页面显示字段
        /// </summary>
        EditInfoPage = 307,
        #endregion

        #region 设备管理操作
        /// <summary>
        ///  添加设备
        /// </summary>
        AddEquipment = 26,

        /// <summary>
        ///  修改设备
        /// </summary>
        EditEquipment = 27,

        /// <summary>
        ///  删除设备
        /// </summary>
        DelEquipment = 28,

        #endregion

        #region 模板管理
        /// <summary>
        ///  添加模板
        /// </summary>
        AddTemplate = 325,

        /// <summary>
        ///  修改模板信息
        /// </summary>
        EditTemplate = 326,

        /// <summary>
        /// 在线修改模板文件
        /// </summary>
        SaveOperationLog = 330,

        /// <summary>
        ///  删除模板
        /// </summary>
        DelTemplate = 327,

        #endregion

        #region 资质管理

        /// <summary>
        ///  添加模板资质权限
        /// </summary>
        AddQualificationPerson = 337,

        /// <summary>
        ///  删除模板资质权限
        /// </summary>
        DelQualificationPerson = 338,
        #endregion

        #endregion

        //==========无损项目枚举end

        #region 人员操作
        /// <summary>
        /// 添加人员
        /// </summary>
        PersonnelAdd = 1,

        /// <summary>
        /// 修改人员
        /// </summary>
        PersonnelEdit = 2,

        /// <summary>
        /// 停用人员
        /// </summary>
        PersonnelDisable = 3,

        /// <summary>
        /// 启用人员
        /// </summary>
        PersonnelEnable = 4,

        /// <summary>
        /// 上传签名
        /// </summary>
        PersonnelAddImg = 14,

        /// <summary>
        /// 上传头像
        /// </summary>
        UploadUserPortrait = 98,

        /// <summary>
        /// 人员修改资料
        /// </summary>
        PersonnelEditInfo = 15,


        /// <summary>
        /// 人员重置密码
        /// </summary>
        ResetPerPwd=13,
        #endregion

        #region 组操作
        /// <summary>
        /// 添加组
        /// </summary>
        GroupAdd = 5,

        /// <summary>
        /// 修改组信息
        /// </summary>
        GroupEdit = 6,

        /// <summary>
        /// 删除组
        /// </summary>
        GroupDel = 7,

        /// <summary>
        /// 添加人员到组
        /// </summary>
        AddPerToGroup = 8,

        /// <summary>
        /// 从组删除人员
        /// </summary>
        DelPerToGroup = 9,
         #endregion

        #region 部门操作
        /// <summary>
        /// 添加部门
        /// </summary>
        AddDepartment = 10,

        /// <summary>
        /// 修改部门信息
        /// </summary>
        EditDepartment = 11,

        /// <summary>
        /// 删除部门
        /// </summary>
        DelDepartment = 12,



         #endregion

        #region 授权操作

        /// <summary>
        ///  组 授权页面
        /// </summary>
        GroupAuthorityPage = 16,

        /// <summary>
        ///  组 授权按钮
        /// </summary>
        GroupAuthorityButton = 17,

        /// <summary>
        ///  人员 授权页面
        /// </summary>
        PersonAuthorityPage = 30,

        /// <summary>
        ///  人员 授权按钮
        /// </summary>
        PersonAuthorityButton = 31,

        #endregion

        #region 文件操作

        /// <summary>
        ///  添加文件
        /// </summary>
        AddFileManagement = 18,

        /// <summary>
        ///  修改文件
        /// </summary>
        UpdateFileManagement = 19,

        /// <summary>
        ///  删除文件
        /// </summary>
        DelFileManagement = 20,

        /// <summary>
        ///  审核文件
        /// </summary>
        AuditFileManagement = 21,

        /// <summary>
        ///  签发文件
        /// </summary>
        IssuedFileManagement = 22,

        #endregion

        #region 字典操作

        /// <summary>
        ///  添加字典
        /// </summary>
        AddDictionary = 36,

        /// <summary>
        ///  修改字典
        /// </summary>
        EditDictionary = 37,

        /// <summary>
        ///  删除字典
        /// </summary>
        DelDictionary = 38,
        #endregion

        #region 标准物质操作
        /// <summary>
        ///  添加标准物质
        /// </summary>
        AddStandardSubstance = 61,

        /// <summary>
        ///  锡膏标准物质
        /// </summary>
        EditStandardSubstance = 68,

        /// <summary>
        ///  删除标准物质
        /// </summary>
        DelStandardSubstance = 69,
        #endregion

        #region 报告操作

        #region 报告编制
        /// <summary>
        ///  报告载入
        /// </summary>
        InsertSubReport = 120,

        /// <summary>
        ///  删除载入报告
        /// </summary>
        DelReport = 122,

        /// <summary>
        ///  添加总报告信息表记录
        /// </summary>
        AddTotalReport = 123,

        /// <summary>
        ///  确认完成子报告
        /// </summary>
        ConfirmCompletionSubReport = 200,


        /// <summary>
        ///  添加报告附件
        /// </summary>
        AddReportAppendix = 210,

        /// <summary>
        ///  删除报告附件
        /// </summary>
        DelReportAppendix = 211,
        #endregion

        #region 报告审核/签发

        /// <summary>
        ///  审核退回报告
        /// </summary>
        SubmitReportReturn = 125,

        /// <summary>
        ///  提交报告到批准
        /// </summary>
        SubmitReportIssued = 126,

        /// <summary>
        ///  提交到报告完成
        /// </summary>
        SubmitReportFinish = 127,

        /// <summary>
        ///  签发报告退回
        /// </summary>
        ReturnIssueReport = 128,

        /// <summary>
        ///  添加版本控制表信息
        /// </summary>
        InsertTotalReportVersion = 130,

        #endregion

        #endregion

        #region 证书操作
        /// <summary>
        ///  新增证书
        /// </summary>
        Add_CertificateType = 154,

        /// <summary>
        ///  添加证书
        /// </summary>
        Add_Certificate = 152,

        /// <summary>
        ///  删除证书
        /// </summary>
        Del_Certificate = 153,
        #endregion

    }



}
