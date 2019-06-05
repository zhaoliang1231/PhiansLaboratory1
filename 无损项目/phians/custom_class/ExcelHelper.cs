using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
namespace phians
{
    public class ExcelHelper
    {

        #region 数据导出至Excel文件
        /// </summary> 
        /// 导出Excel文件，自动返回可下载的文件流 
        /// </summary> 
        public static void DataTable1Excel(System.Data.DataTable dtData)
        {
            GridView gvExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;
            if (dtData != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                curContext.Response.Charset = "utf-8";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                gvExport = new GridView();
                gvExport.DataSource = dtData.DefaultView;
                gvExport.AllowPaging = false;
                gvExport.DataBind();
                gvExport.RenderControl(htmlWriter);
                curContext.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=gb2312\"/>" + strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 导出Excel文件，转换为可读模式
        /// </summary>
        public static void DataTable2Excel(System.Data.DataTable dtData)
        {
            DataGrid dgExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                dgExport = new DataGrid();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 导出Excel文件，并自定义文件名
        /// </summary>
        public static void DataTable3Excel(System.Data.DataTable dtData, String FileName)
        {
            GridView dgExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application nd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "GB2312";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                dgExport = new GridView();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 将数据导出至Excel文件
        /// </summary>
        /// <param name="Table">DataTable对象</param>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        public static bool OutputToExcel(DataTable Table, string ExcelFilePath)
        {
            if (File.Exists(ExcelFilePath))
            {
                throw new Exception("该文件已经存在！");
            }

            if ((Table.TableName.Trim().Length == 0) || (Table.TableName.ToLower() == "table"))
            {
                Table.TableName = "Sheet1";
            }

            //数据表的列数
            int ColCount = Table.Columns.Count;

            //用于记数，实例化参数时的序号 
            int i = 0;

            //创建参数
            OleDbParameter[] para = new OleDbParameter[ColCount];

            //创建表结构的SQL语句
            string TableStructStr = @"Create Table " + Table.TableName + "(";

            //连接字符串
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);

            //创建表结构
            OleDbCommand objCmd = new OleDbCommand();

            //数据类型集合
            ArrayList DataTypeList = new ArrayList();
            DataTypeList.Add("System.Decimal");
            DataTypeList.Add("System.Double");
            DataTypeList.Add("System.Int16");
            DataTypeList.Add("System.Int32");
            DataTypeList.Add("System.Int64");
            DataTypeList.Add("System.Single");

            //遍历数据表的所有列，用于创建表结构
            foreach (DataColumn col in Table.Columns)
            {
                //如果列属于数字列，则设置该列的数据类型为double
                if (DataTypeList.IndexOf(col.DataType.ToString()) >= 0)
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.Double);
                    objCmd.Parameters.Add(para[i]);

                    //如果是最后一列
                    if (i + 1 == ColCount)
                    {
                        TableStructStr += col.ColumnName + " double)";
                    }
                    else
                    {
                        TableStructStr += col.ColumnName + " double,";
                    }
                }
                else
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.VarChar);
                    objCmd.Parameters.Add(para[i]);

                    //如果是最后一列
                    if (i + 1 == ColCount)
                    {
                        TableStructStr += col.ColumnName + " varchar)";
                    }
                    else
                    {
                        TableStructStr += col.ColumnName + " varchar,";
                    }
                }
                i++;
            }

            //创建Excel文件及文件结构
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = TableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //插入记录的SQL语句
            string InsertSql_1 = "Insert into " + Table.TableName + " (";
            string InsertSql_2 = " Values (";
            string InsertSql = "";

            //遍历所有列，用于插入记录，在此创建插入记录的SQL语句
            for (int colID = 0; colID < ColCount; colID++)
            {
                if (colID + 1 == ColCount)  //最后一列
                {
                    InsertSql_1 += Table.Columns[colID].ColumnName + ")";
                    InsertSql_2 += "@" + Table.Columns[colID].ColumnName + ")";
                }
                else
                {
                    InsertSql_1 += Table.Columns[colID].ColumnName + ",";
                    InsertSql_2 += "@" + Table.Columns[colID].ColumnName + ",";
                }
            }

            InsertSql = InsertSql_1 + InsertSql_2;

            //遍历数据表的所有数据行
            for (int rowID = 0; rowID < Table.Rows.Count; rowID++)
            {
                for (int colID = 0; colID < ColCount; colID++)
                {
                    if (para[colID].DbType == DbType.Double && Table.Rows[rowID][colID].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = Table.Rows[rowID][colID].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = InsertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }

        /// <summary>
        /// 将数据导出至Excel文件
        /// </summary>
        /// <param name="Table">DataTable对象</param>
        /// <param name="Columns">要导出的数据列集合</param>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        public static bool OutputToExcel(DataTable Table, ArrayList Columns, string ExcelFilePath)
        {
            if (File.Exists(ExcelFilePath))
            {
                throw new Exception("该文件已经存在！");
            }

            //如果数据列数大于表的列数，取数据表的所有列
            if (Columns.Count > Table.Columns.Count)
            {
                for (int s = Table.Columns.Count + 1; s <= Columns.Count; s++)
                {
                    Columns.RemoveAt(s);   //移除数据表列数后的所有列
                }
            }

            //遍历所有的数据列，如果有数据列的数据类型不是 DataColumn，则将它移除
            DataColumn column = new DataColumn();
            for (int j = 0; j < Columns.Count; j++)
            {
                try
                {
                    column = (DataColumn)Columns[j];
                }
                catch (Exception)
                {
                    Columns.RemoveAt(j);
                }
            }
            if ((Table.TableName.Trim().Length == 0) || (Table.TableName.ToLower() == "table"))
            {
                Table.TableName = "Sheet1";
            }

            //数据表的列数
            int ColCount = Columns.Count;

            //创建参数
            OleDbParameter[] para = new OleDbParameter[ColCount];

            //创建表结构的SQL语句
            string TableStructStr = @"Create Table " + Table.TableName + "(";

            //连接字符串
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);

            //创建表结构
            OleDbCommand objCmd = new OleDbCommand();

            //数据类型集合
            ArrayList DataTypeList = new ArrayList();
            DataTypeList.Add("System.Decimal");
            DataTypeList.Add("System.Double");
            DataTypeList.Add("System.Int16");
            DataTypeList.Add("System.Int32");
            DataTypeList.Add("System.Int64");
            DataTypeList.Add("System.Single");

            DataColumn col = new DataColumn();

            //遍历数据表的所有列，用于创建表结构
            for (int k = 0; k < ColCount; k++)
            {
                col = (DataColumn)Columns[k];

                //列的数据类型是数字型
                if (DataTypeList.IndexOf(col.DataType.ToString().Trim()) >= 0)
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.Double);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " Double)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " Double,";
                    }
                }
                else
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.VarChar);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar,";
                    }
                }
            }

            //创建Excel文件及文件结构
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = TableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //插入记录的SQL语句
            string InsertSql_1 = "Insert into " + Table.TableName + " (";
            string InsertSql_2 = " Values (";
            string InsertSql = "";

            //遍历所有列，用于插入记录，在此创建插入记录的SQL语句
            for (int colID = 0; colID < ColCount; colID++)
            {
                if (colID + 1 == ColCount)  //最后一列
                {
                    InsertSql_1 += Columns[colID].ToString().Trim() + ")";
                    InsertSql_2 += "@" + Columns[colID].ToString().Trim() + ")";
                }
                else
                {
                    InsertSql_1 += Columns[colID].ToString().Trim() + ",";
                    InsertSql_2 += "@" + Columns[colID].ToString().Trim() + ",";
                }
            }

            InsertSql = InsertSql_1 + InsertSql_2;

            //遍历数据表的所有数据行
            DataColumn DataCol = new DataColumn();
            for (int rowID = 0; rowID < Table.Rows.Count; rowID++)
            {
                for (int colID = 0; colID < ColCount; colID++)
                {
                    //因为列不连续，所以在取得单元格时不能用行列编号，列需得用列的名称
                    DataCol = (DataColumn)Columns[colID];
                    if (para[colID].DbType == DbType.Double && Table.Rows[rowID][DataCol.Caption].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = Table.Rows[rowID][DataCol.Caption].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = InsertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 获取Excel文件数据表列表
        /// </summary>
        public static ArrayList GetExcelTables(string ExcelFileName)
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + ExcelFileName))
                {
                    try
                    {
                        conn.Open();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }

                    //获取数据表个数
                    int tablecount = dt.Rows.Count;
                    for (int i = 0; i < tablecount; i++)
                    {
                        string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                        if (TablesList.IndexOf(tablename) < 0)
                        {
                            TablesList.Add(tablename);
                        }
                    }
                }
            }
            return TablesList;
        }

        /// <summary>
        /// 将Excel文件导出至DataTable(第一行作为表头)
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        /// <param name="TableName">数据表名，如果数据表名错误，默认为第一个数据表名</param>
        public static DataTable InputFromExcel(string ExcelFilePath, string TableName)
        {
            if (!File.Exists(ExcelFilePath))
            {
                throw new Exception("Excel文件不存在！");
            }

            //如果数据表名不存在，则数据表名为Excel文件的第一个数据表
            ArrayList TableList = new ArrayList();
            TableList = GetExcelTables(ExcelFilePath);

            if (TableName.IndexOf(TableName) < 0)
            {
                TableName = TableList[0].ToString().Trim();
            }

            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0");
            OleDbCommand cmd = new OleDbCommand("select * from [" + TableName + "$]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
            }
            return table;
        }

        /// <summary>
        /// 获取Excel文件指定数据表的数据列表
        /// </summary>
        /// <param name="ExcelFileName">Excel文件名</param>
        /// <param name="TableName">数据表名</param>
        public static ArrayList GetExcelTableColumns(string ExcelFileName, string TableName)
        {
            DataTable dt = new DataTable();
            ArrayList ColsList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + ExcelFileName))
                {
                    conn.Open();
                    dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, TableName, null });

                    //获取列个数
                    int colcount = dt.Rows.Count;
                    for (int i = 0; i < colcount; i++)
                    {
                        string colname = dt.Rows[i]["Column_Name"].ToString().Trim();
                        ColsList.Add(colname);
                    }
                }
            }
            return ColsList;
        }

        #region aspose导出Excel ——台账
        public static void ExportExcelByAspose(DataTable dt,string path)
        {
            //创建新的EXCELSheet
            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            //ws.FreezePanes(1, 1, 1, 0); //冻结第一行
            Cells cells = ws.Cells;
            //设置格式  
            Aspose.Cells.Style style = wb.CreateStyle();
            style.Font.Size = 15;//文字大小
            style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; //设置居中

            //插入内容
            //第一行
            cells.Merge(0, 0, 1, 20);
            cells[0, 0].PutValue("设备台账");
            cells[0, 0].SetStyle(style);
            cells.SetRowHeight(0, 20.5);//行高
            //cells.SetColumnWidth(0, 3.5);//列宽

            cells[1, 0].PutValue("序号");
            cells[1, 1].PutValue("公司编号");
            cells[1, 2].PutValue("名称");
            cells[1, 3].PutValue("型号");
            cells[1, 4].PutValue("测量范围");
            cells[1, 5].PutValue("准确度");
            cells[1, 6].PutValue("生产厂家");
            cells[1, 7].PutValue("出厂编号");
            cells[1, 8].PutValue("出厂日期");
            cells[1, 9].PutValue("管理状态");
            cells[1, 10].PutValue("使用部门");
            cells[1, 11].PutValue("使用地点");
            cells[1, 12].PutValue("检定部门");
            cells[1, 13].PutValue("检定周期");
            cells[1, 14].PutValue("检定日期");
            cells[1, 15].PutValue("有效日期");
            cells[1, 16].PutValue("检定结果");
            cells[1, 17].PutValue("ABC");
            cells[1, 18].PutValue("数量");
            cells[1, 19].PutValue("备注");

            cells[2, 0].PutValue("No.");
            cells[2, 1].PutValue("ID.No");
            cells[2, 2].PutValue("Name");
            cells[2, 3].PutValue("Model");
            cells[2, 4].PutValue("Measure Range");
            cells[2, 5].PutValue("Accuracy");
            cells[2, 6].PutValue("Produced By");
            cells[2, 7].PutValue("MFG.No");
            cells[2, 8].PutValue("Produce Date");
            cells[2, 9].PutValue("Management State");
            cells[2, 10].PutValue("Using Department");
            cells[2, 11].PutValue("Using place");
            cells[2, 12].PutValue("Calibrated By");
            cells[2, 13].PutValue("Test Frequency");
            cells[2, 14].PutValue("Calibrated Date");
            cells[2, 15].PutValue("Due Date");
            cells[2, 16].PutValue("Result");
            cells[2, 17].PutValue("Color ID");
            cells[2, 18].PutValue("Quantity");
            cells[2, 19].PutValue("Remark");
            int i = 1;
            if (dt.Rows.Count > 0)
            {
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    cells[3 + n, 0].PutValue(i);
                    cells[3 + n, 1].PutValue(dt.Rows[n]["asset_num"]);
                    cells[3 + n, 2].PutValue(dt.Rows[n]["sample_name"]);
                    cells[3 + n, 3].PutValue(dt.Rows[n]["sample_type"]);
                    cells[3 + n, 4].PutValue(dt.Rows[n]["measuring_range"]);
                    cells[3 + n, 5].PutValue(dt.Rows[n]["Accuracy"]);
                    cells[3 + n, 6].PutValue(dt.Rows[n]["manufacturers"]);
                    cells[3 + n, 7].PutValue(dt.Rows[n]["factory_num"]);
                    cells[3 + n, 8].PutValue(dt.Rows[n]["factory_date"]);
                    cells[3 + n, 9].PutValue(dt.Rows[n]["management_state"]);
                    cells[3 + n, 10].PutValue(dt.Rows[n]["customer_name"]);
                    cells[3 + n, 11].PutValue(dt.Rows[n]["using_place"]);
                    cells[3 + n, 12].PutValue(dt.Rows[n]["verification_unit"]);
                    cells[3 + n, 13].PutValue(dt.Rows[n]["verification_cycle"]);
                    cells[3 + n, 14].PutValue(dt.Rows[n]["verification_date"]);
                    cells[3 + n, 15].PutValue(dt.Rows[n]["verification_effective_date"]);
                    cells[3 + n, 16].PutValue(dt.Rows[n]["verification_result"]);
                    cells[3 + n, 17].PutValue(dt.Rows[n]["meterage_type"]);
                    cells[3 + n, 18].PutValue(dt.Rows[n]["count"]);
                    cells[3 + n, 19].PutValue(dt.Rows[n]["remarks"]);
                    i++;
                }
            }
            
                

            ws.AutoFitColumns();//让各列自适应宽度，这个很有用
            //string path = Path.GetTempFileName();
            //Session["FailMsg"] = path;
            wb.Save(path);
        }
        #endregion

        #region aspose导出Excel ——标准物质
        public static void ExportExcelByAspose_standard(DataTable dt, string path)
        {
            //创建新的EXCELSheet
            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            //ws.FreezePanes(1, 1, 1, 0); //冻结第一行
            Cells cells = ws.Cells;
            //设置格式  
            Aspose.Cells.Style style = wb.CreateStyle();
            style.Font.Size = 15;//文字大小
            style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; //设置居中

            //插入内容
            //第一行
            cells.Merge(0, 0, 1, 20);
            cells[0, 0].PutValue("标准物质");
            cells[0, 0].SetStyle(style);
            cells.SetRowHeight(0, 20.5);//行高
            //cells.SetColumnWidth(0, 3.5);//列宽

            cells[1, 0].PutValue("序号");
            cells[1, 1].PutValue("名称");
            cells[1, 2].PutValue("生产厂家");
            cells[1, 3].PutValue("规格");
            cells[1, 4].PutValue("型号");
            cells[1, 5].PutValue("生产批号");
            cells[1, 6].PutValue("产地");
            cells[1, 7].PutValue("有效日期");
            cells[1, 8].PutValue("数量");
            cells[1, 9].PutValue("维护时间");
            cells[1, 10].PutValue("维护人");
            cells[1, 11].PutValue("状态");
            cells[1, 12].PutValue("说明");

            cells[2, 0].PutValue("No.");
            cells[2, 1].PutValue("name_");
            cells[2, 2].PutValue("manufacturers");
            cells[2, 3].PutValue("type_");
            cells[2, 4].PutValue("model_");
            cells[2, 5].PutValue("Batch_num");
            cells[2, 6].PutValue("Place_Origin");
            cells[2, 7].PutValue("Valid_date");
            cells[2, 8].PutValue("count_");
            cells[2, 9].PutValue("maintenance_date");
            cells[2, 10].PutValue("maintenance_personel");
            cells[2, 11].PutValue("state");
            cells[2, 12].PutValue("remarks");

            int i = 1;
            if (dt.Rows.Count > 0)
            {
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    cells[3 + n, 0].PutValue(i);
                    cells[3 + n, 1].PutValue(dt.Rows[n]["name_"]);
                    cells[3 + n, 2].PutValue(dt.Rows[n]["manufacturers"]);
                    cells[3 + n, 3].PutValue(dt.Rows[n]["type_"]);
                    cells[3 + n, 4].PutValue(dt.Rows[n]["model_"]);
                    cells[3 + n, 5].PutValue(dt.Rows[n]["Batch_num"]);
                    cells[3 + n, 6].PutValue(dt.Rows[n]["Place_Origin"]);
                    cells[3 + n, 7].PutValue(dt.Rows[n]["Valid_date"]);
                    cells[3 + n, 8].PutValue(dt.Rows[n]["count_"]);
                    cells[3 + n, 9].PutValue(dt.Rows[n]["maintenance_date"]);
                    cells[3 + n, 10].PutValue(dt.Rows[n]["maintenance_personel"]);
                    cells[3 + n, 11].PutValue(dt.Rows[n]["state"]);
                    cells[3 + n, 12].PutValue(dt.Rows[n]["remarks"]);
                    i++;
                }
            }



            ws.AutoFitColumns();//让各列自适应宽度，这个很有用
            //string path = Path.GetTempFileName();
            //Session["FailMsg"] = path;
            wb.Save(path);
        }

        #endregion
    }
}