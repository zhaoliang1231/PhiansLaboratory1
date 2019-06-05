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
    /// LosslessReport_Issue1 的摘要说明
    /// </summary>
    public class LosslessReport_Issue1 : IHttpHandler, IRequiresSessionState
    {
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.Params.Get("cmd");
            switch (command)
            {
                case "load_list": load_list(context); break;//加载报告签发datagrid
                case "Back_report": Back_report(context); break;//退回报告编制
                case "submit_issue_Report": submit_issue_Report(context); break;//提交报告签发

                case "load_accessory_list": load_accessory_list(context); break;//加载附件列表
                case "save_accessory": save_accessory(context); break;//添加附件
                case "del_accessory": del_accessory(context); break;//删除附件
            }
        }

        /// 加载报告签发datagrid
        public void load_list(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Issue;
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
            string personnel = Convert.ToString(context.Session["loginAccount"]);   //用户
            string select_department = "select count(*) from dbo.TB_user_department where User_count='" + loginAccount + "' and User_department=48";//User_department=48——签发组
            int count = Convert.ToInt32( db.ExecuteScalar(select_department).ToString());

            string sqlwhere = "";
            if (count >0)
            {

                sqlwhere = "where 1=1 ";
                //历史签发
                if (history_flag == "1")
                {
                    sqlwhere += " and state_ != " + state_ + " ";

                }
                //待签发
                if (history_flag == "0")
                {
                    sqlwhere += " and state_ = " + state_;

                }
                //查询
                if (!string.IsNullOrEmpty(search))
                {
                    if (search == "ReviewOverdue")//审核时间逾期：审核签字时间-编制签字时间
                    {
                        sqlwhere += "and Audit_date-Inspection_personnel_date>=" + key + "";
                    }
                    else if (search == "IssueOverdue")//签发逾期
                    {
                        sqlwhere += "and rt.id in ( select ReportID from( select ReportID, min(OperateDate) as EditTime from TB_ProcessRecord  where NodeId='1' group by ReportID)as a where '" + DateTime.Now.ToString() + "'-EditTime>=" + key + ")";
                    }
                    else{
                        sqlwhere += " and rt." + search + " like '%" + key + "%'";
                    }
                }
            }
            else
            {
                sqlwhere = "where 1=0 ";
            }



            string sql = "select * from (select row_number()over(order by rt." + sortby + " " + order + @")RowId,rt.*,ui.User_name as Inspection_personnel_n ,ui2.User_name as Audit_personnel_n from dbo.TB_NDT_report_title rt left join dbo.TB_user_info ui on rt.Inspection_personnel=ui.User_count left join dbo.TB_user_info ui2 on rt.Audit_personnel=ui2.User_count
                                      " + sqlwhere + " and Audit_personnel!='" + personnel + "')a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)  FROM dbo.TB_NDT_report_title as rt " + sqlwhere + "";
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        /// 退回报告编制
        public void Back_report(HttpContext context)
        {
            string id = context.Request.Params.Get("id");//报告的id
            string report_num = context.Request.Params.Get("report_num");
            string Inspection_personnel = context.Request.Params.Get("Inspection_personnel");
            string report_url = context.Request.Params.Get("report_url");
            string return_info = context.Request.Params.Get("return_info");//退回原因
            string FirstInspection_personnel_date = DateTime.Now.ToString();//退回时间
            int state_ = (int)LosslessReportStatusEnum.Edit;


            #region//*****************退回文字信息
            String error_remark = context.Request.Params.Get("error_remarks");//错误原因
            string insert = "";
            if (error_remark!="") {
                string add_date = DateTime.Now.ToLocalTime().ToString();                    //时间
                string addpersonnel = Convert.ToString(context.Session["loginAccount"]);   //用户
                string ReturnNode = "无损报告签发退回";

                string[] error_remarks = error_remark.Split(',');
                
                for (int i = 0; i < error_remarks.Length; i++)
                {
                    insert += ";insert into dbo.TB_NDT_error_log (report_id, error_remarks, constitute_personnel, addpersonnel, add_date, ReturnNode) values ('" + id + "','" + error_remarks[i] + "','" + Inspection_personnel + "','" + addpersonnel + "','" + add_date + "','" + ReturnNode + "')";
                }
            }
            #endregion

            string messageContent = " 报告编号为：" + report_num + "的无损报告需要重新编制;签发退回原因：" + return_info;
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
            string upate_rep = "update dbo.TB_NDT_report_title set return_info=@return_info,return_flag=1,Inspection_personnel_date='',state_=@state_,FirstInspection_personnel_date=@FirstInspection_personnel_date,Audit_personnel='',issue_personnel='',issue_date='' where id=@id ";
            //添加报告流程记录
            string insertProcessRecord = "insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values ('" + id + "','" + login_user + "','" + FirstInspection_personnel_date + "','" + "pass" + "','" + (int)TB_ProcessRecordNodeIdEnum.IssueToEdit + "')";

            #region 报告审核人审核时间,签发人 签发时间书签清除
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
            //删除审核时间
            try
            {
                first_doc.Range.Bookmarks["Audit_date"].Text = "";//审核人签字时间
            }
            catch (Exception ex)
            {
            }
            //删除审核级别
            try
            {
                first_doc.Range.Bookmarks["level_Audit"].Text = "";//审核级别
            }
            catch (Exception ex)
            {
            }
            //删除审核人员签名                    
            try
            {
                first_doc.Range.Bookmarks["Audit_personnel"].Text = "";
            }
            catch
            { }
            first_doc.Save(context.Server.MapPath(report_url), Aspose.Words.SaveFormat.Doc);
            #endregion

            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(messageSqls);
                db.ExecuteNonQueryByTrans(insertProcessRecord);
                db.ExecuteNonQueryByTrans(upate_rep, para);
                if (insert != "")
                {
                    db.ExecuteNonQueryByTrans(insert);
                }
                db.CommitTransacton();

                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "无损报告签发退回", "报告编号为：" + report_num + "的报告");
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
        /// 提交报告完成
        public void submit_issue_Report(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Finish;

            string date = context.Request.Params.Get("date");                    //时间

            //不同信息
            string id = context.Request.Params.Get("ids");//批量提交
            String report_num = context.Request.Params.Get("report_nums");
            String report_url = context.Request.Params.Get("report_urls");
            //string[] ids = id.Split(',');
            //string[] report_urls = report_url.Split(',');
            //string[] report_nums=report_num.Split(',');
            //相同信息
            string issue_personnel = Convert.ToString(context.Session["loginAccount"]);   //签发人

            string getDetectionSql = @"select top 1 Signature as detection_image from dbo.TB_user_info where User_count = '" + issue_personnel + "'";
            DataTable detection_dt = db.ExecuteDataTable(getDetectionSql);
            //判断签名是否存在
            if (!File.Exists(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())))
            {
                context.Response.Write("签名不存在");
                context.Response.End();
            }

            #region 写入多个报告 签发人员时间级别等信息
            //for (int i = 0; i < ids.Length; i++)
            
                Document first_doc = new Document(context.Server.MapPath(report_url));///upload_Folder/word_certificate/0e8b03a4-a7e5-455f-bea1-441bf5076448.doc
                DocumentBuilder first_builder = new DocumentBuilder(first_doc);

                //写入记录文档
                String select = "select top 1 report_url from dbo.TB_NDT_RevisionsRecord where report_id='" + id + "' order by id desc";
                string reportLogURl = db.ExecuteScalar(select).ToString();
                Document record_Logdoc = new Document(context.Server.MapPath(reportLogURl));
                DocumentBuilder record_Logbuilder = new DocumentBuilder(record_Logdoc);

                if (detection_dt.Rows.Count > 0)
                {
                    //写入试验时间
                    try
                    {
                        //DateTime new_time = DateTime.Today;
                        //string Audit_date = string.Format("{0:yyyy-MM-dd}", new_time);
                        first_doc.Range.Bookmarks["issue_date"].Text = date;//检验人签字时间
                        record_Logdoc.Range.Bookmarks["issue_date"].Text = date;
                    }
                    catch (Exception ex)
                    {
                        //context.Response.Write(ex);
                        //context.Response.End();
                    }
                    //写入试验人员签名                    
                    try
                    {
                        first_doc.Range.Bookmarks["issue_personnel"].Text = "";
                        first_builder.MoveToBookmark("issue_personnel");//跳到指定书签
                        double width = 50, height = 20;
                        Aspose.Words.Drawing.Shape shape = first_builder.InsertImage(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                        shape.Width = width ;
                        shape.Height = height ;
                        //shape.VerticalAlignment = VerticalAlignment.Center;
                        first_builder.InsertNode(shape);

                        record_Logdoc.Range.Bookmarks["issue_personnel"].Text = "";
                        record_Logbuilder.MoveToBookmark("issue_personnel");//跳到指定书签
                        //double width = 50, height = 20;
                        Aspose.Words.Drawing.Shape shapes = record_Logbuilder.InsertImage(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                        shapes.Width = width;
                        shapes.Height = height;
                        //shape.VerticalAlignment = VerticalAlignment.Center;
                        record_Logbuilder.InsertNode(shapes);
                    }
                    catch
                    { }
                }

                first_doc.Save(context.Server.MapPath(report_url), Aspose.Words.SaveFormat.Doc);
                record_Logdoc.Save(context.Server.MapPath(reportLogURl), Aspose.Words.SaveFormat.Doc);
            
            #endregion

            string upate_ = "update dbo.TB_NDT_report_title set state_='" + state_ + "',issue_personnel='" + issue_personnel + "',issue_date='" + date + "' where id in (" + id + ") ";

            //添加报告流程记录
            string insertProcessRecord = "insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values ('" + id + "','" + issue_personnel + "','" + DateTime.Now.ToString() + "','" + "pass" + "','" + (int)TB_ProcessRecordNodeIdEnum.IssueToManage + "')";
            try
            {
                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(upate_);
                db.ExecuteNonQueryByTrans(insertProcessRecord);
                db.CommitTransacton();

                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "提交无损报告签发", "报告编号：" + report_num);
                //new Send_message().send_usercount(Audit_personnel, messageContent);
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

        //加载附件列表datagrid
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
            string return_type = "签发退回";
            string sqlwhere = "where ra.report_id='" + report_id + "' and ra.return_type = '" + return_type + "'";

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
            string return_type = "签发退回";

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
                Operation_log.operation_log_(loginAccount, login_username, "上传无损报告退回签发截图", "报告编号:" + report_num);

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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}


//扫查器
//数据采集系统
//运动控制器

//标定样管
//涡流仪

//氦浓度仪
//检测仪器
//湿度计
//温度计
//压力表
//真空计

//测温仪
//使用设备
//照度计

//测温仪
//黑光强度剂
//使用设备

//仪器

//仪器

//超声数据采集系统
//扫查器
//运动控制器

//仪器
//照度计/黑光强度计
