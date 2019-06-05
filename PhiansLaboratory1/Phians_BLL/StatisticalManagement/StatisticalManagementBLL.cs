using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phians_Entity.Common;
using Windows.Excel;
using Phians_Entity.LosslessReport;

namespace Phians_BLL
{
    public class StatisticalManagementBLL
    {
        IStatisticalManagementDAL dal = DALFactory.GetStatisticalManagement();

        #region 统计管理

        #region 无损获取统计数据
        public List<TB_StatisticsCount> GetStatistical(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetStatistical(PageModel, out totalRecord);
        }
        #endregion

        #region 统计列表数据
        public List<TB_NDT_report_title> Report_ArrangeList(TPageModel PageModel, out int totalRecord)
        {
            return dal.Report_ArrangeList(PageModel, out totalRecord);
        }
        #endregion

        #region 获取科室或组
        public List<TB_group> GetGroup(Guid GroupParentId)
        {
            return dal.GetGroup(GroupParentId);
        }
        #endregion

        #region 获取逾期类型
        public List<TB_DictionaryManagement> GetDicitionaryData(Guid DicitionaryParentId)
        {
            return dal.GetDicitionaryData(DicitionaryParentId);
        }
        #endregion

        #region 加载模板文件
        /// <summary>
        /// 加载模板文件
        /// </summary>
        /// <param name="PageModel"></param>

        /// <returns></returns>
        public List<TB_TemplateFile> GetReportTemplate()
        {
            return dal.GetReportTemplate();
        }

        #endregion

        #region 导出列表
        public ReturnDALResult Report_ExportExcl(List<TB_NDT_report_title> list_model, string tempFilePath, string tempFileName)
        {
            ReturnDALResult result = new ReturnDALResult();
            //创建新的EXCELSheet
            Windows.Excel.Workbook wb = new Windows.Excel.Workbook();
            Windows.Excel.Worksheet ws = wb.Worksheets[0];
            Windows.Excel.Cells cells = ws.Cells;
            int i;
            //插入内容
            //第一行
            cells.Merge(0, 0, 1, 13);
            cells[0, 0].PutValue("设备列表");
            //cells[0, 0].SetStyle(style);
            cells.SetRowHeight(0, 20.5);//行高
            //cells.SetColumnWidth(0, 3.5);//列宽
            i = 0;
            if (list_model.Count > 0)
            {
                cells[1, 0].PutValue("NO");
                cells[1, 1].PutValue("报告编号");
                cells[1, 2].PutValue("报告名称");
                cells[1, 3].PutValue("检验人");
                cells[1, 4].PutValue("检验人签字时间");
                cells[1, 5].PutValue("审核人");
                cells[1, 6].PutValue("审核时间");
                cells[1, 7].PutValue("签发人");
                cells[1, 8].PutValue("签发时间");
                cells[1, 9].PutValue("错误类型");
                cells[1, 10].PutValue("检验结果");
                cells[1, 11].PutValue("状态");
                //cells[1, 12].PutValue("状态");
                foreach (var item in list_model)
                {
                    cells[2 + i, 0].PutValue(i + 1);
                    cells[2 + i, 1].PutValue(item.report_num);
                    cells[2 + i, 2].PutValue(item.report_name);
                    cells[2 + i, 3].PutValue(item.Inspection_personnel);
                    cells[2 + i, 4].PutValue(item.Inspection_personnel_date);
                    Style style4 = ws.Cells[2 + i, 4].GetStyle();
                    style4.Custom = "yyyy-mm-dd";
                    ws.Cells[2 + i, 4].SetStyle(style4);

                    cells[2 + i, 5].PutValue(item.Audit_personnel);
                    cells[2 + i, 6].PutValue(item.Audit_date);
                    Style style6 = ws.Cells[2 + i, 6].GetStyle();
                    style6.Custom = "yyyy-mm-dd";
                    ws.Cells[2 + i, 6].SetStyle(style6);

                    cells[2 + i, 7].PutValue(item.issue_personnel);
                    cells[2 + i, 8].PutValue(item.issue_date);
                    Style style8 = ws.Cells[2 + i, 8].GetStyle();
                    style8.Custom = "yyyy-mm-dd";
                    ws.Cells[2 + i, 8].SetStyle(style8);

                    cells[2 + i, 9].PutValue(item.return_info);
                    //cells[2 + i, 10].PutValue(item.return_flag);
                    //switch (item.return_flag)
                    //{
                    //    case 0: cells[2 + i, 10].PutValue("是"); break;
                    //    case 1: cells[2 + i, 10].PutValue("否"); break;
                    //    default: break;

                    //}

                    if (item.Inspection_result == "0")
                    {
                        cells[2 + i, 10].PutValue("不合格");
                    }
                    if (item.Inspection_result == "1")
                    {
                        cells[2 + i, 10].PutValue("合格");
                    }
                    cells[2 + i, 11].PutValue(item.state_);
                    switch (item.state_)
                    {
                        case 1: cells[2 + i, 11].PutValue("编辑"); break;
                        case 2: cells[2 + i, 11].PutValue("审核"); break;
                        case 3: cells[2 + i, 11].PutValue("签发"); break;
                        case 4: cells[2 + i, 11].PutValue("完成"); break;
                        case 5: cells[2 + i, 11].PutValue("异常申请"); break;
                        case 6: cells[2 + i, 11].PutValue("报废申请"); break;
                        case 7: cells[2 + i, 11].PutValue("异常完成"); break;
                        case 8: cells[2 + i, 11].PutValue("报废完成"); break;

                        default: break;

                    }
                    i++;
                }

                ws.Name = "统计导出列表";
                ws.AutoFitColumns();//让各列自适应宽度，这个很有用
                //string path = Path.GetTempFileName();
                //Session["FailMsg"] = path;
                wb.Save(tempFilePath + tempFileName);
                result.Success = 1;
                result.returncontent = tempFileName;
            }
            return result;
        }
        #endregion


        #endregion


        #region 个人工作量统计显示

        #region 个人工作量统计显示
        /// <summary>
        /// 个人工作量统计显示
        /// </summary>
        /// <param name="PageModel">搜索条件</param>
        /// <param name="totalRecord">搜索数量</param>
        /// <returns></returns>
        public List<TB_NDT_report_title> LoadPersonnelTaskStatistics(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadPersonnelTaskStatistics(PageModel, out totalRecord);
        }
        #endregion

        #endregion

    }
}
