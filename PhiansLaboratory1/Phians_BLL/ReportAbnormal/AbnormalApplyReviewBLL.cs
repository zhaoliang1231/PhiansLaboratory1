using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhiansCommon.Enum;

namespace Phians_BLL
{
    public class AbnormalApplyReviewBLL
    {
        IAbnormalApplyReviewDAL dal = DALFactory.GetAbnormalApplyReview();

        #region 加载异常申请信息
        /// <summary>
        /// 加载异常申请信息
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="PageModel">页面传参</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_error_Certificate> LoadUnusualCertificateList(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadUnusualCertificateList(PageModel, out totalRecord);
        }

        #endregion


        #region 异常申请通过审核
        /// <summary>
        /// 申请通过审核
        /// </summary>
        /// <param name="Model">异常申请报告</param>
        /// <param name="type">异常申请结果</param>
        /// <returns></returns>
        public ReturnDALResult PassUnusualApplyBLL(TB_NDT_error_Certificate Model,TB_ProcessRecord TB_ProcessRecord, int type,int flag,Guid Operator)
        {
            int state_ = (int)LosslessReportStatusEnum.Finish;

            return dal.PassUnusualApply(Model, TB_ProcessRecord, type, flag, Operator, state_);
            //int AcceptState = Convert.ToInt32(ReportabnormityEnum.ReportEdit);
            //int CurrentState = Convert.ToInt32(ReportabnormityEnum.ReportApplicationReview);

            //TB_UnusualCertificate UnusualCertificate = dal.LoadUnusualReportInfo(Model.id);//获取总报告信息

            //string Certificate_TemplateUrl = System.Web.HttpContext.Current.Server.MapPath(UnusualCertificate.FileUrl);//报告路径

            #region 创建word对象

            ////创建word对象
            //Aspose.Words.Document contract_doc = null;
            //contract_doc = new Aspose.Words.Document(Certificate_TemplateUrl);
            //Aspose.Words.DocumentBuilder docBuild = new Aspose.Words.DocumentBuilder(contract_doc);
            //Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)contract_doc.GetChild(Aspose.Words.NodeType.Table, 0, true);

            //try
            //{
            //    #region 清除编制信息
            //    //清除编制人签名                    
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Prepared_img"].Text = "";
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //    //清除编制人姓名               
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Prepared"].Text = "";//编制人姓名

            //    }
            //    catch (Exception ex)
            //    {
            //    }

            //    //清除编制人职务             
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Title1"].Text = "";//编制人职务

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    //清除编制日期            
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Date1"].Text = "";//编制日期         

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    #endregion

            //    #region 清除审核信息


            //    //清除审核人签名                    
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Checked_img"].Text = "";

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    //清除审核人姓名               
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Checked"].Text = "";//审核人姓名

            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //    //清除审核人职务             
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Title2"].Text = "";//审核人职务

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    //清除审核日期            
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Date2"].Text = "";//审核日期         

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    #endregion

            //    #region 清除签发信息


            //    //清除审核人签名                    
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Approved_img"].Text = "";

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    //清除审核人姓名               
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Approved"].Text = "";//审核人姓名

            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //    //清除审核人职务             
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Title4"].Text = "";//审核人职务

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    //清除审核日期            
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Date4"].Text = "";//审核日期         

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    #endregion

            //    //写入意见建议           
            //    try
            //    {
            //        contract_doc.Range.Bookmarks["Comment"].Text = "";//意见建议          

            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //    contract_doc.Save(Certificate_TemplateUrl, Aspose.Words.SaveFormat.Doc);

            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            #endregion

           

        }

        #endregion

    }
}
