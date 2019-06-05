using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Words;
using System.Windows;
using System.Configuration;
using phians.custom_class;
using Aspose.Words.Tables;
using phians.Model;
using Aspose.Words.Drawing;
using System.Collections;
using Ionic.Zip;
using System.Text.RegularExpressions;
using Aspose.Words.Replacing;

namespace phians.mainform.Lossless_report
{
    /// <summary>
    /// LosslessReport_Management 的摘要说明
    /// </summary>
    public class LosslessReport_Management : IHttpHandler, IRequiresSessionState
    {
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.QueryString["cmd"];
            switch (command)
            {
                case "load_list": load_list(context); break;//加载报告datagrid
                case "Update_Report_Apply": Update_Report_Apply(context); break;//异常报告申请
                case "Report_Arrange": report_arrange(context); break;//获取统计数据
                case "select_person": select_person(context); break;//获取用户
                case "select_group": select_group(context); break;//获取用户
                case "download": download(context); break;//批量下载
                case "download_word": download_word(context); break;//下载word版
                case "Preview_Report": Preview_Report(context); break;//预览报告


            }
        }

        //查找用户信息
        public void select_person(HttpContext context)
        {

            string error_personne = context.Request.Params.Get("comboxgroup").ToString();

            string sql = "select U.User_name,U.User_count from TB_user_info as U,TB_user_department as D where D.Department_name ='" + error_personne + "' and D.User_count=U.User_count";
            DataTable dt = db.ExecuteDataTable(sql);
            string strJson = jsonHelper.DataTable2ToJson(dt);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();

        }


        public void select_group(HttpContext context)
        {
            string strsql = "select id as User_count,Department_name as User_name  from dbo.TB_department where id!=10";
            DataTable dt = db.ExecuteDataTable(strsql);
            string strJson = jsonHelper.DataTable2ToJson(dt);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();

        }
        public DataTable get_value(string search, string search1, string search2, string search3, string key, string key1, string key2, string key3)
        {

            string sqlwhere = "where report_url!=''";

            //查询
            //查询
            if (!string.IsNullOrEmpty(search))
            {
                if (search == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), " + search + ",120) like '%" + key + "%'";
                }
                else
                {
                    sqlwhere += " and " + search + " like '%" + key + "%'";
                }
            }
            if (!string.IsNullOrEmpty(search1))
            {
                if (search1 == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), " + search1 + ",120) like '%" + key + "%'";
                }
                else
                {
                    sqlwhere += " and " + search1 + " like '%" + key1 + "%'";
                }
            }
            if (!string.IsNullOrEmpty(search2))
            {
                if (search2 == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), " + search2 + ",120) like '%" + key + "%'";
                }
                else
                {
                    sqlwhere += " and " + search2 + " like '%" + key2 + "%'";
                }
            }
            if (!string.IsNullOrEmpty(search3))
            {
                if (search3 == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), " + search3 + ",120) like '%" + key3 + "%'";
                }
                else
                {
                    sqlwhere += " and " + search3 + " like '%" + key3 + "%'";
                }
            }

            string sql = "select report_url,report_num  from TB_NDT_report_title  " + sqlwhere;
            DataTable dt = db.ExecuteDataTable(sql);
            return dt;



        }

        public void download(HttpContext context)
        {

            List<string> report_url = new List<string>();//需要下载的文件名字
            List<string> newfilename = new List<string>();//复制后以后新文件名
            string DownloadCheck = context.Request.Params.Get("DownloadCheck");//下载模式

            string search = context.Request.Params.Get("search");//搜索下载条件
            string search1 = context.Request.Params.Get("search1");
            string search2 = context.Request.Params.Get("search2");
            string search3 = context.Request.Params.Get("search3");
            string key = context.Request.Params.Get("key");
            string key1 = context.Request.Params.Get("key1");
            string key2 = context.Request.Params.Get("key2");
            string key3 = context.Request.Params.Get("key3");

            //下载条件 0选择下载 1搜索下载
            if (DownloadCheck == "0")
            {
                string[] report_url_count = context.Request.Params.Get("report_urls").Split(',');//获取选择行的文件路径
                string[] ids = context.Request.Params.Get("ids").Split(',');
                string sqlwhere = "";
                for (int i = 0; i < ids.Length; i++)
                {
                    sqlwhere += ids[i] + ",";
                }
                sqlwhere = sqlwhere.Substring(0, sqlwhere.Length - 1);
                string sql = "select report_url,report_num from TB_NDT_report_title where id in(" + sqlwhere + ") ";
                DataTable select_dt = db.ExecuteDataTable(sql);
                for (int i = 0; i < select_dt.Rows.Count; i++)
                {
                    string newreport_url = select_dt.Rows[i]["report_url"].ToString().Substring(1, select_dt.Rows[i]["report_url"].ToString().Length - 1);
                    report_url.Add(newreport_url);
                    newfilename.Add((select_dt.Rows[i]["report_num"].ToString()).Replace("/", "_") + ".pdf");
                }

            }
            else if (DownloadCheck == "1")
            {
                DataTable select_dt = get_value(search, search1, search2, search3, key, key1, key2, key3);
                for (int i = 0; i < select_dt.Rows.Count; i++)
                {
                    string newreport_url = select_dt.Rows[i]["report_url"].ToString().Substring(0, select_dt.Rows[i]["report_url"].ToString().Length);
                    report_url.Add(newreport_url.Replace("/", @"\"));
                    newfilename.Add((select_dt.Rows[i]["report_num"].ToString()).Replace("/", "_") + ".pdf");
                }
            }
            else
            {
                context.Response.Write("F");
                context.Response.End();
            }
            //临时存放需要复制的文件夹
            string rootUrl = ConfigurationManager.AppSettings["view_temp_Folder"].ToString() + DateTime.Now.ToString("yyyymmddhhmmss");
            Directory.CreateDirectory(context.Server.MapPath(rootUrl));
            //  
            string path = System.AppDomain.CurrentDomain.BaseDirectory;


            //循环复制文件
            for (int i = 0; i < report_url.Count; i++)
            {

                //拷贝文件(路径+copy的文件,拷贝到的路径+新文件名)
                //System.IO.File.Copy(path + report_url[i], context.Server.MapPath(rootUrl + "/" + newfilename[i]), true);
                //WordConverter new_word = new WordConverter();

                //string flag = new_word.WordToPdf(context.Server.MapPath(report_url), context.Server.MapPath(file_path + new_doc_name));
                // 转换pdf
                //bool flag = new_word.Convertwordtopdf(context.Server.MapPath(rootUrl + "/" + newfilename[i]), context.Server.MapPath(rootUrl + "/" + newfilename[i].ToString().Replace("doc", "pdf")));
                Aspose.Words.Document contract_doc = null;
                contract_doc = new Aspose.Words.Document(path + report_url[i]);
                contract_doc.Save(context.Server.MapPath(rootUrl + "/" + newfilename[i].ToString()), SaveFormat.Pdf);

            }

            //临时文件夹
            // string zipurl = ConfigurationManager.AppSettings["view_temp_Folder"].ToString();
            string savePath = ConfigurationManager.AppSettings["view_temp_Folder"].ToString() + DateTime.Now.ToString("yyyymmdd") + ".zip";

            try
            {
                using (var zip = new ZipFile(Encoding.Default))
                {
                    //把待添加文件添加到压缩包
                    zip.AddDirectory(context.Server.MapPath(rootUrl));
                    //保存压缩包
                    zip.Save(context.Server.MapPath(savePath)); //生成包的名称
                }
                context.Response.Write(savePath);
                //删除临时文件夹
                Directory.Delete(context.Server.MapPath(rootUrl), true);
            }
            catch (Exception ex)
            {

                context.Response.Write("F");

            }
            finally
            {
                context.Response.End();

            }

        }

        public void download_word(HttpContext context)
        {

            string file_Url = context.Request.Params.Get("File_Url");
           string  file_newUrl = file_Url.Remove(32);
            string report_num = context.Request.Params.Get("report_num");
            //string report_format = context.Request.Params.Get("report_format");
            report_num = report_num.Replace('/', '_');
            //拷贝文件(路径+copy的文件,拷贝到的路径+新文件名)
            if (!string.IsNullOrEmpty(file_Url) || !string.IsNullOrEmpty(report_num) )
            {
                System.IO.File.Copy(context.Server.MapPath(file_Url), context.Server.MapPath(file_newUrl + "/" + report_num + ".doc"), true);
                context.Response.Write(file_newUrl + "/" + report_num + ".doc");
                context.Response.End();
            }
            else {
                context.Response.Write("下载失败");
                context.Response.End();
            }

            
        }

        #region   修改申请

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        private void Update_Report_Apply(HttpContext context)
        {
            string flag = context.Request.Params.Get("flag");//判断是否为报废申请
            string report_id = context.Request.Params.Get("report_id");//报告id
            int state = (int)LosslessReportStatusEnum.Abnormal;//报告异常申请状态

            //异常判断是否重复申请
            string checksql = "select count(0) from TB_NDT_error_Certificate where accept_state !=" + (int)LosslessReport_EditApplyEnum.YCWC + " and accept_state !=" + (int)LosslessReport_EditApplyEnum.SQTH + " and accept_state !=" + (int)LosslessReport_EditApplyEnum.BFTH + " and report_id = '" + report_id + "'";
            if (Convert.ToInt32(db.ExecuteScalar(checksql)) > 0)
            {
                context.Response.Write("请勿重复提交申请");
                context.Response.End();
            }

            string review_personnel = context.Request.Params.Get("review_personnel");//评审人
            string constitute_personnel = context.Request.Params.Get("constitute_personnel");//编制人

            string error_remark = context.Request.Params.Get("error_remark");//申请原因
            string login_user = context.Session["loginAccount"].ToString();//申请人
            string date_ = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string other_remarks = context.Request.Params.Get("other_remarks");//其他原因
            string File_url = context.Request.Params.Get("File_url");
            string report_num = context.Request.Params.Get("report_num");

            //SqlParameter[] param = null;
            string messageContent = "";
            string sql = "";

            if (flag == "Scrap")//报废流程
            {
                //消息
                messageContent = " 报告编号为：" + report_num + "的无损异常报告申请报废;";
                string message_type = "无损异常报告报废审核";

                string apply_type = "报废申请";
                //string file_format = hpf.ContentType;
                int accept_state = (int)LosslessReport_EditApplyEnum.BFSH;
                SqlParameter[] param = 
                {
                 new SqlParameter("@report_id",report_id),
                 new SqlParameter("@File_format",".doc"),
                 new SqlParameter("@File_url",File_url),
                 new SqlParameter("@error_remark",error_remark),
                 new SqlParameter("@accept_personnel",login_user),
                 new SqlParameter("@accept_date",date_),
                 new SqlParameter("@review_personnel",review_personnel),                              
                 new SqlParameter("@other_remarks",other_remarks),
                 new SqlParameter("@constitute_personnel",constitute_personnel),
                 new SqlParameter("@accept_state",accept_state),

                 //消息
                 new SqlParameter("@messageContent",messageContent),
                 new SqlParameter("@message_type",message_type),
                 //流程记录
                 new SqlParameter("@DateTime",DateTime.Now.ToString()),
                 new SqlParameter("@NodeResult","pass"),
                 new SqlParameter("@NodeId",(int)TB_ProcessRecordNodeIdEnum.ManageToApply),
                 //报告异常申请状态
                 new SqlParameter("@state_",state)

            };

                // param = param3;
                sql = @"INSERT TB_NDT_error_Certificate ( report_id, File_format, File_url, error_remark, accept_personnel,  accept_date, review_personnel, accept_state,constitute_personnel)
                                        VALUES ( @report_id, @File_format, @File_url, @error_remark, @accept_personnel,  @accept_date, @review_personnel, @accept_state,@constitute_personnel )
                        ;insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@review_personnel,@messageContent,@message_type,@accept_date,@accept_personnel)
                        ;update dbo.TB_NDT_report_title set state_=@state_ where id=@report_id 
                        ;insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values (@report_id,@accept_personnel,@DateTime, @NodeResult,@NodeId)
                        ";
                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                    SQLStringList2.Add(param);
                    SQLStringList.Add(sql);
                    //事务
                    SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                    string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                    string login_username = Convert.ToString(context.Session["login_username"]);
                    Operation_log.operation_log_(loginAccount, login_username, "无损异常报告", "提交无损报告编号为" + report_num + "的报告的修改申请");
                    //发消息
                    Send_message new_message = new Send_message();
                    new_message.send_usercount(review_personnel, messageContent);
                    context.Response.Write("T");
                }
                catch (Exception)
                {
                    context.Response.Write("F");
                }
                finally
                {
                    context.Response.End();
                }
            }
            else
            {//修改流程

                messageContent = " 报告编号为：" + report_num + "的无损异常报告申请审核;";
                string message_type = "无损异常报告申请审核";
                //原报告路径
                string reportPath = context.Server.MapPath(File_url);
                //定义错误报告路径
                string temp_url = ConfigurationManager.AppSettings["Lossless_report_certificate_E"].ToString();
                string file_name = Guid.NewGuid().ToString() + ".doc";
                //错误证书保存位置
                string path = context.Server.MapPath(temp_url + file_name);
                //hpf.SaveAs(path);

                //复制模板
                try
                {
                    System.IO.File.Copy(reportPath, path, true);
                }
                catch
                {
                    context.Response.Write("报告已丢失！");
                    context.ApplicationInstance.CompleteRequest();
                    return;
                }

                string apply_type = "修改申请";
                //string file_format = hpf.ContentType;

                int accept_state = (int)LosslessReport_EditApplyEnum.SQSH;
                SqlParameter[] param = 
            {
                 new SqlParameter("@report_id",report_id),
                 new SqlParameter("@File_format",".doc"),
                 new SqlParameter("@File_url",temp_url + file_name),
                 new SqlParameter("@error_remark",error_remark),
                 new SqlParameter("@accept_personnel",login_user),
                 new SqlParameter("@accept_date",date_),
                 new SqlParameter("@review_personnel",review_personnel),                              
                 new SqlParameter("@other_remarks",other_remarks),
                 new SqlParameter("@constitute_personnel",constitute_personnel),
                 new SqlParameter("@accept_state",accept_state),

                 //消息
                 new SqlParameter("@messageContent",messageContent),
                 new SqlParameter("@message_type",message_type),
                 //流程记录
                 new SqlParameter("@DateTime",DateTime.Now.ToString()),
                 new SqlParameter("@NodeResult","pass"),
                 new SqlParameter("@NodeId",(int)TB_ProcessRecordNodeIdEnum.ManageToApply),
                 //报告异常申请状态
                 new SqlParameter("@state_",state)
            };

                // param = param3;
                sql = @"INSERT TB_NDT_error_Certificate ( report_id, File_format, File_url, error_remark, accept_personnel,  accept_date, review_personnel, accept_state,constitute_personnel)
                                        VALUES ( @report_id, @File_format, @File_url, @error_remark, @accept_personnel,  @accept_date, @review_personnel, @accept_state,@constitute_personnel )
                        ;insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@review_personnel,@messageContent,@message_type,@accept_date,@accept_personnel)
                        ;update dbo.TB_NDT_report_title set state_=@state_ where id=@report_id
                         ;insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values (@report_id,@accept_personnel,@DateTime, @NodeResult,@NodeId)
                        ";
                //int result = db.ExecuteNonQuery(sql, param);


                ////消息
                //string insert_message2 = "insert into  dbo.TB_show_message(User_count,message,message_type,create_time,message_push_personnel) values (@review_personnel,@messageContent,@message_type,@date_,@login_user)";
                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                    SQLStringList2.Add(param);
                    SQLStringList.Add(sql);
                    //事务
                    SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                    string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                    string login_username = Convert.ToString(context.Session["login_username"]);
                    Operation_log.operation_log_(loginAccount, login_username, "无损异常报告", "提交无损报告编号为" + report_num + "的报告的修改申请");
                    //发消息
                    Send_message new_message = new Send_message();
                    new_message.send_usercount(review_personnel, messageContent);
                    context.Response.Write("T");
                }
                catch (Exception)
                {
                    context.Response.Write("F");
                }
                finally
                {
                    context.Response.End();
                }

            }



            //if (result > 0)
            //    context.Response.Write("T");
            //else
            //    context.Response.Write("F");
            //context.Response.End();
        }


        #endregion


        #region  预览报告
        /// <summary>
        /// 预览报告
        /// </summary>
        /// <param name="context"></param>
        public void Preview_Report(HttpContext context)
        {
            string id = context.Request.Params.Get("id");

            string report_url = "";
            //判断证书是否存在
            string getReportUrlSql = "select report_url from dbo.TB_NDT_report_title where id ='" + id + "'";
            string file_path = ConfigurationManager.AppSettings["view_temp_Folder"].ToString();
            //定义报告新文件名
            string new_doc_name = Guid.NewGuid().ToString() + ".pdf";
            report_url = db.ExecuteScalar(getReportUrlSql).ToString();
            // Document pdf_ = new Document(context.Server.MapPath(report_url));

            if (report_url == "")
            {
                context.Response.Write("F");
            }
            else
            {
                try
                {
                    string NEW_DOC = context.Server.MapPath(file_path + Guid.NewGuid().ToString() + ".doc");
                    //复制源文件
                    File.Copy(context.Server.MapPath(report_url), NEW_DOC);
                    //WordConverter new_word = new WordConverter();

                    ////string flag = new_word.WordToPdf(context.Server.MapPath(report_url), context.Server.MapPath(file_path + new_doc_name));
                    //// 转换pdf
                    //bool flag = new_word.Convertwordtopdf(NEW_DOC, context.Server.MapPath(file_path + new_doc_name));
                    //// context.Response.Write(file_path + new_doc_name);
                    ////pdf_.Save(context.Server.MapPath(file_path + new_doc_name), SaveFormat.Pdf);
                    //if (flag)
                    //{
                    //    context.Response.Write(file_path + new_doc_name);
                    //}
                    //else
                    //{
                    //    context.Response.Write(flag);
                    //}

                    //创建word对象
                    Aspose.Words.Document contract_doc = null;
                    contract_doc = new Aspose.Words.Document(NEW_DOC);
                    contract_doc.Save(context.Server.MapPath(file_path + new_doc_name), SaveFormat.Pdf);
                    context.Response.Write(file_path + new_doc_name);
                }
                catch (Exception e)
                {

                    context.Response.Write(e.ToString());
                }


            }


            context.Response.End();
        }
        #endregion



        /// 加载报告签发datagrid
        public void load_list(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Finish;
            int state_2 = (int)LosslessReportStatusEnum.Abnormal;
            //string Audit_personnel = Convert.ToString(context.Session["loginAccount"]);//检测人员

            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");

            string search1 = context.Request.Params.Get("search1");
            string search2 = context.Request.Params.Get("search2");
            string search3 = context.Request.Params.Get("search3");
            string search4 = context.Request.Params.Get("search4");
            string search = context.Request.Params.Get("search");
            string key = context.Request.Params.Get("key");
            string key1 = context.Request.Params.Get("key1");
            string key2 = context.Request.Params.Get("key2");
            string key3 = context.Request.Params.Get("key3");
            string key4 = context.Request.Params.Get("key4");
            string history_flag = context.Request.Params.Get("history_flag");
            string loginAccount = Convert.ToString(context.Session["loginAccount"]);
            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;

            string sqlwhere = "where (rt.state_ = " + state_ + " or rt.state_ = " + state_2 + ")";

            #region 查询条件
            //查询
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(search))
            {
                if (search == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), rt." + search + ",120) like '%" + key + "%'";
                }
                else if ((search == "Inspection_personnel" || search == "Audit_personnel" || search == "issue_personnel") && !string.IsNullOrEmpty(key))
                {
                    string select_count = "select user_count from tb_user_info where user_name='" + key + "'";
                    key = db.ExecuteScalar(select_count).ToString();
                    if (key == "")
                    {
                        sqlwhere += " and 1=0";
                    }
                    else
                    {
                        sqlwhere += " and rt." + search + " like '%" + key + "%'";
                    }

                }
                else if (search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sqlwhere += "and rt.Audit_date-rt.Inspection_personnel_date>=" + key + "";
                }
                else if (search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sqlwhere += "and rt.issue_date-rt.Audit_date>=" + key + "";
                }
                else if (search == "EditOverdue")//编制逾期:检验人签字时间-创建时间
                {
                    sqlwhere += "and rt.Inspection_personnel_date-rt.report_CreationTime>=" + key + "";
                }
                else
                {
                    sqlwhere += " and rt." + search + " like '%" + key + "%'";
                }


            }
            if (!string.IsNullOrEmpty(key1) && !string.IsNullOrEmpty(search1))
            {
                if (search1 == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), rt." + search1 + ",120) like '%" + key + "%'";
                }
                else if ((search1 == "Inspection_personnel" || search1 == "Audit_personnel" || search1 == "issue_personnel") && !string.IsNullOrEmpty(key1))
                {
                    string select_count = "select user_count from tb_user_info where user_name='" + key1 + "'";
                    key1 = db.ExecuteScalar(select_count).ToString();
                    if (key1 == "")
                    {
                        sqlwhere += " and 1=0";
                    }
                    else
                    {
                        sqlwhere += " and rt." + search1 + " like '%" + key1 + "%'";
                    }
                }
                else if (search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sqlwhere += "and rt.Audit_date-rt.Inspection_personnel_date>=" + key + "";
                }
                else if (search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sqlwhere += "and rt.issue_date-rt.AuditIssueRetrunTime>=" + key + "";
                }
                else
                {

                    sqlwhere += " and rt." + search1 + " like '%" + key1 + "%'";
                }
            }
            if (!string.IsNullOrEmpty(key2) && !string.IsNullOrEmpty(search2))
            {
                if (search2 == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), rt." + search2 + ",120) like '%" + key + "%'";
                }
                else if ((search2 == "Inspection_personnel" || search2 == "Audit_personnel" || search2 == "issue_personnel") && !string.IsNullOrEmpty(key2))
                {
                    string select_count = "select user_count from tb_user_info where user_name='" + key2 + "'";
                    key2 = db.ExecuteScalar(select_count).ToString();
                    if (key2 == "")
                    {
                        sqlwhere += " and 1=0";
                    }
                    else
                    {
                        sqlwhere += " and rt." + search2 + " like '%" + key2 + "%'";
                    }
                }
                else if (search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sqlwhere += "and rt.Audit_date-rt.Inspection_personnel_date>=" + key + "";
                }
                else if (search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sqlwhere += "and rt.issue_date-rt.AuditIssueRetrunTime>=" + key + "";
                }
                else
                {

                    sqlwhere += " and rt." + search2 + " like '%" + key2 + "%'";
                }
            }
            if (!string.IsNullOrEmpty(key3) && !string.IsNullOrEmpty(search3))
            {
                if (search3 == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), rt." + search3 + ",120) like '%" + key3 + "%'";
                }
                else if ((search3 == "Inspection_personnel" || search3 == "Audit_personnel" || search3 == "issue_personnel") && !string.IsNullOrEmpty(key3))
                {
                    string select_count = "select user_count from tb_user_info where user_name='" + key3 + "'";
                    key3 = db.ExecuteScalar(select_count).ToString();
                    if (key3 == "")
                    {
                        sqlwhere += " and 1=0";
                    }
                    else
                    {
                        sqlwhere += " and rt." + search3 + " like '%" + key3 + "%'";
                    }
                }
                else if (search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sqlwhere += "and rt.Audit_date-rt.Inspection_personnel_date>=" + key + "";
                }
                else if (search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sqlwhere += "and rt.issue_date-rt.AuditIssueRetrunTime>=" + key + "";
                }
                else
                {

                    sqlwhere += " and rt." + search3 + " like '%" + key3 + "%'";
                }
            }
            if (!string.IsNullOrEmpty(key4) && !string.IsNullOrEmpty(search4))
            {
                if (search4 == "Inspection_date")
                {
                    sqlwhere += " and convert(varchar(10), rt." + search4 + ",120) like '%" + key4 + "%'";
                }
                else if ((search4 == "Inspection_personnel" || search4 == "Audit_personnel" || search4 == "issue_personnel") && !string.IsNullOrEmpty(key4))
                {
                    string select_count = "select user_count from tb_user_info where user_name='" + key4 + "'";
                    key4 = db.ExecuteScalar(select_count).ToString();
                    if (key4 == "")
                    {
                        sqlwhere += " and 1=0";
                    }
                    else
                    {
                        sqlwhere += " and rt." + search4 + " like '%" + key4 + "%'";
                    }
                }
                else if (search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                {
                    sqlwhere += "and rt.Audit_date-rt.Inspection_personnel_date>=" + key + "";
                }
                else if (search == "IssueOverdue")//签发逾期:报告完成时间-审核签字时间
                {
                    sqlwhere += "and rt.issue_date-rt.AuditIssueRetrunTime>=" + key + "";
                }
                else
                {

                    sqlwhere += " and rt." + search4 + " like '%" + key4 + "%'";
                }
            }

            #endregion

            string sql = "select * from (select row_number()over(order by rt." + sortby + " " + order + @")RowId,rt.*,ui1.User_name as Inspection_personnel_n,ui2.User_name as Audit_personnel_n,ui3.User_name as issue_personnel_n ,td.Department_name as Audit_groupid_n from dbo.TB_NDT_report_title rt left join tb_user_info ui1 on rt.Inspection_personnel=ui1.User_count left join tb_user_info ui2 on rt.Audit_personnel=ui2.User_count left join tb_user_info ui3 on rt.issue_personnel=ui3.User_count left join dbo.TB_department as td on rt.Audit_groupid=td.id
                                      " + sqlwhere + ")a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_report_title as rt " + sqlwhere + "";
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }

        public void Report_LoadList(HttpContext context) {
            string sql = "";
            int ParentId = Convert.ToInt32(context.Request.Params.Get("ParentId"));
            
            string report_type = context.Request.Params.Get("report_type").ToString();//报告类型
            string dateStart = context.Request.Params.Get("dateStart").ToString();//报告开始时间
            string dateEnd = context.Request.Params.Get("dateEnd").ToString();//报告结束时间

            string sqlwhere = "";
            switch (report_type)
            {
                case "report_num"://获取报告量
                    if (ParentId != 0)
                    {
                        sqlwhere = "and Parent_id='" + ParentId + "'";
                    }
                    else {
                        int id = Convert.ToInt32(context.Request.Params.Get("id"));
                        sqlwhere = "and id='" + id + "'";
                    }
                    sql = @"select TU.Department_name, RT.* from dbo.TB_NDT_report_title as RT 
                     left join TB_user_department as TU on RT.Inspection_personnel=TU.User_count
                     where RT.Inspection_personnel in ( select User_count from TB_user_department where User_department in (select id from dbo.TB_department where 1=1 "+sqlwhere+"))";
                    break;
                 
                default:
                    break;
            }

            

            //String group = context.Request.Params.Get("comboxgroup");//组名字
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }


        public void report_arrange(HttpContext context)
        {
            DataTable dt = new DataTable();

            //string group = context.Request.Params.Get("comboxgroup");
            string person = context.Request.Params.Get("comboxperson").ToString();
            //string checktype = context.Request.Params.Get("checktype").ToString();
            //统计个人
            if (!string.IsNullOrWhiteSpace(person))
            {
                dt = person_Dt(context);
            }
            //统计组
            else
            {
                dt = groupTab(context);
            }

            if (dt.Rows.Count <= 0)
            {
                context.Response.Write("数据为空");
                context.Response.End();
            }
            else
            {
                string strJson = jsonHelper.DataTable2ToJson(dt);//DataSet数据转化为Json数据    
                context.Response.Write(strJson);//返回给前台页面   
                context.Response.End();
            }
        }

        //public DataTable allgroupTab(HttpContext context)
        //{
        //    DataTable dt = new DataTable();
        //    string checktype = context.Request.Params.Get("checktype").ToString();
        //    string dateStart = context.Request.Params.Get("dateStart").ToString();
        //    string dateEnd = context.Request.Params.Get("dateEnd").ToString();
        //    string group = context.Request.Params.Get("comboxgroup");
        //    string sql = "";

        //    switch (checktype)
        //    {

        //        case "1":
        //            sql += "select Department_name as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.Inspection_personnel where TB_NDT_report_title.state_!=1 ";
        //            if (dateStart != "" && dateEnd == "")
        //            {
        //                sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
        //            }
        //            if (dateStart != "" && dateEnd != "")
        //            {
        //                sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
        //            }
        //            sql += "group by us.Department_name";
        //            dt = db.ExecuteDataTable(sql);
        //            break;
        //        case "2":
        //            sql += "select  TB_user_info.User_name,UT.sumCount from (";
        //            sql += "select us.User_count as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.Inspection_personnel where TB_NDT_report_title.state_!=1  ";
        //            if (dateStart != "" && dateEnd == "")
        //            {
        //                sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
        //            }
        //            if (dateStart != "" && dateEnd != "")
        //            {
        //                sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
        //            }
        //            sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
        //            dt = db.ExecuteDataTable(sql);
        //            break;
        //        default:
        //            break;
        //    }
        //    return dt;
        //}
        //统计组
        public DataTable groupTab(HttpContext context)
        {

            DataTable dt = new DataTable();
            string report_type = context.Request.Params.Get("report_type").ToString();//报告类型
            string dateStart = context.Request.Params.Get("dateStart").ToString();//报告开始时间
            string dateEnd = context.Request.Params.Get("dateEnd").ToString();//报告结束时间
            string group = context.Request.Params.Get("comboxgroup");//组名字
            string checktype = context.Request.Params.Get("checktype").ToString();//0组、1人
            string day = context.Request.Params.Get("day");//耗时天数
            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
            {
                DateTime starttime = Convert.ToDateTime(dateStart);
                DateTime endtime = Convert.ToDateTime(dateEnd);
                if (DateTime.Compare(starttime, endtime) > 0)
                {
                    context.Response.Write("开始时间大于结束时间");
                    context.Response.End();
                }
            }

            string sql = "";
            switch (report_type)
            {
                case "report_num"://统计报告报告量
                    if (checktype == "0")//统计所有组
                    {
                        sql += "select Department_name as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.Inspection_personnel where TB_NDT_report_title.state_!=1 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";
                    }
                    else
                    { //统计组下的所有人
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.Inspection_personnel where TB_NDT_report_title.state_!=1  ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
                    }

                    dt = db.ExecuteDataTable(sql);
                    break;
                case "report_Issue"://统计报告报签发
                    if (checktype == "0")//统计所有组
                    {
                        sql += "select Department_name as User_name,COUNT(issue_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.issue_personnel where TB_NDT_report_title.state_!=1 ";
                         if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";
                    }
                    else //统计组下的所有人
                    {
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(issue_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.issue_personnel where TB_NDT_report_title.state_=3  ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
                    
                    }

                    dt = db.ExecuteDataTable(sql);
                    break;
                case "report_Edit"://统计报告报编辑
                    if (checktype == "0")
                    {
                        sql += "select Department_name as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.Inspection_personnel where TB_NDT_report_title.state_!=1 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";
                    }
                    else
	                {
                        //统计组下的所有人
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.Inspection_personnel where TB_NDT_report_title.state_!=1  ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
	                }

                    dt = db.ExecuteDataTable(sql);
                    break;
                case "report_Audit"://统计报告报审核
                      if (checktype == "0")//统计所有组
                    {
                        sql += "select Department_name as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title on us.User_department=TB_NDT_report_title.Audit_groupid where TB_NDT_report_title.state_!=1 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";
                    }
                    else
                    { //统计组下的所有人
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(Audit_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title on us.User_count=TB_NDT_report_title.Audit_personnel where TB_NDT_report_title.state_!=1  ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
                    }

                    dt = db.ExecuteDataTable(sql);
                    break;
                case "report_Type"://报告类型
                    sql += "select COUNT(report_name) as sumCount,report_name as User_name from TB_NDT_report_title where state_!=1 and  1=1  ";
                      if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += " group by report_name ";
                
                    break;
                case "error_type"://统计错误类型
                    if (checktype == "0")//统计所有组错误类型 
                    {
                        sql += "select COUNT(el.error_remarks) as sumCount,el.error_remarks as User_name from dbo.TB_NDT_error_log el left join TB_user_department ud on el.addpersonnel=ud.User_count where 1=1 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by el.error_remarks";

                    }
                    else
                    {
                        sql += "select COUNT(el.error_remarks) as sumCount,el.error_remarks as User_name from dbo.TB_NDT_error_log el left join TB_user_department ud on el.addpersonnel=ud.User_count where ud.Department_name='" + group + "'";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by el.error_remarks,ud.Department_name ";

                    }
                    dt = db.ExecuteDataTable(sql);
                    break;

                case "Edittime"://编制耗时：编制签字时间-报告创建时间
                    if (checktype == "0")//统计所有组编制耗时
                    {
                        sql += "select Department_name as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title as rt on us.User_count=rt.Inspection_personnel where rt.Inspection_personnel_date-rt.ReportCreationTime>=" + day + " and rt.state_=4 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";

                    }
                    else
                    {
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(Inspection_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title as rt on us.User_count=rt.Inspection_personnel where rt.state_=4 and rt.Inspection_personnel_date-rt.ReportCreationTime>=" + day + "";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
                    }
                    dt = db.ExecuteDataTable(sql);
                    break;
                case "Reviewtime"://审核耗时:审核签字时间-编制签字时间
                    if (checktype == "0")//统计所有组审核耗时
                    {
                        sql += "select Department_name as User_name,COUNT(Audit_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title as rt on us.User_count=rt.Audit_personnel where rt.Audit_date-rt.Inspection_personnel_date>=" + day + "and rt.state_=4 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";

                    }
                    else
                    {
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(Audit_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title as rt on us.User_count=rt.Audit_personnel where rt.state_=4 and rt.Audit_date-rt.Inspection_personnel_date>=" + day + "";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
                    }
                    dt = db.ExecuteDataTable(sql);
                    break;
                case "Issuetime"://签发耗时：签发签字时间-审核签字时间
                    if (checktype == "0")//统计所有组签发耗时
                    {
                        sql += "select Department_name as User_name,COUNT(issue_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title as rt on us.User_count=rt.issue_personnel where rt.issue_date - rt.Audit_date>=" + day + " and rt.state_!=1 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";

                    }
                    else
                    {
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(issue_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title as rt on us.User_count=rt.issue_personnel where rt.state_!=1 and rt.issue_date - rt.Audit_date>" + day + "";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
                    }
                    dt = db.ExecuteDataTable(sql);
                    break;
                case "Alltime"://总耗时：签发签字时间-报告创建时间
                    if (checktype == "0")//统计所有组总耗时
                    {
                        sql += "select Department_name as User_name,COUNT(issue_personnel) as sumCount from (select * from TB_user_department )as us left join TB_NDT_report_title as rt on us.User_count=rt.issue_personnel where rt.issue_date - rt.ReportCreationTime>=" + day + " and rt.state_!=1 ";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + " 00:00:00.000' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.Department_name";

                    }
                    else
                    {
                        sql += "select  TB_user_info.User_name,UT.sumCount from (";
                        sql += "select us.User_count as User_name,COUNT(issue_personnel) as sumCount from (select * from TB_user_department where Department_name='" + group + "')as us left join TB_NDT_report_title as rt on us.User_count=rt.issue_personnel where rt.state_!=1 and rt.issue_date - rt.ReportCreationTime>" + day + "";
                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and rt.Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by us.User_count) as UT left join dbo.TB_user_info on TB_user_info.User_count=UT.User_name";
                    }
                    dt = db.ExecuteDataTable(sql);
                    break;
                case "passing_rate":
                    //统计所有组一次通过率
                    if (checktype == "0")
                    {
                        sql = "with t1 as(select Department_name,Count(Department_name) 报告数量 from TB_NDT_report_title left join TB_user_department on TB_NDT_report_title.Inspection_personnel=TB_user_department.User_count where  state_!=1  ";

                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        //报告一次通过的数量 
                        sql += "group by  Department_name), t2 as(select Department_name,Count(Department_name) 一次通过数量 from TB_NDT_report_title left join TB_user_department on TB_NDT_report_title.Inspection_personnel=TB_user_department.User_count where  state_=4  and return_flag=0  ";

                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by  Department_name ) select t1.Department_name as User_name ,cast (CAST( t2.一次通过数量 AS decimal)/CAST ( t1.报告数量  as decimal)*100 as decimal(10,0)) as sumCount from t1 left join t2 on t1.Department_name=t2.Department_name where t2.Department_name is not null";
                    }
                    else
                    {
                        //统计组下所有人一次通过率

                        sql = "with t1 as(select Count(User_count) 报告数量 ,User_count as User_Name from TB_NDT_report_title left join TB_user_department on TB_NDT_report_title.Inspection_personnel=TB_user_department.User_count where Department_name='" + group + "' and state_!=1  ";//统计个人报告数

                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        //报告个人一次通过的数量
                        sql += "group by  User_count) , t2 as(select User_count as User_name ,Count(User_count) 一次通过数量 from TB_NDT_report_title left join TB_user_department on TB_NDT_report_title.Inspection_personnel=TB_user_department.User_count where Department_name='" + group + "' and state_=4  and return_flag=0  ";

                        if (dateStart != "" && dateEnd == "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                        }
                        if (dateStart != "" && dateEnd != "")
                        {
                            sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                        }
                        sql += "group by  User_count )  select TU.User_Name as User_name ,cast (CAST( t2.一次通过数量 AS decimal)/CAST ( t1.报告数量  as decimal)*100 as decimal(10,0)) as sumCount from t1 left join t2 on t1.User_Name=t2.User_Name left join dbo.TB_user_info as TU on t1.User_Name= TU.User_count where t1.报告数量 is not null and t2.一次通过数量 is not null";
                    }
                    break;
                    
            }
            dt = db.ExecuteDataTable(sql);
            return dt;

        }

        //统计人
        public DataTable person_Dt(HttpContext context)
        {

            DataTable dt = new DataTable();
            string report_type = context.Request.Params.Get("report_type").ToString();

            string dateStart = context.Request.Params.Get("dateStart").ToString();
            string dateEnd = context.Request.Params.Get("dateEnd").ToString();

            if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
            {
                DateTime starttime = Convert.ToDateTime(dateStart);
                DateTime endtime = Convert.ToDateTime(dateEnd);
                if (DateTime.Compare(starttime, endtime) > 0)
                {
                    context.Response.Write("开始时间大于结束时间");
                    context.Response.End();
                }
            }
            string person = context.Request.Params.Get("comboxperson").ToString();
            string sql = "";
            switch (report_type)
            {
                case "report_num"://个人报告量
                    sql = "select COUNT(Inspection_personnel) as sumCount,Inspection_personnel as User_name from TB_NDT_report_title where TB_NDT_report_title.state_!=1 and Inspection_personnel in('" + person + "') ";
                    if (dateStart != "" && dateEnd == "")
                    {
                        sql += "and Inspection_date between '" + dateStart + "' and GETDATE() ";
                    }
                    if (dateStart != "" && dateEnd != "")
                    {
                        sql += "and Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                    }
                    sql += "group by Inspection_personnel ";
                    dt = db.ExecuteDataTable(sql);
                    break;
                case "error_type"://个人错误类型
                    sql = "select error_remarks as User_name,COUNT(error_remarks) as sumCount from TB_NDT_error_log where addpersonnel='" + person + "' ";
                    if (dateStart != "" && dateEnd == "")
                    {
                        sql += "and add_date between '" + dateStart + "' and GETDATE() ";
                    }
                    if (dateStart != "" && dateEnd != "")
                    {
                        sql += "and add_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                    }
                    sql += "group by error_remarks";
                    dt = db.ExecuteDataTable(sql);
                    break;
                case "passing_rate"://个人一次通过率
                    sql = "select TB_user_info.User_name,CAST( CAST( TT.oneShot AS decimal)/CAST ( TT.sumCount  as decimal)*100 AS decimal(10,2)) sumCount from (select t.Inspection_personnel,SUM(T.sumCount) 'sumCount',sum(T.oneShot) 'oneShot' from ( select Inspection_personnel,case when state_!=1 then COUNT( Inspection_personnel) else 0 end 'sumCount',case when  state_=4 and return_flag=0 then COUNT( Inspection_personnel) else 0 end 'oneShot'  from TB_NDT_report_title";
                    if (dateStart != "" && dateEnd == "")
                    {
                        sql += " where Inspection_date between '" + dateStart + "' and GETDATE() ";
                    }
                    if (dateStart != "" && dateEnd != "")
                    {
                        sql += " where Inspection_date between '" + dateStart + "' and '" + dateEnd + " 23:59:59.000'";
                    }
                    sql += " group by  Inspection_personnel,state_,return_flag) T where  T.sumCount!=0 group by t.Inspection_personnel) TT left join TB_user_info on TT.Inspection_personnel=TB_user_info.User_count where TB_user_info.User_count='" + person + "'";
                    dt = db.ExecuteDataTable(sql);
                    break;
            }
            return dt;

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