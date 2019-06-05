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
using Aspose.Words;
using Phians_Entity.LosslessReport;
using System.IO;
using Ionic.Zip;

namespace Phians_BLL
{
    public class ReportManagementBLL
    {
        /// <summary>
        /// 报告管理数据 interface
        /// </summary>
        /// 
        IReportManagementDAL dal = DALFactory.GetReportManagement();

        #region 加载报告管理信息
        /// <summary>
        /// 加载报告管理信息
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadReportManageList(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadReportManageList(PageModel, out totalRecord);
        }

        #endregion

        #region 添加线下报告信息
        /// <summary>
        /// 添加线下报告信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <returns></returns>
        public ReturnDALResult AddUnderLineReportInfo(TB_NDT_report_title model, Guid LogPersonnel)
        {

            return dal.AddUnderLineReportInfo(model, LogPersonnel);
        }

        #endregion

        #region 异常报告申请
        /// <summary>
        /// 异常报告申请
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_NDT_error_Certificate"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitAbnormalReport(TB_NDT_report_title model, TB_ProcessRecord TB_ProcessRecord, TB_NDT_error_Certificate error_Certificate, Guid LogPersonnel)
        {

            return dal.SubmitAbnormalReport(model, TB_ProcessRecord, error_Certificate, LogPersonnel);
        }

        #endregion

        #region 批量下载
        /// <summary>
        /// 批量下载
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnDALResult Alldownload(dynamic model)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();

            List<ReportManagementDownload> DownloadInfo = new List<ReportManagementDownload>();
            //下载条件 0选择下载 1搜索下载
            if (model.DownloadCheck == "0")
            {
                DownloadInfo = dal.Choosedownload(model.ids);
            }
            else
            {
                DownloadInfo = dal.Searchdownload(model);
            }

            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(model.rootUrl));

            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            //循环导出Doc
            foreach (var item in DownloadInfo)
            {
                //Aspose.Words.Document contract_doc = null;
                //contract_doc = new Aspose.Words.Document(path + item.report_url);
                //contract_doc.Save(System.Web.HttpContext.Current.Server.MapPath(model.rootUrl + "/" + item.newfilename.ToString()), SaveFormat.Doc);
                if (System.IO.File.Exists(path+item.report_url))
                {

                    System.IO.File.Copy(path + item.report_url, System.Web.HttpContext.Current.Server.MapPath(model.rootUrl + "/" + item.newfilename.ToString()));
                }
            }

            //打包zip
            try
            {
                using (var zip = new ZipFile(Encoding.Default))
                {
                    //把待添加文件添加到压缩包
                    zip.AddDirectory(System.Web.HttpContext.Current.Server.MapPath(model.rootUrl));
                    //保存压缩包
                    zip.Save(System.Web.HttpContext.Current.Server.MapPath(model.savePath)); //生成包的名称
                }
                //删除临时文件夹
                Directory.Delete(System.Web.HttpContext.Current.Server.MapPath(model.rootUrl), true);

                ReturnDALResult.Success = 1;
                ReturnDALResult.returncontent = model.savePath;
            }
            catch (Exception e)
            {

                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = e.ToString();
            }
            return ReturnDALResult;
        }

        #endregion

        #region 根据列导出报告
        /// <summary>
        /// 根据列导出报告
        /// </summary>
        /// <param name="search">搜索条件</param>
        /// <param name="key">搜索值</param>
        /// <param name="tempFilePath">导出路径</param>
        /// <param name="type">导出类型</param>
        /// <param name="ids">报告id集合</param>
        /// <param name="columns">列集合</param>
        /// <returns></returns>
        public ReturnDALResult Report_ExportExcl(string search3, string key3, string search2, string key2, string search1, string key1, string search, string key, string tempFilePath, int type, string ids)
        {
            ReturnDALResult result = new ReturnDALResult();
            //获取展示的列
            List<TB_PageShowCustom> PageList = dal.loadPageShowSetting("104");

            string columns = "";
            //创建新的EXCELSheet
            Windows.Excel.Workbook wb = new Windows.Excel.Workbook();
            Windows.Excel.Worksheet ws = wb.Worksheets[0];
            Windows.Excel.Cells cells = ws.Cells;
            //插入列中文名
            for (int c = 0; c < PageList.Count; c++)
            {
                //插入内容
                //第一行
                cells.Merge(0, 0, 1, 13);
                cells[0, 0].PutValue("报告导出");
                //cells[0, 0].SetStyle(style);
                cells.SetRowHeight(0, 20.5);//行高
                //cells.SetColumnWidth(0, 3.5);//列宽
                cells[1, c].PutValue(PageList[c].Title);
                if (PageList[c].FieldName == "Inspection_personnel_n") PageList[c].FieldName = "Inspection_personnel";
                if (PageList[c].FieldName == "Audit_personnel_n") PageList[c].FieldName = "Audit_personnel";
                if (PageList[c].FieldName == "issue_personnel_n") PageList[c].FieldName = "issue_personnel";
                if (PageList[c].FieldName == "Audit_groupid_n") PageList[c].FieldName = "Audit_groupid";
                columns += PageList[c].FieldName + ",";
            }
            columns = columns.Substring(0, columns.Length - 1);
            //根据英文列名查找值
            System.Data.DataTable reportlist = dal.Report_ExportExcl(search3, key3, search2, key2, search1, key1, search, key, type, ids, columns);
            if (reportlist.Rows.Count > 0)
            {
                for (int i = 0; i < reportlist.Rows.Count; i++)
                {
                    for (int j = 0; j < PageList.Count; j++)
                    {
                        
                        if (PageList[j].FieldName.ToString() == "state_")
                        {
                            switch (reportlist.Rows[i][PageList[j].FieldName.ToString()].ToString())
                            {
                                case "4":
                                    cells[i + 2, j].PutValue("完成");
                                    break;
                                case "6":
                                    cells[i + 2, j].PutValue("报废申请");
                                    break;
                                case "7":
                                    cells[i + 2, j].PutValue("异常完成");
                                    break;
                                case "8":
                                    cells[i + 2, j].PutValue("报废完成");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            cells[i + 2, j].PutValue(reportlist.Rows[i][PageList[j].FieldName.ToString()]);
                        }
                    }

                }
                ws.Name = "报告管理导出";
                ws.AutoFitColumns();//让各列自适应宽度，这个很有用
                //string path = Path.GetTempFileName();
                //Session["FailMsg"] = path;
                string fileName="报告管理导出" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
                wb.Save(System.Web.HttpContext.Current.Server.MapPath(tempFilePath) + fileName);
                result.Success = 1;
                result.returncontent = tempFilePath + fileName;
            }
            return result;
        }
        #endregion

        #region 获取报告信息
        /// <summary>
        /// 获取报告信息
        /// </summary>
        /// <param name="id">报告id</param>
        /// <returns></returns>
        public TB_NDT_report_title LoadReportInfo(int id)
        {

            return dal.LoadReportInfo(id);
        }

        #endregion
    }
}
