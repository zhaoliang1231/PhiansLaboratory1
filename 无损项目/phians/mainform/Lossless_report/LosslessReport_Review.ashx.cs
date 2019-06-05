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

namespace phians.mainform.Lossless_report
{
    /// <summary>
    /// LosslessReport_Review1 的摘要说明
    /// </summary>
    public class LosslessReport_Review1 : IHttpHandler, IRequiresSessionState
    {
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.Params.Get("cmd");
            switch (command)
            {
                case "load_list": load_list(context); break;//加载报告审核datagrid
                case "Back_report": Back_report(context); break;//退回报告编制
                case "submit_review_Report": submit_review_Report(context); break;//提交报告签发
                case "load_professional_department": load_professional_department(context); break;//加载审核组
                case "load_responsible_people": load_responsible_people(context); break;//加载审核组人员

                    ///*********退回
                case "load_accessory_list": load_accessory_list(context); break;//加载附件列表
                case "save_accessory": save_accessory(context); break;//添加附件
                case "del_accessory": del_accessory(context); break;//删除附件

                case "all_errorinfo": AllErrorInfo(context); break;//加载全部退回原因
                case "return_errorinfo": ReturnErrorInfo(context); break;//加载已选退回原因


                case "load_record_url": load_record_url(context); break;//加载记录文档链接

                    //****************提交签发前 判断报告是二级还是三级审批
                case "loadlevel": LoadLevel(context); break;//二级还是三级审批

            }
        }

        #region 判断为两级/三级审核
        public void LoadLevel(HttpContext context)
        {
            String tm_id = context.Request.Params.Get("tm_id");

      
            int state_ = 4;
            if (tm_id == "1")
            {
                state_ = (int)NDE_report_review.Heat_treatment;
            }
            if (tm_id == "2")
            {
                state_ = (int)NDE_report_review.VT;
            }
            else if (tm_id == "3")
            {
                state_ = (int)NDE_report_review.DT;
            }
            else if (tm_id == "4" || tm_id == "5")
            {
                state_ = (int)NDE_report_review.ECT;
            }
            else if (tm_id == "6")
            {
                state_ = (int)NDE_report_review.LT;
            }
            else if (tm_id == "7" || tm_id == "8")
            {
                state_ = (int)NDE_report_review.MT;
            }
            else if (tm_id == "10" || tm_id == "11")
            {
                state_ = (int)NDE_report_review.UT;
            }
            else if (tm_id == "12")
            {
                state_ = (int)NDE_report_review.Water_pressure;
            }
                else if (tm_id == "26")
            {
                state_ = (int)NDE_report_review.PT;
            }
            else if (tm_id == "27")
            {
                state_ = (int)NDE_report_review.RT;
            }
            else {
                state_ = 4;
            }

         


            context.Response.Write(state_);
            context.Response.End();

        }
            #endregion

        //加载记录文档链接
        public void load_record_url(HttpContext context)
        {
            string id = context.Request.Params.Get("id");

            try
            {
                String select = "select top 1 report_url from dbo.TB_NDT_RevisionsRecord where report_id='" + id + "' order by id desc";
                string report_url = db.ExecuteScalar(select).ToString();
                context.Response.Write(report_url);
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        /// 加载报告审核datagrid
        public void load_list(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Audit;
            //string Audit_personnel = Convert.ToString(context.Session["loginAccount"]);//检测人员

            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");
            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;

            string search = context.Request.Params.Get("search");
            string key = context.Request.Params.Get("key");
            string history_flag = context.Request.Params.Get("history_flag");
            string loginAccount = Convert.ToString(context.Session["loginAccount"]);

            //string select_department = "select User_department from dbo.TB_user_department where User_count='" + loginAccount + "' and User_department=47";//User_department=47——审核组
            //string Audit_personnel = db.ExecuteScalar(select_department).ToString();

            string sqlwhere = "where 1=1 and TB_u.User_count='" + loginAccount + "' ";
            //查询
            //if (Audit_personnel == "47")
            //{
              
                //历史审核
                if (history_flag == "1")
                {
                    sqlwhere += " and state_ != " + state_ + "  ";

                }
                //待审核
                if (history_flag == "0")
                {
                    sqlwhere += " and state_ = " + state_ + "  ";

                }
                //查询
                if (!string.IsNullOrEmpty(search))
                {
                    if (search == "ReviewOverdue")
                    {
                        sqlwhere += "and rt.id in ( select ReportID from( select ReportID, min(OperateDate) as EditTime from TB_ProcessRecord  where NodeId='0' group by ReportID)as a where '" + DateTime.Now.ToString() + "'-EditTime>=" + key + ")";
                    }
                    else
                    {
                        sqlwhere += " and " + search + " like '%" + key + "%'";
                    }
                }



                string sql = "select * from (select row_number()over(order by rt." + sortby + " " + order + @")RowId,rt.*,ui.User_name as Inspection_personnel_n from dbo.TB_NDT_report_title rt left join  dbo.TB_user_department as TB_u  on TB_u.User_department= rt.Audit_groupid left join dbo.TB_user_info ui on rt.Inspection_personnel=ui.User_count 
                                      " + sqlwhere + ")a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_report_title as rt left join dbo.TB_user_department as TB_u  on TB_u.User_department= rt.Audit_groupid " + sqlwhere + "";
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        /// 退回报告编制
        public void Back_report(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Edit;

            string id = context.Request.Params.Get("id");//报告的id
            string Inspection_personnel = context.Request.Params.Get("Inspection_personnel");
            string return_info = context.Request.Params.Get("return_info");//退回原因
            string report_url = context.Request.Params.Get("report_url");
            string report_num = context.Request.Params.Get("report_num");
            string FirstInspection_personnel_date = DateTime.Now.ToString();//退回时间
            #region//*****************退回文字信息
            String error_remark = context.Request.Params.Get("error_remarks");//错误原因
            string insert = "";
            if (error_remark != "")
            {
                string add_date = DateTime.Now.ToLocalTime().ToString();                    //时间
                string addpersonnel = Convert.ToString(context.Session["loginAccount"]);   //用户
                string ReturnNode = "无损报告审核退回";

                string[] error_remarks = error_remark.Split(',');
                for (int i = 0; i < error_remarks.Length; i++)
                {
                    insert += ";insert into dbo.TB_NDT_error_log (report_id, error_remarks, constitute_personnel, addpersonnel, add_date, ReturnNode) values ('" + id + "','" + error_remarks[i] + "','" + Inspection_personnel + "','" + addpersonnel + "','" + add_date + "','" + ReturnNode + "')";
                }
            }
            #endregion

            string messageContent = " 报告编号为：" + report_num + "的无损报告需要重新编制;审核退回原因：" + return_info;
            string message_type = "无损报告重新编制";
            //时间
            string date = DateTime.Now.ToLocalTime().ToString();  
            //登录用户
            string login_user = Convert.ToString(context.Session["loginAccount"]);

               string messageSqls = "insert into  dbo.TB_show_message(User_count,message,message_type,create_time,send_time,message_push_personnel) values ('"
                    + Inspection_personnel + "','" + messageContent + "','" + message_type + "','" + date + "','" + date + "','" + login_user + "') ";

               SqlParameter[] para = 
                {
                    new SqlParameter("@state_",state_),
                    new SqlParameter("@return_info",return_info),
                    new SqlParameter("@FirstInspection_personnel_date",FirstInspection_personnel_date),
                    new SqlParameter("@id",id)
                };
               string upate_rep = "update dbo.TB_NDT_report_title set return_info=@return_info,return_flag=1,Inspection_personnel_date='',state_=@state_,FirstInspection_personnel_date=@FirstInspection_personnel_date,Audit_personnel='' where id=@id ";
            //添加报告流程记录
               string insertProcessRecord = "insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values ('" + id + "','" + login_user + "','" + FirstInspection_personnel_date + "','" + "pass" + "','" + (int)TB_ProcessRecordNodeIdEnum.ReviewToEdit + "')";
               #region 报告审核人审核时间书签清除
               Document first_doc = new Document(context.Server.MapPath(report_url));///upload_Folder/word_certificate/0e8b03a4-a7e5-455f-bea1-441bf5076448.doc
               DocumentBuilder first_builder = new DocumentBuilder(first_doc);

               //删除试验时间
               try
               {
                   first_doc.Range.Bookmarks["Inspection_personnel_date"].Text = "";//检验人签字时间
               }
               catch (Exception ex)
               {
               }
               //删除检验级别
               try
               {
                   first_doc.Range.Bookmarks["level_Inspection"].Text = "";//审核级别
               }
               catch (Exception ex)
               {
               }
               //删除试验人员签名                    
               try
               {
                   first_doc.Range.Bookmarks["Inspection_personnel"].Text = "";
               }
               catch (Exception ex)
               {
               }
               first_doc.Save(context.Server.MapPath(report_url), Aspose.Words.SaveFormat.Doc);
               #endregion

            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(messageSqls);
                db.ExecuteNonQueryByTrans(upate_rep, para);
                db.ExecuteNonQueryByTrans(insertProcessRecord);
                if (insert!="") {
                    db.ExecuteNonQueryByTrans(insert);
                }
                db.CommitTransacton();

                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "无损报告审核退回", "报告编号为：" + report_num + "的报告");

                //发消息
                Send_message new_message = new Send_message();
                new_message.send_usercount(Inspection_personnel, messageContent);
                context.Response.Write("T");
            }
            catch (System.Exception ex)
            {
                db.RollbackTransaction();
                context.Response.Write(ex);

            }
            finally
            {
                context.Response.End();
            }
        }
        /// 提交报告签发
        public void submit_review_Report(HttpContext context)
        {
            //int state_ = (int)LosslessReportStatusEnum.Issue;   

            string date = context.Request.Params.Get("level_date");                   //时间
            //string level_Audit = context.Request.Params.Get("level_Audit");                   //时间
            string login_user = Convert.ToString(context.Session["loginAccount"]);   //用户 编制人员

            //不同信息
            string id = context.Request.Params.Get("ids");//批量提交
            String report_num = context.Request.Params.Get("report_nums");
            String report_url = context.Request.Params.Get("report_urls");
            String tm_id = context.Request.Params.Get("tm_ids");

            //string[] ids = id.Split(',');
            //string[] report_urls = report_url.Split(',');
            //string[] tm_ids = tm_id.Split(',');
            //string[] report_nums = report_num.Split(',');
            //相同信息
            string issue_personnel = context.Request.Params.Get("group");   //签发人
            string Audit_personnel = Convert.ToString(context.Session["loginAccount"]);   //审核人
            String level_Audit = context.Request.Params.Get("level_Audit");
            string FirstAudit_date = DateTime.Now.ToString();//第一次审核签字时间
            string insertProcessRecord = "";
            //判断日期是否为空
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }
            //判断级别是否为空
            if (string.IsNullOrEmpty(level_Audit))
            {
                level_Audit = "II";
            }



            string getDetectionSql = @"select top 1 Signature as detection_image from dbo.TB_user_info where User_count = '" + Audit_personnel + "'";
            DataTable detection_dt = db.ExecuteDataTable(getDetectionSql);
            //判断签名是否存在
            if (!File.Exists(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())))
            {
                context.Response.Write("签名不存在");
                context.Response.End();
            }

            string upate_ = "";
            //string messageSqls = "";
            //string messageContent = "";
            string report_num_info = "";
            string Spaces = " ";
            #region 写入多个报告 审核人员时间级别等信息 
            //for (int i = 0; i < ids.Length; i++) {
            Document first_doc = new Document(context.Server.MapPath(report_url));///upload_Folder/word_certificate/0e8b03a4-a7e5-455f-bea1-441bf5076448.doc
                                                                            ///
                DocumentBuilder first_builder = new DocumentBuilder(first_doc);
            //写入记录文档
                String select = "select top 1 report_url from dbo.TB_NDT_RevisionsRecord where report_id='" + id + "' order by id desc";
                string reportLogURl = db.ExecuteScalar(select).ToString();
                Document record_Logdoc = new Document(context.Server.MapPath(reportLogURl));
                DocumentBuilder record_Logbuilder = new DocumentBuilder(record_Logdoc);
                if (detection_dt.Rows.Count > 0)
                {
                    //写入审核人签字时间
                    try
                    {
                        
                        first_doc.Range.Bookmarks["Audit_date"].Text = date;//审核人签字时间
                        record_Logdoc.Range.Bookmarks["Audit_date"].Text = date;
                    }
                    catch (Exception ex)
                    {
                        //context.Response.Write(ex);
                        //context.Response.End();
                    }
                    //写入审核级别
                    try
                    {
                        first_doc.Range.Bookmarks["level_Audit"].Text = level_Audit;//审核级别
                        record_Logdoc.Range.Bookmarks["level_Audit"].Text = level_Audit;
                    }
                    catch (Exception ex)
                    {
                    }
                    //写入审核人员签名                    
                    try
                    {
                        first_doc.Range.Bookmarks["Audit_personnel"].Text = "";
                        first_builder.MoveToBookmark("Audit_personnel");//跳到指定书签
                        double width = 50, height = 20;
                        Aspose.Words.Drawing.Shape shape = first_builder.InsertImage(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                        shape.Width = width;
                        shape.Height = height;
                        //shape.VerticalAlignment = VerticalAlignment.Center;
                        first_builder.InsertNode(shape);

                        record_Logdoc.Range.Bookmarks["Audit_personnel"].Text = "";
                        record_Logbuilder.MoveToBookmark("Audit_personnel");//跳到指定书签
                        //double widths = 50, heights = 20;
                        Aspose.Words.Drawing.Shape shapes = record_Logbuilder.InsertImage(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                        shapes.Width = width;
                        shapes.Height = height;
                        //shape.VerticalAlignment = VerticalAlignment.Center;
                        record_Logbuilder.InsertNode(shapes);

                    }
                    catch
                    { }
                //}

                first_doc.Save(context.Server.MapPath(report_url), Aspose.Words.SaveFormat.Doc);
                record_Logdoc.Save(context.Server.MapPath(reportLogURl), Aspose.Words.SaveFormat.Doc);


                #region//判断为两级/三级审核
                int state_ = 3;//默认三级审核
                if (tm_id == "1")
                {
                    state_ = (int)NDE_report_review.Heat_treatment;
                }
                else if (tm_id == "2")
                {
                    state_ = (int)NDE_report_review.VT;
                }
                else if (tm_id == "3")
                {
                    state_ = (int)NDE_report_review.DT;
                }
                else if (tm_id == "4" || tm_id == "5")
                {
                    state_ = (int)NDE_report_review.ECT;
                }
                else if (tm_id == "6")
                {
                    state_ = (int)NDE_report_review.LT;
                }
                else if (tm_id == "7" || tm_id == "8")
                {
                    state_ = (int)NDE_report_review.MT;
                }
                else if (tm_id == "10" || tm_id == "11")
                {
                    state_ = (int)NDE_report_review.UT;
                }
                else if (tm_id == "12")
                {
                    state_ = (int)NDE_report_review.Water_pressure;
                }
                else if (tm_id == "26")
                {
                    state_ = (int)NDE_report_review.PT;
                }
                else if (tm_id == "27")
                {
                    state_ = (int)NDE_report_review.RT;
                }
                #endregion

                
                if (state_ == 3) {
                    //判断是不是第一次提交
                    string getRetrunTimeSql = @"select FirstAudit_date from dbo.TB_NDT_report_title where id='" + id + "' and FirstAudit_date is null";
                    DataTable RetrunTime_dt = db.ExecuteDataTable(getRetrunTimeSql);
                    string setRetrunTimeSql = "";
                    //如果是第一次提交修改第一次编制签名时间
                    if (RetrunTime_dt.Rows.Count > 0)
                    {
                        setRetrunTimeSql = ",FirstAudit_date='" + FirstAudit_date + "'";
                    }

                    upate_ += "update dbo.TB_NDT_report_title set level_Audit='" + level_Audit + "',state_='" + state_ + "',issue_personnel='" + issue_personnel + "',Audit_date='" + date + "',Audit_personnel='" + Audit_personnel +"'"+ setRetrunTimeSql+" where id =" + id + " ";

                    report_num_info += Spaces + report_num;
                    //写入报告流程记录
                    insertProcessRecord = "insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values ('" + id + "','" + login_user + "','" + FirstAudit_date + "','" + "pass" + "','" + (int)TB_ProcessRecordNodeIdEnum.ReviewToIssue + "')";
                }
                else if (state_ == 4)
                {
                    upate_ += "update dbo.TB_NDT_report_title set level_Audit='" + level_Audit + "',state_='" + state_ + "',Audit_date='" + date + "',Audit_personnel='" + Audit_personnel + "' where id =" + id + " ";
                }
                else {
                    context.Response.Write("F");
                    context.Response.End();
                }

            }
            #endregion

            //messageContent = "您有 报告编号为：" + report_num_info + " 的无损报告需要签发";
            //string message_type = "无损待报告签发";

            //messageSqls = "insert into  dbo.TB_show_message(User_count,message,message_type,create_time,send_time,message_push_personnel) values ('"
            //     + issue_personnel + "','" + messageContent + "','" + message_type + "','" + date + "','" + date + "','" + Audit_personnel + "') ";


            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(upate_);
                db.ExecuteNonQueryByTrans(insertProcessRecord);
                db.CommitTransacton();

                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "无损报告审核", "报告编号：" + report_num);
                //new Send_message().send_usercount(Audit_personnel, messageContent);
                ////发消息
                //Send_message new_message = new Send_message();
                //new_message.send_usercount(issue_personnel, messageContent);
                context.Response.Write("T");
            }
            catch (System.Exception ex)
            {
                db.RollbackTransaction();
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }
        }
        /// 加载附件列表datagrid
        public void load_accessory_list(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");

            string report_id = context.Request.Params.Get("report_id");
            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;
             string return_type = "审核退回";
             string sqlwhere = "where ra.report_id='" + report_id + "' and ra.return_type = '" + return_type + "' ";

            string sql = "select * from (select row_number()over(order by ra." + sortby + " " + order + @")RowId,ra.*, ui.User_name as Add_personnel_n from dbo.TB_NDT_return_accessory as ra left join dbo.TB_user_info as ui on ra.Add_personnel=ui.User_count 
                                      " + sqlwhere + ")a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_return_accessory as ra " + sqlwhere;
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //添加附件
        public void save_accessory(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Edit;

            context.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            context.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            context.Response.Charset = "UTF-8";
            // 获取上传文件文件
            string report_id = context.Request.Params.Get("report_id");
            string report_num = context.Request.Params.Get("report_num");
            string return_info = context.Request.Params.Get("return_info");
            string remarks = context.Request.Params.Get("remarks");
            string return_type = "审核退回";

            string Filedata = context.Request.Params.Get("Filedata");
            HttpPostedFile file = context.Request.Files["Filedata"];

            string fileName2 = Path.GetFileName(file.FileName);                      //原始文件名称,包含扩展名  
            string filename = fileName2.Substring(0, fileName2.LastIndexOf("."));     //文件名称，去掉扩展名 
            string fileExtension = Path.GetExtension(fileName2).ToLower();                     //文件扩展名
            if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".doc" && fileExtension != ".docx" && fileExtension != ".pdf")
            {
                context.Response.Write("请上传图片或文档格式文件");
                context.Response.End();
            }
            string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称
            string date = DateTime.Now.ToLocalTime().ToString();                    //保存时间
            string personnel = Convert.ToString(context.Session["loginAccount"]);   //用户

            //文件保存路径
            string uploadPaths = context.Server.MapPath(ConfigurationManager.AppSettings["Lossless_report_Picture"].ToString());
            file.SaveAs(uploadPaths + saveName);

            string filename_url = ConfigurationManager.AppSettings["Lossless_report_Picture"].ToString() + saveName;

            SqlParameter[] para = 
                {
                    new SqlParameter("@report_id",report_id),
                    new SqlParameter("@filename",filename),
                    new SqlParameter("@return_info",return_info),
                    new SqlParameter("@filename_url",filename_url),
                    new SqlParameter("@personnel",personnel),
                    new SqlParameter("@date",date),
                    new SqlParameter("@return_type",return_type),
                    new SqlParameter("@fileExtension",fileExtension),
                    new SqlParameter("@report_id2",report_id),
                    new SqlParameter("@state_",state_),
                    //new SqlParameter("@Inspection_personnel",Inspection_personnel),
                    //new SqlParameter("@messageContent",messageContent),
                    //new SqlParameter("@message_type",message_type),
                    new SqlParameter("@remarks",remarks)
                };

            String insert_sql = "INSERT INTO dbo.TB_NDT_return_accessory (report_id,Picture_name,return_info,Picture_url,Add_personnel,Add_time,Picture_format,return_type,remarks) values("
                + " @report_id, @filename ,@return_info ,@filename_url ,@personnel ,@date ,@fileExtension ,@return_type ,@remarks ) " 
                + ";update dbo.TB_NDT_report_title set return_info=@return_info where id=@report_id";


            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                SQLStringList2.Add(para);
                SQLStringList.Add(insert_sql);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "上传无损报告退回审核截图", "报告编号:" + report_num);

                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        //删除附件
        public void del_accessory(HttpContext context)
        {
            string report_num = context.Request.Params.Get("report_num");
            string id = context.Request.Params.Get("id");

            String del_sql = "delete dbo.TB_NDT_return_accessory where id='" + id + "' ";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                SQLStringList.Add(del_sql);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "删除无损报告退回审核截图", "报告编号:" + report_num);

                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }

        //加载组
        public void load_professional_department(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select id,Department_name from dbo.TB_department ";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }
        public string Dataset2Json1(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {

                json.Append("[");

                json.Append(DataTable2Json(dt));
                json.Append("]");
            }
            return json.ToString();
        }
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    string text_ = dt.Rows[i][j].ToString().Trim().Replace("\\", "\\\\");
                    text_ = text_.Replace("\r", "");
                    text_ = text_.Replace("\n", "");
                    //\r\n
                    jsonBuilder.Append(text_);
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }

            return jsonBuilder.ToString();
        }
        //加载组人员
        public void load_responsible_people(HttpContext context)
        {
            String User_department = context.Request.Params.Get("User_department");
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select u.User_count,u.User_name from TB_user_info as u "
            + " left join  dbo.TB_user_department as d  on u.User_count=d.User_count  where"
                + " d.User_department= '" + User_department + "' ";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }

        ///加载全部退回原因
        public void AllErrorInfo(HttpContext context)
        {
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            string h_order = context.Request.Params.Get("order");
            string h_sortname = context.Request.Params.Get("sort");
            if (h_order == null)
            {
                h_order = "desc";
                h_sortname = "id";
            }
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by " + h_sortname + " " + h_order + ")RowId,* from dbo.TB_dictionary_managing_context where group_id=17 )a where RowId  >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_dictionary_managing_context where group_id=17 ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        #region
        ///添加退回原因
        public void AddErrorInfo(HttpContext context)
        {
            String report_num = context.Request.Params.Get("report_num");
            String id = context.Request.Params.Get("id");//id
            String error_remarks = context.Request.Params.Get("error_remarks");//错误原因
            string constitute_personnel = context.Request.Params.Get("constitute_personnel");//报告编制人
            string add_date = DateTime.Now.ToLocalTime().ToString();                    //时间
            string addpersonnel = Convert.ToString(context.Session["loginAccount"]);   //用户

            string select_info = "select * from dbo.TB_NDT_error_log where error_remarks='" + error_remarks + "' and report_id='" + id + "'";
            int count_1 = 0;
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, select_info);
            while (dr.Read())
            {
                count_1 = 1;
            }
            dr.Close();

            if (count_1 == 1)
            {
                context.Response.Write("不可重复添加");
                context.Response.End();
            }
            else
            {
                string insert = "insert into dbo.TB_NDT_error_log (report_id, error_remarks, constitute_personnel, addpersonnel, add_date) values ('" + id + "','" + error_remarks + "','" + constitute_personnel + "','" + addpersonnel + "','" + add_date + "')";

                try
                {
                    List<string> SQLStringList = new List<string>();
                    SQLStringList.Add(insert);

                    //sql事务
                    SQLHelper.ExecuteSqlTran(SQLStringList);
                    string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                    string login_username = Convert.ToString(context.Session["login_username"]);
                    Operation_log.operation_log_(loginAccount, login_username, "写入无损报告退回原因", "报告编号(" + report_num + ")");
                    context.Response.Write("T");

                }
                catch (Exception)
                {

                    context.Response.Write("F");

                }
                finally { context.Response.End(); }

            }


        }
        ///删除退回原因
        public void DelErrorInfo(HttpContext context)
        {
            //从任务表中获取
            String id = context.Request.Params.Get("id");//已选原因的id
            String report_num = context.Request.Params.Get("report_num");
            String error_remarks = context.Request.Params.Get("error_remarks");//错误原因

            string delete = "delete dbo.TB_NDT_error_log where id='" + id + "'";
            try
            {
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(delete);

                //sql事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "取消已选无损退回原因", "报告编号" + report_num + "：'" + error_remarks + "'");
                context.Response.Write("T");

            }
            catch (Exception)
            {

                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        #endregion
        //加载已选退回原因
        public void ReturnErrorInfo(HttpContext context)
        {
            String id = context.Request.Params.Get("id");//报告的id
            string getpage1 = context.Request.Params.Get("page");
            string getrows1 = context.Request.Params.Get("rows");
            string h_order = context.Request.Params.Get("order");
            string h_sortname = context.Request.Params.Get("sort");
            if (h_order == null)
            {
                h_order = "desc";
                h_sortname = "id";
            }
            int getpage = Convert.ToInt32(getpage1);
            int getrows = Convert.ToInt32(getrows1);
            int frist = getrows * (getpage - 1) + 1;
            int newrow = getrows * getpage;
            SQLHelper sqla = new SQLHelper();
            string strfaca = "select * from (select row_number()over(order by " + h_sortname + " " + h_order + ")RowId,* from dbo.TB_NDT_error_log where report_id ='"
                + id + "')a where RowId  >= '" + frist + "'and RowId <='" + newrow + "'";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_NDT_error_log where report_id ='" + id + "'";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
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