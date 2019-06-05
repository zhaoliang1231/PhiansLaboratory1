using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform.Lossless_report
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            string command = Request.QueryString["cmd"];
            switch (command)
            {
                case "load_list": Load_List(); break;//加载datagrid
                case "get_report_url": Get_Report_Url(); break;//获取报告链接
                case "submit_audit": Submint_Audit(); break; //提交审核
            }
        }
          private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();


        /// <summary>
        /// 加载列表
        /// </summary>
        /// <param name="context"></param>
        public void Load_List()
        {
            int state = (int)LosslessReport_EditApplyEnum.YCSH;
            int state0 = (int)LosslessReport_EditApplyEnum.YCBZ;
            int state2 = (int)LosslessReport_EditApplyEnum.YCTH;
            int state3 = (int)LosslessReport_EditApplyEnum.YCWC;
            int page = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            string order = Request.Params.Get("order");
            string sortby = Request.Params.Get("sort");

            string search = Request.Params.Get("search");
            string key = Request.Params.Get("key");
            string history_flag = Request.Params.Get("history_flag");
            string loginAccount = Convert.ToString(Session["loginAccount"]);
            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;

            string sqlwhere = "where 1=1 ";
            //历史编制
            if (history_flag == "1")
            {
                sqlwhere += "and (uc.accept_state = " + state + " or uc.accept_state = " + state3 + ") and uc.constitute_personnel='" + loginAccount + "' ";

            }
            //待编制
            if (history_flag == "0")
            {
                sqlwhere += "and (uc.accept_state = " + state0 + "or uc.accept_state = " + state2 + ")  and uc.constitute_personnel='" + loginAccount + "' ";

            }
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

        /// <summary>
        /// 获取报告链接
        /// </summary>
        /// <param name="context"></param>
        public void Get_Report_Url()
        {
            string report_id = Request.Params.Get("report_id");
            string report_url = "";
            //判断证书是否存在
            string getReportUrlSql = "select report_url from TB_NDT_report_title where id =" + report_id;
            report_url = db.ExecuteScalar(getReportUrlSql).ToString();
            if (report_url == "")
            {
                Response.Write("F");
            }
            else
            {
                Response.Write(report_url);
            }

            Response.End();
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="context"></param>
        public void Submint_Audit()
        {
            string id = Request.Params.Get("id");
            string login_user = Session["loginAccount"].ToString();
            string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            int state = (int)LosslessReport_EditApplyEnum.YCSH;
            string review_personnel_word = Request.Params.Get("review_personnel");
            string report_num = Request.Params.Get("report_num");

            string messageContent = " 报告编号为：" + report_num + "的无损异常报告需要审核;";
            string message_type = "无损异常报告需要审核";
            if (string.IsNullOrEmpty(review_personnel_word)) {

                Response.Write("F");

                Response.End();
            }

            string sql = "update TB_NDT_error_Certificate set accept_state=@state,constitute_date=@constitute_date,review_personnel_word=@review_personnel_word where id =@id;insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@review_personnel_word,@messageContent,@message_type,@constitute_date,@login_user);insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values (@id,@login_user,@DateTime, @NodeResult,@NodeId)";

            SqlParameter[] param = 
            {
                new SqlParameter("@state",state),
                new SqlParameter("@review_personnel_word",review_personnel_word),
                new SqlParameter("@constitute_date",date),
                new SqlParameter("@id",id),
                new SqlParameter("@messageContent",messageContent),
                new SqlParameter("@message_type",message_type),
                new SqlParameter("@login_user",login_user),
                 //流程记录
                 new SqlParameter("@DateTime",DateTime.Now.ToString()),
                 new SqlParameter("@NodeResult","pass"),
                 new SqlParameter("@NodeId",(int)TB_ProcessRecordNodeIdEnum.AbnEditToAbnReview)
            };

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                SQLStringList2.Add(param);
                SQLStringList.Add(sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                string loginAccount = Convert.ToString(Session["loginAccount"]);
                string login_username = Convert.ToString(Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "异常报告申请编制", "通过报告编号为" + report_num + "的异常报告");
                //发消息
                Send_message new_message = new Send_message();
                new_message.send_usercount(review_personnel_word, messageContent);
                Response.Write("T");
            }
            catch (Exception)
            {
                Response.Write("F");
            }
            finally
            {
                Response.End();
            }

            //int result = db.ExecuteNonQuery(sql, param);
            //if (result > 0)
            //    Response.Write("T");
            //else
            //    Response.Write("F");

            //Response.End();
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