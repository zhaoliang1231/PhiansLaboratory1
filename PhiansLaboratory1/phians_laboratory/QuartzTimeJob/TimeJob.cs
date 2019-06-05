using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Threading.Tasks;

namespace phians_laboratory.custom_class
{
    public class TimeJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
            //向c:\Quartz.txt写入当前时间并换行
            System.IO.File.AppendAllText(@"d:\Quartz.txt", DateTime.Now + Environment.NewLine);
            await Task.Delay(1);
        }
    }
}