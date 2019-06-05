using Aspose.Words;
using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace phians.test
{
    /// <summary>
    /// tes_word_create 的摘要说明
    /// </summary>
    public class tes_word_create : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.Params.Get("cmd");
            switch (command)
            {
                  //  加工零件精度检查单
                case "test": test(context); break;
                //  加 工 零 件 检 验 记 录 表
                case "test2": test2(context); break;
            }
        }
       // 加 工 零 件 检 验 记 录 表
        public void  test2( HttpContext context){

            //定义报告新文件名
            string new_doc_name = Guid.NewGuid().ToString() + ".doc";
            //文件夹路径
            string file_path = ConfigurationManager.AppSettings["view_temp_Folder"].ToString();
            // 模板绝对路径
            string modelMapPath = context.Server.MapPath("/Upload/2.doc");
            //报告绝对路径
            //报告相对路径
            string report_path = file_path + new_doc_name;
            string reportMapPath = context.Server.MapPath(report_path);

           //复制模板
            try
            {
                System.IO.File.Copy(modelMapPath, reportMapPath, true);
            }
            catch
            {
                context.Response.Write("模板不存在！");
                context.ApplicationInstance.CompleteRequest();
                return;
            }
            //创建word对象
            Document contract_doc = null;
            contract_doc = new Document(reportMapPath);
            DocumentBuilder docBuild = new DocumentBuilder(contract_doc);
            NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);

            DataTable basicdata = new DataTable();//表格数据

            DataTable testdata = new DataTable();//表格数据
            DataColumn dc = null;
            dc = testdata.Columns.Add("S_Number"); //序号
            dc = testdata.Columns.Add("Dimensioning");//检测项目
            dc = testdata.Columns.Add("DimSizeGoal"); //加工目标尺寸
            dc = testdata.Columns.Add("MeasureFrequency");//记录频次
            dc = testdata.Columns.Add("MeasureValue");
            DataRow newRow;
            for (int i = 0; i < 10; i++)
            {
                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "1";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "2";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "3";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "4";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "5";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "6";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "7";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "8";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "9";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "10";
                testdata.Rows.Add(newRow);
                //11
                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "11";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "12";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "13";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "14";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "15";
                testdata.Rows.Add(newRow);
                if (i >= 6)
                {
                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "16";
                testdata.Rows.Add(newRow);

                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "17";
                testdata.Rows.Add(newRow);
              
                newRow = testdata.NewRow();
                newRow["S_Number"] = i;
                newRow["Dimensioning"] = "2";
                newRow["DimSizeGoal"] = "3";
                newRow["MeasureFrequency"] = "4";
                newRow["MeasureValue"] = "18";
                testdata.Rows.Add(newRow);
                }
            }
          
            IList<Data_Model> testDataList = CollectionHelper.ConvertTo<Data_Model>(testdata);
            //零件id
            List<string> S_Numberlist = testDataList.GroupBy(n => n.S_Number).Select(n => n.Key).ToList();
            //检测项目
            List<string> valueTypeList = testDataList.GroupBy(n => n.Dimensioning).Select(n => n.Key).ToList();
           
            create_word new_doc_ = new create_word();

            new_doc_.make_word(contract_doc, S_Numberlist, testDataList, 18, valueTypeList,basicdata);


            contract_doc.Save(reportMapPath, Aspose.Words.SaveFormat.Doc);
            context.Response.Write("T");

        
        }

       // 加工零件精度检查单
        public void test(HttpContext context)
        {

            //定义报告新文件名
            string new_doc_name = Guid.NewGuid().ToString() + ".doc";
            //文件夹路径
            string file_path = ConfigurationManager.AppSettings["view_temp_Folder"].ToString();
            // 模板绝对路径
            string modelMapPath = context.Server.MapPath("/Upload/1.doc");
            //报告绝对路径
            //报告相对路径
            string report_path = file_path + new_doc_name;
            string reportMapPath = context.Server.MapPath(report_path);
           
       

       

            //复制模板
            try
            {
                System.IO.File.Copy(modelMapPath, reportMapPath, true);
            }
            catch
            {
                context.Response.Write("模板不存在！");
                context.ApplicationInstance.CompleteRequest();
                return;
            }
            //创建word对象
            Document contract_doc = null;
            contract_doc = new Document(reportMapPath);
            DocumentBuilder docBuild = new DocumentBuilder(contract_doc);
            NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);

            DataTable tblDatas = new DataTable();//表格数据
            DataTable Basic_Data = new DataTable();//基础数据
            DataColumn dc = null;
            dc = tblDatas.Columns.Add("S_Number");
            dc = tblDatas.Columns.Add("Dimension");
            dc = tblDatas.Columns.Add("TopLower");
            dc = tblDatas.Columns.Add("Measure");
            dc = tblDatas.Columns.Add("JerkValue");
            DataRow newRow;

            newRow = tblDatas.NewRow();
            newRow["S_Number"] = "1";
            newRow["Dimension"] = "x";
            newRow["TopLower"] = "xb";
            newRow["Measure"] = "xbb";
            newRow["JerkValue"] = "xbbb";
            tblDatas.Rows.Add(newRow);

            newRow = tblDatas.NewRow();
            newRow["S_Number"] = "2";
            newRow["Dimension"] = "x";
            newRow["TopLower"] = "xb";
            newRow["Measure"] = "xbb";
            newRow["JerkValue"] = "xbbb";
            tblDatas.Rows.Add(newRow);

            newRow = tblDatas.NewRow();
            newRow["S_Number"] = "3";
            newRow["Dimension"] = "x";
            newRow["TopLower"] = "xb";
            newRow["Measure"] = "xbb";
            newRow["JerkValue"] = "xbbb";
            tblDatas.Rows.Add(newRow);

            newRow = tblDatas.NewRow();
            newRow["S_Number"] = "4";
            newRow["Dimension"] = "x";
            newRow["TopLower"] = "xb";
            newRow["Measure"] = "xbb";
            newRow["JerkValue"] = "xbbb";
            tblDatas.Rows.Add(newRow);
            create_word new_doc_ = new create_word();

            new_doc_.create_word2(contract_doc, tblDatas,Basic_Data);
            contract_doc.Save(reportMapPath, Aspose.Words.SaveFormat.Doc);
            context.Response.Write("T");
        
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