
using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Words.Tables;
using System.Web;

namespace phians.test
{
    /// <summary>
    /// get_table 的摘要说明
    /// </summary>
    public class get_table : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.QueryString["cmd"];
            switch (command)
            {
                case "print_table": ExtractTableData(context); break;

            }


        }
        public string get_table_1(HttpContext context)
        {

            // 模板绝对路径
            string modelMapPath = context.Server.MapPath("");
            //报告绝对路径
            string reportMapPath = context.Server.MapPath("");
            //创建word对象
            Document contract_doc = null;
            contract_doc = new Document(reportMapPath);
            DocumentBuilder docBuild = new DocumentBuilder(contract_doc);
            NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);
            
            return "";

        }
        private List<WordDocumentTable> WordDocumentTables
        {
            get
            {
                List<WordDocumentTable> wordDocTable = new List<WordDocumentTable>();
                //Reads the data from the first Table of the document.      
                wordDocTable.Add(new WordDocumentTable(0));
                //Reads the data from the second table and its second column.   
                //This table has only one row.      
                wordDocTable.Add(new WordDocumentTable(1, 1));
                //Reads the data from third table, second row and second cell.      
                wordDocTable.Add(new WordDocumentTable(2, 1, 1));
                return wordDocTable;
            }
        }
        public void ExtractTableData(HttpContext context)
{
    string reportMapPath = context.Server.MapPath("/Upload/自动超声波检验报告1-检验报告.docx");
    Document LobjAsposeDocument = new Document(reportMapPath);
       
        foreach(WordDocumentTable wordDocTable in WordDocumentTables) 
        {  
            Aspose.Words.Tables.Table tablex = (Aspose.Words.Tables.Table)
            LobjAsposeDocument.GetChild
            (NodeType.Table, wordDocTable.TableID, true);
            NodeCollection tables = LobjAsposeDocument.GetChildNodes(NodeType.Table, true);
            int dataTableIndex = 0;
            int dataRowIndex = 0;
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
                    int rowIndex = table.Rows.IndexOf(row);

                    // Iterate through all cells in the row
                    foreach (Cell cell in row.Cells)
                    {
                        int cellIndex = row.Cells.IndexOf(cell);

                        // Get the plain text content of this cell.
                        string cellText = cell.ToString(SaveFormat.Text).Trim();
                        int row_count = row.Count;
                        int cell_ = cell.Count;
                        if (cellText == "key1010")
                        {
                            dataRowIndex = rowIndex;
                            dataTableIndex = tableIndex;
                        }

                    }
                }

            }
           // string cellData = tablex.Range.Text;
          
        
            //if (wordDocTable.ColumnID > 0)  
            //{   
            //    if (wordDocTable.RowID == 0)   
            //    {    
            //        NodeCollection LobjCells =
            //        tablex.GetChildNodes(NodeType.Cell, true);    
            //        cellData = LobjCells[wordDocTable.ColumnID].GetText();
            //    }   
            //    else   
            //    {    
            //        NodeCollection LobjRows =
            //        tablex.GetChildNodes(NodeType.Row, true);    
            //        cellData = ((Aspose.Words.Tables.Row)(LobjRows[wordDocTable.RowID])).
            //        Cells[wordDocTable.ColumnID].GetText();   
            //    }  
            //}
 
            //Console.WriteLine(String.Format("Data in Table {0},Row {1}, Column {2} : {3}",wordDocTable.TableID,wordDocTable.RowID,wordDocTable.ColumnID,cellData));
        
        }
    
}
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}