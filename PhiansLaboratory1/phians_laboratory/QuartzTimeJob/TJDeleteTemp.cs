using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;

namespace phians_laboratory.QuartzTimeJob
{
   /// <summary>
   /// 删除所有临时文件
   /// </summary>
    public class TJDeleteTemp:IJob
    {


        public async Task Execute(IJobExecutionContext context)
        {
         //删除所有临时文件
            string view_tempPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Path.Combine("view_temp")); //

            // System.IO.File.AppendAllText(@"d:\Quartz.txt", DateTime.Now + Environment.NewLine );
            // 查找临时文件夹
            try
            {
                DirectoryInfo dir = new DirectoryInfo(view_tempPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            await Task.Delay(1);
        }
    }
}