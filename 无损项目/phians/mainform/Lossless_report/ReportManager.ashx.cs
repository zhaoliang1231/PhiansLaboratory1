using phians.custom_class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace phians.mainform.Lossless_report
{
    /// <summary>
    /// ReportManager1 的摘要说明
    /// </summary>
    public class ReportManager1 : IHttpHandler, IRequiresSessionState
    {
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.Params.Get("cmd");
            switch (command)
            {
                case "load_List": load_List(context); break;//加载报告审核datagrid
                case "ReportSearch": ReportSearch(context); break;//加载报告审核datagrid
                //case "search_list": search_list(context); break;
                //case "Load_GasphysicList": Load_GasphysicList(context); break;//加载报告审核datagrid
                // case "download_word": download_word(context); break;//预览报告
            }
        }
        #region 加载报告
        /// <summary>
        /// 加载报告审核
        /// </summary>
        /// <param name="context"></param>
        public void load_List(HttpContext context)
        {
            //int state_ = (int)LosslessReportStatusEnum.Finish;
            //int state_2 = (int)LosslessReportStatusEnum.Abnormal;
            //string Audit_personnel = Convert.ToString(context.Session["loginAccount"]);//检测人员

            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");
            string DBType = context.Request.Params.Get("DBType");//查哪个库

            string search1 = context.Request.Params.Get("search1");
            string search = context.Request.Params.Get("search");
            string key = context.Request.Params.Get("key");
            string key1 = context.Request.Params.Get("key1");


            string search2 = context.Request.Params.Get("search2");
            string search3 = context.Request.Params.Get("search3");
            string key2 = context.Request.Params.Get("key2");
            string key3 = context.Request.Params.Get("key3");

            string history_flag = context.Request.Params.Get("history_flag");
            string loginAccount = Convert.ToString(context.Session["loginAccount"]);
            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;



            string sql = "";
            if (DBType == "理化系统")
            {
                string sqlwhere = "";
                if (!string.IsNullOrEmpty(key2))
                {
                    if (search2 == "issue_date_")
                    {
                        sqlwhere += " and convert(varchar(10), ci." + search2 + ",120) like '%" + key2 + "%'";
                    }
                    else
                    {
                        sqlwhere += " and ci." + search2 + " like '%" + key2 + "%'";
                    }
                }
                if (!string.IsNullOrEmpty(key3))
                {
                    if (search3 == "issue_date_")
                    {
                        sqlwhere += " and convert(varchar(10), ci." + search3 + ",120) like '%" + key3 + "%'";
                    }
                    else
                    {
                        sqlwhere += " and ci." + search3 + " like '%" + key3 + "%'";
                    }
                }
                //EastGas_Test_lihua_20171103,东方重机：DB_phians_Gasphysic2017
                sql = "select * from (select row_number()over(order by ci." + sortby + " " + order + @")RowId,ci.*,ui.user_name as clientele_name FROM DB_phians_Gasphysic2017.dbo.TB_contract_info ci left join DB_phians_Gasphysic2017.dbo.tb_user_info ui on ci.clientele = ui.user_count
                                WHERE contract_state = " + 6 + sqlwhere + " )a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
                sql += @"select count(0)   
                                    FROM DB_phians_Gasphysic2017.dbo.TB_contract_info ci where contract_state = " + 6 + sqlwhere + "";
            }
            else
            {
                string sqlwhere = "where (rt.state_ = " + 4 + " or rt.state_ = " + 5 + ")";//报告完成状态 

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
                //
                //EastGas_Test_wusun_20171212，东方重机：DB_phians_Lossless
                sql = "select * from (select row_number()over(order by rt." + sortby + " " + order + @")RowId,RT.*,UI.user_name as Inspection_personnel_n,UI1.user_name as issue_personnel_n,UI3.user_name as  Audit_personnel_n from DB_phians_Lossless.dbo.TB_NDT_report_title RT left join DB_phians_Lossless..tb_user_info UI on RT.Inspection_personnel=UI.User_count left join DB_phians_Lossless..tb_user_info UI1 on RT.issue_personnel=UI1.User_count LEFT JOIN DB_phians_Lossless..tb_user_info UI3 ON RT.Audit_personnel=UI3.User_count
                                      " + sqlwhere + ")a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
                sql += @"select count(0)   
                                    FROM DB_phians_Lossless.dbo.TB_NDT_report_title as rt " + sqlwhere + "";
            }
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }

        #endregion



        #region 搜索报告
        /// <summary>
        /// 搜索报告
        /// </summary>
        /// <param name="context"></param>
        public void ReportSearch(HttpContext context)
        {

            try
            {

                string DBType = context.Request.Params.Get("SysType");//v系统类型:0:理化1：无损
                string Inspection_NO = context.Request.Params.Get("Inspection_NO");//检验编号

                string url = "";//报告路径
                string sql = "";
                string verification_result_Sql = "";
                string DataBase = ConfigurationManager.AppSettings["DataBase"].ToString();
                string DataBaseLossless = ConfigurationManager.AppSettings["DataBaseLossless"].ToString();

                ReportModel model = new ReportModel();
                if (DBType == "0")
                {
                    url = "http://10.115.100.68:8081";
                    sql = "select Inspection_NO,contract_num as report_num,report_total_url as report_url  FROM " + DataBase + ".dbo.TB_contract_info where Inspection_NO like'%" + Inspection_NO + "%'";
                    //查询试验结果
                    
                    DataTable dt = db.ExecuteDataTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        model.Inspection_NO = dt.Rows[0]["Inspection_NO"].ToString();
                        model.contract_num = dt.Rows[0]["report_num"].ToString();
                        model.report_url = url + dt.Rows[0]["report_url"].ToString();
                        verification_result_Sql = "select test_project, verification_result FROM " + DataBase + ".dbo.TB_contract_task where contract_num ='" + model.contract_num + "'";
                        DataTable result_dt = db.ExecuteDataTable(verification_result_Sql);
                        for (int i = 0; i < result_dt.Rows.Count;i++ )
                        {
                            model.verification_result += result_dt.Rows[i]["test_project"].ToString() + ":" + result_dt.Rows[i]["verification_result"].ToString() + ",";
                        }
                    }

                }
                else
                {
                    url = "http://10.115.100.68:8087";
                    sql = "select Inspection_NO,report_num,report_url,case  Inspection_result when '0' then '不合格' when '1' then '合格' else '空' end as Inspection_result from  " + DataBaseLossless + ".dbo.TB_NDT_report_title where Inspection_NO like '%" + Inspection_NO + "%'";
                    DataTable dt = db.ExecuteDataTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        model.Inspection_NO = dt.Rows[0]["Inspection_NO"].ToString();
                        model.contract_num = dt.Rows[0]["report_num"].ToString();
                        model.report_url = url + dt.Rows[0]["report_url"].ToString();
                        model.verification_result = dt.Rows[0]["Inspection_result"].ToString();
                    }
                }

                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Phians_Entity.SingleExecuteResult<ReportModel>(true, "成功", model)));
                //context.Response.End();
            }
            catch (Exception e)
            {
               context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Phians_Entity.ExecuteResult(false,"失败")));
               context.Response.End();
            }

            
           
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

    public class ReportModel
    {

        ///<summary>
        /// 检验编号
        /// </summary>		
        public string Inspection_NO
        {
            get;
            set;
        }

        ///<summary>
        /// 报告编号
        /// </summary>		
        public string contract_num
        {
            get;
            set;
        }
        ///<summary>
        /// 报告路径
        /// </summary>		
        public string report_url
        {
            get;
            set;
        }
        ///<summary>
        /// 试验结论
        /// </summary>		
        public string verification_result
        {
            get;
            set;
        }
    }
}