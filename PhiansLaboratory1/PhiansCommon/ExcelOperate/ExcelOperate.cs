using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Excel;
using Windows.Excel.Rendering;
using System.Data;
using Windows.Excel.Drawing;

namespace PhiansCommon.ExcelOperate
{
   public class ExcelOperate
   {
       #region 保存工作簿成图片Jpeg
       /// <summary>
       /// /
       /// </summary>
       /// <param name="NewWorkbook">工作簿对象</param>
       /// <param name="targetPath">保存目标路径</param>
       /// <param name="WorksheetsIndex">工作簿sheet入口</param>
       public static void SavesheetToimage(Workbook NewWorkbook,  int WorksheetsIndex,string targetPath)
       {

           Worksheet Newworksheet = NewWorkbook.Worksheets[WorksheetsIndex];
           ImageOrPrintOptions imgOptions = new ImageOrPrintOptions();
           //保存图片格式
           imgOptions.ImageFormat = ImageFormat.Jpeg;
           Newworksheet.PageSetup.LeftMargin = 0;
           Newworksheet.PageSetup.RightMargin = 0;
           Newworksheet.PageSetup.BottomMargin = 0;
           Newworksheet.PageSetup.TopMargin = 0;
           imgOptions.OnePagePerSheet = true;
           imgOptions.PrintingPage = PrintingPageType.IgnoreBlank;
           SheetRender NewSheetRender = new SheetRender(Newworksheet, imgOptions);
           Image imgTemp = NewSheetRender.ToImage(0);
           //保存图片路径
           imgTemp.Save(targetPath);
       }
       #endregion

       #region 保存工作簿图表成图片Jpeg
       /// <summary>
       /// 
       /// </summary>
       /// <param name="NewWorkbook">工作簿对象</param>
       /// <param name="WorksheetsIndex">工作簿sheet入口</param>
       /// <param name="targetPath">保存目标路径</param>
       public static void SaveChartToimage(Workbook NewWorkbook,int WorksheetsIndex,string targetPath) {
           Windows.Excel.Charts.Chart NewChart = NewWorkbook.Worksheets[WorksheetsIndex].Charts[0];
           NewChart.ToImage(targetPath, System.Drawing.Imaging.ImageFormat.Jpeg);
       }
       #endregion


       #region 保存工作簿图形对象为图片Jpeg
       /// <summary>
       /// 
       /// </summary>
       /// <param name="NewWorkbook">工作簿对象</param>
       /// <param name="WorksheetsIndex">工作簿sheet入口</param>
       /// <param name="targetPath">保存目标路径</param>
       public static void Save(Workbook NewWorkbook, int WorksheetsIndex, string targetPath)
       {


           Windows.Excel.Drawing.Picture picd = NewWorkbook.Worksheets[WorksheetsIndex].Pictures[0];
           ImageOrPrintOptions imgOptions = new ImageOrPrintOptions();
           //保存图片格式
           imgOptions.ImageFormat = ImageFormat.Jpeg;
           //转成图片对象
           Image imgTemp = picd.ToImage(imgOptions);
           imgTemp.Save(targetPath);
       }
       #endregion

 

       #region 读取工作簿sheet转成DataTable
       /// <summary>
       /// /
       /// </summary>
       /// <param name="NewWorkbook">工作簿对象</param>
       /// <param name="WorksheetsIndex">工作簿sheet入口</param>
       /// <param name="bolTitle">是否第一行作为列名</param>
       /// <returns></returns>
       public static DataTable WorksheetToDataTable(Workbook NewWorkbook, int WorksheetsIndex,bool bolTitle) 
       {
           Worksheet NewWorksheet = NewWorkbook.Worksheets[WorksheetsIndex];
           DataTable NewDataTable = NewWorksheet.Cells.ExportDataTableAsString(2, 0, NewWorksheet.Cells.MaxDataRow + 1,NewWorksheet.Cells.MaxDataColumn + 1, bolTitle);
           return NewDataTable;

       }
       #endregion
       public static void WorksheetToDataTable(Workbook NewWorkbook, int WorksheetsIndex, string content)
       {
           Worksheet NewWorksheet = NewWorkbook.Worksheets[WorksheetsIndex];
           Windows.Excel.Drawing.Shape wordart = NewWorkbook.Worksheets[0].Shapes.AddTextEffect(MsoPresetTextEffect.TextEffect1, content, "宋体", 50, false, true, 1, 0, 1, 0, 130, 800);
           //Get the fill format of the word art
           MsoFillFormat wordArtFormat = wordart.FillFormat;
           //设置颜色
           wordArtFormat.ForeColor = System.Drawing.Color.Red;
           // 设置透明度
           wordArtFormat.Transparency = 0.5;
           //Make the line invisible
           MsoLineFormat lineFormat = wordart.LineFormat;

           lineFormat.IsVisible = false;

       }

       /// <summary>
       /// WorksheetToDataTable
       /// </summary>
       /// <param name="NewWorkbook">工作簿对象</param>
       /// <param name="WorksheetsIndex">工作簿sheet入口</param>
       /// <param name="RowsIndex">行入口</param>
       /// <param name="ColumnIndex">列人口</param>
       /// <param name="bolTitle">是否第一行设置为标题</param>
       /// <returns></returns>
       public static DataTable WorksheetToDataTable(Workbook NewWorkbook, int WorksheetsIndex, int RowsIndex, int ColumnIndex, bool bolTitle)
       {
           Worksheet NewWorksheet = NewWorkbook.Worksheets[WorksheetsIndex];
           DataTable NewDataTable = NewWorksheet.Cells.ExportDataTableAsString(RowsIndex, ColumnIndex, NewWorksheet.Cells.MaxDataRow + 1 - RowsIndex, NewWorksheet.Cells.MaxDataColumn + 1, bolTitle);
           return NewDataTable;

       }

       /// <summary>
       /// WorksheetToDataTable
       /// </summary>
       /// <param name="NewWorkbook">工作簿对象</param>
       /// <param name="WorksheetsIndex">工作簿sheet入口</param>
       /// <param name="RowsIndex">行入口</param>
       /// <param name="ColumnIndex">列人口</param>
       /// <param name="bolTitle">是否第一行设置为标题</param>
       /// <returns></returns>
       public static DataTable WorksheetToDataTable(Workbook NewWorkbook, int WorksheetsIndex, int RowsIndex, int ColumnIndex, int MaxDataRow, bool bolTitle)
       {
           Worksheet NewWorksheet = NewWorkbook.Worksheets[WorksheetsIndex];
           //防止输入的行多于原文件的行数
           if (NewWorksheet.Cells.MaxDataRow + 1 - RowsIndex < MaxDataRow)
           {
               MaxDataRow = NewWorksheet.Cells.MaxDataRow + 1 - RowsIndex;
           }
           DataTable NewDataTable = NewWorksheet.Cells.ExportDataTableAsString(RowsIndex, ColumnIndex, MaxDataRow, NewWorksheet.Cells.MaxDataColumn + 1, bolTitle);
           return NewDataTable;

       }


       #region 读取工作簿sheet转成DataTable
       /// <summary>
       /// /
       /// </summary>
       /// <param name="NewWorkbook">工作簿对象</param>
       /// <param name="WorksheetsIndex">工作簿sheet入口</param>
       /// <param name="bolTitle">是否第一行作为列名</param>
       /// <returns></returns>
       public static DataTable WorksheetToDataTable(int firstRow, int firstColumn, Workbook NewWorkbook, int WorksheetsIndex, bool bolTitle)
       {
           Worksheet NewWorksheet = NewWorkbook.Worksheets[WorksheetsIndex];
           DataTable NewDataTable = NewWorksheet.Cells.ExportDataTableAsString(firstRow, firstColumn, NewWorksheet.Cells.MaxDataRow + 1, NewWorksheet.Cells.MaxDataColumn + 1, bolTitle);
           return NewDataTable;

       }
       #endregion
       
   }
}
