using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity.Common;
using PhiansCommon.Enum;
using Aspose.Words;

namespace Phians_BLL
{
    public class AbnormalReportEditBLL
    {
        IAbnormalReportEditDAL dal = DALFactory.GetAbnormalReportEdit();
        //IAbnormalReportIssueDAL Issudal = DALFactory.GetAbnormalReportIssue();
        //IReportEditDAL ReportEditdal = DALFactory.GetReportEdit();


        #region 异常报告提交
        /// <summary>
        /// 异常报告提交审核
        /// </summary>
        /// <param name="Model">异常报告流程记录表信息</param>
        /// <param name="MTRNO">MTR单号</param>
        /// <param name="AcceptState">异常报告申请状态</param>
        /// <returns></returns>

        public ReturnDALResult SubmitAbnormalReportReview(TB_NDT_error_Certificate Model,TB_ProcessRecord TB_ProcessRecord,TB_NDT_report_title reportModule, Guid Operator, string PageType)
        {
            return dal.SubmitAbnormalReportReview(Model,TB_ProcessRecord, reportModule,Operator,PageType);
        }

        #endregion

        #region 加载异常报告信息
        /// <summary>
        /// 加载异常申请信息
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="PageModel">页面传参</param>
        /// <param name="totalRecord">输出总记录</param>      
        /// <returns>返回项目信息实体集</returns>
        /// 
        public List<TB_NDT_error_Certificate> GetUnusualCertificateListBLL(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetUnusualCertificateList(PageModel, out totalRecord);
            
        }
        #endregion





        #region 退回编制
        public ReturnDALResult BackAbnormalReviewReport(TB_NDT_error_Certificate module,TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel)
        {
            return dal.BackAbnormalReviewReport(module,TB_ProcessRecord, LogPersonnel);
        }
        #endregion
    }
}
