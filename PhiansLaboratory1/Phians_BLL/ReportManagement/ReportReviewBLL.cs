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

namespace Phians_BLL.ReportManagement
{
    /// <summary>
    /// 报告审核业务层
    /// </summary>
    public class ReportReviewBLL
    {
        /// <summary>
        /// 报告审核数据 interface
        /// </summary>
        /// 
        IReportReviewDAL dal = DALFactory.GetReportReview();
        IReportEditDAL dalEdit = DALFactory.GetReportEdit();

        #region 加载报告审核
        /// <summary>
        /// 加载报告审核
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadReportReviewList(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadReportReviewList(PageModel, out totalRecord);
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
        public ReturnDALResult BackReviewReport(TB_NDT_report_title model, Guid LogPersonnel, List<TB_NDT_error_log> TB_NDT_error_log, TB_ProcessRecord TB_ProcessRecord)
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

            ReturnDALResult ReturnDALResult2 = dal.BackReviewReport(model, LogPersonnel, TB_NDT_error_log, TB_ProcessRecord, (int)LosslessReportStatusEnum.Audit);

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

        #region 提交审核报告
        /// <summary>
        /// 提交审核报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitReviewReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel, int tm_id)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();

            #region 判断签名是否存在，签名不存在则返回信息,签名存在则写入签名

            TB_user_info TB_user_info = dalEdit.getDetection(model.Audit_personnel);

            //判断签名是否存在
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_user_info.Signature.ToString())))
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "签名不存在，请上传签名！";
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

            #region 获取模板信息（二级三级审批）

            TB_TemplateFile TemplateFileInfo = dalEdit.LoadTemplateFile(tm_id);

            if (TemplateFileInfo.ReviewLevel == 2)
            {
                model.state_ = (int)LosslessReportStatusEnum.Finish;
                TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ReviewToManage;
            }
            else if (TemplateFileInfo.ReviewLevel == 3)
            {
                model.state_ = (int)LosslessReportStatusEnum.Issue;
                TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ReviewToIssue;
            }
            else
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "模板不存在评审等级！";
                return ReturnDALResult;
            }

            #endregion


            ReturnDALResult ReturnDALResult2 = dal.SubmitReviewReport(model, TB_ProcessRecord, LogPersonnel, (int)LosslessReportStatusEnum.Audit);

            //如果执行不成功，则直接返回信息
            if (ReturnDALResult2.Success != 1)
            {
                return ReturnDALResult2;
            }


            #region 获取报告文档版本记录信息

            TB_NDT_RevisionsRecord TB_NDT_RevisionsRecord = dal.GetRevisionsRecord(model.id);

            #endregion

            #region 报告签名处理

            //正常报告处理
            Document first_doc = new Document(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url));
            DocumentBuilder first_builder = new DocumentBuilder(first_doc);

            //版本报告处理
            Document record_Logdoc = new Document(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_RevisionsRecord.report_url));
            DocumentBuilder record_Logbuilder = new DocumentBuilder(record_Logdoc);
            //写入审核人签字时间
            try
            {

                first_doc.Range.Bookmarks["Audit_date"].Text = Convert.ToString(model.Audit_date).Split(' ')[0];//审核人签字时间
                record_Logdoc.Range.Bookmarks["Audit_date"].Text = Convert.ToString(model.Audit_date).Split(' ')[0];
            }
            catch (Exception ex)
            {
            }
            //写入审核级别
            try
            {
                first_doc.Range.Bookmarks["level_Audit"].Text = model.level_Audit;//审核级别
                record_Logdoc.Range.Bookmarks["level_Audit"].Text = model.level_Audit;
            }
            catch (Exception ex)
            {
            }
            //写入审核人员签名                    
            try
            {
                first_doc.Range.Bookmarks["Audit_personnel"].Text = "";
                first_builder.MoveToBookmark("Audit_personnel");//跳到指定书签
                double width = 50, height = 20;
                Aspose.Words.Drawing.Shape shape = first_builder.InsertImage(System.Web.HttpContext.Current.Server.MapPath(TB_user_info.Signature.ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                shape.Width = width;
                shape.Height = height;
                //shape.VerticalAlignment = VerticalAlignment.Center;
                first_builder.InsertNode(shape);

                record_Logdoc.Range.Bookmarks["Audit_personnel"].Text = "";
                record_Logbuilder.MoveToBookmark("Audit_personnel");//跳到指定书签
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

        #region --退回原因

        #region 加载全部退回原因
        /// <summary>
        /// 加载全部退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_DictionaryManagement> AllErrorInfo(TPageModel PageModel, out int totalRecord)
        {
            return dal.AllErrorInfo(PageModel, out totalRecord);
        }

        #endregion

        #region 添加退回原因
        /// <summary>
        /// 添加退回原因
        /// </summary>
        /// <param name="model"></param>
        /// <param name="report_num"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult AddErrorInfo(TB_NDT_error_log model, string report_num, Guid LogPersonnel)
        {
            return dal.AddErrorInfo(model, report_num, LogPersonnel);
        }

        #endregion

        #region 加载已选退回原因
        /// <summary>
        /// 加载已选退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <param name="id">report_id</param>
        /// <returns></returns>
        public List<TB_NDT_error_log> ReturnErrorInfo(TPageModel PageModel, out int totalRecord, int report_id)
        {
            return dal.ReturnErrorInfo(PageModel, out totalRecord, report_id);
        }

        #endregion

        #endregion

        #region 审核人员自己退回未开始签发的报告
        /// <summary>
        /// 审核人员自己退回未开始签发的报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult TakeBackReviewReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, Guid LogPersonnel)
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

            model.report_num = TB_NDT_report_title.report_num;
            #endregion

            ReturnDALResult ReturnDALResult2 = dal.TakeBackReviewReport(model, LogPersonnel, TB_ProcessRecord);

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

            //保存报告
            first_doc.Save(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url), Aspose.Words.SaveFormat.Doc);//报告

            #endregion

            return ReturnDALResult2;
        }

        #endregion

        #region 判断人员资质
        /// <summary>
        /// 判断人员资质
        /// </summary>
        /// <param name="model">人员资质信息</param>
        /// <returns></returns>
        public ReturnDALResult JudgingPersonnelQualifications(TB_PersonnelQualification model)
        {
            return dal.JudgingPersonnelQualifications(model);
        }

        #endregion

    }
}
