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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string command = Request.QueryString["cmd"];
            switch (command)
            {
                case "load_list": Load_List(); break;//加载datagrid
                case "audit": Audit(); break;//审核

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
                sqlwhere += "and uc.accept_state != " + state + " and uc.review_personnel_word='" + loginAccount + "' ";

            }
            //待编制
            if (history_flag == "0")
            {
                sqlwhere += "and uc.accept_state = " + state + "and uc.review_personnel_word='" + loginAccount + "'";

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
        /// 审核
        /// </summary>
        /// <param name="context"></param>
        public void Audit()
        {
            string id = Request.Params.Get("id");
            string report_id = Request.Params.Get("report_id");
            string report_num = Request.Params.Get("report_num");
            string accept_personnel = Request.Params.Get("accept_personnel");//申请人
            string constitute_personnel = Request.Params.Get("constitute_personnel");//编制人员
            string type = Request.Params.Get("type");//1是通过，0是拒绝
            string login_user = Session["loginAccount"].ToString();
            string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            int result = -1;
            //string insertProcessRecord = "";
            if (type == "1")//通过
            {
                string contract_num = Request.Params.Get("contract_num");
                string messageContent = " 报告编号为：" + report_num + "的无损异常报告审核通过;";
                string message_type = "无损异常报告审核通过";

                string uc_sql = "update TB_NDT_error_Certificate set review_personnel_word_date =@review_personnel_word_date,accept_state = @state where id =@id;insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@accept_personnel,@messageContent,@message_type,@review_personnel_word_date,@login_user);update dbo.TB_NDT_report_title set state_=@state_ where id=@report_id ;insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values (@report_id,@login_user,@DateTime, @NodeResult,@NodeId)";
                try
                {
                    db.BeginTransaction();
                    SqlParameter[] param = 
                    {
                       
                    
                        new SqlParameter("@report_id",report_id),
                        new SqlParameter("@review_personnel_word_date",date),
                        new SqlParameter("@id",id),
                        new SqlParameter("@contract_num",contract_num),
                        new SqlParameter("@state",(int)LosslessReport_EditApplyEnum.YCWC),
                        new SqlParameter("@messageContent",messageContent),
                        new SqlParameter("@message_type",message_type),
                        new SqlParameter("@accept_personnel",accept_personnel),
                        new SqlParameter("@login_user",login_user),
                        //流程记录
                        new SqlParameter("@DateTime",DateTime.Now.ToString()),
                        new SqlParameter("@NodeResult","pass"),
                        new SqlParameter("@NodeId",(int)TB_ProcessRecordNodeIdEnum.AbnReviewToAbnManage),
                        //报告状态
                        new SqlParameter("@state_",(int)LosslessReportStatusEnum.Finish)
                    };

                    try
                    {
                        //SQL语句
                        List<string> SQLStringList = new List<string>();
                        List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                        SQLStringList2.Add(param);
                        SQLStringList.Add(uc_sql);
                        //事务
                        SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                        string loginAccount = Convert.ToString(Session["loginAccount"]);
                        string login_username = Convert.ToString(Session["login_username"]);
                        Operation_log.operation_log_(loginAccount, login_username, "无损异常报告修改申请审核", "通过报告编号为" + report_num + "的异常报告");
                        //发消息
                        Send_message new_message = new Send_message();
                        new_message.send_usercount(accept_personnel, messageContent);
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
                    //db.ExecuteNonQueryByTrans(uc_sql, param);

                    //result = 1;

                    //db.CommitTransacton();


                    //string loginAccount = Convert.ToString(Session["loginAccount"]);
                    //string login_username = Convert.ToString(Session["login_username"]);
                    //Operation_log.operation_log_(loginAccount, login_username, "无损异常报告修改申请审核", "通过id为" + id + "的异常报告");
                }
                catch
                {
                    db.RollbackTransaction();
                    result = -1;
                }

            }
            else
            {
                string return_info = Request.Params.Get("return_info");
                string messageContent = " 报告编号为：" + report_num + "的无损异常报告需要编制;";
                string message_type = "无损异常报告需要编制";
                string uc_sql = "update TB_NDT_error_Certificate set review_personnel_word_date =@review_personnel_word_date,accept_state = @state,review_remarks_word =@review_remarks_word where id =@id ;insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@constitute_personnel,@messageContent,@message_type,@review_personnel_word_date,@login_user);insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId,Remark) values (@id,@login_user,@DateTime, @NodeResult,@NodeId,@review_remarks_word)";
                SqlParameter[] param = 
                {
                
                    new SqlParameter("@review_personnel_word_date",date),
                    new SqlParameter("@id",id),
                    new SqlParameter("@review_remarks_word",return_info),
                    new SqlParameter("@state",(int)LosslessReport_EditApplyEnum.YCTH),
                    new SqlParameter("@messageContent",messageContent),
                    new SqlParameter("@message_type",message_type),
                    new SqlParameter("@constitute_personnel",constitute_personnel),
                    new SqlParameter("@login_user",login_user),
                    //流程记录
                    new SqlParameter("@DateTime",DateTime.Now.ToString()),
                    new SqlParameter("@NodeResult","pass"),
                    new SqlParameter("@NodeId",(int)TB_ProcessRecordNodeIdEnum.AbnReviewToAbnEdit)
                };
               
                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                    SQLStringList2.Add(param);
                    SQLStringList.Add(uc_sql);
                    //事务
                    SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                    string loginAccount = Convert.ToString(Session["loginAccount"]);
                    string login_username = Convert.ToString(Session["login_username"]);
                    Operation_log.operation_log_(loginAccount, login_username, "无损异常报告修改申请审核", "拒绝报告编号为" + report_num + "的异常报告");
                    //发消息
                    Send_message new_message = new Send_message();
                    new_message.send_usercount(constitute_personnel, messageContent);
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

                //result = db.ExecuteNonQuery(uc_sql, param);

                //string loginAccount = Convert.ToString(Session["loginAccount"]);
                //string login_username = Convert.ToString(Session["login_username"]);
                //Operation_log.operation_log_(loginAccount, login_username, "无损异常报告修改申请审核", "拒绝id为" + id + "的异常报告");
            }

            if (result > 0)
                Response.Write("T");
            else
                Response.Write("F");

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