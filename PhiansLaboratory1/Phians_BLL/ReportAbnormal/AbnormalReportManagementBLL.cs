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
   public class AbnormalReportManagementBLL
    {
       IAbnormalReportManagementDAL dal = DALFactory.GetAbnormalManagement();

        public List<TB_NDT_error_Certificate> GetUnusualCertificateListBLL(TPageModel PageModel, out int totalRecord)
        {
            return dal.GetUnusualCertificateListBLL(PageModel, out totalRecord);
        }
    }
}
