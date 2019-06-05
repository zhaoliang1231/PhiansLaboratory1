using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace phians.mainform.Lossless_report
{
    public partial class LosslessReport_Apply_Audit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string command = Request.QueryString["cmd"];
            switch (command)
            {
                case "load_list": Load_List(); break;//加载datagrid
                case "audit": Audit(); break;//审核
                case "view_report": view_report(); break;//查看报告
                case "check_url": check_url(); break;//查看报告
            }
        }



        
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();


        public void check_url() {
            string File_url = Request.Params.Get("File_url");
            if (File.Exists(Server.MapPath(File_url)))
            {
                Response.Write(File_url);

            }
            else { Response.Write("F"); }
            Response.End();
      
        
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public void Load_List()
        {
            int state = (int)LosslessReport_EditApplyEnum.SQSH;
            int state2 = (int)LosslessReport_EditApplyEnum.BFSH;

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
            //历史审核
            if (history_flag == "1")
            {
                sqlwhere += "and (uc.accept_state != " + state + " and uc.accept_state != " + state2 + ") and uc.review_personnel='" + loginAccount + "' ";

            }
            //待审核
            if (history_flag == "0")
            {
                sqlwhere += "and (uc.accept_state = " + state + " or uc.accept_state = " + state2 + ")";

            }
            if (!string.IsNullOrEmpty(search))
            {
                sqlwhere += " and " + search + " like '%" + key + "%'";
            }

            string sql = "select * from (select row_number()over(order by uc." + sortby + " " + order + @")RowId,uc.*,ui.user_name as review_personnel_n ,ui2.user_name as  accept_personnel_n ,TB_NDT_R.report_num,TB_NDT_R.report_name,TB_NDT_R.clientele,TB_NDT_R.clientele_department FROM TB_NDT_error_Certificate as  uc "
              +" left join tb_user_info as  ui on uc.review_personnel = ui.user_count  "
              + " left join tb_user_info as  ui2 on uc.accept_personnel = ui2.user_count  "
               + " left join TB_NDT_report_title as  TB_NDT_R on TB_NDT_R.id = uc.report_id  " 
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
        public void Audit( )
        {
            string report_num = Request.Params.Get("report_num");
            string report_id = Request.Params.Get("report_id");
            string constitute_personnel = Request.Params.Get("constitute_personnel");
            string accept_personnel = Request.Params.Get("accept_personnel");

            string login_user = Session["loginAccount"].ToString();
            string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string id = Request.Params.Get("id");
            string type = Request.Params.Get("type");//1为通过，0为拒绝
            string flag = Request.Params.Get("flag");//"Scrap"为报废，""为修改
            int NodeId ;
            //int result = -1;
            if (type == "1")
            {
                string messageContent = "";
                string message_type = "";
                int state = 0; //异常申请状态
                int state_ = (int)LosslessReportStatusEnum.Scrap; // 报告状态——报废
                string message = "";
                string sql = "";
                if (flag == "Scrap")
                {
                    messageContent = " 报告编号为：" + report_num + "的无损异常报告报废完成;";
                    message_type = "无损异常报告报废完成";
                    state=(int)LosslessReport_EditApplyEnum.BFWC;
                    message = "异常报告报废申请审核通过";

                    sql = sql + ";update dbo.TB_NDT_report_title set state_=@state_ where id=@report_id;";

                    //报告流程：报废通过
                    NodeId = (int)TB_ProcessRecordNodeIdEnum.BFApplyToManage;

                }
                else {
                    messageContent = " 报告编号为：" + report_num + "的无损异常报告需要编制;";
                    message_type = "无损异常报告需要编制";
                    state=(int)LosslessReport_EditApplyEnum.YCBZ;
                    message = "异常报告修改申请审核通过";

                    //报告流程：异常通过
                    NodeId = (int)TB_ProcessRecordNodeIdEnum.ApplyToAbnEdit;
                }
                    SqlParameter[] param = 
                    {
                        //报告
                        new SqlParameter("@report_id",report_id),
                         new SqlParameter("@state_",state_),
                        //流程记录
                         new SqlParameter("@DateTime",DateTime.Now.ToString()),
                         new SqlParameter("@NodeResult","pass"),
                         new SqlParameter("@NodeId",NodeId),
                        //异常
                        new SqlParameter("@review_personnel",login_user),
                        new SqlParameter("@review_date",date),
                       new SqlParameter("@accept_state",state),
                        new SqlParameter("@id",id),
                        new SqlParameter("@message_type",message_type),
                        new SqlParameter("@messageContent",messageContent),
                        new SqlParameter("@constitute_personnel",constitute_personnel)
                    };


                    sql += "update TB_NDT_error_Certificate set review_personnel=@review_personnel,review_date=@review_date,accept_state=@accept_state where id =@id;insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@constitute_personnel,@messageContent,@message_type,@review_date,@review_personnel);insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values (@report_id,@review_personnel,@DateTime, @NodeResult,@NodeId)";
                    //result = db.ExecuteNonQuery(sql, param);


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
                        Operation_log.operation_log_(loginAccount, login_username, message, "通过报告编号为" + report_num + "的异常报告");
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

            }
            else if (type == "0")//拒绝
            {
                string messageContent = "";
                string message_type = "";
                string message = "";
                int state = 0;//异常状态
                int state_ = (int)LosslessReportStatusEnum.Finish; // 报告状态——报废

                if (flag == "Scrap")
                {
                    messageContent = " 报告编号为：" + report_num + "的无损异常报告报废审核退回;";
                    message_type = "无损异常报告报废审核退回";
                    state = (int)LosslessReport_EditApplyEnum.BFTH;
                    message = "异常报告报废申请审核拒绝";
                }
                else
                {
                    messageContent = " 报告编号为：" + report_num + "的无损异常报告审核退回;";
                    message_type = "无损异常报告审核退回";
                    state = (int)LosslessReport_EditApplyEnum.SQTH;
                    message = "异常报告修改申请审核拒绝";
                }
                string return_info = Request.Params.Get("return_info");
                //int state = (int)LosslessReport_EditApplyEnum.SQTH;
                SqlParameter[] param = 
                {
                    //报告
                        new SqlParameter("@report_id",report_id),
                         new SqlParameter("@state_",state_),
                          //流程记录
                 new SqlParameter("@DateTime",DateTime.Now.ToString()),
                 new SqlParameter("@NodeResult","pass"),
                 new SqlParameter("@NodeId",(int)TB_ProcessRecordNodeIdEnum.ApplyToManage),
                        //异常
                    new SqlParameter("@review_personnel",login_user),
                    new SqlParameter("@review_date",date),
                    new SqlParameter("@return_info",return_info),
                    new SqlParameter("@accept_state",state),
                    new SqlParameter("@id",id),
                        new SqlParameter("@message_type",message_type),
                        new SqlParameter("@messageContent",messageContent),
                        new SqlParameter("@accept_personnel",accept_personnel)
                };

                string sql = "update TB_NDT_error_Certificate set review_personnel=@review_personnel,review_date=@review_date,accept_state=@accept_state,review_remarks=@return_info where id =@id;insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@accept_personnel,@messageContent,@message_type,@review_date,@review_personnel);update dbo.TB_NDT_report_title set state_=@state_ where id=@report_id;insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values (@report_id,@accept_personnel,@DateTime, @NodeResult,@NodeId)";
                //result = db.ExecuteNonQuery(sql, param);
               
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
                    Operation_log.operation_log_(loginAccount, login_username, message, "拒绝报告编号为" + report_num + "的异常报告");
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
                //string loginAccount = Convert.ToString(Session["loginAccount"]);
                //string login_username = Convert.ToString(Session["login_username"]);
                //Operation_log.operation_log_(loginAccount, login_username, "异常报告申请审核", "拒绝id为" + id + "的异常报告");
            }

            //if (result > 0)
            //    Response.Write("T");
            //else
            //    Response.Write("F");
            //Response.End();


        }

        /// <summary>
        /// 预览报告
        /// </summary>
        public void view_report( )
        {
            string id = Request.Params.Get("id");

            //判断证书是否存在
            string sql = "select file_url from dbo.TB_unusual_Certificate where id ='" + id + "'";
            string File_url = db.ExecuteScalar(sql).ToString();
            if (File_url == "")
            {
                Response.Write("F");
            }
            else
            {
                Response.Write(File_url);

            }

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