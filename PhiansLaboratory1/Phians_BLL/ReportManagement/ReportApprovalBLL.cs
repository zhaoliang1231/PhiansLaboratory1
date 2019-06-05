using Aspose.Words;
using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using PhiansCommon.Enum;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
    public class ReportApprovalBLL
    {
        /// <summary>
        /// 报告签发数据 interface
        /// </summary>
        /// 
        IReportApprovalDAL dal = DALFactory.GetReportApproval();
        IReportEditDAL dalEdit = DALFactory.GetReportEdit();
        IReportReviewDAL dalReview = DALFactory.GetReportReview();

        #region 加载报告签发
        /// <summary>
        /// 加载报告签发
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadReportIssueList(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadReportIssueList(PageModel, out totalRecord);
        }

        #endregion

        #region 退回报告编制
        /// <summary>
        /// 退回报告编制
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_NDT_error_log"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult BackIssueReport(TB_NDT_report_title model, Guid LogPersonnel, List<TB_NDT_error_log> TB_NDT_error_log, TB_ProcessRecord TB_ProcessRecord)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            #region 获取报告信息

            TB_NDT_report_title TB_NDT_report_title = dalEdit.getNDTReportInfo(model.id);

            //判断报告是否存在
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url)))
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "报告文件不存在！";
                return ReturnDALResult;


            }
            #endregion

            ReturnDALResult ReturnDALResult2 = dal.BackIssueReport(model, LogPersonnel, TB_NDT_error_log, TB_ProcessRecord, (int)LosslessReportStatusEnum.Issue);

            //执行成功
            if (ReturnDALResult2.Success == 0)
            {
                return ReturnDALResult2;
            }

            #region 报告签名处理

            //正常报告处理
            Document first_doc = new Document(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url));
            DocumentBuilder first_builder = new DocumentBuilder(first_doc);

            //写入审核人签字时间
            try
            {
                first_doc.Range.Bookmarks["Inspection_personnel_date"].Text = "";//检验人签字时间
                first_doc.Range.Bookmarks["Audit_date"].Text = "";//审核人签字时间
                first_doc.Range.Bookmarks["issue_date"].Text = "";//签发人签字时间

            }
            catch (Exception ex)
            {
            }
            //写入审核级别
            try
            {
                first_doc.Range.Bookmarks["level_Inspection"].Text = "";//编制级别
                first_doc.Range.Bookmarks["level_Audit"].Text = "";//审核级别
            }
            catch (Exception ex)
            {
            }
            //写入审核签发人员签名                    
            try
            {
                first_doc.Range.Bookmarks["Inspection_personnel"].Text = "";
                first_doc.Range.Bookmarks["Audit_personnel"].Text = "";//审核人签字
                first_doc.Range.Bookmarks["issue_personnel"].Text = "";//签发人签字

            }
            catch
            { }



            //保存两份报告
            first_doc.Save(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url), Aspose.Words.SaveFormat.Doc);//签发报告

            #endregion

            return ReturnDALResult2;


        }

        #endregion

        #region 提交签发报告
        /// <summary>
        /// 提交签发报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitIssueReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();


            #region 判断签名是否存在，签名不存在则返回信息,签名存在则写入签名

            TB_user_info TB_user_info = dalEdit.getDetection(TB_ProcessRecord.Operator);
            if (!string.IsNullOrEmpty(TB_user_info.Signature))
            {
                //判断签名是否存在
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_user_info.Signature.ToString())))
                {
                    ReturnDALResult.Success = 0;
                    ReturnDALResult.returncontent = "签名不存在";
                    return ReturnDALResult;
                }
            }
            else
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "签名不存在";
                return ReturnDALResult;
            }
           

            #endregion

            #region 获取报告信息

            TB_NDT_report_title TB_NDT_report_title = dalEdit.getNDTReportInfo(model.id);

            //判断报告是否存在
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url)))
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "报告文件不存在！";
                return ReturnDALResult;

            }
            #endregion


            model.report_num = TB_NDT_report_title.report_num;
            model.report_url = TB_NDT_report_title.report_url;

            ReturnDALResult ReturnDALResult2 = dal.SubmitIssueReport(model, TB_ProcessRecord, LogPersonnel, (int)LosslessReportStatusEnum.Issue);

            //如果执行不成功，则直接返回信息
            if (ReturnDALResult2.Success != 1)
            {
                return ReturnDALResult2;
            }


            #region 获取报告文档版本记录信息

            TB_NDT_RevisionsRecord TB_NDT_RevisionsRecord = dalReview.GetRevisionsRecord(model.id);

            #endregion

            #region 报告签名处理

            //正常报告处理
            Document first_doc = new Document(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url));
            DocumentBuilder first_builder = new DocumentBuilder(first_doc);

            //版本报告处理
            Document record_Logdoc = new Document(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_RevisionsRecord.report_url));
            DocumentBuilder record_Logbuilder = new DocumentBuilder(record_Logdoc);

            //写入签发人签字时间
            try
            {

                first_doc.Range.Bookmarks["issue_date"].Text = Convert.ToString(model.issue_date).Split(' ')[0];//审核人签字时间
                record_Logdoc.Range.Bookmarks["issue_date"].Text = Convert.ToString(model.issue_date).Split(' ')[0];
            }
            catch (Exception ex)
            {
            }
            //写入签发人员签名                    
            try
            {
                first_doc.Range.Bookmarks["issue_personnel"].Text = "";
                first_builder.MoveToBookmark("issue_personnel");//跳到指定书签
                double width = 50, height = 20;
                Aspose.Words.Drawing.Shape shape = first_builder.InsertImage(System.Web.HttpContext.Current.Server.MapPath(TB_user_info.Signature.ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                shape.Width = width;
                shape.Height = height;
                //shape.VerticalAlignment = VerticalAlignment.Center;
                first_builder.InsertNode(shape);

                record_Logdoc.Range.Bookmarks["issue_personnel"].Text = "";
                record_Logbuilder.MoveToBookmark("issue_personnel");//跳到指定书签
                //double widths = 50, heights = 20;
                Aspose.Words.Drawing.Shape shapes = record_Logbuilder.InsertImage(System.Web.HttpContext.Current.Server.MapPath(TB_user_info.Signature.ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                shapes.Width = width;
                shapes.Height = height;
                //shape.VerticalAlignment = VerticalAlignment.Center;
                record_Logbuilder.InsertNode(shapes);

            }
            catch
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "插入签名失败！";
                return ReturnDALResult;
            }

            //保存两份报告
            first_doc.Save(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url), Aspose.Words.SaveFormat.Doc);//签发报告
            record_Logdoc.Save(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_RevisionsRecord.report_url), Aspose.Words.SaveFormat.Doc);//版本报告

            #endregion

            return ReturnDALResult2;

        }

        #endregion


    }
}
