using Aspose.Words;
using Aspose.Words.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhiansCommon
{
    public class WordAddTableRows
    {
        /// <summary>
        /// Word文档表格自动添加行
        /// </summary>
        /// <param name="contract_doc">word文档</param>
        /// <param name="FirstBookmarks">书签入口</param>
        /// <param name="testData">索引的数据DataTable</param>
        public static void WordAddTableRows_T(Document contract_doc, string FirstBookmarks, DataTable testData)
        {            
            Aspose.Words.DocumentBuilder docBuild = new Aspose.Words.DocumentBuilder(contract_doc);
            NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);//表格

            //尝试找到书签FirstBookmarks 插入文本"key1010"
            try
            {
                contract_doc.Range.Bookmarks[FirstBookmarks].Text = "key1010";
            }
            catch
            {
            }

            //table入口地址
            //行入口地址
            int dataRowIndex = 0;
            int flag_ = 0;      //第几列
            double height = 0.56;
            int dataTableIndex = 0;
            int dataTable_count = 0;
            #region 获取书签所在行（table的入口）
            foreach (Aspose.Words.Tables.Table Temptable in tables)
            {
                if (dataRowIndex != 0)
                    break;
                // 获取表的父节点中包含的表节点的索引
                int tableIndex = Temptable.ParentNode.ChildNodes.IndexOf(Temptable);

                // 循环表中的所有行
                foreach (Row row in Temptable.Rows)
                {
                    if (dataRowIndex != 0)
                        break;
                    //第几行
                    int rowIndex = Temptable.Rows.IndexOf(row);

                    // 循环行中的所有单元格
                    foreach (Cell cell in row.Cells)
                    {
                        int cellIndex = row.Cells.IndexOf(cell);


                        string cellText = "";
                        try
                        {
                            // 获取此单元格的文本内容
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
            List<string> listcolumn = new List<string>();//表格1表格内容集合

            for (var i = flag_; i < 30; i++)//30指列的数量
            {
                try
                {
                    docBuild.MoveToCell(dataTableIndex, dataRowIndex, i, 0); //移动单元格 
                    if (docBuild.CurrentNode != null)
                    {
                        if (docBuild.CurrentNode.NodeType == NodeType.BookmarkStart)
                        {
                            listcolumn.Add((docBuild.CurrentNode as BookmarkStart).Name);//每列书签名称 放入到 listcolumn
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
            docBuild.MoveToBookmark(FirstBookmarks);//跳到指定书签
            height = docBuild.RowFormat.Height;
            docBuild.MoveToBookmark("table"); //跳到指定书签
            int row_count = 1;
            int CC = 0;                 //判断表格是否添加了行——默认模板只有第一行 第二行需要代码添加
            //写入记录 从第2行开始
            for (var m = 1; m < 30; m++)
            {
                CC = 1;
                row_count = row_count + 1;
                //循环表格第一行所有书签，插入表格行
                for (var i = 0; i < listcolumn.Count - flag_; i++)
                {
                    docBuild.InsertCell(); // 添加一个单元格
                    //docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;
                    //docBuild.CellFormat.Borders.Color = System.Drawing.Color.Black;
                    //设置单元格宽度
                    docBuild.CellFormat.Width = widthList[i + flag_];
                    //设置单元格高度
                    docBuild.RowFormat.Height = height;
                    //单元格边框控制
                
                    docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;
                    // 第一行去掉top边框；
                    if (m == 1) {
                        docBuild.CellFormat.Borders.Top.LineStyle = LineStyle.None;
                    }
                      
                    //第一个单元格去掉左边边框线
                        if (i == 0)
                        {
                          
                            docBuild.CellFormat.Borders.Left.LineStyle = LineStyle.None;
                        }
                   // 最后一个单元格去掉右边线
                        if (i == listcolumn.Count - (flag_ + 1))
                        { docBuild.CellFormat.Borders.Right.LineStyle = LineStyle.None; }
                        // 最后一行去掉Bottom边框；
                        if (row_count == testData.Rows.Count)
                        {
                            docBuild.CellFormat.Borders.Bottom.LineStyle = LineStyle.None;
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
                    docBuild.Font.Size = 10;
                    docBuild.Font.Name = "Arial";
                    //是否加粗
                    docBuild.Bold = false;

                    #region 插入内容
                    if (m < testData.Rows.Count)//有内容的 —— 插入内容
                    {
                        string listcolumn_name = listcolumn[i + flag_].ToString(); //word表格书签的名字   flag_是第一个书签[SerialNo]的列序号（从0开始）

                        if (testData.Columns.Contains(listcolumn_name))//判断sql表格中是否存在该书签名的列
                        {
                            string listcolumn_value = testData.Rows[m][listcolumn[i + flag_]].ToString();//获取sql表格第m列的i + flag_行的值 插入到word表格的位置
                            if (string.IsNullOrEmpty(listcolumn_value))
                            {
                                listcolumn_value = "";
                            }
                            docBuild.Write(listcolumn_value);//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;插入到word表格的位置
                        }
                    }
                    else //没内容的 —— 插入空行
                    {
                        if (m == testData.Rows.Count && listcolumn[i + flag_].ToString() == "ProjectName")
                        {
                            docBuild.Write("以下空白");//插入到word表格的位置
                        }
                        else {
                            docBuild.Write("");//插入到word表格的位置
                        }

                    }
                    #endregion

                }
                docBuild.EndRow();
            }
            if (CC == 1)
            {
                docBuild.EndTable();
            }
            contract_doc.Range.Bookmarks["table"].Text = ""; // 清掉标示
            #endregion
           
        }



    }

}
