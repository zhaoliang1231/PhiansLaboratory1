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
    /// LosslessReport_Monitor2 的摘要说明
    /// </summary>
    public class LosslessReport_Monitor2 : IHttpHandler, IRequiresSessionState
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
                case "Preview_Report": Preview_Report(context); break;//预览报告

            }
        }

        /// <summary>
        /// 加载报告审核
        /// </summary>
        /// <param name="context"></param>

        public void load_list(HttpContext context)
        {
            //int state_ = (int)LosslessReportStatusEnum.Finish;
            //int state_2 = (int)LosslessReportStatusEnum.Abnormal;
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

            string sqlwhere = "where 1=1 ";

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
                else
                {

                    sqlwhere += " and rt." + search4 + " like '%" + key4 + "%'";
                }
            }


            string sql = "select * from (select row_number()over(order by rt." + sortby + " " + order + @")RowId,RT.*,UI.user_name as Inspection_personnel_n,UI1.user_name as issue_personnel_n,UI3.user_name as  Audit_personnel_n,td.Department_name as Auit_groupid_n  from dbo.TB_NDT_report_title RT left join tb_user_info UI on RT.Inspection_personnel=UI.User_count left join tb_user_info UI1 on RT.issue_personnel=UI1.User_count LEFT JOIN tb_user_info UI3 ON RT.Audit_personnel=UI3.User_count left join dbo.TB_department as td on rt.Audit_groupid=td.id
                                      " + sqlwhere + ")a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_report_title as rt " + sqlwhere + "";
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }

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
                    WordConverter new_word = new WordConverter();

                    //string flag = new_word.WordToPdf(context.Server.MapPath(report_url), context.Server.MapPath(file_path + new_doc_name));
                    // 转换pdf
                    bool flag = new_word.Convertwordtopdf(NEW_DOC, context.Server.MapPath(file_path + new_doc_name));
                    // context.Response.Write(file_path + new_doc_name);
                    //pdf_.Save(context.Server.MapPath(file_path + new_doc_name), SaveFormat.Pdf);
                    if (flag)
                    {
                        context.Response.Write(file_path + new_doc_name);
                    }
                    else
                    {
                        context.Response.Write(flag);
                    }
                }
                catch (Exception e)
                {

                    context.Response.Write(e.ToString());
                }


            }


            context.Response.End();
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}