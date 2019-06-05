using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity.LosslessReport
{
    /// <summary>
    /// 批量下载model
    /// </summary>
    public class ReportManagementDownload
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string report_url { get; set; }//需要下载的文件连接
        /// <summary>
        /// 文件名称
        /// </summary>
        public string newfilename { get; set; }//复制后以后新文件名
    }
}
