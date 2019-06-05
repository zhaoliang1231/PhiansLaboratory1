using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Tables;
using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web;
using PhiansCommon.Enum;
using Phians_Entity.LosslessReport;

namespace Phians_BLL
{
    /// <summary>
    /// 报告编辑业务层
    /// </summary>
    public class ReportEditBLL
    {
        IReportEditDAL dal = DALFactory.GetReportEdit();

        #region 获取报告编制列表
        /// <summary>
        /// 获取报告编制列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_report_title> LoadReportEditList(int pageIndex, int pageSize, out int totalRecord, string search, string key, string history_flag, string Inspection_personnel)
        {
            //MTR状态
            int ReportEditStatus = Convert.ToInt32(LosslessReportStatusEnum.Edit); // 报告编制

            return dal.LoadReportEditList(pageIndex, pageSize, out totalRecord, search, key, history_flag, Inspection_personnel, ReportEditStatus);
        }

        #endregion

        #region 页面字段显示List
        /// <summary>
        /// 页面字段显示List
        /// </summary>
        /// <param name="PageId">页面id</param>
        /// <returns></returns>
        public string loadPageShowSetting(string PageId)
        {
            List<TB_PageShowCustom> TB_PageShowCustom = dal.loadPageShowSetting(PageId);

            DataTable new_table = ListToDataTable.ListToDataTable_(TB_PageShowCustom);

            //将搜索出来的字段拼接成json串
            string strJson = "[";
            for (int i = 0; i < new_table.Rows.Count; i++)
            {
                strJson = strJson + "{ \"fieldname\":\"" + Convert.ToString(new_table.Rows[i][2]) + "\",\"hidden\":" + Convert.ToString(new_table.Rows[i][5]).ToLower() + ",\"sortable\":" + Convert.ToString(new_table.Rows[i][6]).ToLower() + " },";
            }
            strJson = strJson.Substring(0, strJson.Length - 1); //去掉最后的逗号
            strJson = strJson + "]";

            return strJson;
        }

        #endregion

        #region 写入报告信息
        /// <summary>
        /// 写入报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="flag">判断是否为复制的报告 为“1”则复制报告 </param>
        /// <returns>结果类</returns>
        public ReturnDALResult AddReportBaseInfo(TB_NDT_report_title model, string flag, int Finish, int id, string outputPath, Guid LogPersonnel)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string report_url = "";
            string destFileName = System.Web.HttpContext.Current.Server.MapPath(outputPath);

            TB_TemplateFile TB_TemplateFile = dal.LoadTemplateFile(Convert.ToInt32(model.tm_id));
            if (TB_TemplateFile == null)
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "找不到模板！";
                return ReturnDALResult;

            }
            model.report_format = ".doc";//报告都是doc管理
            model.report_name = TB_TemplateFile.FileName;

            //判断是否为复制的报告 为“1”则复制报告
            if (flag == "1")
            {
                //拷贝报告url 
                report_url = dal.CopyReport(id);

                #region 判断报告是否存在
                // 绝对路径
                string sourceFileName = System.Web.HttpContext.Current.Server.MapPath(report_url);

                //如果报告文档存在，则复制文档
                if (File.Exists(sourceFileName))
                {
                    #region 拷贝报告

                    try
                    {
                        System.IO.File.Copy(sourceFileName, destFileName, true);//拷贝报告
                    }
                    catch (Exception e)
                    {
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = e.ToString();
                        return ReturnDALResult;
                    }

                    #endregion

                    //清空和填写部分表头信息
                    //创建word对象
                    Aspose.Words.Document contract_doc = null;
                    contract_doc = new Aspose.Words.Document(destFileName);
                    Aspose.Words.DocumentBuilder docBuild = new Aspose.Words.DocumentBuilder(contract_doc);
                    //Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)contract_doc.GetChild(NodeType.Table, 0, true);
                    NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);
                    //清空部分数据
                    #region 部分数据重新填写
                    foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
                    {
                        switch (mark.Name)
                        {
                            //表头信息
                            case "report_num": mark.Text = model.report_num; break;
                            case "report_num1": mark.Text = model.report_num; break;
                            case "Inspection_personnel": mark.Text = ""; break;//清空检验人签名
                            case "Inspection_personnel_date": mark.Text = ""; break;//清空检验时间
                            case "level_Inspection": mark.Text = ""; break;//清空检验人级别
                            case "Audit_personnel": mark.Text = ""; break;//清空审核人签名
                            case "Audit_date": mark.Text = ""; break;//清空审核时间
                            case "level_Audit": mark.Text = ""; break;//清空审核人级别

                            case "issue_personnel": mark.Text = ""; break;//清空签发级别
                            case "issue_date": mark.Text = ""; break;//清空签发人签名


                            case "clientele_department": mark.Text = model.clientele_department; break;
                            case "Work_instruction": mark.Text = model.Work_instruction; break;
                            case "heat_treatment": mark.Text = model.heat_treatment; break;
                            case "application_num": mark.Text = model.application_num; break;
                            case "Project_name": mark.Text = model.Project_name; break;
                            case "Subassembly_name": mark.Text = model.Subassembly_name; break;
                            case "Material": mark.Text = model.Material; break;
                            case "Type_": mark.Text = model.Type_; break;
                            case "Chamfer_type": mark.Text = model.Chamfer_type; break;
                            case "Drawing_num": mark.Text = model.Drawing_num; break;
                            case "Procedure_": mark.Text = model.Procedure_; break;
                            case "Inspection_context": mark.Text = model.Inspection_context; break;
                            case "Inspection_opportunity": mark.Text = model.Inspection_opportunity; break;
                            case "circulation_NO": mark.Text = model.circulation_NO; break;
                            case "procedure_NO": mark.Text = model.procedure_NO; break;
                            case "apparent_condition": mark.Text = model.apparent_condition; break;
                            case "manufacturing_process": mark.Text = model.manufacturing_process; break;
                            case "Batch_Num": mark.Text = model.Batch_Num; break;
                            case "Inspection_NO": mark.Text = model.Inspection_NO; break;
                            case "remarks": mark.Text = model.remarks; break;
                            case "Inspection_date": mark.Text = Convert.ToString(model.Inspection_date); break;
                            case "Tubes_num": mark.Text = model.Tubes_num; break;
                            case "Tubes_Size": mark.Text = model.Tubes_Size; break;
                            case "disable_report_num": mark.Text = model.disable_report_num; break;
                            case "welding_method": mark.Text = model.welding_method; break;


                            //表头重复信息 ——水压测试报告
                            case "report_num2": mark.Text = model.report_num; break;
                            case "report_num3": mark.Text = model.report_num; break;
                            case "report_num4": mark.Text = model.report_num; break;


                        }
                    }
                    #endregion
                    contract_doc.Save(destFileName, Aspose.Words.SaveFormat.Doc);
                }

                #endregion

            }
            model.report_url = outputPath;
            return dal.AddReportBaseInfo(model, Finish, LogPersonnel);

        }

        #endregion

        #region ---复制报告

        #region 获取复制报告列表
        /// 模板下拉框
        /// </summary>
        /// <returns></returns>
        public List<TB_NDT_report_title> loadReportCopy(TPageModel PageModel, out int totalRecord)
        {
            return dal.loadReportCopy(PageModel, out totalRecord);

        }

        #endregion

        #region 回显复制报告信息
        /// 回显复制报告信息
        /// </summary>
        /// <returns></returns>
        public List<TB_NDT_report_title> ReportCopyShow(TPageModel PageModel)
        {
            return dal.ReportCopyShow(PageModel);

        }

        #endregion

        #endregion

        #region 模板下拉框
        /// <summary>
        /// 模板下拉框
        /// </summary>
        /// <returns></returns>
        public List<LosslessComboboxEntityss> LoadTemplateCombobox()
        {
            return dal.LoadTemplateCombobox();

        }

        #endregion

        #region 修改报告信息
        /// <summary>
        /// 修改报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="LogPersonnel">操作人 </param>
        /// <returns>结果类</returns>
        public ReturnDALResult EditReportBaseInfo(TB_NDT_report_title model, Guid LogPersonnel)
        {
            return dal.EditReportBaseInfo(model, LogPersonnel);

        }

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
        public ReturnDALResult AddTextData(TB_NDT_test_probereport_data model, string report_num, string report_name, DateTime date, Guid LogPersonnel, string equipment_id, string equipment_name, string equipment_name_R)
        {

            return dal.AddTextData(model, report_num, report_name, date, LogPersonnel, equipment_id, equipment_name, equipment_name_R);

        }

        #endregion

        #region 删除信息
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">删除的id</param>
        /// <param name="report_num">报告编号</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <returns></returns>
        public ReturnDALResult DataDel(int id, string report_num, Guid LogPersonnel)
        {
            return dal.DataDel(id, report_num, LogPersonnel);

        }

        #endregion

        #region 获取设备
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <returns></returns>
        public List<ComboboxEntity_ss> GetEquipmentInfoBLL()
        {

            return dal.GetEquipmentInfo();

        }
        #endregion

        #region 获取报告设备
        /// <summary>
        /// 获取报告设备
        /// </summary>
        /// <returns></returns>
        public List<TB_NDT_test_equipment> GetReportEquipmentInfoBLL(int report_id)
        {


            return dal.GetReportEquipmentInfo(report_id);

        }
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
        public List<TB_NDT_probe_library> GetProbeTestBLL(int pageIndex, int pageSize, out int totalRecord, string search, string key, int Probe_state)
        {
            return dal.GetProbeTest(pageIndex, pageSize, out  totalRecord, search, key, Probe_state);
        }
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
        public List<TB_NDT_test_probe> GetRrportProbeBLL(int pageIndex, int pageSize, out int totalRecord, string search, string key, int ReportId)
        {

            return dal.GetRrportProbe(pageIndex, pageSize, out  totalRecord, search, key, ReportId);

        }
        #endregion

        #region 添加探头报报告
        /// <summary>
        /// 添加探头报报告
        /// </summary>
        /// <param name="ReportId">报告ID</param>
        /// <param name="probe_id">探头ID</param>
        /// <returns></returns>
        public ReturnDALResult add_Probe_testBLL(string ReportId, string probe_id)
        {

            return dal.add_Probe_test(ReportId, probe_id);
        }
        #endregion

        #region 删除已经添加探头
        /// <summary>
        /// 删除已经添加探头
        /// </summary>
        /// <param name="id">已经选择探头标识</param>     
        /// <returns></returns>
        public ReturnDALResult Delete_Probe_testBLL(string id)
        {

            return dal.Delete_Probe_test(id);

        }

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
        public List<TB_NDT_TestBlockLibrary> GetTestBlockLibrary(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetTestBlockLibrary(PageModel, out totalRecord);
        }
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
        public List<TB_NDT_TestTestBlock> GetTestTestBlock(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetTestTestBlock(PageModel, out totalRecord);
        }

        #endregion

        #region 添加试块到报告
        /// <summary>
        /// 添加探头报报告
        /// </summary>
        /// <param name="ReportId">报告ID</param>
        /// <param name="CalBlockID">试块ID</param>
        /// <param name="ProbeID">探头ID</param>
        /// <returns></returns>
        public ReturnDALResult Add_TestTestBlock(string CalBlockID, string ProbeID, string ReportID)
        {
            return dal.Add_TestTestBlock(CalBlockID, ProbeID, ReportID);
        }
        #endregion

        #region 删除已经添加探头
        /// <summary>
        /// 删除已经添加探头
        /// </summary>
        /// <param name="id">已经选择探头标识</param>     
        /// <returns></returns>
        public ReturnDALResult Delete_TestTestBlock(string id)
        {
            return dal.Delete_TestTestBlock(id);
        }
        #endregion
        #endregion

        #region 载入报告

        /// <summary>
        /// 载入报告
        /// </summary>
        /// <param name="report_num"></param>
        /// <param name="id"></param>
        /// <param name="tempPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="tm_id"></param>
        /// <param name="return_url"></param>
        /// <returns></returns>
        public ReturnDALResult Filling_report(string report_num, int id, string outputPath, int tm_id, string return_url)
        {

            #region 报告url获取

            TB_TemplateFile TB_TemplateFile = dal.LoadTemplateFile(tm_id);

            #endregion

            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_TemplateFile.FileUrl)))
            {
                ReturnDALResult ReturnDALResult = new ReturnDALResult();
                ReturnDALResult.Success = 2;
                ReturnDALResult.returncontent = "模板不存在！";
                return ReturnDALResult;
            }
            try
            {
                System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath(TB_TemplateFile.FileUrl), outputPath, true);
            }
            catch (Exception)
            {

                throw;
            }
            //复制模板


            //创建word对象
            Aspose.Words.Document contract_doc = null;
            contract_doc = new Aspose.Words.Document(outputPath);
            Aspose.Words.DocumentBuilder docBuild = new Aspose.Words.DocumentBuilder(contract_doc);
            //Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)contract_doc.GetChild(NodeType.Table, 0, true);
            NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);

            //插入探头表格
            #region 插入探头表格

            //标志——表示是否需要插入探头表格
            int temp_flag = 0;
            try
            {
                if (tm_id == 13)
                {
                    contract_doc.Range.Bookmarks["Probe_num"].Text = "key1010";
                }
                else
                {
                    contract_doc.Range.Bookmarks["id"].Text = "key1010";
                }
            }
            catch
            {
                temp_flag = 1;
            }
            if (temp_flag == 0)
            {
                #region
                #region 数据拼接
                DataTable testData = new DataTable();//总数居table

                //获取测试数据/试验基础信息TB_record_title_info       
                List<TB_NDT_test_probe> probe_library = dal.GetReportProbeLibrary(id);
                DataTable testData_item = ListToDataTable.ListToDataTable_<TB_NDT_test_probe>(probe_library);
                testData.Merge(testData_item);
                #endregion

                //table入口地址
                //行入口地址
                int dataRowIndex = 0;
                int flag_ = 0;
                double height = 0.56;
                int dataTableIndex = 0;
                int dataTable_count = 0;
                #region 获取书签所在行（table的入口）
                foreach (Aspose.Words.Tables.Table table in tables)
                {
                    if (dataRowIndex != 0)
                        break;
                    // Get the index of the table node as contained in the parent node of the table
                    int tableIndex = table.ParentNode.ChildNodes.IndexOf(table);

                    // Iterate through all rows in the table
                    foreach (Row row in table.Rows)
                    {
                        if (dataRowIndex != 0)
                            break;
                        //第几行
                        int rowIndex = table.Rows.IndexOf(row);

                        // Iterate through all cells in the row
                        foreach (Cell cell in row.Cells)
                        {
                            int cellIndex = row.Cells.IndexOf(cell);

                            // Get the plain text content of this cell.
                            string cellText = "";
                            try
                            {
                                cellText = cell.ToString(SaveFormat.Text).Trim();
                            }
                            catch
                            {
                            }



                            if (cellText == "key1010")
                            {
                                if (cellIndex != 0)
                                {
                                    flag_ = cellIndex;

                                }
                                dataRowIndex = rowIndex;

                                dataTableIndex = dataTable_count;
                                break;
                            }

                        }
                    }
                    dataTable_count++;

                }


                #endregion

                #region//获取表格1每列书签名称
                int count1 = 0;
                List<double> widthList = new List<double>();//表格1宽度集合

                List<string> CellVerticalAlignment_1 = new List<string>();//垂直样式
                List<string> ParagraphAlignment_1 = new List<string>();//水平样式
                //单元高度

                //获取表格列数
                //for (var i = 0; i < testData.Columns.Count; i++)
                //{
                //    if (contract_doc.Range.Bookmarks[testData.Columns[i].ColumnName.Trim()] != null)
                //    {
                //        Bookmark mark = contract_doc.Range.Bookmarks[testData.Columns[i].ColumnName.Trim()];
                //        mark.Text = "";
                //        count++;
                //    }
                //}
                List<string> listcolumn = new List<string>();//表格1表格内容集合

                for (var i = flag_; i < 30; i++)
                {
                    try
                    {
                        docBuild.MoveToCell(dataTableIndex, dataRowIndex, i, 0); //移动单元格 
                        if (docBuild.CurrentNode != null)
                        {
                            if (docBuild.CurrentNode.NodeType == NodeType.BookmarkStart)
                            {
                                listcolumn.Add((docBuild.CurrentNode as BookmarkStart).Name);
                            }
                        }
                        else
                            listcolumn.Add("");

                        widthList.Add(docBuild.CellFormat.Width);

                        CellVerticalAlignment_1.Add((docBuild.CellFormat.VerticalAlignment).ToString());
                        ParagraphAlignment_1.Add((docBuild.ParagraphFormat.Alignment).ToString());

                        count1++;
                    }
                    catch
                    {
                        break;
                    }
                }
                if (tm_id == 13)
                {
                    docBuild.MoveToBookmark("Probe_num");//跳到指定书签
                }
                else
                {
                    docBuild.MoveToBookmark("id");//跳到指定书签
                }
                height = docBuild.RowFormat.Height;
                docBuild.MoveToBookmark("table"); //开始添加值 
                int row_count = 1;
                int CC = 0;
                int testData_total_count = 0;
                if (tm_id == 13)
                {
                    if (testData.Rows.Count < 5)
                    {
                        testData_total_count = 4;
                    }
                }
                else
                {
                    if (testData.Rows.Count < 4)
                    {
                        testData_total_count = 3;
                    }
                }

                #endregion

                #region//写入记录 从第2行开始

                int id_no = 2;
                for (var m = 1; m < testData_total_count; m++)
                {
                    CC = 1;
                    row_count = row_count + 1;
                    for (var i = 0; i < listcolumn.Count; i++)
                    {
                        docBuild.InsertCell(); // 添加一个单元格 
                        //docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;
                        //docBuild.CellFormat.Borders.Color = System.Drawing.Color.Black;
                        //设置单元格宽度
                        docBuild.CellFormat.Width = widthList[i];
                        //设置单元格高度
                        docBuild.RowFormat.Height = height;

                        docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;

                        if (i == 0)
                        {
                            docBuild.CellFormat.Borders.Left.LineStyle = LineStyle.None;
                        }
                        if (i == listcolumn.Count - 1)
                        { docBuild.CellFormat.Borders.Right.LineStyle = LineStyle.None; }

                        docBuild.CellFormat.Borders.Top.LineStyle = LineStyle.None;

                        if (row_count == testData_total_count)
                        {
                            docBuild.CellFormat.Borders.Bottom.LineStyle = LineStyle.None;

                        }

                        //docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                        //docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                        //垂直样式
                        switch (CellVerticalAlignment_1[i])
                        {
                            case "Center": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; break;
                            case "Top": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Top; break;
                            case "Bottom": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom; break;

                        }
                        //水平样式
                        switch (ParagraphAlignment_1[i])
                        {
                            case "Center": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center; break;
                            case "ArabicHighKashida": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicHighKashida; break;
                            case "ArabicLowKashida": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicLowKashida; break;
                            case "ArabicMediumKashida":
                                docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicMediumKashida;
                                break;
                            case "Distributed": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Distributed; break;
                            case "Justify": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Justify; break;
                            case "Left": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Left; break;
                            case "Right": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Right; break;
                            case "ThaiDistributed": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ThaiDistributed; break;

                        }

                        docBuild.Font.Size = 9;
                        //是否加粗  
                        docBuild.Bold = false;
                        try
                        {
                            if (testData.Columns.Contains(listcolumn[i].ToString()))
                            {
                                if (tm_id == 5 || tm_id == 10 || tm_id == 11)
                                {
                                    testData.Rows[m][listcolumn[i]].ToString();
                                    if (listcolumn[i].ToString().Trim() == "id")
                                    {
                                        docBuild.Write(id_no.ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                                        id_no++;
                                    }
                                    else
                                    {
                                        docBuild.Write(testData.Rows[m][listcolumn[i]].ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                                    }

                                }
                                else
                                {
                                    if (listcolumn[i].ToString().Trim() != "id")
                                    {
                                        docBuild.Write(testData.Rows[m][listcolumn[i]].ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;

                                    }
                                }

                            }
                        }
                        catch { }


                    }
                    docBuild.EndRow();
                }
                if (CC == 1)
                {
                    docBuild.EndTable();
                }

                contract_doc.Range.Bookmarks["table"].Text = ""; // 清掉标示 
                try
                {
                    if (tm_id == 13)
                    {
                        contract_doc.Range.Bookmarks["Probe_num"].Text = "key1010";
                    }
                    else
                    {
                        contract_doc.Range.Bookmarks["id"].Text = "key1010";
                    }
                }
                catch
                {

                }

                #endregion

                #region 表格第一行记录
                //填写第一行记录5 10 11 需要序号
                if (tm_id == 5 || tm_id == 10 || tm_id == 11)
                {
                    if (testData.Rows.Count > 0)
                    {
                        foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
                        {
                            switch (mark.Name)
                            {
                                case "id": mark.Text = "1"; break;
                                case "Probe_Manufacture": mark.Text = testData.Rows[0]["Probe_Manufacture"].ToString(); break;
                                case "Probe_type": mark.Text = testData.Rows[0]["Probe_type"].ToString(); break;
                                case "Probe_num": mark.Text = testData.Rows[0]["Probe_num"].ToString(); break;
                                case "Coil_Size": mark.Text = testData.Rows[0]["Coil_Size"].ToString(); break;
                                case "Probe_Length": mark.Text = testData.Rows[0]["Probe_Length"].ToString(); break;
                                case "Cable_Length": mark.Text = testData.Rows[0]["Cable_Length"].ToString(); break;
                                case "Chip_size": mark.Text = testData.Rows[0]["Chip_size"].ToString(); break;
                                case "Probe_frequency": mark.Text = testData.Rows[0]["Probe_frequency"].ToString(); break;
                                case "Mode_L": mark.Text = testData.Rows[0]["Mode_L"].ToString(); break;
                                case "Mode_T": mark.Text = testData.Rows[0]["Mode_T"].ToString(); break;
                                case "Angle": mark.Text = testData.Rows[0]["Angle"].ToString(); break;
                                case "Shoe": mark.Text = testData.Rows[0]["Shoe"].ToString(); break;
                                case "Nom_Angle": mark.Text = testData.Rows[0]["Nom_Angle"].ToString(); break;
                            }
                        }
                    }
                }
                else
                {
                    if (testData.Rows.Count > 0)
                    {
                        foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
                        {
                            switch (mark.Name)
                            {
                                case "Probe_num": mark.Text = testData.Rows[0]["Probe_num"].ToString(); break;
                                case "Probe_type": mark.Text = testData.Rows[0]["Probe_type"].ToString(); break;
                                case "Probe_size": mark.Text = testData.Rows[0]["Probe_size"].ToString(); break;
                                case "Probe_frequency": mark.Text = testData.Rows[0]["Probe_frequency"].ToString(); break;
                            }
                        }
                    }
                }

                #endregion

                #endregion
            }
            #endregion

            //插入试块表格
            #region 插入试块表格

            //标志——表示是否需要插入试块表格
            int Block_flag = 0;
            try
            {
                contract_doc.Range.Bookmarks["BlockId"].Text = "key10101";

            }
            catch
            {
                Block_flag = 1;
            }
            if (Block_flag == 0)
            {
                #region 插入数据

                #region 数据拼接
                DataTable testData = new DataTable();//总数居table

                //获取测试数据/试验基础信息TB_record_title_info       
                List<TB_NDT_TestTestBlock> TestTestBlock = dal.GetReportTestTestBlock(id);
                DataTable testData_item = ListToDataTable.ListToDataTable_<TB_NDT_TestTestBlock>(TestTestBlock);

                #region 使字段对应书签
                try
                {
                    testData_item.Columns["ID"].ColumnName = "BlockId";//ID列改为BlockId
                    testData_item.Columns["Angle"].ColumnName = "BlockAngle";//Angle列改为BlockAngle

                }
                catch (Exception)
                {
                  
                }

                #endregion

                testData.Merge(testData_item);
                #endregion

                //table入口地址
                //行入口地址
                int dataRowIndex = 0;
                int flag_ = 0;
                double height = 0.56;
                int dataTableIndex = 0;
                int dataTable_count = 0;
                #region 获取书签所在行（table的入口）
                foreach (Aspose.Words.Tables.Table table in tables)
                {
                    if (dataRowIndex != 0)
                        break;
                    // Get the index of the table node as contained in the parent node of the table
                    int tableIndex = table.ParentNode.ChildNodes.IndexOf(table);

                    // Iterate through all rows in the table
                    foreach (Row row in table.Rows)
                    {
                        if (dataRowIndex != 0)
                            break;
                        //第几行
                        int rowIndex = table.Rows.IndexOf(row);

                        // Iterate through all cells in the row
                        foreach (Cell cell in row.Cells)
                        {
                            int cellIndex = row.Cells.IndexOf(cell);

                            // Get the plain text content of this cell.
                            string cellText = "";
                            try
                            {
                                cellText = cell.ToString(SaveFormat.Text).Trim();
                            }
                            catch
                            {
                            }



                            if (cellText == "key10101")
                            {
                                if (cellIndex != 0)
                                {
                                    flag_ = cellIndex;

                                }
                                dataRowIndex = rowIndex;

                                dataTableIndex = dataTable_count;
                                break;
                            }

                        }
                    }
                    dataTable_count++;

                }


                #endregion

                #region//获取表格1每列书签名称
                int count1 = 0;
                List<double> widthList = new List<double>();//表格1宽度集合

                List<string> CellVerticalAlignment_1 = new List<string>();//垂直样式
                List<string> ParagraphAlignment_1 = new List<string>();//水平样式
                //单元高度

                //获取表格列数
                //for (var i = 0; i < testData.Columns.Count; i++)
                //{
                //    if (contract_doc.Range.Bookmarks[testData.Columns[i].ColumnName.Trim()] != null)
                //    {
                //        Bookmark mark = contract_doc.Range.Bookmarks[testData.Columns[i].ColumnName.Trim()];
                //        mark.Text = "";
                //        count++;
                //    }
                //}
                List<string> listcolumn = new List<string>();//表格1表格内容集合

                for (var i = flag_; i < 30; i++)
                {
                    try
                    {
                        docBuild.MoveToCell(dataTableIndex, dataRowIndex, i, 0); //移动单元格 
                        if (docBuild.CurrentNode != null)
                        {
                            if (docBuild.CurrentNode.NodeType == NodeType.BookmarkStart)
                            {
                                listcolumn.Add((docBuild.CurrentNode as BookmarkStart).Name);
                            }
                        }
                        else
                            listcolumn.Add("");

                        widthList.Add(docBuild.CellFormat.Width);

                        CellVerticalAlignment_1.Add((docBuild.CellFormat.VerticalAlignment).ToString());
                        ParagraphAlignment_1.Add((docBuild.ParagraphFormat.Alignment).ToString());

                        count1++;
                    }
                    catch
                    {
                        break;
                    }
                }
                docBuild.MoveToBookmark("BlockId");//跳到指定书签

                height = docBuild.RowFormat.Height;
                docBuild.MoveToBookmark("BlockTable"); //开始添加值 
                int row_count = 1;
                int CC = 0;
                int testData_total_count = 0;

                if (testData.Rows.Count < 4)
                {
                    testData_total_count = 3;//至少三行
                }

                #endregion

                #region//写入记录 从第2行开始

                int id_no = 2;
                for (var m = 1; m < testData_total_count; m++)
                {
                    CC = 1;
                    row_count = row_count + 1;
                    for (var i = 0; i < listcolumn.Count; i++)
                    {
                        if (listcolumn[i].ToString() == "") 
                        {
                            continue;
                        }
                        docBuild.InsertCell(); // 添加一个单元格 
                        //docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;
                        //docBuild.CellFormat.Borders.Color = System.Drawing.Color.Black;
                        //设置单元格宽度
                        docBuild.CellFormat.Width = widthList[i];
                        //设置单元格高度
                        docBuild.RowFormat.Height = height;

                        docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;

                        if (i == 0)
                        {
                            docBuild.CellFormat.Borders.Left.LineStyle = LineStyle.None;
                        }
                        if (i == listcolumn.Count - 1)
                        { docBuild.CellFormat.Borders.Right.LineStyle = LineStyle.None; }

                        docBuild.CellFormat.Borders.Top.LineStyle = LineStyle.None;

                        if (row_count == testData_total_count)
                        {
                            docBuild.CellFormat.Borders.Bottom.LineStyle = LineStyle.None;

                        }

                        //docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                        //docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                        //垂直样式
                        switch (CellVerticalAlignment_1[i])
                        {
                            case "Center": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; break;
                            case "Top": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Top; break;
                            case "Bottom": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom; break;

                        }
                        //水平样式
                        switch (ParagraphAlignment_1[i])
                        {
                            case "Center": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center; break;
                            case "ArabicHighKashida": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicHighKashida; break;
                            case "ArabicLowKashida": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicLowKashida; break;
                            case "ArabicMediumKashida":
                                docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicMediumKashida;
                                break;
                            case "Distributed": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Distributed; break;
                            case "Justify": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Justify; break;
                            case "Left": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Left; break;
                            case "Right": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Right; break;
                            case "ThaiDistributed": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ThaiDistributed; break;

                        }

                        docBuild.Font.Size = 9;
                        //是否加粗  
                        docBuild.Bold = false;
                        try
                        {
                            if (testData.Columns.Contains(listcolumn[i].ToString()))
                            {
                                testData.Rows[m][listcolumn[i]].ToString();
                                if (listcolumn[i].ToString().Trim() == "BlockId")
                                {
                                    docBuild.Write(id_no.ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                                    id_no++;
                                }
                                else
                                {
                                    docBuild.Write(testData.Rows[m][listcolumn[i]].ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                                }


                            }
                        }
                        catch { }


                    }
                    docBuild.EndRow();
                }
                if (CC == 1)
                {
                    docBuild.EndTable();
                }

                contract_doc.Range.Bookmarks["BlockTable"].Text = ""; // 清掉标示 
                contract_doc.Range.Bookmarks["BlockId"].Text = "";
                //try
                //{
                //    contract_doc.Range.Bookmarks["BlockId"].Text = "key10101";

                //}
                //catch
                //{

                //}

                #endregion

                #region 表格第一行记录
                if (testData.Rows.Count > 0)
                {
                    foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
                    {
                        switch (mark.Name)
                        {
                            case "BlockId": mark.Text = "1"; break;
                            case "CalBlock": mark.Text = testData.Rows[0]["CalBlock"].ToString(); break;
                            case "Reflector": mark.Text = testData.Rows[0]["Reflector"].ToString(); break;
                            case "BlockAngle": mark.Text = testData.Rows[0]["BlockAngle"].ToString(); break;
                            case "TFront": mark.Text = testData.Rows[0]["TFront"].ToString(); break;
                            case "C_S": mark.Text = testData.Rows[0]["C_S"].ToString(); break;
                            case "InstrumentSet": mark.Text = testData.Rows[0]["InstrumentSet"].ToString(); break;
                        }
                    }
                }


                #endregion

                #endregion
            }
            #endregion


            //表头信息
            #region 表头信息
            String report_name = "";
            String clientele_department = "";
            String application_num = "";
            String Project_name = "";
            String Subassembly_name = "";
            String Material = "";
            String Type_ = "";
            String Chamfer_type = "";
            String Drawing_num = "";
            String Procedure_ = "";
            String Inspection_context = "";
            String Inspection_opportunity = "";
            String circulation_NO = "";
            String procedure_NO = "";
            String apparent_condition = "";
            String manufacturing_process = "";
            String Batch_Num = "";
            String Inspection_NO = "";
            String remarks = "";
            String Inspection_date = "";
            String Inspection_result = "";
            String figure = "";
            String Tubes_Size = "";
            String Tubes_num = "";
            String disable_report_num = "";
            String welding_method = "";
            String ModelNum = "";
            String Inspection_personnel = "";
            String Work_instruction = "";
            String heat_treatment = "";
            #endregion

            #region 插入设备 判断报告
            #region 定义
            string equipment_1 = "";
            string equipment_2 = "";
            string equipment_3 = "";
            string equipment_4 = "";
            string equipment_5 = "";
            string equipment_6 = "";
            string equipment_7 = "";
            string equipment_8 = "";
            string equipment_9 = "";
            string equipment_10 = "";
            string equipment_11 = "";
            string equipment_12 = "";
            string equipment_13 = "";
            string equipment_14 = "";
            string equipment_15 = "";
            string equipment_16 = "";
            string equipment_17 = "";
            string equipment_18 = "";
            string equipment_19 = "";
            string equipment_20 = "";
            string equipment_21 = "";
            string equipment_22 = "";
            string equipment_23 = "";
            string equipment_24 = "";
            string equipment_25 = "";
            string equipment_26 = "";
            string equipment_27 = "";
            string equipment_28 = "";
            string equipment_29 = "";
            string equipment_30 = "";
            string equipment_31 = "";
            string equipment_32 = "";
            string equipment_33 = "";
            string equipment_34 = "";
            string equipment_35 = "";
            string equipment_36 = "";
            string equipment_37 = "";
            string equipment_38 = "";
            string equipment_39 = "";
            string equipment_40 = "";
            string equipment_41 = "";
            string equipment_42 = "";
            string equipment_43 = "";
            string equipment_44 = "";
            string equipment_45 = "";
            string equipment_46 = "";
            string equipment_47 = "";
            string equipment_48 = "";
            string equipment_49 = "";
            string equipment_50 = "";
            string equipment_51 = "";
            string equipment_52 = "";
            string equipment_53 = "";
            string equipment_54 = "";
            string equipment_55 = "";
            string equipment_56 = "";
            string equipment_57 = "";
            string equipment_58 = "";
            string equipment_59 = "";
            #endregion
            List<TB_NDT_test_equipment> ReportEquipmentInfo = dal.GetReportEquipmentInfo(id);//报告使用设备
            if (tm_id == 4)
            {
                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)
                    {//扫查器
                        equipment_4 = item.equipment_Type.ToString().Trim();
                        equipment_5 = item.equipment_num.Trim();
                        equipment_6 = item.Remarks.Trim();

                    } if (i == 1)//数据采集系统
                    {

                        equipment_1 = item.equipment_Type.Trim();
                        equipment_2 = item.equipment_num.Trim();
                        equipment_3 = item.Remarks.Trim();
                    } if (i == 2)//运动控制器
                    {

                        equipment_7 = item.equipment_Type.Trim();
                        equipment_8 = item.equipment_num.Trim();
                        equipment_9 = item.Remarks.Trim();
                    }
                    i++;
                }


            } if (tm_id == 5)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//标定样管
                    {

                        equipment_13 = item.equipment_Type.ToString().Trim();
                        equipment_14 = item.equipment_num.Trim();

                    } if (i == 1)//涡流仪
                    {


                        equipment_10 = item.Manufacture.Trim();
                        equipment_11 = item.equipment_Type.ToString().Trim();
                        equipment_12 = item.equipment_num.Trim();
                    }
                    i++;
                }

            } if (tm_id == 6)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//氦浓度仪
                    {

                        equipment_19 = item.equipment_name.Trim();
                        equipment_20 = item.equipment_num.Trim();

                    } if (i == 1)//检测仪器
                    {


                        equipment_15 = item.equipment_name.Trim();
                        equipment_16 = item.equipment_num.Trim();
                    }
                    if (i == 2)//湿度计
                    {

                        equipment_21 = item.equipment_name.Trim();
                        equipment_22 = item.equipment_num.Trim();
                    }
                    if (i == 3)//温度计
                    {


                        equipment_23 = item.equipment_name.Trim();
                        equipment_24 = item.equipment_num.Trim();
                    }
                    if (i == 4)//压力表
                    {

                        equipment_25 = item.equipment_name.Trim();
                        equipment_26 = item.equipment_num.Trim();
                    }
                    if (i == 5)//真空计
                    {

                        equipment_17 = item.equipment_name.Trim();
                        equipment_18 = item.equipment_num.Trim();
                    }
                    i++;
                }

            } if (tm_id == 7)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//测温仪
                    {

                        equipment_30 = item.equipment_Type.Trim();
                        equipment_31 = item.equipment_num.Trim();

                    } if (i == 1)//使用设备
                    {

                        equipment_27 = item.Manufacture.Trim();
                        equipment_28 = item.equipment_Type.Trim();
                        equipment_29 = item.equipment_num.Trim();
                    } if (i == 2)//照度计
                    {

                        equipment_32 = item.equipment_Type.Trim();
                        equipment_33 = item.equipment_num.Trim();
                    }
                    i++;
                }

            } if (tm_id == 8)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//测温仪
                    {

                        equipment_37 = item.equipment_Type.Trim();
                        equipment_38 = item.equipment_num.Trim();
                    } if (i == 1)//黑光强度剂
                    {

                        equipment_39 = item.equipment_Type.Trim();
                        equipment_40 = item.equipment_num.Trim();
                    } if (i == 2)//使用设备
                    {

                        equipment_34 = item.Manufacture.Trim();
                        equipment_35 = item.equipment_Type.Trim();
                        equipment_36 = item.equipment_num.Trim();
                    }
                    i++;
                }

            } if (tm_id == 10)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//仪器
                    {

                        equipment_41 = item.Manufacture.Trim();
                        equipment_42 = item.equipment_Type.Trim();
                        equipment_43 = item.equipment_num.Trim();
                    }
                    i++;
                }

            } if (tm_id == 11)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//仪器
                    {

                        equipment_44 = item.Manufacture.Trim();
                        equipment_45 = item.equipment_Type.Trim();
                        equipment_46 = item.equipment_num.Trim();
                    }
                    i++;
                }

            } if (tm_id == 13)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//超声数据采集系统
                    {

                        equipment_47 = item.equipment_Type.Trim();
                        equipment_48 = item.equipment_num.Trim();
                        equipment_49 = item.Remarks.Trim();

                    } if (i == 1)//扫查器
                    {

                        equipment_50 = item.equipment_Type.Trim();
                        equipment_51 = item.equipment_num.Trim();
                        equipment_52 = item.Remarks.Trim();
                    } if (i == 2)//运动控制器
                    {
                        equipment_53 = item.equipment_Type.Trim();
                        equipment_54 = item.equipment_num.Trim();
                        equipment_55 = item.Remarks.Trim();
                    }
                    i++;
                }

            } if (tm_id == 26)
            {

                int i = 0;
                foreach (var item in ReportEquipmentInfo)
                {
                    if (i == 0)//仪器
                    {

                        equipment_56 = item.equipment_Type.Trim();
                        equipment_57 = item.equipment_num.Trim();

                    } if (i == 1)//照度计/黑光强度计
                    {

                        equipment_58 = item.equipment_Type.Trim();
                        equipment_59 = item.equipment_num.Trim();
                    }
                    i++;
                }

            }
            #endregion

            try
            {
                //表头信息
                #region

                TB_NDT_report_title model = dal.Getreport_title(id);


                report_name = model.report_name.Trim();
                //report_num = rt_dr["report_num"].ToString().Trim();
                clientele_department = model.clientele_department.Trim();
                application_num = model.application_num.Trim();
                Project_name = model.Project_name.Trim();
                Subassembly_name = model.Subassembly_name.Trim();
                Material = model.Material.Trim();
                Type_ = model.Type_.Trim();
                Chamfer_type = model.Chamfer_type.Trim();
                Drawing_num = model.Drawing_num.Trim();
                Procedure_ = model.Procedure_.Trim();
                Inspection_context = model.Inspection_context.Trim();
                Inspection_opportunity = model.Inspection_opportunity.Trim();
                circulation_NO = model.circulation_NO.Trim();
                procedure_NO = model.procedure_NO.Trim();
                apparent_condition = model.apparent_condition.Trim();
                manufacturing_process = model.manufacturing_process.Trim();
                Batch_Num = model.Batch_Num.Trim();
                Inspection_NO = model.Inspection_NO.Trim();
                remarks = model.remarks.Trim();
                Inspection_date = model.Inspection_date.ToString().Split(' ')[0];
                Inspection_result = model.Inspection_result.Trim();
                figure = model.figure.ToString();
                Tubes_Size = model.Tubes_Size.Trim();
                Tubes_num = model.Tubes_num.Trim();
                disable_report_num = model.disable_report_num.Trim();
                welding_method = model.welding_method.Trim();
                ModelNum = model.ModelNum.Trim();
                //Inspection_personnel = rt_dr["Inspection_personnel"].ToString().Trim();
                Work_instruction = model.Work_instruction.Trim();
                heat_treatment = model.heat_treatment.Trim();



                if (!string.IsNullOrEmpty(Inspection_date))
                {
                    DateTime Inspection_date1 = Convert.ToDateTime(Inspection_date);
                    Inspection_date = string.Format("{0:yyyy-MM-dd}", Inspection_date1);
                }
                #endregion

            }
            catch
            {
            }

            TB_NDT_test_probereport_data ProbeDataModel = dal.Getreport_probe(id);//测试数据信息
            //插入书签
            #region 插入书签
            foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
            {
                switch (mark.Name)
                {
                    //表头信息
                    case "report_num": mark.Text = report_num; break;
                    case "clientele_department": mark.Text = clientele_department; break;
                    case "Work_instruction": mark.Text = Work_instruction; break;
                    case "heat_treatment": mark.Text = heat_treatment; break;
                    case "application_num": mark.Text = application_num; break;
                    case "Project_name": mark.Text = Project_name; break;
                    case "Subassembly_name": mark.Text = Subassembly_name; break;
                    case "Material": mark.Text = Material; break;
                    case "Type_": mark.Text = Type_; break;
                    case "Chamfer_type": mark.Text = Chamfer_type; break;
                    case "Drawing_num": mark.Text = Drawing_num; break;
                    case "Procedure_": mark.Text = Procedure_; break;
                    case "Inspection_context": mark.Text = Inspection_context; break;
                    case "Inspection_opportunity": mark.Text = Inspection_opportunity; break;
                    case "circulation_NO": mark.Text = circulation_NO; break;
                    case "procedure_NO": mark.Text = procedure_NO; break;
                    case "apparent_condition": mark.Text = apparent_condition; break;
                    case "manufacturing_process": mark.Text = manufacturing_process; break;
                    case "Batch_Num": mark.Text = Batch_Num; break;
                    case "Inspection_NO": mark.Text = Inspection_NO; break;
                    case "remarks": mark.Text = remarks; break;
                    case "Inspection_date": mark.Text = Inspection_date.ToString().Split(' ')[0]; break;
                    case "Inspection_personnel": mark.Text = Inspection_personnel; break;
                    case "Tubes_num": mark.Text = Tubes_num; break;
                    case "Tubes_Size": mark.Text = Tubes_Size; break;
                    case "disable_report_num": mark.Text = disable_report_num; break;
                    case "welding_method": mark.Text = welding_method; break;
                    case "ModelNum": mark.Text = ModelNum; break;
                    case "Inspection_result_yes": if (Inspection_result.Trim() == "0" || Inspection_result.Trim() == "False")
                        {
                            insert_char163(docBuild, "Inspection_result_yes");
                        }
                        else if (Inspection_result.Trim() == "1" || Inspection_result.Trim() == "True")
                        {
                            insert_char82(docBuild, "Inspection_result_yes");
                        } break;
                    case "Inspection_result_no": if (Inspection_result.Trim() == "0" || Inspection_result.Trim() == "False")
                        {
                            insert_char82(docBuild, "Inspection_result_no");
                        }
                        else if (Inspection_result.Trim() == "1" || Inspection_result.Trim() == "True")
                        {
                            insert_char163(docBuild, "Inspection_result_no");
                        } break;
                    case "figure_yes": if (figure.Trim() == "0" || figure.Trim() == "False")
                        {
                            insert_char163(docBuild, "figure_yes");
                        }
                        else if (figure.Trim() == "1" || figure.Trim() == "True")
                        {
                            insert_char82(docBuild, "figure_yes");
                        } break;
                    case "figure_no": if (figure.Trim() == "0" || figure.Trim() == "False")
                        {
                            insert_char82(docBuild, "figure_no");
                        }
                        else if (figure.Trim() == "1" || figure.Trim() == "True")
                        {
                            insert_char163(docBuild, "figure_no");
                        } break;


                    //表头重复信息 ——水压测试报告
                    case "report_num2": mark.Text = report_num; break;
                    case "report_num3": mark.Text = report_num; break;
                    case "report_num4": mark.Text = report_num; break;
                    case "clientele_department2": mark.Text = clientele_department; break;
                    case "application_num2": mark.Text = application_num; break;
                    case "Project_name2": mark.Text = Project_name; break;
                    case "Subassembly_name2": mark.Text = Subassembly_name; break;
                    case "Material2": mark.Text = Material; break;
                    case "Type_2": mark.Text = Type_; break;
                    case "Chamfer_type2": mark.Text = Chamfer_type; break;
                    case "Drawing_num2": mark.Text = Drawing_num; break;
                    case "Procedure_2": mark.Text = Procedure_; break;
                    case "Inspection_context2": mark.Text = Inspection_context; break;
                    case "Inspection_opportunity2": mark.Text = Inspection_opportunity; break;
                    case "circulation_NO2": mark.Text = circulation_NO; break;
                    case "procedure_NO2": mark.Text = procedure_NO; break;
                    case "apparent_condition2": mark.Text = apparent_condition; break;
                    case "manufacturing_process2": mark.Text = manufacturing_process; break;
                    case "Batch_Num2": mark.Text = Batch_Num; break;
                    case "Inspection_NO2": mark.Text = Inspection_NO; break;
                    case "remarks2": mark.Text = remarks; break;
                    case "Inspection_date2": mark.Text = Inspection_date.ToString().Split(' ')[0]; break;
                    case "Inspection_personnel2": mark.Text = Inspection_personnel; break;
                    case "Tubes_num2": mark.Text = Tubes_num; break;
                    case "Tubes_Size2": mark.Text = Tubes_Size; break;
                    case "disable_report_num2": mark.Text = disable_report_num; break;
                    case "welding_method2": mark.Text = welding_method; break;
                    case "ModelNum2": mark.Text = ModelNum; break;


                    //设备信息
                    case "equipment_1": mark.Text = equipment_1; break;
                    case "equipment_2": mark.Text = equipment_2; break;
                    case "equipment_3": mark.Text = equipment_3; break;
                    case "equipment_4": mark.Text = equipment_4; break;
                    case "equipment_5": mark.Text = equipment_5; break;
                    case "equipment_6": mark.Text = equipment_6; break;
                    case "equipment_7": mark.Text = equipment_7; break;
                    case "equipment_8": mark.Text = equipment_8; break;
                    case "equipment_9": mark.Text = equipment_9; break;
                    case "equipment_10": mark.Text = equipment_10; break;
                    case "equipment_11": mark.Text = equipment_11; break;
                    case "equipment_12": mark.Text = equipment_12; break;
                    case "equipment_13": mark.Text = equipment_13; break;
                    case "equipment_14": mark.Text = equipment_14; break;
                    case "equipment_15": mark.Text = equipment_15; break;
                    case "equipment_16": mark.Text = equipment_16; break;
                    case "equipment_17": mark.Text = equipment_17; break;
                    case "equipment_18": mark.Text = equipment_18; break;
                    case "equipment_19": mark.Text = equipment_19; break;
                    case "equipment_20": mark.Text = equipment_20; break;
                    case "equipment_21": mark.Text = equipment_21; break;
                    case "equipment_22": mark.Text = equipment_22; break;
                    case "equipment_23": mark.Text = equipment_23; break;
                    case "equipment_24": mark.Text = equipment_24; break;
                    case "equipment_25": mark.Text = equipment_25; break;
                    case "equipment_26": mark.Text = equipment_26; break;
                    case "equipment_27": mark.Text = equipment_27; break;
                    case "equipment_28": mark.Text = equipment_28; break;
                    case "equipment_29": mark.Text = equipment_29; break;
                    case "equipment_30": mark.Text = equipment_30; break;
                    case "equipment_31": mark.Text = equipment_31; break;
                    case "equipment_32": mark.Text = equipment_32; break;
                    case "equipment_33": mark.Text = equipment_33; break;
                    case "equipment_34": mark.Text = equipment_34; break;
                    case "equipment_35": mark.Text = equipment_35; break;
                    case "equipment_36": mark.Text = equipment_36; break;
                    case "equipment_37": mark.Text = equipment_37; break;
                    case "equipment_38": mark.Text = equipment_38; break;
                    case "equipment_39": mark.Text = equipment_39; break;
                    case "equipment_40": mark.Text = equipment_40; break;
                    case "equipment_41": mark.Text = equipment_41; break;
                    case "equipment_42": mark.Text = equipment_42; break;
                    case "equipment_43": mark.Text = equipment_43; break;
                    case "equipment_44": mark.Text = equipment_44; break;
                    case "equipment_45": mark.Text = equipment_45; break;
                    case "equipment_46": mark.Text = equipment_46; break;
                    case "equipment_47": mark.Text = equipment_47; break;
                    case "equipment_48": mark.Text = equipment_48; break;
                    case "equipment_49": mark.Text = equipment_49; break;
                    case "equipment_50": mark.Text = equipment_50; break;
                    case "equipment_51": mark.Text = equipment_51; break;
                    case "equipment_52": mark.Text = equipment_52; break;
                    case "equipment_53": mark.Text = equipment_53; break;
                    case "equipment_54": mark.Text = equipment_54; break;
                    case "equipment_55": mark.Text = equipment_55; break;
                    case "equipment_56": mark.Text = equipment_56; break;
                    case "equipment_57": mark.Text = equipment_57; break;
                    case "equipment_58": mark.Text = equipment_58; break;
                    case "equipment_59": mark.Text = equipment_59; break;


                    //测试数据信息
                    case "Data1": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data1); break;
                    case "Data2": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data2); break;
                    case "Data3": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data3); break;
                    case "Data4": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data4); break;
                    case "Data5": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data5); break;
                    case "Data6": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data6); break;
                    case "Data7": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data7); break;
                    case "Data8": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data8); break;
                    case "Data9": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data9); break;
                    case "Data10": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data10); break;
                    case "Data11": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data11); break;
                    case "Data12": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data12); break;
                    case "Data13": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data13); break;
                    case "Data14": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data14); break;
                    case "Data15": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data15); break;
                    case "Data16": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data16); break;
                    case "Data17": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data17); break;
                    case "Data18": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data18); break;
                    case "Data19": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data19); break;
                    case "Data20": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data20); break;
                    case "Data21": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data21); break;
                    case "Data22": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data22); break;
                    case "Data23": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data23); break;
                    case "Data24": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data24); break;
                    case "Data25": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data25); break;
                    case "Data26": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data26); break;
                    case "Data27": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data27); break;
                    case "Data28": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data28); break;
                    case "Data29": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data29); break;
                    case "Data30": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data30); break;
                    case "Data31": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data31); break;
                    case "Data32": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data32); break;
                    case "Data33": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data33); break;
                    case "Data34": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data34); break;
                    case "Data35": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data35); break;
                    case "Data36": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data36); break;
                    case "Data37": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data37); break;
                    case "Data38": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data38); break;
                    case "Data39": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data39); break;
                    case "Data40": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data40); break;
                    case "Data41": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data41); break;
                    case "Data42": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data42); break;
                    case "Data43": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data43); break;
                    case "Data44": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data44); break;
                    case "Data45": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data45); break;
                    case "Data46": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data46); break;
                    case "Data47": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data47); break;
                    case "Data48": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data48); break;
                    case "Data49": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data49); break;
                    case "Data50": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data50); break;
                    case "Data51": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data51); break;
                    case "Data52": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data52); break;
                    case "Data53": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data53); break;
                    case "Data54": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data54); break;
                    case "Data55": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data55); break;
                    case "Data56": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data56); break;
                    case "Data57": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data57); break;
                    case "Data58": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data58); break;
                    case "Data59": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data59); break;
                    case "Data60": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data60); break;
                    case "Data61": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data61); break;
                    case "Data62": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data62); break;
                    case "Data63": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data63); break;
                    case "Data64": if (ProbeDataModel != null) insert_char82_163(mark, docBuild, mark.Name, ProbeDataModel.Data64); break;

                }
                //else if (mark.Name == "customer_name")
                //    mark.Text = dr["customer_name"].ToString();
            }
            #endregion

            contract_doc.Save(outputPath, Aspose.Words.SaveFormat.Doc);


            //保存证书路径到数据库      
            return dal.SaveReportUrl(id, return_url);


        }


        #region 判断是否为打勾书签
        /// <summary>
        ///   判断是否为打勾书签
        /// </summary>
        /// <param name="mark">书签对象</param>
        /// <param name="docBuild">word文件builder</param>
        /// <param name="mark_Name">书签名字</param>
        /// <param name="insertData">参入文件</param>
        public void insert_char82_163(Aspose.Words.Bookmark mark, Aspose.Words.DocumentBuilder docBuild, string mark_Name, string insertData)
        {
            if (insertData != null)
            {
                if (insertData == "1,flag")//如果为勾选
                {
                    insert_char82(docBuild, mark_Name);
                }
                else if (insertData == "flag")//如果不为勾选
                {
                    insert_char163(docBuild, mark_Name);
                }
                else { mark.Text = insertData; }//为文字输入框
            }
            else
            {
                mark.Text = "";
            }
        }

        #endregion

        #region 打勾
        /// <summary>
        ///  打勾
        /// </summary>
        /// <param name="docBuild">word文件builder</param>
        /// <param name="mark_Name">书签名字</param>
        public void insert_char82(Aspose.Words.DocumentBuilder docBuild, string mark_Name)
        {


            docBuild.MoveToBookmark(mark_Name);
            docBuild.Font.Name = "Wingdings 2";
            docBuild.Font.Size = 10.0;
            docBuild.Write(char.ConvertFromUtf32(162));

        }

        #endregion

        #region 不打勾

        /// <summary>
        /// 不打勾
        /// </summary>
        /// <param name="docBuild">word文件builder</param>
        /// <param name="mark_Name">书签名字</param>
        public void insert_char163(Aspose.Words.DocumentBuilder docBuild, string mark_Name)
        {


            docBuild.MoveToBookmark(mark_Name);
            docBuild.Font.Name = "Wingdings 2";
            docBuild.Font.Size = 10.0;
            docBuild.Write(char.ConvertFromUtf32(163));

        }
        #endregion

        #endregion

        #region 表头信息
        public TB_NDT_report_title Getreport_title(int id)
        {

            return dal.Getreport_title(id);

        }
        #endregion

        #region 附件

        #region 获取附件列表
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="report_id">报告id</param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_accessory> GetReportAccessoryListBLL(TPageModel PageModel, int report_id, out int totalRecord)
        {

            return dal.GetReportAccessoryList(PageModel, report_id, out totalRecord);
        }
        #endregion

        #region 新增附件
        /// <summary>
        /// 新增文件
        /// </summary>
        /// <param name="files"></param>
        /// <param name="model"></param>
        /// <param name="MapPath">文件绝对路径</param>
        /// <returns></returns>
        public ReturnDALResult AddFileManagement(string report_num, Guid LogPersonnel, HttpFileCollection files, TB_NDT_report_accessory model, string Pathurl)
        {
            ReturnDALResult ReturnResult = new ReturnDALResult();
            if (files != null && files.Count > 0)
            {
                string fileName = Path.GetFileName(files[0].FileName);          //文件名称
                string fileExtension = Path.GetExtension(fileName).ToLower();  //文件扩展名
                model.accessory_format = fileExtension;
                model.accessory_url = model.accessory_url + fileExtension;
                if (fileExtension != ".pdf" && fileExtension != ".png" && fileExtension != ".jpeg" && fileExtension != ".jpg" && fileExtension != ".zip")
                {
                    ReturnResult.returncontent = "格式错误文件格式错误，支持.pdf，.png，.jpg，.jpeg，.zip";
                    ReturnResult.Success = 0;
                    return ReturnResult;

                }
                //判断路径是否存在
                if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(Pathurl)))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(Pathurl));
                }
                files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(model.accessory_url));//保存路径+文件名+格式
                //修改数据库保存路径
                return dal.SaveReportAccessory(report_num, LogPersonnel, model);
            }
            else
            {
                ReturnResult.Success = 0;
                ReturnResult.returncontent = "文件为空";
                return ReturnResult;
            }

        }
        #endregion;

        #region 删除报告附件
        /// <summary>
        /// 删除报告附件
        /// </summary>
        /// <param name="AccessoryID">附件ID</param>     
        /// <returns></returns>
        public ReturnDALResult DelAccessoryBLL(int AccessoryID, string report_num, Guid LogPersonnel)
        {


            ReturnDALResult model = dal.DelAccessory(AccessoryID, report_num, LogPersonnel);
            //删除文件
            if (model.Success == 1)
            {
                if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(model.returncontent)))
                {
                    File.Delete(System.Web.HttpContext.Current.Server.MapPath(model.returncontent));
                }
            }
            return model;

        }
        #endregion

        #endregion

        #region 提交审核报告
        /// <summary>
        /// 提交审核报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="login_user"></param>
        /// <param name="new_report_url"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitEditReport(TB_NDT_report_title model, Guid LogPersonnel, string login_user, string new_report_url)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();


            #region 判断签名是否存在，签名不存在则返回信息,签名存在则写入签名

            TB_user_info TB_user_info = dal.getDetection(login_user);

            if (TB_user_info != null)
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
                ReturnDALResult.returncontent = "用户不存在";
                return ReturnDALResult;
            }


            #endregion

            //获取报告信息
            TB_NDT_report_title TB_NDT_report_title = dal.getNDTReportInfo(model.id);

            //判断报告是否存在
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url)))
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "报告文件不存在！";
                return ReturnDALResult;

            }

            model.report_num = TB_NDT_report_title.report_num;
            model.report_url = TB_NDT_report_title.report_url;


            #region 添加报告文档版本记录信息

            TB_NDT_RevisionsRecord TB_NDT_RevisionsRecord = new TB_NDT_RevisionsRecord();

            TB_NDT_RevisionsRecord.add_date = DateTime.Now;
            TB_NDT_RevisionsRecord.addpersonnel = login_user;
            TB_NDT_RevisionsRecord.report_id = model.id;
            TB_NDT_RevisionsRecord.report_url = new_report_url;
            TB_NDT_RevisionsRecord.ReturnNode = (int)NDT_RevisionsRecordEnum.EditSubmit;

            #endregion

            #region 添加报告流程记录

            TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
            TB_ProcessRecord.ReportID = model.id;
            TB_ProcessRecord.Operator = login_user;
            TB_ProcessRecord.OperateDate = DateTime.Now;
            TB_ProcessRecord.NodeResult = "pass";
            TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.EditToReview;


            #endregion

            //报告审核
            ReturnDALResult SubmitEditReport = dal.SubmitEditReport(model, LogPersonnel, login_user, new_report_url, TB_NDT_RevisionsRecord, TB_ProcessRecord, (int)LosslessReportStatusEnum.Edit);

            //如果执行不成功，则直接返回信息
            if (SubmitEditReport.Success != 1)
            {
                return SubmitEditReport;
            }

            #region 获取报告信息，插入签名

            //获取报告路径
            Document first_doc = new Document(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url));
            DocumentBuilder first_builder = new DocumentBuilder(first_doc);

            #region 插入签名

            //写入试验人员签名                    
            try
            {
                first_doc.Range.Bookmarks["Inspection_personnel"].Text = "";
                first_builder.MoveToBookmark("Inspection_personnel");//跳到指定书签
                double width = 50, height = 20;
                Aspose.Words.Drawing.Shape shape = first_builder.InsertImage(System.Web.HttpContext.Current.Server.MapPath(TB_user_info.Signature.ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                shape.Width = width;
                shape.Height = height;
                // shape.VerticalAlignment = VerticalAlignment.Center;
                first_builder.InsertNode(shape);
            }
            catch (Exception ex)
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "插入签名失败！";
                return ReturnDALResult;
            }
            //写入检验级别
            try
            {
                first_doc.Range.Bookmarks["level_Inspection"].Text = model.level_Inspection;//审核级别
            }
            catch (Exception ex)
            {
            }

            //写入试验时间
            try
            {
                first_doc.Range.Bookmarks["Inspection_personnel_date"].Text = model.Inspection_personnel_date.ToString().Split(' ')[0];//检验人签字时间
            }
            catch (Exception ex)
            {
            }

            #endregion

            first_doc.Save(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url), Aspose.Words.SaveFormat.Doc);//提交到审核的报告
            first_doc.Save(System.Web.HttpContext.Current.Server.MapPath(new_report_url), Aspose.Words.SaveFormat.Doc);//记录文档（存在历史记录中）

            #endregion

            return SubmitEditReport;

        }

        #endregion

        #region 查看退回原因
        /// <summary>
        /// 查看退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_error_log> LoadErrorInfo(TPageModel PageModel, out int totalRecord)
        {
            return dal.LoadErrorInfo(PageModel, out totalRecord);
        }

        #endregion

        #region 查看修改记录
        /// <summary>
        /// 查看修改记录
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_RevisionsRecord> ReadRecord(TPageModel PageModel, out int totalRecord)
        {
            return dal.ReadRecord(PageModel, out totalRecord);
        }

        #endregion

        #region 获取模板信息
        public TB_TemplateFile LoadTemplateFile(int id)
        {

            return dal.LoadTemplateFile(id);
        }

        #endregion

        #region 将报告状态更改成已开始
        public ReturnDALResult ReportCondition(TB_NDT_report_title model)
        {

            return dal.ReportCondition(model);
        }

        #endregion

        #region 编制人员自己退回未开始审核报告
        /// <summary>
        /// 编制人员自己退回未开始审核报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult TakeBackEditReport(TB_NDT_report_title model, Guid LogPersonnel, TB_ProcessRecord TB_ProcessRecord)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();

            #region 获取报告信息

            TB_NDT_report_title TB_NDT_report_title = dal.getNDTReportInfo(model.id);

            //判断报告是否存在
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(TB_NDT_report_title.report_url)))
            {
                ReturnDALResult.Success = 0;
                ReturnDALResult.returncontent = "报告文件不存在！";
                return ReturnDALResult;

            }

            model.report_num = TB_NDT_report_title.report_num;
            #endregion

            ReturnDALResult ReturnDALResult2 = dal.TakeBackEditReport(model, LogPersonnel, TB_ProcessRecord);

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


    }
}
