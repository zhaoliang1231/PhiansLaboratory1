using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL
{
    public class ReportInspectBLL
    {
        /// <summary>
        /// 报告查看数据 interface
        /// </summary>
        /// 
        IReportInspectDAL dal = DALFactory.GetReportInspect();

    }
}
