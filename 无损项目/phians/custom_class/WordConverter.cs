
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Word = Microsoft.Office.Interop.Word; 
namespace phians.custom_class
{
    class WordConverter
    {
        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }
        //构造函数  
        public WordConverter()
        { }
        //// <summary>
        /// 把Word文件转换成pdf文件2
        /// </summary>
        /// <param name="sourcePath">需要转换的文件路径和文件名称</param>
        /// <param name="targetPath">转换完成后的文件的路径和文件名名称</param>
        /// <returns>成功返回true,失败返回false</returns>
        public string WordToPdf(object sourcePath, string targetPath)
        {
              string result = "0";
           Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
           Document document = null;
           try
           {
              application.Visible = false;
              document = application.Documents.Open(sourcePath);
              document.ExportAsFixedFormat(targetPath, WdExportFormat.wdExportFormatPDF);
              result = "1";
           }
           catch (Exception e)
           {
              //Console.WriteLine(e.Message);
               result = e.ToString(); ;
           }
           finally
           {
              document.Close();
           }
           return result;
        }
        //将word文档转换成PDF格式
        Word.WdExportFormat wd = Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF;
        public bool Convertwordtopdf(string sourcePath, string targetPath)
    {
        bool result;
        object paramMissing = Type.Missing;
       
        Word.ApplicationClass wordApplication = new Word.ApplicationClass();
        Word.Document wordDocument = null;
        try
        {
            object paramSourceDocPath = sourcePath;
            string paramExportFilePath = targetPath;



            wordDocument = wordApplication.Documents.Open(
                    ref paramSourceDocPath);

            if (wordDocument != null)
                wordDocument.ExportAsFixedFormat(paramExportFilePath, wd);
            wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
            wordDocument = null;
            wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
           
            wordApplication = null;
            result = true;
        }
        catch
        {

            result = false;

        }

        finally
        {

            if (wordDocument != null)
            {
                wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
                wordDocument = null;
            }
            if (wordApplication != null)
            {
                wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
                wordApplication = null;
            }
        }
       
        return result;
    }



        }

    }
