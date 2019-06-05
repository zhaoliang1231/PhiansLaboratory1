using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform.Lossless_report
{
    public partial class LosslessReport_Apply_Management : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string command = Request.QueryString["cmd"];
            switch (command)
            {
                case "load_list": Load_List(); break;//加载datagrid
            }
        }
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();

      

        /// <summary>
        /// 加载列表
        /// </summary>
        /// <param name="context"></param>
        public void Load_List( )
        {
            string login_user = Session["loginAccount"].ToString();

            int page = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            string order = Request.Params.Get("order");
            string sortby = Request.Params.Get("sort");

            string search = Request.Params.Get("search");
            string key = Request.Params.Get("key");

            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;

            string sqlwhere = "";
            if (!string.IsNullOrEmpty(search))
            {
                sqlwhere += " and " + search + " like '%" + key + "%'";
            }

            string sql = "select * from (select row_number()over(order by uc." + sortby + " " + order + @")RowId,uc.*,ui.user_name as review_personnel_n ,ui2.user_name as  accept_personnel_n ,ui3.user_name as constitute_personnel_n ,ui4.user_name as review_personnel_word_n ,TB_NDT_R.report_num,TB_NDT_R.report_name,TB_NDT_R.clientele,TB_NDT_R.clientele_department FROM TB_NDT_error_Certificate as  uc "
             + @"left join tb_user_info as  ui on uc.review_personnel = ui.user_count  "
             + @"left join tb_user_info as  ui2 on uc.accept_personnel = ui2.user_count  "
              + @"left join tb_user_info as  ui3 on uc.constitute_personnel = ui3.user_count  "
              + @"left join tb_user_info as  ui4 on uc.review_personnel_word = ui4.user_count  "
              + @"left join TB_NDT_report_title as  TB_NDT_R on TB_NDT_R.id = uc.report_id  "
               + sqlwhere + " )a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM TB_NDT_error_Certificate  as uc " + sqlwhere + "";
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            Response.Write(strJson);//返回给前台页面   
            Response.End();
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