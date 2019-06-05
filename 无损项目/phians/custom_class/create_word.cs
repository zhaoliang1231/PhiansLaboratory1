using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Tables;
using phians.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace phians.custom_class
{

    public class Data_Model
    {

        //零件id
        public string S_Number { get; set; }


        //检测项目
        public string Dimensioning { get; set; }

        //加工尺寸
        public string DimSizeGoal { get; set; }
        //频次
        public string MeasureFrequency { get; set; }
        //测量值
        public string MeasureValue { get; set; }


    }

    public class create_word
    {

        #region 加 工 零 件 检 验 记 录 表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="new_doc"> word对象</param>
        /// <param name="S_Numberlist"> 零件id列表 </param>
        /// <param name="test_data"> 测试数据list</param>
        /// <param name="total_count">  频次 整数</param>
        /// <param name="valueTypeList"> 检测项目list</param>
        /// <param name="Basic_Data"> 基础数据</param>
        public void make_word(Document new_doc, List<string> S_Numberlist, IList<Data_Model> test_data, int total_count, List<string> valueTypeList, DataTable Basic_Data)
        {

            //创建word对象
            List<string> namelist_ = new List<string>();

           
            DocumentBuilder docBuild = new DocumentBuilder(new_doc);

      
            NodeCollection table_all = new_doc.GetChildNodes(NodeType.Table, true);
            try
            {
                new_doc.Range.Bookmarks["S_Number"].Text = "key1010";

                new_doc.Range.Bookmarks["MeasureValue"].Text = "key10101";
            }
            catch
            {

                return;
            }
          

            #region 获取书签所在行（table的入口）
            //table入口地址
            int dataTableIndex0 = 0;//第一个table入口          
            //行入口地址
            int dataRowIndex0 = 0;
            double height = 0.56;
            int flag_ = 0;
            int table_count = 0;
            
            int cellIndex_ = 0;//行入库
            //查找行入口和table入口
            foreach (Aspose.Words.Tables.Table table in table_all)
            {

                if (dataRowIndex0 != 0)
                    break;
           
                foreach (Row row in table.Rows)
                {
                    if (dataRowIndex0 != 0)
                        break;
                    //第几行
                    int rowIndex = table.Rows.IndexOf(row);

                    // Iterate through all cells in the row
                    foreach (Cell cell in row.Cells)
                    {
                        int cellIndex = row.Cells.IndexOf(cell);

                        // Get the plain text content of this cell.
                        string cellText = cell.ToString(SaveFormat.Text).Trim();

                        if (cellText == "key1010")
                        {
                            if (cellIndex != 0)
                            {
                                flag_ = cellIndex;

                            }
                            dataRowIndex0 = rowIndex;

                            dataTableIndex0 = table_count;
                           
                        }
                        if (cellText == "key10101")
                        {

                            cellIndex_ = cellIndex;
                           
                        
                        }

                    
                    }
                }
                table_count++;

                
            }
            #endregion


            try
            {
                new_doc.Range.Bookmarks["S_Number"].Text = "";

                new_doc.Range.Bookmarks["MeasureValue"].Text = "";
            }
            catch
            {

                return;
            }
            Aspose.Words.Tables.Table tables = (Aspose.Words.Tables.Table)new_doc.GetChild(NodeType.Table, dataTableIndex0, true);
            Aspose.Words.Tables.Table tableClone = (Aspose.Words.Tables.Table)tables.Clone(true);
            int count = 0;//每一列
           int  total_count_flag = 1; //记录列数目
            List<double> widthList = new List<double>();
         

            //获取每个表多少个元素
            for (var i = 0; i < 30; i++)
            {
                try
                {
                    
                    docBuild.MoveToCell(dataTableIndex0, dataRowIndex0, i, 0); //移动单元格 
                    widthList.Add(docBuild.CellFormat.Width);
                }
                catch
                {
                    break;
                }
            }
            count = widthList.Count;
            //写第一行数据表头

            for (int i = cellIndex_; i < count; i++)
            {
                docBuild.MoveToCell(dataTableIndex0, dataRowIndex0-1, i, 0); //移动单元格 
                try
                {
                    docBuild.Write("NO" + total_count_flag);
                    total_count_flag++;
                }
                catch
                {
                    
                }
                
            }

            int row1 = dataRowIndex0;//第几行        
            //填充每行的值内容
           
          string S_Number = test_data[0].S_Number.ToString();         
          int rows=dataRowIndex0;
          int cc = 0;
          foreach (var a in S_Numberlist)
          {
              cc++;
              
                  //数据集合
                  var dataList = (from p in test_data where  p.S_Number == a  select new { p.S_Number, p.Dimensioning, p.MeasureFrequency,p.MeasureValue,p.DimSizeGoal }).ToList();
                  if (dataList.Count > 0) { 
                     Row clonedRow = (Row)tables.LastRow.Clone(true);

                     tables.Rows.Insert(rows, clonedRow);
               
                

                  for (var i = 0; i < widthList.Count; i++)
                  {
                      docBuild.MoveToCell(dataTableIndex0, rows, i, 0); //移动单元格 
                      if (i == 0)
                        
                          docBuild.Write(a);

                   
                      //检测项目
                      if (i == 1)
                      {
                          docBuild.Write(dataList[0].Dimensioning);
                      }
                      //目标尺寸
                      if (i == 2)
                      {
                          docBuild.Write(dataList[0].DimSizeGoal);
                      }
                      //频次
                      if (i == 3)
                      {
                          docBuild.Write(dataList[0].MeasureFrequency);
                       
                      }
                    if(i>3)
                      {
                          try
                          {
                              docBuild.Write(dataList[i-4].MeasureValue);//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                          }
                          catch
                          {
                              docBuild.Write("合格");
                          }
                      }

                  }
                  rows++;
                  }
              
          }



          #region 一个表格填不下，新建表格继续填写

          int tableIndex = 0; // 表格
          // elementdocCount<elementCount 第一个表格填充的元素小于标准里的元素；说明需要增加第二个表格添加数据
          int elementIndex = cellIndex_;
      
        
          //重新查找表格入口
        int total_count_flag_=total_count_flag-1;   //填充元素个数
          while (total_count_flag < total_count)
          {

             int dataRowIndex2 = 0;    
            int table_count2 = 0;
             int new_index = 0;
           
              tables.ParentNode.InsertAfter(tableClone, tables);//插入拷贝表格

           

           // 重新获取表格入口
              foreach (Aspose.Words.Tables.Table table in table_all)
              {
                  if (dataRowIndex2 != 0)
                      break;
                  foreach (Row row in table.Rows)
                  {
                      if (dataRowIndex2 != 0)
                          break;
                      foreach (Cell cell in row.Cells)
                      {
                          int cellIndex = row.Cells.IndexOf(cell);

                          // Get the plain text content of this cell.
                          string cellText = cell.ToString(SaveFormat.Text).Trim();
                          if (cellText == "检测项目")
                          {
                              new_index = table_count2;
                          }
                      }
                  }
                  table_count2++;
              }
              tables = (Aspose.Words.Tables.Table)new_doc.GetChild(NodeType.Table, new_index, true);
              tableClone = (Aspose.Words.Tables.Table)tables.Clone(true);
              //写第一行数据表头

              for (int i = cellIndex_; i < count; i++)
              {
                  docBuild.MoveToCell(new_index, dataRowIndex0 - 1, i, 0); //移动单元格 
                  try
                  {
                      docBuild.Write("NO" + total_count_flag);
                      total_count_flag++;
                  }
                  catch
                  {

                  }

              }

              int dataRowIndex0_= dataRowIndex0;
              foreach (var a in S_Numberlist)
              {
                 
                      //数据集合
                      int j = 0;
                      var dataList = (from p in test_data where  p.S_Number == a select new { p.S_Number, p.Dimensioning, p.MeasureFrequency, p.MeasureValue, p.DimSizeGoal,NO=j++ }).ToList();
                      if (dataList.Count > 0)
                      {
                          Row clonedRow = (Row)tables.LastRow.Clone(true);
                          tables.Rows.Insert(rows, clonedRow);

                          for (var ii = 0; ii < widthList.Count; ii++)
                          {
                              docBuild.MoveToCell(new_index, dataRowIndex0_, ii, 0); //移动单元格 
                              if (ii == 0)

                                  docBuild.Write(a);


                              //检测项目
                              if (ii == 1)
                              {
                                  docBuild.Write(dataList[0].Dimensioning);
                              }
                              //目标尺寸
                              if (ii == 2)
                              {
                                  docBuild.Write(dataList[0].DimSizeGoal);
                              }
                              //频次
                              if (ii == 3)
                              {
                                  docBuild.Write(dataList[0].MeasureFrequency);

                              }
                              if (ii > 3)
                              {
                                  try
                                  {
                                      docBuild.Write(dataList[elementIndex + ii - 3].MeasureValue);//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                                  }
                                  catch
                                  {
                                      docBuild.Write("合格");
                                  }
                              }

                          }
                          dataRowIndex0_++;
                          rows++;
                      }
                 
              }
              elementIndex = elementIndex+ total_count_flag_;
         
          }
          #endregion


            //foreach (Aspose.Words.Bookmark mark in new_doc.Range.Bookmarks)
            //{
            //    switch (mark.Name)
            //    {
                        
            //        case "ProcessCode": mark.Text = Basic_Data.Rows[0]["ProcessCode"].ToString(); break;//图纸编号
            //        case "MeasureUserName": mark.Text = Basic_Data.Rows[0]["MeasureUserName"].ToString(); break;//批次号
            //        case "PartName": mark.Text = Basic_Data.Rows[0]["PartName"].ToString(); break;//零件名称
            //        case "PartCount": mark.Text = Basic_Data.Rows[0]["PartCount"].ToString(); break;//数量
            //        case "PartCode": mark.Text = Basic_Data.Rows[0]["PartCode"].ToString(); break;//零件代号
            //        case "Process_SerialNumber": mark.Text = Basic_Data.Rows[0]["Process_SerialNumber"].ToString(); break;//工序号NO1
            //        case "ProcessEquipment": mark.Text = Basic_Data.Rows[0]["ProcessEquipment"].ToString(); break;//加工设备
            //        case "CastingCode": mark.Text = Basic_Data.Rows[0]["CastingCode"].ToString(); break;//铸件编号
            //        case "BatchCode": mark.Text = Basic_Data.Rows[0]["BatchCode"].ToString(); break;//作业者
            //        case "MeasureTime": mark.Text = Basic_Data.Rows[0]["MeasureTime"].ToString(); break;//作业时间
            //        case "ReportCheck_UserName": mark.Text = Basic_Data.Rows[0]["ReportCheck_UserName"].ToString(); break;//班长
            //        case "ReportCheck_MeasureTime": mark.Text = Basic_Data.Rows[0]["ReportCheck_MeasureTime"].ToString(); break;//班长签字时间
            //        case "Part_ReportCode": mark.Text = Basic_Data.Rows[0]["Part_ReportCode"].ToString(); break;//子报告编号
            //        case "ProcessCompile_Picture":
            //            insert_image(new_doc, docBuild, "ProcessCompile_Picture", Basic_Data.Rows[0]["ProcessCompile_Picture"].ToString()); break;//编制
            //        case "ProcessChecked_Picture":
            //            insert_image(new_doc, docBuild, "ProcessChecked_Picture", Basic_Data.Rows[0]["ProcessChecked_Picture"].ToString()); break;//审核
            //        case "ProcessIssue_Picture":
            //            insert_image(new_doc, docBuild, "ProcessIssue_Picture", Basic_Data.Rows[0]["ProcessIssue_Picture"].ToString()); break;//签发
            //        case "PartReport_OneBar_Code": //条形码
            //            try
            //            {

            //                new_doc.Range.Bookmarks["PartReport_OneBar_Code"].Text = "";
            //                docBuild.MoveToBookmark("PartReport_OneBar_Code");//跳到指定书签
            //                Aspose.BarCode.BarCodeBuilder builder = new Aspose.BarCode.BarCodeBuilder();
            //                //Set the Code text for the barcode  
            //                builder.CodeText = Basic_Data.Rows[0]["PartReport_OneBar_Code"].ToString();

            //                //Set the symbology type to Code128  
            //                builder.SymbologyType = Aspose.BarCode.Symbology.Code128;

            //                ////Insert the barCode image into document  
            //                docBuild.InsertImage(builder.BarCodeImage,
            //                                      Aspose.Words.Drawing.RelativeHorizontalPosition.Margin,
            //                                      1,
            //                                      Aspose.Words.Drawing.RelativeVerticalPosition.Margin,
            //                                      1,
            //                                      120,
            //                                      38,
            //                                      Aspose.Words.Drawing.WrapType.TopBottom);

            //            }
            //            catch { } break;
            //        default: break;

            //    }
            //}

        }
        #endregion

      

        #region 加工零件精度检查单
       /// <summary>
       /// 
       /// </summary>
        /// <param name="new_doc"> word 对象</param>
        /// <param name="testData"> 测试数据</param>
       /// <param name="Basic_Data">基础数据</param>
        public void create_word2(Document new_doc, DataTable testData,DataTable Basic_Data)
        {
            int Test_flag = 0;

            DocumentBuilder docBuild = new DocumentBuilder(new_doc);
            NodeCollection tables = new_doc.GetChildNodes(NodeType.Table, true);
            try
            {
                new_doc.Range.Bookmarks["S_Number"].Text = "key1010";
                new_doc.Range.Bookmarks["S_Number1"].Text = "key10101";

            }
            catch
            {
               
                return;
            }
             try
            {
                new_doc.Range.Bookmarks["test_"].Text = "";
                Test_flag = 1;
            }
            catch
            {
               
                return;
            }
            
            #region 获取书签所在行（table的入口）
            //table入口地址
            int dataTableIndex = 0;//第一个table入口
            int dataTableIndex2 = 0;//第二个table入口
            //行入口地址
            int dataRowIndex = 0;
            double height = 0.56; 
            int flag_ = 0;
            int table_count = 0;
         //查找行入口和table入口
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
                        string cellText = cell.ToString(SaveFormat.Text).Trim();

                        if (cellText == "key1010")
                        {
                            if (cellIndex != 0)
                            {
                                flag_ = cellIndex;

                            }
                            dataRowIndex = rowIndex;

                            dataTableIndex = table_count;
                            break;
                        }

                        if (cellText == "key10101")
                        {
                            dataTableIndex2 = table_count;
                            break;
                        }


                    }
                }
                table_count++;  


            }
            #endregion


            #region 获取样式

            int count = 0;
            List<double> widthList = new List<double>();//表格1宽度集合

            List<string> CellVerticalAlignment_1 = new List<string>();//垂直样式
            List<string> ParagraphAlignment_1 = new List<string>();//水平样式                   
            List<string> listcolumn = new List<string>();//表格1表格内容集合
            //获取表格1每列书签名称
            for (var i = 0; i < 30; i++)
            {
                try
                {
                    docBuild.MoveToCell(dataTableIndex, dataRowIndex, i, 0); //移动单元格 
                    if (docBuild.CurrentNode != null)
                    {
                        if (docBuild.CurrentNode.NodeType == NodeType.BookmarkStart)
                        {
                           //获取表格书签
                            listcolumn.Add((docBuild.CurrentNode as BookmarkStart).Name);
                        }
                    }
                    else
                        listcolumn.Add("");
                  
                    //获取单元格宽度
                    widthList.Add(docBuild.CellFormat.Width);
                    //获取垂直样式
                    CellVerticalAlignment_1.Add((docBuild.CellFormat.VerticalAlignment).ToString());
                    //获取水平样式
                    ParagraphAlignment_1.Add((docBuild.ParagraphFormat.Alignment).ToString());

                    count++;
                }
                catch
                {
                    break;
                }
            }
            #endregion
            //第一个表格第一行
            if (testData.Rows.Count>=1) { 
            foreach (Aspose.Words.Bookmark mark in new_doc.Range.Bookmarks)
            {
                switch (mark.Name)
                {

                    case "S_Number": mark.Text = testData.Rows[0]["S_Number"].ToString(); break;
                    case "Dimension": mark.Text = testData.Rows[0]["Dimension"].ToString(); break;
                    case "TopLower": mark.Text = testData.Rows[0]["TopLower"].ToString(); break;
                    case "Measure": mark.Text = testData.Rows[0]["Measure"].ToString(); break;
                    case "JerkValue": mark.Text = testData.Rows[0]["JerkValue"].ToString(); break;
                        

                }
            }


            }
           // 第二个表格第一行
            if (testData.Rows.Count >=2)
            {
                foreach (Aspose.Words.Bookmark mark in new_doc.Range.Bookmarks)
                {
                    switch (mark.Name)
                    {

                        case "S_Number1": mark.Text = testData.Rows[1]["S_Number"].ToString(); break;
                        case "Dimension1": mark.Text = testData.Rows[1]["Dimension"].ToString(); break;
                        case "TopLower1": mark.Text = testData.Rows[1]["TopLower"].ToString(); break;
                        case "Measure1": mark.Text = testData.Rows[1]["Measure"].ToString(); break;
                        case "JerkValue1": mark.Text = testData.Rows[1]["JerkValue"].ToString(); break;


                    }
                }


            }


            #region    开始插入表格1数据
     

            if (testData.Rows.Count > 2) 
            {

                docBuild.MoveToBookmark("S_Number");//跳到指定书签
                height = docBuild.RowFormat.Height;
                docBuild.MoveToBookmark("table"); //开始添加值 
            
                int row_count = 1;
                int CC = 0;
            //写入记录 从第2行开始
            for (var m = 2; m < testData.Rows.Count; m++)
            {
                
                row_count = row_count + 1;
               //  判断奇数偶数表格
                int a = m % 2;
                //  判断奇数偶数表格
                if(a==0){
                    CC = 1;
                for (var i = 0; i < listcolumn.Count - flag_; i++)
                {
                    docBuild.InsertCell(); // 添加一个单元格 
                    //docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;
                    //docBuild.CellFormat.Borders.Color = System.Drawing.Color.Black;
                    //设置单元格宽度
                    docBuild.CellFormat.Width = widthList[i + flag_];
                    //设置单元格高度
                    docBuild.RowFormat.Height = height;
                    //单元格样式
                    if (flag_ != 0 || Test_flag==1)
                    {
                        docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;

                        if (i == 0)
                        {
                            docBuild.CellFormat.Borders.Left.LineStyle = LineStyle.None;
                        }
                        if (i == listcolumn.Count - (flag_ + 1))
                        { docBuild.CellFormat.Borders.Right.LineStyle = LineStyle.None; }

                        docBuild.CellFormat.Borders.Top.LineStyle = LineStyle.None;
                        // 判断是否最后一行
                        if (row_count == testData.Rows.Count)
                        {
                            docBuild.CellFormat.Borders.Bottom.LineStyle = LineStyle.None;

                        }
                    }
                    //docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                    //docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                    //垂直样式
                    switch (CellVerticalAlignment_1[i + flag_])
                    {
                        case "Center": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; break;
                        case "Top": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Top; break;
                        case "Bottom": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom; break;

                    }
                    //水平样式
                    switch (ParagraphAlignment_1[i + flag_])
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
                    //判断书签是否在table里列名里
                    {
                        if (testData.Columns.Contains(listcolumn[i + flag_].ToString()))
                            docBuild.Write(testData.Rows[m][listcolumn[i + flag_]].ToString());
                    }
                 
                }
                docBuild.EndRow();
               
            }
                if (CC == 1)
                {
                    docBuild.EndTable();
                    CC = 0;
                }
              
            }
          
        }





            new_doc.Range.Bookmarks["table"].Text = ""; // 清掉标示 




            #endregion


            #region    开始插入表格2数据


            if (testData.Rows.Count > 2)
            {

                docBuild.MoveToBookmark("S_Number1");//跳到指定书签
                height = docBuild.RowFormat.Height;
                docBuild.MoveToBookmark("table2"); //开始添加值 

                int row_count = 1;
                int CC = 0;
                //写入记录 从第2行开始
                for (var m = 2; m < testData.Rows.Count; m++)
                {
                   
                    row_count = row_count + 1;
                    //  判断奇数偶数表格
                    int a = m % 2;
                    //  判断奇数偶数表格
                    if (a != 0)
                    {
                        CC = 1;
                        for (var i = 0; i < listcolumn.Count - flag_; i++)
                        {
                            docBuild.InsertCell(); // 添加一个单元格 
                            //docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;
                            //docBuild.CellFormat.Borders.Color = System.Drawing.Color.Black;
                            //设置单元格宽度
                            docBuild.CellFormat.Width = widthList[i + flag_];
                            //设置单元格高度
                            docBuild.RowFormat.Height = height;
                            //单元格样式
                            if (flag_ != 0 || Test_flag == 1)
                            {
                                docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;

                                if (i == 0)
                                {
                                    docBuild.CellFormat.Borders.Left.LineStyle = LineStyle.None;
                                }
                                if (i == listcolumn.Count - (flag_ + 1))
                                { docBuild.CellFormat.Borders.Right.LineStyle = LineStyle.None; }

                                docBuild.CellFormat.Borders.Top.LineStyle = LineStyle.None;
                                // 判断是否最后一行
                                if (row_count == testData.Rows.Count)
                                {
                                    docBuild.CellFormat.Borders.Bottom.LineStyle = LineStyle.None;

                                }
                            }
                            //docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                            //docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                            //垂直样式
                            switch (CellVerticalAlignment_1[i + flag_])
                            {
                                case "Center": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; break;
                                case "Top": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Top; break;
                                case "Bottom": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom; break;

                            }
                            //水平样式
                            switch (ParagraphAlignment_1[i + flag_])
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
                            //判断书签是否在table里列名里
                            {
                                if (testData.Columns.Contains(listcolumn[i + flag_].ToString()))
                                    docBuild.Write(testData.Rows[m][listcolumn[i + flag_]].ToString());
                            }

                        }
                        docBuild.EndRow();
                       
                    }

                    if (CC == 1)
                    {
                        docBuild.EndTable();
                        CC = 0;
                    }
                }
               
            }
            new_doc.Range.Bookmarks["table2"].Text = ""; // 清掉标示 



            #endregion

           //基础数据插入
            if (Basic_Data.Rows.Count >0)
            {
                foreach (Aspose.Words.Bookmark mark in new_doc.Range.Bookmarks)
                {
                    switch (mark.Name)
                    {

                        case "PartID": mark.Text = Basic_Data.Rows[0]["PartID"].ToString(); break;//检查项目
                        case "PartName": mark.Text = Basic_Data.Rows[0]["PartName"].ToString(); break;//检查项目
                        case "GroupName": mark.Text = Basic_Data.Rows[0]["GroupName"].ToString(); break;//班组
                        case "ProcessName": mark.Text = Basic_Data.Rows[0]["ProcessName"].ToString(); break;//工序
                        case "MeasureTime": mark.Text = Basic_Data.Rows[0]["MeasureTime"].ToString(); break;//测量时间
                        case "MeasureUserID": mark.Text = Basic_Data.Rows[0]["MeasureUserID"].ToString(); break;//测量者
                        case "Part_ReportCode": mark.Text = Basic_Data.Rows[0]["Part_ReportCode"].ToString(); break;//子报告编号
                        case "ReportCheck_Picture":
                            insert_image(new_doc, docBuild, "ReportCheck_Picture", Basic_Data.Rows[0]["ReportCheck_Picture"].ToString()); break;//班长确认
                        case "ProcessCompile_Picture":
                            insert_image(new_doc, docBuild, "ProcessCompile_Picture", Basic_Data.Rows[0]["ProcessCompile_Picture"].ToString()); break;//编制
                        case "ProcessChecked_Picture":
                            insert_image(new_doc, docBuild, "ProcessChecked_Picture", Basic_Data.Rows[0]["ProcessChecked_Picture"].ToString()); break;//审核
                        case "ProcessIssue_Picture":
                            insert_image(new_doc, docBuild, "ProcessIssue_Picture", Basic_Data.Rows[0]["ProcessIssue_Picture"].ToString()); break;//签发
                        case "DWG_ATPicture":
                            insert_image(new_doc, docBuild, "DWG_ATPicture", Basic_Data.Rows[0]["ProcessIssue_Picture"].ToString()); break;//图纸
                            
                        case "PartReport_OneBar_Code": //条形码
                            try
                            {

                                new_doc.Range.Bookmarks["PartReport_OneBar_Code"].Text = "";
                                docBuild.MoveToBookmark("PartReport_OneBar_Code");//跳到指定书签
                                Aspose.BarCode.BarCodeBuilder builder = new Aspose.BarCode.BarCodeBuilder();
                                //Set the Code text for the barcode  
                                builder.CodeText = Basic_Data.Rows[0]["PartReport_OneBar_Code"].ToString();

                                //Set the symbology type to Code128  
                                builder.SymbologyType = Aspose.BarCode.Symbology.Code128;

                                ////Insert the barCode image into document  
                                docBuild.InsertImage(builder.BarCodeImage,
                                                      Aspose.Words.Drawing.RelativeHorizontalPosition.Margin,
                                                      1,
                                                      Aspose.Words.Drawing.RelativeVerticalPosition.Margin,
                                                      1,
                                                      120,
                                                      38,
                                                      Aspose.Words.Drawing.WrapType.TopBottom);

                            }
                            catch { } break;
                        default: break;
                    }
                    
                }


            }

        }



        #endregion


        #region 写入签名
        
        /// <summary>
        /// //写入签名
        /// </summary>
        /// <param name="new_doc"> word 对象</param>
        /// <param name="docBuild">docBuild </param>
        /// <param name="Bookmarks_name"> 书签名字</param>
        /// <param name="image_url"> 图片url绝对路径</param>
        public void insert_image(Document new_doc, DocumentBuilder docBuild, string Bookmarks_name, string image_url)
        {
            //写入试验人员签名                    
            try
            {
                new_doc.Range.Bookmarks[Bookmarks_name].Text = "";
                docBuild.MoveToBookmark(Bookmarks_name);//跳到指定书签
                double width = 35, height = 15;
                Aspose.Words.Drawing.Shape shape = docBuild.InsertImage(image_url); //插入图片：自动控制大小，并不遮挡后面的内容
                shape.Width = width - 2;
                shape.Height = height - 2;
                shape.VerticalAlignment = VerticalAlignment.Bottom;
                docBuild.InsertNode(shape);
            }
            catch
            { }
        }
        #endregion
        #region 写入签名

        /// <summary>
        /// //写入图纸
        /// </summary>
        /// <param name="new_doc"> word 对象</param>
        /// <param name="docBuild">docBuild </param>
        /// <param name="Bookmarks_name"> 书签名字</param>
        /// <param name="image_url"> 图片url绝对路径</param>
        public void insert_image2(Document new_doc, DocumentBuilder docBuild, string Bookmarks_name, string image_url)
        {
            //写入试验人员签名                    
            try
            {
                new_doc.Range.Bookmarks[Bookmarks_name].Text = "";
                docBuild.MoveToBookmark(Bookmarks_name);//跳到指定书签
                double width = 300, height = 500;
                Aspose.Words.Drawing.Shape shape = docBuild.InsertImage(image_url); //插入图片：自动控制大小，并不遮挡后面的内容
                shape.Width = width - 2;
                shape.Height = height - 2;
                shape.VerticalAlignment = VerticalAlignment.Bottom;
                docBuild.InsertNode(shape);
            }
            catch
            { }
        }
        #endregion




        public int  gettable_index(Table table_all,string  s_context){
            int dataRowIndex0 = 0;
            int dataTableIndex0 = 0;
            int table_count = 0;
           foreach (Aspose.Words.Tables.Table table in table_all)
            {

                if (dataRowIndex0 != 0)
                    break;           
                foreach (Row row in table.Rows)
                {
                    if (dataRowIndex0 != 0)
                        break;
                    //第几行
                    int rowIndex = table.Rows.IndexOf(row);
                    foreach (Cell cell in row.Cells)
                    {
                        int cellIndex = row.Cells.IndexOf(cell);

                        // Get the plain text content of this cell.
                        string cellText = cell.ToString(SaveFormat.Text).Trim();

                        if (cellText == s_context)
                        {
                           
                            dataRowIndex0 = rowIndex;
                            dataTableIndex0 = table_count;                          
                        }
                    }
                }
                table_count++;             
            }


           return dataTableIndex0; 
        }
    }


   
   
}