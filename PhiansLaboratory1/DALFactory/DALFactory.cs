using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phans_DAL_INTERFACE;
using Phans_DAL_INTERFACE.ManageQuality;
using Phans_DAL_INTERFACE.LedgerManagement;

namespace Phans_DAL_DALFactory
{
    /// <summary>
    /// 工厂类
    /// </summary>
    public class DALFactory
    {
        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object GetInstance(string name)
        {
            //ILog log = LogManager.GetLogger(typeof(Factory));  //初始化日志记录器

            string configName = System.Configuration.ConfigurationManager.AppSettings["DataAccess"];
            if (string.IsNullOrEmpty(configName))
            {
                //log.Fatal("没有从配置文件中获取命名空间名称！");   //Fatal致命错误，优先级最高
                throw new InvalidOperationException();    //抛错，代码不会向下执行了
            }

            string className = string.Format("{0}.{1}", configName, name);  //Phians_DAL.传入的类名name

            //加载程序集
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(configName);
                //创建指定类型的对象实例
                return assembly.CreateInstance(className);
            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        ///用户入口
        /// </summary>
        /// <returns></returns>
        public static IUser_infoDAL GetUserDAL()
        {
            IUser_infoDAL dal = GetInstance("UserDAL") as IUser_infoDAL;
            return dal;
        }
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public static IMainformDAL GetMainform()
        {
            IMainformDAL dal = GetInstance("MainformDAL") as IMainformDAL;
            return dal;
        }


        #region 公用数据调用
        /// <summary>
        /// 公用数据调用
        /// </summary>
        /// <returns></returns>
        public static ICommonDAL GetCommonDAL()
        {
            ICommonDAL dal = GetInstance("CommonDAL") as ICommonDAL;
            return dal;
        }
        #endregion

        #region 管理页面
        /// <summary>
        /// 管理页面
        /// </summary>
        /// <returns></returns>
        public static IFunctionalModuleDAL GetFunctionalModule()
        {
            IFunctionalModuleDAL dal = GetInstance("FunctionalModuleDAL") as IFunctionalModuleDAL;
            return dal;
        }
        #endregion

        #region 日志管理
        /// <summary>
        /// 日志管理
        /// </summary>
        /// <returns></returns>
        public static IOperationLogDAL GetLog()
        {
            IOperationLogDAL dal = GetInstance("OperationLogDAL") as IOperationLogDAL;
            return dal;
        }
        #endregion

        #region 部门管理
        /// <summary>
        /// 日志管理
        /// </summary>
        /// <returns></returns>
        public static IOrganizationDAL GetOrganization()
        {
            IOrganizationDAL dal = GetInstance("OrganizationDAL") as IOrganizationDAL;
            return dal;
        }
        #endregion

        #region 消息管理
        /// <summary>
        /// 消息管理
        /// </summary>
        /// <returns></returns>
        public static IMessageDAL GetMessage()
        {
            IMessageDAL dal = GetInstance("MessageDAL") as IMessageDAL;
            return dal;
        }
        #endregion

        #region 用户管理
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        public static IOperationUserDAL GetOperationUserDAL()
        {
            IOperationUserDAL dal = GetInstance("OperationUserDAL") as IOperationUserDAL;
            return dal;
        }
        #endregion

        #region 权限管理
        /// <summary>
        /// 权限管理
        /// </summary>
        /// <returns></returns>
        public static IAuthorityManagementDAL GetAuthorityManagementDAL()
        {
            IAuthorityManagementDAL dal = GetInstance("AuthorityManagementDAL") as IAuthorityManagementDAL;
            return dal;
        }
        #endregion

        #region 人员管理
        /// <summary>
        /// 人员管理
        /// </summary>
        /// <returns></returns>
        public static IPersonalManagementDAL GetPersonalManagement()
        {
            IPersonalManagementDAL dal = GetInstance("PersonalManagementDAL") as IPersonalManagementDAL;
            return dal;
        }
        #endregion

        #region 字典管理
        /// <summary>
        /// 字典管理
        /// </summary>
        /// <returns></returns>

        public static IDictionaryManagementDAL GetDictionaryManagement()
        {
            IDictionaryManagementDAL dal = GetInstance("DictionaryManagementDAL") as IDictionaryManagementDAL;
            return dal;
        }
        #endregion

        #region 质量管理
        /// <summary>
        /// 质量管理
        /// </summary>
        /// <returns></returns>
        public static IEnvironmentManageDAL GetManageQuality()
        {
            IEnvironmentManageDAL dal = GetInstance("EnvironmentManageDAL") as IEnvironmentManageDAL;
            return dal;
        }
        #endregion

        #region 设备管理
        /// <summary>
        /// 设备管理
        /// </summary>
        /// <returns></returns>
        public static IEquipmentManagementDAL GetLedgerManagement()
        {
            IEquipmentManagementDAL dal = GetInstance("EquipmentManagementDAL") as IEquipmentManagementDAL;
            return dal;
        }
        #endregion

        #region 试块库管理
        /// <summary>
        /// 试块库管理
        /// </summary>
        /// <returns></returns>
        public static ITestBlockLibraryDAL GeTTestBlockLibraryDAL()
        {
            ITestBlockLibraryDAL dal = GetInstance("TestBlockLibraryDAL") as ITestBlockLibraryDAL;
            return dal;
        }
        #endregion

        #region 夹具管理
        /// <summary>
        /// 夹具管理
        /// </summary>
        /// <returns></returns>
        public static IFixtureManagementDAL GetFixtureManagement()
        {
            IFixtureManagementDAL dal = GetInstance("FixtureManagementDAL") as IFixtureManagementDAL;
            return dal;
        }
        #endregion

        #region 报告编制
        /// <summary>
        /// 报告编制
        /// </summary>
        /// <returns></returns>
        public static IReportEditDAL GetReportEdit()
        {
            IReportEditDAL dal = GetInstance("ReportEditDAL") as IReportEditDAL;
            return dal;
        }
        #endregion

        #region 报告审核
        /// <summary>
        /// 报告审核
        /// </summary>
        /// <returns></returns>
        public static IReportReviewDAL GetReportReview()
        {

            IReportReviewDAL dal = GetInstance("ReportReviewDAL") as IReportReviewDAL;
            return dal;
        }

        #endregion

        #region 报告签发
        /// <summary>
        /// 报告签发
        /// </summary>
        /// <returns></returns>
        public static IReportApprovalDAL GetReportApproval()
        {

            IReportApprovalDAL dal = GetInstance("ReportApprovalDAL") as IReportApprovalDAL;
            return dal;
        }

        #endregion

        #region 报告签发
        /// <summary>
        /// 报告签发
        /// </summary>
        /// <returns></returns>
        public static IPageShowSettingDAL GetPageShowSetting()
        {

            IPageShowSettingDAL dal = GetInstance("PageShowSettingDAL") as IPageShowSettingDAL;
            return dal;
        }

        #endregion

        #region 报告管理
        /// <summary>
        /// 报告管理
        /// </summary>
        /// <returns></returns>
        public static IReportManagementDAL GetReportManagement()
        {

            IReportManagementDAL dal = GetInstance("ReportManagementDAL") as IReportManagementDAL;
            return dal;
        }

        #endregion

        #region 报告查看
        /// <summary>
        /// 报告查看
        /// </summary>
        /// <returns></returns>
        public static IReportInspectDAL GetReportInspect()
        {

            IReportInspectDAL dal = GetInstance("ReportInspectDAL") as IReportInspectDAL;
            return dal;
        }

        #endregion

        #region 异常申请审核
        /// <summary>
        /// 异常申请审核
        /// </summary>
        /// <returns></returns>
        public static IAbnormalApplyReviewDAL GetAbnormalApplyReview()
        {

            IAbnormalApplyReviewDAL dal = GetInstance("AbnormalApplyReviewDAL") as IAbnormalApplyReviewDAL;
            return dal;
        }

        #endregion

        #region 异常报告编制
        /// <summary>
        /// 异常报告编制
        /// </summary>
        /// <returns></returns>
        public static IAbnormalReportEditDAL GetAbnormalReportEdit()
        {

            IAbnormalReportEditDAL dal = GetInstance("AbnormalReportEditDAL") as IAbnormalReportEditDAL;
            return dal;
        }

        #endregion

        #region 异常报告审核
        /// <summary>
        /// 异常报告审核
        /// </summary>
        /// <returns></returns>
        public static IAbnormalReportManagementDAL GetAbnormalManagement()
        {

            IAbnormalReportManagementDAL dal = GetInstance("AbnormalReportManagementDAL") as IAbnormalReportManagementDAL;
            return dal;
        }

        #endregion



        #region 通用获取文件

        /// <summary>
        /// 通用获取文件
        /// </summary>
        /// <returns></returns>
        public static IOfficeOperateDAL GetFile()
        {

            IOfficeOperateDAL dal = GetInstance("OfficeOperateDAL") as IOfficeOperateDAL;
            return dal;

        }

        #endregion

        #region 项目监控
        /// <summary>
        /// 项目监控
        /// </summary>
        /// <returns></returns>
        public static IItemMonitoringDAL GetItemMonitoring()
        {

            IItemMonitoringDAL dal = GetInstance("ItemMonitoringDAL") as IItemMonitoringDAL;
            return dal;
        }

        #endregion

        #region 统计管理
        /// <summary>
        /// 统计管理
        /// </summary>
        /// <returns></returns>
        public static IStatisticalManagementDAL GetStatisticalManagement()
        {

            IStatisticalManagementDAL dal = GetInstance("StatisticalManagementDAL") as IStatisticalManagementDAL;
            return dal;
        }

        #endregion

        #region 资质管理（模板）
        /// <summary>
        /// 统计管理
        /// </summary>
        /// <returns></returns>
        public static IProjectReviewDAL GetProjectReview()
        {

            IProjectReviewDAL dal = GetInstance("ProjectReviewDAL") as IProjectReviewDAL;
            return dal;
        }

        #endregion

        #region 资质管理（权限）
        /// <summary>
        /// 统计管理
        /// </summary>
        /// <returns></returns>
        public static IQualificationManagementDAL GetQualificationManagement()
        {

            IQualificationManagementDAL dal = GetInstance("QualificationManagementDAL") as IQualificationManagementDAL;
            return dal;
        }

        #endregion

    }
}
