using Aspose.Cells;
using Ionic.Zip;
using Newtonsoft.Json;
using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;


namespace phians.mainform
{
    /// <summary>
    /// barcode 的摘要说明
    /// </summary>
    public class barcode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.QueryString["cmd"];
            switch (command)
            {
                case "printbarcode": PrintBarCode(context); break;//批量导入
                case "exportexcel": ExportExcel(context); break;//导出
                case "test": GetData(); break;
            }
        }
        private void PrintBarCode(HttpContext context)
        {
            string strs = context.Request.Params.Get("barcodestrs");
            List<string> url = new BarCode().GetImageUrl(strs);
            string json = JsonConvert.SerializeObject(url);
            context.Response.Write(json);
            context.Response.End();
        }

        private void ExportExcel(HttpContext context)
        {

        }


        public void Export(DataTable dt)
        {
            Workbook dataWorkbook = new Workbook();
            Worksheet dataWorksheet = dataWorkbook.Worksheets[0];
            Workbook modelWorkbook = new Workbook(HttpContext.Current.Server.MapPath(@"~/Upload/Model/出库单.xlsx"));
            Worksheet modelWorksheet = modelWorkbook.Worksheets[0];
            Cells datacells = dataWorksheet.Cells;
            Cells modelcells = modelWorksheet.Cells;

            //复制表格
            if (dt.Rows.Count > 0)
            {
                int i = (dt.Rows.Count / 12) + 1;//几页
                for (int j = 0; j < i; j++)
                {
                    datacells.CopyRows(modelcells, 0, 12 * j, 22);
                    datacells[3 + 22 * i, 0].PutValue("");//
                    datacells[4 + 22 * i, 0].PutValue("委托日期：" + "");//委托日期
                    datacells[4 + 22 * i, 2].PutValue("委托部门：" + "");//委托部门
                    datacells[4 + 22 * i, 7].PutValue("借件人：" + "");//委托部门
                }
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    int a = n / 12; int b = n % 12;//取某页某行的数据
                    datacells[6 + a * 22 + b, 0].PutValue(n + 1);//序号
                    datacells[6 + a * 22 + b, 1].PutValue(dt.Rows[n]["name"]);//名称
                    datacells[6 + a * 22 + b, 2].PutValue("");//规格
                    datacells[6 + a * 22 + b, 3].PutValue("");//出厂编号
                    datacells[6 + a * 22 + b, 4].PutValue("");//公司编号
                    datacells[6 + a * 22 + b, 5].PutValue("");//数量
                    datacells[6 + a * 22 + b, 6].PutValue("");//委托人
                    datacells[6 + a * 22 + b, 7].PutValue("");//取件日期
                    datacells[6 + a * 22 + b, 8].PutValue("");//去见人
                    datacells[6 + a * 22 + b, 9].PutValue("");//备足

                }

            }
            dataWorkbook.Save(HttpContext.Current.Server.MapPath(@"~/Upload/Model/") + "exportexcel.xlsx");


        }

        public void GetData() 
        {
            string sql = "select * from tb_standard_library_info where acceptance_standard =  '10001'";
            DataTable dt = new DBHelper().ExecuteDataTable(sql);
            List<StandardData> list = new HandleData().GetAllData(dt);
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