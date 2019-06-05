using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class AbnormalReportManagementDAL : IAbnormalReportManagementDAL
    {
        #region 获取集合
        public List<TB_NDT_error_Certificate> GetUnusualCertificateListBLL(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            //sql.Append("select * from TB_UnusualCertificate WHERE 1=1 ");

            sql.Append(" select uc.*,ui.user_name as review_personnel_n ,ui2.user_name as  accept_personnel_n ,ui3.user_name as constitute_personnel_n ,ui4.user_name as review_personnel_word_n ,TB_NDT_R.report_num,TB_NDT_R.report_name,TB_NDT_R.clientele,TB_NDT_R.clientele_department,TB_NDT_R.id as ReportId FROM TB_NDT_error_Certificate as  uc");
            sql.Append("left join tb_user_info as  ui on uc.review_personnel = ui.user_count");
            sql.Append("left join tb_user_info as  ui2 on uc.accept_personnel = ui2.user_count");
            sql.Append("left join tb_user_info as  ui3 on uc.constitute_personnel = ui3.user_count");
            sql.Append("left join TB_NDT_report_title as  TB_NDT_R on TB_NDT_R.id = uc.report_id");
            sql.Append("left join tb_user_info as  ui4 on uc.review_personnel_word = ui4.user_count");
            sql.Append(" where (accept_state=@0 or accept_state=@1 )  ", PageModel.SearchList_[0].Key,PageModel.SearchList_[1].Key);


            if (!string.IsNullOrEmpty(PageModel.SearchList_[2].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[2].Search + " like '%" + PageModel.SearchList_[2].Key + "%' ");
            }

            sql.OrderBy(" uc.id desc ");
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_error_Certificate>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion
    }
}
