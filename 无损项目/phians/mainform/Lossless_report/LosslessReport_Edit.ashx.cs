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
using Aspose.Words.Fields;

namespace phians.mainform.Lossless_report
{
    /// <summary>
    /// LosslessReport_Edit1 的摘要说明
    /// </summary>
    public class LosslessReport_Edit1 : IHttpHandler, IRequiresSessionState
    {
        private readonly DBHelper db = new DBHelper();
        private readonly JsonHelper jsonHelper = new JsonHelper();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string command = context.Request.Params.Get("cmd");
            switch (command)
            {
                case "loadPageShowSetting": loadPageShowSetting(context); break;//页面字段显示list
                case "load_list": load_list(context); break;//加载list
                case "editInfo": editInfo(context); break;//写入报告信息
                case "editInfo2": editInfo2(context); break;//修改报告信息
                case "AddTextData": AddTextData(context); break;//添加检测数据
                case "DataDel": DataDel(context); break;//删除信息
                case "Filling_report": Filling_report(context); break;//载入报告模板文件
                case "Get_Report_Url": Get_Report_Url(context); break;//在线编辑
                case "submit_edit_Report": submit_edit_Report(context); break;//提交审核报告

                case "load_professional_department": load_professional_department(context); break;//加载审核组
                case "load_responsible_people": load_responsible_people(context); break;//加载审核组人员
                case "load_Device_test": load_Device_test(context); break;//加载试样设备
                case "load_Probe_test": load_Probe_test(context); break;//加载试验探头
                case "load_Probe_test2": load_Probe_test2(context); break;//加载已选试验探头
                case "load_equipment_info": load_equipment_info(context); break;//回显设备信息
                case "add_Probe_test": add_Probe_test(context); break;//添加试验探头
                case "del_Probe_test": del_Probe_test(context); break;//删除试验探头

                //*********************附件加载/操作
                case "load_accessory": load_accessory(context); break;//加载附件datagrid
                case "upload_accessory": upload_accessory(context); break;//添加附件
                case "update_accessory": update_accessory(context); break;//修改附件附注
                case "del_accessory": del_accessory(context); break;//删除附件
                case "AddAccessory": AddAccessory(context); break;//附件添加到报告
                case "downloadAccessory": DownloadAccessory(context); break;//批量下载附件

                //退回附件显示列表
                case "load_accessory_list": load_accessory_list(context); break;//加载附件列表
                case "GET_MESINFO": GET_MESINFO(context); break;//获取mes系统信息

                //********************回显复制报告信息
                case "loadReportCopy": loadReportCopy(context); break;//加载待复制的报告
                case "reportcopyshow": ReportCopyShow(context); break;//回显复制报告信息

                //**********************显示退回信息
                case "load_errorinfo": LoadErrorInfo(context); break;//加载退回原因
  

                //********************记录文档
                //case "AddRecord": AddRecord(context); break;//产生已提交的记录文档
                case "readrecord": ReadRecord(context); break;//查看所有历史的记录文档
                case "Preview_Report": Preview_Report(context); break;//查看所有历史的记录文档_打开word

                case "GET_MESlist": GET_MESlist(context); break;//获取mes系统信息
                case "GetOrderNUM": GetOrderNUM(context); break;//获取SAp订单号
                case "Getmethod": Getmethod(context); break;//根据流转卡号和工序号查方法

            }
        }

        /// 页面字段显示List
        public void loadPageShowSetting(HttpContext context)
        {
            //int PageId = 101;//报告编制页面的fid
            string PageId = context.Request.Params.Get("PageId");//报告编制页面PageId=101，报告审核页面PageId=102，报告签发页面PageId=103，报告管理页面PageId=104，报告监控页面PageId=113
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select * from dbo.TB_PageShowCustom where PageId='" + PageId + "' order by FieldSort";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            //int count = dt.Rows.Count;
            //string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据  

            //将搜索出来的字段拼接成json串
            string strJson = "[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //if (Convert.ToString(dt.Rows[i][2]) == "state_")//如果为状态，这需要formatter
                //{
                //    strJson = strJson + "{ field:'" + Convert.ToString(dt.Rows[i][2]) + "',title:'" + Convert.ToString(dt.Rows[i][3]) + "',hidden:'" + Convert.ToString(dt.Rows[i][5]).ToLower() + "',sortable:'" + Convert.ToString(dt.Rows[i][6]).ToLower() 
                //        + "',formatter: function (value, row, index) { if (value == \"1\") {return \"编辑\";} if (value == \"2\") {  return \"审核\";}if (value == \"3\") { return \"签发\";} if (value == \"4\") {  return \"完成\";}}  },";
                //}
                //else
                //{
                strJson = strJson + "{ \"fieldname\":\"" + Convert.ToString(dt.Rows[i][2]) + "\",\"hidden\":" + Convert.ToString(dt.Rows[i][5]).ToLower() + ",\"sortable\":" + Convert.ToString(dt.Rows[i][6]).ToLower() + " },";
                //}
            }
            strJson = strJson.Substring(0, strJson.Length - 1); //去掉最后的逗号
            strJson = strJson + "]";

            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }

        #region 根据流转卡号和工序号查方法
        public void Getmethod(HttpContext context)
        {

            string circulation_NO = context.Request.Params.Get("circulation_NO");//流转卡号
            string procedure_NO = context.Request.Params.Get("procedure_NO");//工序号
            string retrun_OrderNUM = "";
            try
            {
                get_interface.phians_webservice_N1SoapClient new_client = new get_interface.phians_webservice_N1SoapClient();
                retrun_OrderNUM = new_client.Getmethod(circulation_NO, procedure_NO);
                context.Response.Write(retrun_OrderNUM);
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
        #endregion


        #region 获取SAp订单号
        public void GetOrderNUM(HttpContext context)
        {

            string OrderNUM = context.Request.Params.Get("OrderNUM");
            string retrun_OrderNUM = "";
            try
            {
                get_interface.phians_webservice_N1SoapClient new_client = new get_interface.phians_webservice_N1SoapClient();
                retrun_OrderNUM = new_client.GetOrderNUM(OrderNUM);
                context.Response.Write(retrun_OrderNUM);
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
        #endregion
        #region  批量下载附件
        /// <summary>
        /// 批量下载附件
        /// </summary>
        /// <param name="context"></param>
        public void DownloadAccessory(HttpContext context)
        {
            List<string> report_url = new List<string>();//需要下载的文件路径
            List<string> newfilename = new List<string>();//复制后以后新文件名

            //string[] report_url_count = context.Request.Params.Get("report_urls").Split(',');//获取选择行的文件路径
            string ids = context.Request.Params.Get("ids");

            string sql = "select accessory_url,accessory_name,accessory_format from dbo.TB_NDT_report_accessory where id in(" + ids + ") ";
            DataTable select_dt = db.ExecuteDataTable(sql);
            for (int i = 0; i < select_dt.Rows.Count; i++)
            {
                string newreport_url = select_dt.Rows[i]["accessory_url"].ToString().Substring(1, select_dt.Rows[i]["accessory_url"].ToString().Length - 1);
                report_url.Add(newreport_url);
                newfilename.Add((select_dt.Rows[i]["accessory_name"].ToString()).Replace("/", "_") + select_dt.Rows[i]["accessory_format"].ToString());
            }

            //临时存放需要复制的文件夹
            string rootUrl = context.Server.MapPath(ConfigurationManager.AppSettings["view_temp_Folder"].ToString()) + DateTime.Now.ToString("yyyyMMddhhmmss");
            Directory.CreateDirectory(rootUrl);
            //  
            string path = System.AppDomain.CurrentDomain.BaseDirectory;

            //循环复制文件
            for (int i = 0; i < report_url.Count; i++)
            {
                //拷贝文件(路径+copy的文件,拷贝到的路径+新文件名)
                System.IO.File.Copy(path + report_url[i], rootUrl + "/" + newfilename[i], true);
            }

            //临时文件夹
            // string zipurl = ConfigurationManager.AppSettings["view_temp_Folder"].ToString();
            string savePath = ConfigurationManager.AppSettings["view_temp_Folder"].ToString() + DateTime.Now.ToString("yyyyMMddhhmmss") + ".zip";

            try
            {
                using (var zip = new ZipFile(Encoding.Default))
                {
                    //把待添加文件添加到压缩包
                    zip.AddDirectory(rootUrl);
                    //保存压缩包
                    zip.Save(context.Server.MapPath(savePath)); //生成包的名称
                }
                context.Response.Write(savePath);
                //删除临时文件夹
                Directory.Delete(rootUrl, true);
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
        #endregion


        #region  查看记录文档word
        /// <summary>
        /// 预览报告
        /// </summary>
        /// <param name="context"></param>
        public void Preview_Report(HttpContext context)
        {
            string id = context.Request.Params.Get("id");//记录文档的id

            try
            {
                String select = "select report_url from dbo.TB_NDT_RevisionsRecord where id='" + id + "'";
                string report_url = db.ExecuteScalar(select).ToString();
                context.Response.Write(report_url);
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }
        }
        #endregion


        //查看所有历史的记录文档
        public void ReadRecord(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");

            string id = context.Request.Params.Get("id");//报告id

            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;
            string sql = "select * from (select row_number()over(order by rr." + sortby + " " + order + @")RowId, rr.*,ui.User_name as addpersonnel_n from dbo.TB_NDT_RevisionsRecord rr left join dbo.TB_user_info ui on rr.addpersonnel=ui.User_count where rr.report_id='" + id + "' )a where RowId  >= " + fristrow + " and RowId <=" + lastrow + " order by id desc;";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_RevisionsRecord where report_id='" + id + "'";
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //产生已提交的记录文档
        public void AddRecord(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Audit;
            int state_1 = (int)LosslessReportStatusEnum.Issue;
            string select = "select report_url from dbo.TB_NDT_report_title where state_='" + state_ + "' or state_='" + state_1 + "'";
            string report_url = "";
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, select);

            while (dr.Read())
            {
                if (dr["report_url"].ToString() != "")
                {
                    report_url = dr["report_url"].ToString();
                    Document first_doc = new Document(context.Server.MapPath(report_url));
                    first_doc.Save(context.Server.MapPath(report_url), Aspose.Words.SaveFormat.Doc);
                }
            }
            dr.Close();
        }

        //加载退回原因
        public void LoadErrorInfo(HttpContext context)
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
            string strfaca = "select * from (select row_number()over(order by el." + h_sortname + " " + h_order + ")RowId,el.*,rt.return_info,ui.User_name as addpersonnel_n from dbo.TB_NDT_error_log as el left join dbo.TB_NDT_report_title rt on el.report_id=rt.id left join dbo.TB_user_info ui on el.addpersonnel=ui.User_count where report_id ='"
                + id + "')a where RowId  >= '" + frist + "'and RowId <='" + newrow + "' order by id desc ";
            DataSet ds = sqla.GetDataSet(strfaca);
            //将Dataset转化为Datable    
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strsql2 = "select * from dbo.TB_NDT_error_log where report_id ='" + id + "' order by id desc ";
            string strJson = sqla.Dataset2Json(ds, strsql2, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }

        /// 报告编辑List
        public void loadReportCopy(HttpContext context)
        {
            //int state_ = (int)LosslessReportStatusEnum.Edit;

            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");

            string search = context.Request.Params.Get("search");
            string key = context.Request.Params.Get("key");
            //string history_flag = context.Request.Params.Get("history_flag");
            //string loginAccount = Convert.ToString(context.Session["loginAccount"]);

            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;

            string sqlwhere = "where 1=1 ";
            ////历史
            //if (history_flag == "1")
            //{
            //    sqlwhere += "and rt.state_ != " + state_ + " and rt.Inspection_personnel='" + loginAccount + "' ";

            //}
            ////待编制
            //if (history_flag == "0")
            //{
            //    sqlwhere += "and rt.state_ = " + state_ + " and rt.Inspection_personnel='" + loginAccount + "' ";

            //}
            //查询判断
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sqlwhere += " and rt." + search + " like '%" + key + "%'";
            }

            string sql = "select * from (select row_number()over(order by rt." + sortby + " " + order + @")RowId, rt.*, tp.id as tp_id, tp.report_num as tp_report_num, tp.tm_id as tp_tm_id, tp.Data1, tp.Data2, tp.Data3, tp.Data4, tp.Data5, tp.Data6, tp.Data7, tp.Data8, tp.Data9, tp.Data10, tp.Data11, tp.Data12, tp.Data13, tp.Data14, tp.Data15, tp.Data16, tp.Data17, tp.Data18, tp.Data19, tp.Data20, tp.Data21, tp.Data22, tp.Data23, tp.Data24, tp.Data25, tp.Data26, tp.Data27, tp.Data28, tp.Data29, tp.Data30, tp.Data31, tp.Data32, tp.Data33, tp.Data34, tp.Data35, tp.Data36, tp.Data37, tp.Data38, tp.Data39, tp.Data40, tp.Data41, tp.Data42, tp.Data43, tp.Data44, tp.Data45, tp.Data46, tp.Data47, tp.Data48, tp.Data49, tp.Data50, tp.Data51, tp.Data52, tp.Data53, tp.Data54, tp.Data55, tp.Data56, tp.Data57, tp.Data58, tp.Data59, tp.Data60, tp.Data61, tp.Data62, tp.Data63, tp.Data64 from dbo.TB_NDT_report_title as rt left join dbo.TB_NDT_test_probereport_data as tp on rt.id = tp.report_num 
                                      " + sqlwhere + " )a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_report_title as rt left join dbo.TB_NDT_test_probereport_data as tp on rt.id = tp.report_num " + sqlwhere;
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }

        /// 回显复制报告信息
        public void ReportCopyShow(HttpContext context)
        {
            string report_num = context.Request.Params.Get("report_num");
            int state_ = (int)LosslessReportStatusEnum.Scrap;

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


            string sql = "select * from (select row_number()over(order by " + sortby + " " + order + @")RowId, * from dbo.TB_NDT_report_title where report_num='" + report_num + "' and state_!=" + state_ + " )a where RowId  >= " + lastrow + " and RowId <=" + fristrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_report_title where report_num='" + report_num + "' and state_!=" + state_ + "";
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }


        #region  获取MES系统信息

        public void GET_MESINFO(HttpContext context)
        {

            string circulation_NO = context.Request.Params.Get("circulation_NO");//流转卡号
            string procedure_NO = context.Request.Params.Get("procedure_NO");//工序号
            string return_info = "";

            try
            {
                // get_interface.phians_webservice_N1SoapClient new_client = new get_interface.phians_webservice_N1SoapClient();
                MESINTERFACE.MES_INTERFACESoapClient new_client = new MESINTERFACE.MES_INTERFACESoapClient();
                return_info = new_client.GET_MESINFO(circulation_NO + "-" + procedure_NO);
            }
            catch (Exception e)
            {
                return_info = e.ToString(); ;

            }
            finally
            {

                context.Response.Write(return_info);
                context.Response.End();
            }


        }

        #endregion


        #region  获取MES系统信息（table json）

        public void GET_MESlist(HttpContext context)
        {

            string circulation_NO = context.Request.Params.Get("key");//流转卡号       
            string return_info = "";

            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");
            try
            {
                // get_interface.phians_webservice_N1SoapClient new_client = new get_interface.phians_webservice_N1SoapClient();
                MESINTERFACE.MES_INTERFACESoapClient new_client = new MESINTERFACE.MES_INTERFACESoapClient();
                return_info = new_client.GET_MESINFO2(circulation_NO, page, pagesize, order, sortby);
            }
            catch (Exception e)
            {
                return_info = e.ToString(); ;

            }
            finally
            {

                context.Response.Write(return_info);
                context.Response.End();
            }


        }

        #endregion

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

            string sqlwhere = "where ra.report_id='" + report_id + "' ";

            string sql = "select * from (select row_number()over(order by ra." + sortby + " " + order + @")RowId,ra.*, ui.User_name as Add_personnel_n from dbo.TB_NDT_return_accessory as ra left join dbo.TB_user_info as ui on ra.Add_personnel=ui.User_count 
                                      " + sqlwhere + ")a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_return_accessory as ra " + sqlwhere;
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }


        //*********************附件加载/操作
        public void load_accessory(HttpContext context)
        {
            string report_id = context.Request.Params.Get("report_id");

            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");

            string search = context.Request.Params.Get("search");
            string key = context.Request.Params.Get("key");

            if (order == null)
            {
                order = "desc";
                sortby = "id";
            }
            int fristrow = pagesize * (page - 1) + 1;
            int lastrow = page * pagesize;

            string sqlwhere = " where report_id='" + report_id + "' ";

            //查询判断
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sqlwhere += " and " + search + " like '%" + key + "%'";
            }

            string sql = "select * from (select row_number()over(order by " + sortby + " " + order + @")RowId, * from dbo.TB_NDT_report_accessory 
                                      " + sqlwhere + " )a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    from dbo.TB_NDT_report_accessory " + sqlwhere;
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //上传合同模板文件
        public void upload_accessory(HttpContext context)
        {
            context.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
            context.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            context.Response.Charset = "UTF-8";
            // 获取上传文件文件
            string report_num = context.Request.Params.Get("report_num");
            string report_id = context.Request.Params.Get("report_id");
            string accessory_name = context.Request.Params.Get("accessory_name");
            string remarks = context.Request.Params.Get("remarks");
            string Filedata = context.Request.Params.Get("Filedata");
            HttpPostedFile file = context.Request.Files["Filedata"];

            string fileName2 = Path.GetFileName(file.FileName);                      //原始文件名称,包含扩展名  
            string filename = fileName2.Substring(0, fileName2.LastIndexOf("."));     //文件名称，去掉扩展名 
            string fileExtension = Path.GetExtension(fileName2).ToLower();                     //文件扩展名
            if (fileExtension != ".pdf" && fileExtension != ".png" && fileExtension != ".jpeg" && fileExtension != ".jpg" && fileExtension != ".zip")
            {
                context.Response.Write("格式错误");
                context.Response.End();
            }
            string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称
            string date = DateTime.Now.ToLocalTime().ToString();                    //保存时间
            string personnel = Convert.ToString(context.Session["loginAccount"]);   //用户

            //文件保存路径
            string uploadPaths = context.Server.MapPath(ConfigurationManager.AppSettings["Lossless_report_accessory"].ToString());
            file.SaveAs(uploadPaths + saveName);

            string filename_url = ConfigurationManager.AppSettings["Lossless_report_accessory"].ToString() + saveName;

            SqlParameter[] para = 
                {
                    new SqlParameter("@report_id",report_id),
                    new SqlParameter("@accessory_name",accessory_name),
                    new SqlParameter("@filename_url",filename_url),
                    new SqlParameter("@personnel",personnel),
                    new SqlParameter("@date",date),
                    new SqlParameter("@fileExtension",fileExtension),
                    new SqlParameter("@remarks",remarks)
                };

            String insert_sql = "INSERT INTO dbo.TB_NDT_report_accessory (report_id,accessory_name,accessory_url,add_personnel,add_date,accessory_format,remarks) values("
                + " @report_id ,@accessory_name ,@filename_url ,@personnel ,@date ,@fileExtension ,@remarks ) ";
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
                Operation_log.operation_log_(loginAccount, login_username, "上传无损报告附件", "报告编号:" + report_num);

                context.Response.Write("T");
            }
            catch (Exception)
            {
                context.Response.Write("F");

            }
            finally { context.Response.End(); }

        }
        //修改附件附注
        private void update_accessory(HttpContext context)
        {
            String id = context.Request.Params.Get("id");//附件id
            String report_id = context.Request.Params.Get("report_id");
            String remarks = context.Request.Params.Get("remarks");

            SqlParameter[] para = 
                {
                    new SqlParameter("@id",id),
                    new SqlParameter("@remarks",remarks)
                };

            String insert_sql = "update dbo.TB_NDT_report_accessory set remarks=@remarks where id=@id";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                SQLStringList2.Add(para);
                SQLStringList.Add(insert_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "修改无损报告附件备注", "报告编号:" + report_id + "附件id：" + id);
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
        ////删除附件
        private void del_accessory(HttpContext context)
        {
            String id = context.Request.Params.Get("id");//附件id
            String report_id = context.Request.Params.Get("report_id");
            String report_num = context.Request.Params.Get("report_num");

            String del_sql = "delete dbo.TB_NDT_report_accessory  where id='" + id + "'";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(del_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "删除无损报告附件", "报告编号:" + report_num + "附件id：" + id);
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
        ////附件添加到报告
        private void AddAccessory(HttpContext context)
        {
            String report_id = context.Request.Params.Get("report_id");
            String report_url = context.Request.Params.Get("report_url");
            Document header_doc = new Document(context.Server.MapPath(report_url));
            DocumentBuilder builderx = new DocumentBuilder(header_doc);

            String select_sql = "select * from dbo.TB_NDT_report_accessory where report_id='" + report_id + "'";
            string urls = "";
            SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, select_sql);
            while (dr.Read())
            {
                if (urls == "")
                {
                    urls = dr["accessory_url"].ToString();
                }
                else
                {
                    urls = urls + ";" + dr["accessory_url"].ToString();
                }
            }
            dr.Close();

            if (urls != "")
            {
                string[] url_array = urls.Split(';');

                for (int i = 0; i < url_array.Length; i++)
                {
                    Document doc = new Document(context.Server.MapPath(url_array[i]));
                    header_doc.AppendDocument(doc, ImportFormatMode.KeepSourceFormatting);
                    //Section sec1 = builder.CurrentSection;
                    //builderx.CurrentSection.AppendContent(sec1);
                    header_doc.FirstSection.PageSetup.SectionStart = SectionStart.Continuous;
                    header_doc.FirstSection.PageSetup.RestartPageNumbering = true;

                }
                header_doc.Save(context.Server.MapPath(report_url), Aspose.Words.SaveFormat.Doc);
                context.Response.Write(report_url);//返回给前台页面   
                context.Response.End();
            }
            else
            {
                context.Response.Write("不存在附件");//返回给前台页面   
                context.Response.End();
            }



        }

        /// 报告编辑List
        public void load_list(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Edit;

            int page = Convert.ToInt32(context.Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(context.Request.Params.Get("rows"));
            string order = context.Request.Params.Get("order");
            string sortby = context.Request.Params.Get("sort");

            string search = context.Request.Params.Get("search");
            string key = context.Request.Params.Get("key");
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
            //历史
            if (history_flag == "1")
            {
                sqlwhere += "and rt.state_ != " + state_ + " and rt.Inspection_personnel='" + loginAccount + "' ";

            }
            //待编制
            if (history_flag == "0")
            {
                sqlwhere += "and rt.state_ = " + state_ + " and rt.Inspection_personnel='" + loginAccount + "' ";

            }
            //查询判断
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sqlwhere += " and rt." + search + " like '%" + key + "%'";
            }

            string sql = "select * from (select row_number()over(order by rt." + sortby + " " + order + @")RowId, rt.*, tp.id as tp_id, tp.report_num as tp_report_num, tp.tm_id as tp_tm_id, tp.Data1, tp.Data2, tp.Data3, tp.Data4, tp.Data5, tp.Data6, tp.Data7, tp.Data8, tp.Data9, tp.Data10, tp.Data11, tp.Data12, tp.Data13, tp.Data14, tp.Data15, tp.Data16, tp.Data17, tp.Data18, tp.Data19, tp.Data20, tp.Data21, tp.Data22, tp.Data23, tp.Data24, tp.Data25, tp.Data26, tp.Data27, tp.Data28, tp.Data29, tp.Data30, tp.Data31, tp.Data32, tp.Data33, tp.Data34, tp.Data35, tp.Data36, tp.Data37, tp.Data38, tp.Data39, tp.Data40, tp.Data41, tp.Data42, tp.Data43, tp.Data44, tp.Data45, tp.Data46, tp.Data47, tp.Data48, tp.Data49, tp.Data50, tp.Data51, tp.Data52, tp.Data53, tp.Data54, tp.Data55, tp.Data56, tp.Data57, tp.Data58, tp.Data59, tp.Data60, tp.Data61, tp.Data62, tp.Data63, tp.Data64 from dbo.TB_NDT_report_title as rt left join dbo.TB_NDT_test_probereport_data as tp on rt.id = tp.report_num 
                                      " + sqlwhere + " )a where RowId  >= " + fristrow + " and RowId <=" + lastrow + ";";
            sql += @"select count(0)   
                                    FROM dbo.TB_NDT_report_title as rt left join dbo.TB_NDT_test_probereport_data as tp on rt.id = tp.report_num " + sqlwhere;
            DataSet ds = db.ExecuteDataSet(sql);
            string strJson = jsonHelper.DataSetToJson(ds);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面   
            context.Response.End();
        }
        //写入报告信息
        private void editInfo(HttpContext context)
        {

            String welding_method = context.Request.Params.Get("welding_method");
            String Job_num = context.Request.Params.Get("Job_num");//工号

            String disable_report_num = context.Request.Params.Get("disable_report_num");
            String Tubes_num = context.Request.Params.Get("Tubes_num");
            String Tubes_Size = context.Request.Params.Get("Tubes_Size");
            String figure = context.Request.Params.Get("figure");
            String Inspection_result = context.Request.Params.Get("Inspection_result");

            String clientele = context.Request.Params.Get("clientele");
            String clientele_department = context.Request.Params.Get("clientele_department");
            String application_num = context.Request.Params.Get("application_num");
            String Project_name = context.Request.Params.Get("Project_name");
            String Subassembly_name = context.Request.Params.Get("Subassembly_name");
            String Material = context.Request.Params.Get("Material");
            String Type_ = context.Request.Params.Get("Type_");
            String Chamfer_type = context.Request.Params.Get("Chamfer_type");
            String Drawing_num = context.Request.Params.Get("Drawing_num");
            String Procedure_ = context.Request.Params.Get("Procedure_");
            String Inspection_context = context.Request.Params.Get("Inspection_context");
            String Inspection_opportunity = context.Request.Params.Get("Inspection_opportunity");
            String circulation_NO = context.Request.Params.Get("circulation_NO");
            String procedure_NO = context.Request.Params.Get("procedure_NO");//
            String apparent_condition = context.Request.Params.Get("apparent_condition");//
            String manufacturing_process = context.Request.Params.Get("manufacturing_process");//
            String Batch_Num = context.Request.Params.Get("Batch_Num");//
            String Inspection_NO = context.Request.Params.Get("Inspection_NO");//
            String remarks = context.Request.Params.Get("remarks");//
            String Inspection_date = context.Request.Params.Get("Inspection_date");//
            String Work_instruction = context.Request.Params.Get("Work_instruction");//
            String heat_treatment = context.Request.Params.Get("heat_treatment");//
            String report_num = context.Request.Params.Get("report_num");
            String ReportCreationTime = DateTime.Now.ToString();
            int state_1 = (int)LosslessReportStatusEnum.Scrap;

            //判断报告编号是否重复
            string select_sql = "select * from dbo.TB_NDT_report_title where report_num='" + report_num + "' and state_!=" + state_1 + "";
            int flag = 0;
            SqlDataReader dr_ = SQLHelper.ExecuteReader(CommandType.Text, select_sql);
            while (dr_.Read())
            {
                flag = 1;
            }
            dr_.Close();

            if (flag == 0)
            {
                int state_ = (int)LosslessReportStatusEnum.Edit;

                #region copy报告 针对复制报告
                String flag1 = context.Request.Params.Get("flag");//判断是否为复制的报告 为“1”则复制报告 
                string report_url = "";
                if (flag1 == "1")
                {
                    String id = context.Request.Params.Get("id");//复制报告信息 需要已完成的报告id
                    //获取报告被复制报告url
                    string select_url = "select report_url from dbo.TB_NDT_report_title where id='" + id + "'";
                    report_url = db.ExecuteScalar(select_url).ToString();

                    #region 判断报告是否存在
                    //if(!File.Exists(context.Server.MapPath(report_url)))//如果不存在
                    //{
                    //    context.Response.Write("选择复制的报告不存在");
                    //    context.Response.End();
                    //}
                    #endregion

                    //定义报告新文件名
                    string new_doc_name = Guid.NewGuid().ToString() + ".doc";
                    //定义保存位置Lossless_report_certificate_E
                    string save_url = ConfigurationManager.AppSettings["Lossless_report_"].ToString();
                    // 模板路径
                    string tempPath = context.Server.MapPath(report_url);
                    //证书保存路径
                    string outputPath = context.Server.MapPath(save_url + new_doc_name);

                    try
                    {
                        System.IO.File.Copy(tempPath, outputPath, true);
                    }
                    catch (Exception)
                    {
                        context.Response.Write("选择复制的报告不存在");
                        context.Response.End();
                    }
                    //清空和填写部分表头信息
                    //创建word对象
                    Aspose.Words.Document contract_doc = null;
                    contract_doc = new Aspose.Words.Document(outputPath);
                    Aspose.Words.DocumentBuilder docBuild = new Aspose.Words.DocumentBuilder(contract_doc);
                    //Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)contract_doc.GetChild(NodeType.Table, 0, true);
                    NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);
                    //清空部分数据
                    #region
                    foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
                    {
                        switch (mark.Name)
                        {
                            //表头信息
                            case "report_num": mark.Text = report_num; break;
                            case "report_num1": mark.Text = report_num; break;
                            case "Inspection_personnel": mark.Text = ""; break;//清空检验人签名
                            case "Inspection_personnel_date": mark.Text = ""; break;//清空检验时间
                            case "level_Inspection": mark.Text = ""; break;//清空检验人级别
                            case "Audit_personnel": mark.Text = ""; break;//清空审核人签名
                            case "Audit_date": mark.Text = ""; break;//清空审核时间
                            case "level_Audit": mark.Text = ""; break;//清空审核人级别

                            case "issue_personnel": mark.Text = ""; break;//清空签发级别
                            case "issue_date": mark.Text = ""; break;//清空签发人签名


                            case "clientele_department": mark.Text = clientele_department; break;
                            case "Work_instruction": mark.Text = Work_instruction; break;
                            case "heat_treatment": mark.Text = heat_treatment; break;
                            case "application_num": mark.Text = application_num; break;
                            case "Project_name": mark.Text = Project_name; break;
                            case "Subassembly_name": mark.Text = Subassembly_name; break;
                            case "Material": mark.Text = Material; break;
                            case "Type_": mark.Text = Type_; break;
                            case "Chamfer_type": mark.Text = Chamfer_type; break;
                            case "Drawing_num": mark.Text = Drawing_num; break;
                            case "Procedure_": mark.Text = Procedure_; break;
                            case "Inspection_context": mark.Text = Inspection_context; break;
                            case "Inspection_opportunity": mark.Text = Inspection_opportunity; break;
                            case "circulation_NO": mark.Text = circulation_NO; break;
                            case "procedure_NO": mark.Text = procedure_NO; break;
                            case "apparent_condition": mark.Text = apparent_condition; break;
                            case "manufacturing_process": mark.Text = manufacturing_process; break;
                            case "Batch_Num": mark.Text = Batch_Num; break;
                            case "Inspection_NO": mark.Text = Inspection_NO; break;
                            case "remarks": mark.Text = remarks; break;
                            case "Inspection_date": mark.Text = Inspection_date; break;
                            case "Tubes_num": mark.Text = Tubes_num; break;
                            case "Tubes_Size": mark.Text = Tubes_Size; break;
                            case "disable_report_num": mark.Text = disable_report_num; break;
                            case "welding_method": mark.Text = welding_method; break;


                            //表头重复信息 ——水压测试报告
                            case "report_num2": mark.Text = report_num; break;
                            case "report_num3": mark.Text = report_num; break;
                            case "report_num4": mark.Text = report_num; break;


                        }
                        //else if (mark.Name == "customer_name")
                        //    mark.Text = dr["customer_name"].ToString();
                    }
                    #endregion
                    contract_doc.Save(outputPath, Aspose.Words.SaveFormat.Doc);
                    report_url = save_url + new_doc_name;
                }

                #endregion

                String tm_id = context.Request.Params.Get("tm_id");
                String report_format = context.Request.Params.Get("report_format");
                String report_name = context.Request.Params.Get("report_name");
                //String report_url = context.Request.Params.Get("report_url");

                //string Inspection_date = DateTime.Now.ToLocalTime().ToString();                    //时间
                string Inspection_personnel = Convert.ToString(context.Session["loginAccount"]);   //用户

                SqlParameter[] para = 
                {
                    new SqlParameter("@report_url",report_url),
                    new SqlParameter("@Job_num",Job_num),
                    new SqlParameter("@state_",state_),
                    new SqlParameter("@tm_id",tm_id),
                    new SqlParameter("@report_format",report_format),
                    new SqlParameter("@report_name",report_name),
                    new SqlParameter("@welding_method",welding_method),                   
                    new SqlParameter("@disable_report_num",disable_report_num),
                    new SqlParameter("@Tubes_num",Tubes_num),
                    new SqlParameter("@Tubes_Size",Tubes_Size),
                    new SqlParameter("@figure",figure),
                    new SqlParameter("@Inspection_result",Inspection_result),                       
                    new SqlParameter("@report_num",report_num),
                    new SqlParameter("@clientele",clientele),
                    new SqlParameter("@clientele_department",clientele_department),
                    new SqlParameter("@application_num",application_num),                  
                    new SqlParameter("@Project_name",Project_name),
                    new SqlParameter("@Subassembly_name",Subassembly_name),
                    new SqlParameter("@Material",Material),
                    new SqlParameter("@Type_",Type_),
                    new SqlParameter("@Chamfer_type",Chamfer_type),
                    new SqlParameter("@Drawing_num",Drawing_num),
                    new SqlParameter("@Procedure_",Procedure_),
                    new SqlParameter("@Inspection_context",Inspection_context),
                    new SqlParameter("@Inspection_opportunity",Inspection_opportunity),
                    new SqlParameter("@circulation_NO",circulation_NO),
                    new SqlParameter("@procedure_NO",procedure_NO),
                    new SqlParameter("@apparent_condition",apparent_condition),
                    new SqlParameter("@manufacturing_process",manufacturing_process),
                    new SqlParameter("@Batch_Num",Batch_Num),
                    new SqlParameter("@Inspection_NO",Inspection_NO),
                    new SqlParameter("@remarks",remarks),
                    new SqlParameter("@Inspection_date",Inspection_date),
                    new SqlParameter("@Work_instruction",Work_instruction),
                    new SqlParameter("@heat_treatment",heat_treatment),
                    new SqlParameter("@Inspection_personnel",Inspection_personnel),
                    new SqlParameter("ReportCreationTime",ReportCreationTime)
                };

                String insert_sql = "insert into dbo.TB_NDT_report_title (report_url,Work_instruction,heat_treatment,Job_num,welding_method, disable_report_num, Tubes_num, Tubes_Size, figure, Inspection_result, tm_id, clientele_department, clientele, application_num, Project_name, Subassembly_name, Material, Type_, Chamfer_type, Drawing_num, Procedure_, Inspection_context, Inspection_opportunity, circulation_NO, procedure_NO, apparent_condition, manufacturing_process, Batch_Num, Inspection_NO, report_num, remarks, state_, Inspection_personnel, Inspection_date, report_name, report_format,ReportCreationTime) values ( @report_url,@Work_instruction,@heat_treatment,@Job_num,@welding_method,@disable_report_num,@Tubes_num,@Tubes_Size,@figure,@Inspection_result,@tm_id,@clientele_department,@clientele,@application_num,@Project_name,@Subassembly_name,@Material,@Type_,@Chamfer_type,@Drawing_num,@Procedure_,@Inspection_context,@Inspection_opportunity,@circulation_NO,@procedure_NO,@apparent_condition, @manufacturing_process,@Batch_Num,@Inspection_NO,@report_num,@remarks,@state_,@Inspection_personnel,@Inspection_date,@report_name,@report_format,@ReportCreationTime)";

                try
                {
                    //SQL语句
                    List<string> SQLStringList = new List<string>();
                    List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                    SQLStringList2.Add(para);
                    SQLStringList.Add(insert_sql);
                    //事务
                    SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                    string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                    string login_username = Convert.ToString(context.Session["login_username"]);
                    Operation_log.operation_log_(loginAccount, login_username, "添加无损报告信息", "报告编号:" + report_num);
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
            {
                context.Response.Write("报告编号重复");
                context.Response.End();
            }

        }
        //修改报告信息
        private void editInfo2(HttpContext context)
        {
            String id = context.Request.Params.Get("id");
            String report_num = context.Request.Params.Get("report_num");
            String Job_num = context.Request.Params.Get("Job_num");//工号
            String tm_id = context.Request.Params.Get("tm_id");
            String report_format = context.Request.Params.Get("report_format");
            String report_name = context.Request.Params.Get("report_name");
            //String report_url = context.Request.Params.Get("report_url");
            String disable_report_num = context.Request.Params.Get("disable_report_num");
            String Work_instruction = context.Request.Params.Get("Work_instruction");//
            String heat_treatment = context.Request.Params.Get("heat_treatment");//
            String welding_method = context.Request.Params.Get("welding_method");
            String Tubes_num = context.Request.Params.Get("Tubes_num");
            String Tubes_Size = context.Request.Params.Get("Tubes_Size");
            String figure = context.Request.Params.Get("figure");
            String Inspection_result = context.Request.Params.Get("Inspection_result");
            String clientele = context.Request.Params.Get("clientele");
            String clientele_department = context.Request.Params.Get("clientele_department");
            String application_num = context.Request.Params.Get("application_num");
            String Project_name = context.Request.Params.Get("Project_name");
            String Subassembly_name = context.Request.Params.Get("Subassembly_name");
            String Material = context.Request.Params.Get("Material");
            String Type_ = context.Request.Params.Get("Type_");
            String Chamfer_type = context.Request.Params.Get("Chamfer_type");
            String Drawing_num = context.Request.Params.Get("Drawing_num");
            String Procedure_ = context.Request.Params.Get("Procedure_");
            String Inspection_context = context.Request.Params.Get("Inspection_context");
            String Inspection_opportunity = context.Request.Params.Get("Inspection_opportunity");
            String circulation_NO = context.Request.Params.Get("circulation_NO");
            String procedure_NO = context.Request.Params.Get("procedure_NO");//
            String apparent_condition = context.Request.Params.Get("apparent_condition");//
            String manufacturing_process = context.Request.Params.Get("manufacturing_process");//
            String Batch_Num = context.Request.Params.Get("Batch_Num");//
            String Inspection_NO = context.Request.Params.Get("Inspection_NO");//
            String remarks = context.Request.Params.Get("remarks");//
            String Inspection_date = context.Request.Params.Get("Inspection_date");//
            //string Inspection_date = DateTime.Now.ToLocalTime().ToString();                    //时间
            //string Inspection_personnel = Convert.ToString(context.Session["loginAccount"]);   //用户

            SqlParameter[] para = 
                {
                    new SqlParameter("@id",id),
                    new SqlParameter("@tm_id",tm_id),
                    new SqlParameter("@Job_num",Job_num),
                    new SqlParameter("@report_format",report_format),
                    new SqlParameter("@report_name",report_name),
                    new SqlParameter("@welding_method",welding_method),                   
                    new SqlParameter("@disable_report_num",disable_report_num),
                    new SqlParameter("@Tubes_num",Tubes_num),
                    new SqlParameter("@Tubes_Size",Tubes_Size),
                    new SqlParameter("@figure",figure),
                    new SqlParameter("@Inspection_result",Inspection_result),                       
                    //new SqlParameter("@report_num",report_num),
                    new SqlParameter("@clientele",clientele),
                    new SqlParameter("@clientele_department",clientele_department),
                    new SqlParameter("@application_num",application_num),                  
                    new SqlParameter("@Project_name",Project_name),
                    new SqlParameter("@Subassembly_name",Subassembly_name),
                    new SqlParameter("@Material",Material),
                    new SqlParameter("@Type_",Type_),
                    new SqlParameter("@Chamfer_type",Chamfer_type),
                    new SqlParameter("@Drawing_num",Drawing_num),
                    new SqlParameter("@Procedure_",Procedure_),
                    new SqlParameter("@Inspection_context",Inspection_context),
                    new SqlParameter("@Inspection_opportunity",Inspection_opportunity),
                    new SqlParameter("@circulation_NO",circulation_NO),
                    new SqlParameter("@procedure_NO",procedure_NO),
                    new SqlParameter("@apparent_condition",apparent_condition),
                    new SqlParameter("@manufacturing_process",manufacturing_process),
                    new SqlParameter("@Batch_Num",Batch_Num),
                    new SqlParameter("@Inspection_NO",Inspection_NO),
                    new SqlParameter("@remarks",remarks),
                    new SqlParameter("@heat_treatment",heat_treatment),
                    new SqlParameter("@Work_instruction",Work_instruction),
                    new SqlParameter("@Inspection_date",Inspection_date)
                };

            String insert_sql = "update dbo.TB_NDT_report_title set Work_instruction=@Work_instruction,heat_treatment=@heat_treatment,Job_num=@Job_num, welding_method=@welding_method, disable_report_num=@disable_report_num, Tubes_num=@Tubes_num, Tubes_Size=@Tubes_Size, figure=@figure, Inspection_result=@Inspection_result, tm_id=@tm_id, clientele_department=@clientele_department, clientele=@clientele, application_num="
                + "@application_num, Project_name=@Project_name, Subassembly_name=@Subassembly_name, Material=@Material, Type_=@Type_, Chamfer_type=@Chamfer_type, Drawing_num=@Drawing_num, Procedure_=@Procedure_, Inspection_context=@Inspection_context, Inspection_opportunity="
                + "@Inspection_opportunity, circulation_NO=@circulation_NO, procedure_NO=@procedure_NO, apparent_condition=@apparent_condition, manufacturing_process=@manufacturing_process, Batch_Num=@Batch_Num, Inspection_NO=@Inspection_NO, remarks=@remarks, "
                + " Inspection_date=@Inspection_date, report_name=@report_name, report_format=@report_format where id=@id";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                SQLStringList2.Add(para);
                SQLStringList.Add(insert_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "修改无损报告信息", "报告编号:" + report_num);
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
        //添加检测数据
        private void AddTextData(HttpContext context)
        {
            string report_num = context.Request.Params.Get("report_num");
            String report_id = context.Request.Params.Get("report_id");
            String report_name = context.Request.Params.Get("report_name");
            String tm_id = context.Request.Params.Get("tm_id");
            string date = DateTime.Now.ToLocalTime().ToString();                    //时间
            string personnel = Convert.ToString(context.Session["loginAccount"]);   //用户
            int state_1 = (int)LosslessReportStatusEnum.Scrap;
            //测试数据data
            #region
            string Data1 = context.Request.Params.Get("Data1");
            string Data2 = context.Request.Params.Get("Data2");
            string Data3 = context.Request.Params.Get("Data3");
            string Data4 = context.Request.Params.Get("Data4");
            string Data5 = context.Request.Params.Get("Data5");
            string Data6 = context.Request.Params.Get("Data6");
            string Data7 = context.Request.Params.Get("Data7");
            string Data8 = context.Request.Params.Get("Data8");
            string Data9 = context.Request.Params.Get("Data9");
            string Data10 = context.Request.Params.Get("Data10");
            string Data11 = context.Request.Params.Get("Data11");
            string Data12 = context.Request.Params.Get("Data12"); ;
            string Data13 = context.Request.Params.Get("Data13"); ;
            string Data14 = context.Request.Params.Get("Data14");
            string Data15 = context.Request.Params.Get("Data15");
            string Data16 = context.Request.Params.Get("Data16"); ;
            string Data17 = context.Request.Params.Get("Data17"); ;
            string Data18 = context.Request.Params.Get("Data18"); ;
            string Data19 = context.Request.Params.Get("Data19");
            string Data20 = context.Request.Params.Get("Data20");
            string Data21 = context.Request.Params.Get("Data21"); ;
            string Data22 = context.Request.Params.Get("Data22");
            string Data23 = context.Request.Params.Get("Data23");
            string Data24 = context.Request.Params.Get("Data24");
            string Data25 = context.Request.Params.Get("Data25");
            string Data26 = context.Request.Params.Get("Data26");
            string Data27 = context.Request.Params.Get("Data27");
            string Data28 = context.Request.Params.Get("Data28");
            string Data29 = context.Request.Params.Get("Data29");
            string Data30 = context.Request.Params.Get("Data30");
            string Data31 = context.Request.Params.Get("Data31");
            string Data32 = context.Request.Params.Get("Data32");
            string Data33 = context.Request.Params.Get("Data33");
            string Data34 = context.Request.Params.Get("Data34");
            string Data35 = context.Request.Params.Get("Data35");
            string Data36 = context.Request.Params.Get("Data36");
            string Data37 = context.Request.Params.Get("Data37");
            string Data38 = context.Request.Params.Get("Data38");
            string Data39 = context.Request.Params.Get("Data39");
            string Data40 = context.Request.Params.Get("Data40");
            string Data41 = context.Request.Params.Get("Data41");
            string Data42 = context.Request.Params.Get("Data42");
            string Data43 = context.Request.Params.Get("Data43");
            string Data44 = context.Request.Params.Get("Data44");
            string Data45 = context.Request.Params.Get("Data45");
            string Data46 = context.Request.Params.Get("Data46");
            string Data47 = context.Request.Params.Get("Data47");
            string Data48 = context.Request.Params.Get("Data48");
            string Data49 = context.Request.Params.Get("Data49");
            string Data50 = context.Request.Params.Get("Data50");
            string Data51 = context.Request.Params.Get("Data51");
            string Data52 = context.Request.Params.Get("Data52");
            string Data53 = context.Request.Params.Get("Data53");
            string Data54 = context.Request.Params.Get("Data54");
            string Data55 = context.Request.Params.Get("Data55");
            string Data56 = context.Request.Params.Get("Data56");
            string Data57 = context.Request.Params.Get("Data57");
            string Data58 = context.Request.Params.Get("Data58");
            string Data59 = context.Request.Params.Get("Data59");
            string Data60 = context.Request.Params.Get("Data60");
            string Data61 = context.Request.Params.Get("Data61");
            string Data62 = context.Request.Params.Get("Data62");
            string Data63 = context.Request.Params.Get("Data63");
            string Data64 = context.Request.Params.Get("Data64");
            #endregion

            string sql = "";

            //测试数据表格信息填入
            #region
            SqlParameter[] para = 
                {
                    new SqlParameter("@report_num",report_id),
                    new SqlParameter("@tm_id",tm_id),
                    new SqlParameter("@Data1",Data1),
                    new SqlParameter("@Data2",Data2),
                    new SqlParameter("@Data3",Data3),
                    new SqlParameter("@Data4",Data4),
                    new SqlParameter("@Data5",Data5),
                    new SqlParameter("@Data6",Data6),
                    new SqlParameter("@Data7",Data7),
                    new SqlParameter("@Data8",Data8),
                    new SqlParameter("@Data9",Data9),
                    new SqlParameter("@Data10",Data10),
                    new SqlParameter("@Data11",Data11),
                    new SqlParameter("@Data12",Data12),
                    new SqlParameter("@Data13",Data13),
                    new SqlParameter("@Data14",Data14),
                    new SqlParameter("@Data15",Data15),
                    new SqlParameter("@Data16",Data16),
                    new SqlParameter("@Data17",Data17),
                    new SqlParameter("@Data18",Data18),
                    new SqlParameter("@Data19",Data19),
                    new SqlParameter("@Data20",Data20),
                    new SqlParameter("@Data21",Data21),
                    new SqlParameter("@Data22",Data22),
                    new SqlParameter("@Data23",Data23),
                    new SqlParameter("@Data24",Data24),
                    new SqlParameter("@Data25",Data25),
                    new SqlParameter("@Data26",Data26),
                    new SqlParameter("@Data27",Data27),
                    new SqlParameter("@Data28",Data28),
                    new SqlParameter("@Data29",Data29),
                    new SqlParameter("@Data30",Data30),
                    new SqlParameter("@Data31",Data31),
                    new SqlParameter("@Data32",Data32),
                    new SqlParameter("@Data33",Data33),
                    new SqlParameter("@Data34",Data34),
                    new SqlParameter("@Data35",Data35),
                    new SqlParameter("@Data36",Data36),
                    new SqlParameter("@Data37",Data37),
                    new SqlParameter("@Data38",Data38),
                    new SqlParameter("@Data39",Data39),
                    new SqlParameter("@Data40",Data40),
                    new SqlParameter("@Data41",Data41),
                    new SqlParameter("@Data42",Data42),
                    new SqlParameter("@Data43",Data43),
                    new SqlParameter("@Data44",Data44),
                    new SqlParameter("@Data45",Data45),
                    new SqlParameter("@Data46",Data46),
                    new SqlParameter("@Data47",Data47),
                    new SqlParameter("@Data48",Data48),
                    new SqlParameter("@Data49",Data49),
                    new SqlParameter("@Data50",Data50),
                    new SqlParameter("@Data51",Data51),
                    new SqlParameter("@Data52",Data52),
                    new SqlParameter("@Data53",Data53),
                    new SqlParameter("@Data54",Data54),
                    new SqlParameter("@Data55",Data55),
                    new SqlParameter("@Data56",Data56),
                    new SqlParameter("@Data57",Data57),
                    new SqlParameter("@Data58",Data58),
                    new SqlParameter("@Data59",Data59),
                    new SqlParameter("@Data60",Data60),
                    new SqlParameter("@Data61",Data61),
                    new SqlParameter("@Data62",Data62),
                    new SqlParameter("@Data63",Data63),
                    new SqlParameter("@Data64",Data64)
                };

            //String sql = "";
            int state_ = (int)LosslessReportStatusEnum.Scrap;
            string select_test_data = "select * from dbo.TB_NDT_test_probereport_data where report_num='" + report_id + "'";
            int flag = 0;
            SqlDataReader dr_info = SQLHelper.ExecuteReader(CommandType.Text, select_test_data);
            while (dr_info.Read())
            {
                flag = 1;
            }
            dr_info.Close();
            if (flag == 0)
            {
                sql = "insert into dbo.TB_NDT_test_probereport_data (report_num, tm_id, Data1, Data2, Data3, Data4, Data5, Data6, Data7, Data8, Data9, Data10,"
                    + " Data11, Data12, Data13, Data14, Data15, Data16, Data17, Data18, Data19, Data20, Data21, Data22, Data23, Data24, Data25, Data26, Data27, Data28, Data29, Data30,"
                    + " Data31, Data32, Data33, Data34, Data35, Data36, Data37, Data38, Data39, Data40, Data41, Data42, Data43, Data44, Data45, Data46, Data47, Data48, Data49, Data50,"
                    + " Data51, Data52, Data53, Data54, Data55, Data56, Data57, Data58, Data59, Data60, Data61, Data62, Data63, Data64) "
                    + "values (@report_num, @tm_id, @Data1, @Data2, @Data3, @Data4, @Data5, @Data6, @Data7, @Data8, @Data9, @Data10, "
                    + "@Data11, @Data12, @Data13, @Data14, @Data15, @Data16, @Data17, @Data18, @Data19, @Data20, "
                    + "@Data21, @Data22, @Data23, @Data24, @Data25, @Data26, @Data27, @Data28, @Data29, @Data30, "
                    + "@Data31, @Data32, @Data33, @Data34, @Data35, @Data36, @Data37, @Data38, @Data39, @Data40, "
                    + "@Data41, @Data42, @Data43, @Data44, @Data45, @Data46, @Data47, @Data48, @Data49, @Data50, "
                    + "@Data51, @Data52, @Data53, @Data54, @Data55, @Data56, @Data57, @Data58, @Data59, @Data60, "
                    + "@Data61, @Data62, @Data63, @Data64)";
            }
            else
            {
                sql = "update dbo.TB_NDT_test_probereport_data set tm_id=@tm_id, Data1=@Data1, Data2=@Data2, Data3=@Data3, Data4=@Data4, Data5=@Data5, Data6=@Data6, Data7=@Data7, "
                    + "Data8=@Data8, Data9=@Data9, Data10=@Data10, Data11=@Data11, Data12=@Data12, Data13=@Data13, Data14=@Data14, Data15=@Data15, Data16=@Data16, Data17=@Data17, "
                    + "Data18=@Data18, Data19=@Data19, Data20=@Data20, Data21=@Data21, Data22=@Data22, Data23=@Data23, Data24=@Data24, Data25=@Data25, Data26=@Data26, Data27=@Data27, "
                    + "Data28=@Data28, Data29=@Data29, Data30=@Data30, Data31=@Data31, Data32=@Data32, Data33=@Data33, Data34=@Data34, Data35=@Data35, Data36=@Data36, Data37=@Data37, "
                    + "Data38=@Data38, Data39=@Data39, Data40=@Data40, Data41=@Data41, Data42=@Data42, Data43=@Data43, Data44=@Data44, Data45=@Data45, Data46=@Data46, Data47=@Data47, "
                    + "Data48=@Data48, Data49=@Data49, Data50=@Data50, Data51=@Data51, Data52=@Data52, Data53=@Data53, Data54=@Data54, Data55=@Data55, Data56=@Data56, Data57=@Data57, "
                    + "Data58=@Data58, Data59=@Data59, Data60=@Data60, Data61=@Data61, Data62=@Data62, Data63=@Data63, Data64=@Data64 where report_num=@report_num ";
            }

            #endregion

            //设备表格信息填入
            #region
            string update_sql = "";
            if (tm_id != "27")
            {
                //设备
                //String report_id = context.Request.Params.Get("report_id");
                String equipment_id = context.Request.Params.Get("equipment_id");
                String equipment_name = context.Request.Params.Get("equipment_name");
                String equipment_name_R = context.Request.Params.Get("equipment_name_R");//label名称s

                string[] equipment_ids = equipment_id.Split(',');
                string[] equipment_names = equipment_name.Split(',');
                string[] equipment_name_Rs = equipment_name_R.Split(',');
                //string sql_test_equipment = "";
                for (int j = 0; j < equipment_ids.Length; j++)
                {//查找该报告是否已经存在该设备
                    string select_test_equipment = "select * from dbo.TB_NDT_test_equipment where report_id='" + report_id + "' and equipment_name_R='" + equipment_name_Rs[j] + "'";
                    int flag_eq = 0;
                    SqlDataReader dr_equipment = SQLHelper.ExecuteReader(CommandType.Text, select_test_equipment);
                    while (dr_equipment.Read())
                    {
                        flag_eq = 1;
                    }
                    dr_equipment.Close();

                    //搜索计量库的设备信息
                    string select_equipment = "select * from dbo.TB_standing_book where id='" + equipment_ids[j] + "' ";
                    string id = "";
                    string sample_type = "";
                    string asset_num = "";
                    string measuring_range = "";
                    string manufacturers = "";
                    string verification_effective_date = "";
                    string remarks1 = "";
                    SqlDataReader dr_s = SQLHelper.ExecuteReader1(CommandType.Text, select_equipment);
                    while (dr_s.Read())
                    {
                        id = dr_s["id"].ToString();
                        sample_type = dr_s["sample_type"].ToString();
                        asset_num = dr_s["asset_num"].ToString();
                        measuring_range = dr_s["measuring_range"].ToString();
                        verification_effective_date = dr_s["verification_effective_date"].ToString();
                        manufacturers = dr_s["manufacturers"].ToString();
                        remarks1 = dr_s["remarks1"].ToString();
                    }
                    dr_s.Close();

                    if (flag_eq == 0)
                    {

                        update_sql += ";insert into dbo.TB_NDT_test_equipment (equipment_id, report_id, equipment_name_R, equipment_name, equipment_Type, equipment_num, range_, Manufacture, effective, Remarks) values ('" + id + "', '" + report_id + "', '" + equipment_name_Rs[j] + "', '" + equipment_names[j] + "','" + sample_type + "','" + asset_num + "','" + measuring_range + "','" + manufacturers + "','" + verification_effective_date + "','" + remarks1 + "')";

                    }
                    else
                    {
                        update_sql += ";update dbo.TB_NDT_test_equipment set equipment_id='" + equipment_ids[j] + "', report_id='" + report_id + "', equipment_name_R='"
                            + equipment_name_Rs[j] + "', equipment_name='" + equipment_names[j] + "', equipment_Type='" + sample_type + "', equipment_num='" + asset_num + "', range_='" + measuring_range + "', Manufacture='" + manufacturers + "', effective='" + verification_effective_date + "', Remarks='" + remarks1 + "' where equipment_name_R='" + equipment_name_Rs[j] + "' and report_id='" + report_id + "'";
                    }



                }
            }

            #endregion


            try
            {

                db.BeginTransaction();
                db.ExecuteNonQueryByTrans(sql, para);
                db.ExecuteNonQueryByTrans(update_sql);
                db.CommitTransacton();
                ////SQL语句
                //List<string> SQLStringList = new List<string>();
                //List<SqlParameter[]> SQLStringList2 = new List<SqlParameter[]>();
                //SQLStringList2.Add(para);
                //SQLStringList.Add(sql);
                ////事务
                //SQLHelper.ExecuteSqlTran(SQLStringList, SQLStringList2);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "添加无损检测数据", "报告编号:" + report_num);
                context.Response.Write("T");
            }
            catch (Exception)
            {
                db.RollbackTransaction();
                context.Response.Write("F");
            }
            finally
            {
                context.Response.End();
            }


        }
        //删除信息
        private void DataDel(HttpContext context)
        {
            String id = context.Request.Params.Get("id");
            String report_num = context.Request.Params.Get("report_num");

            String del_sql = "delete dbo.TB_NDT_report_title where id='" + id + "'";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(del_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "删除无损报告信息", "报告编号:" + report_num);
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
        /// 载入报告
        public void Filling_report(HttpContext context)
        {
            String id = context.Request.Params.Get("id");//报告ID
            String report_num = context.Request.Params.Get("report_num");
            String field_fomat = context.Request.Params.Get("report_format");//模板格式
            String tm_id = context.Request.Params.Get("tm_id");
            String Template = tm_id + field_fomat;//模板名称 + 格式 ？ 1.docx


            string templet_url = "/upload_Folder/Lossless_report/" + Template;
            //定义报告新文件名
            string new_doc_name = Guid.NewGuid().ToString() + ".doc";
            //定义保存位置Lossless_report_certificate_E
            string save_url = ConfigurationManager.AppSettings["Lossless_report_"].ToString();
            // 模板路径
            string tempPath = context.Server.MapPath(templet_url);
            //证书保存路径
            string outputPath = context.Server.MapPath(save_url + new_doc_name);
            //返回的证书路径
            string return_url = save_url + new_doc_name;


            try
            {
                System.IO.File.Copy(tempPath, outputPath, true);
            }
            catch (Exception)
            {

                throw;
            }
            //复制模板

            //保存证书路径到数据库
            SQLHelper sql2 = new SQLHelper();
            SqlConnection con = sql2.getConn();
            string certificate_file = "UPDATE dbo.TB_NDT_report_title SET report_url='" + return_url + "' where id= '" + id + "'";
            SqlCommand cmd1 = new SqlCommand(certificate_file, con);
            int count = cmd1.ExecuteNonQuery();
            con.Close();

            //创建word对象
            Aspose.Words.Document contract_doc = null;
            contract_doc = new Aspose.Words.Document(outputPath);
            Aspose.Words.DocumentBuilder docBuild = new Aspose.Words.DocumentBuilder(contract_doc);
            //Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)contract_doc.GetChild(NodeType.Table, 0, true);
            NodeCollection tables = contract_doc.GetChildNodes(NodeType.Table, true);

            //插入探头表格
            #region

            //标志——表示是否需要插入探头表格
            int temp_flag = 0;
            try
            {
                if (tm_id == "13")
                {
                    contract_doc.Range.Bookmarks["Probe_num"].Text = "key1010";
                }
                else
                {
                    contract_doc.Range.Bookmarks["id"].Text = "key1010";
                }
            }
            catch
            {
                temp_flag = 1;
            }
            if (temp_flag == 0)
            {
                #region
                #region 数据拼接
                DataTable testData = new DataTable();//总数居table

                //获取测试数据/试验基础信息TB_record_title_info
                string testDataSql = "select pl.* from dbo.TB_NDT_test_probe as tp left join dbo.TB_NDT_probe_library as pl on tp.probe_id=pl.id where report_id='" + id + "'";
                DataTable testData_item = db.ExecuteDataTable(testDataSql);



                testData.Merge(testData_item);
                #endregion

                //table入口地址
                //行入口地址
                int dataRowIndex = 0;
                int flag_ = 0;
                double height = 0.56;
                int dataTableIndex = 0;
                int dataTable_count = 0;
                #region 获取书签所在行（table的入口）
                foreach (Aspose.Words.Tables.Table table in tables)
                {
                    if (dataRowIndex != 0)
                        break;
                    // Get the index of the table node as contained in the parent node of the table
                    int tableIndex = table.ParentNode.ChildNodes.IndexOf(table);

                    // Iterate through all rows in the table
                    foreach (Row row in table.Rows)
                    {
                        if (dataRowIndex != 0)
                            break;
                        //第几行
                        int rowIndex = table.Rows.IndexOf(row);

                        // Iterate through all cells in the row
                        foreach (Cell cell in row.Cells)
                        {
                            int cellIndex = row.Cells.IndexOf(cell);

                            // Get the plain text content of this cell.
                            string cellText = "";
                            try
                            {
                                cellText = cell.ToString(SaveFormat.Text).Trim();
                            }
                            catch
                            {
                            }



                            if (cellText == "key1010")
                            {
                                if (cellIndex != 0)
                                {
                                    flag_ = cellIndex;

                                }
                                dataRowIndex = rowIndex;

                                dataTableIndex = dataTable_count;
                                break;
                            }

                        }
                    }
                    dataTable_count++;

                }


                #endregion

                #region//获取表格1每列书签名称
                int count1 = 0;
                List<double> widthList = new List<double>();//表格1宽度集合

                List<string> CellVerticalAlignment_1 = new List<string>();//垂直样式
                List<string> ParagraphAlignment_1 = new List<string>();//水平样式
                //单元高度

                //获取表格列数
                //for (var i = 0; i < testData.Columns.Count; i++)
                //{
                //    if (contract_doc.Range.Bookmarks[testData.Columns[i].ColumnName.Trim()] != null)
                //    {
                //        Bookmark mark = contract_doc.Range.Bookmarks[testData.Columns[i].ColumnName.Trim()];
                //        mark.Text = "";
                //        count++;
                //    }
                //}
                List<string> listcolumn = new List<string>();//表格1表格内容集合

                for (var i = flag_; i < 30; i++)
                {
                    try
                    {
                        docBuild.MoveToCell(dataTableIndex, dataRowIndex, i, 0); //移动单元格 
                        if (docBuild.CurrentNode != null)
                        {
                            if (docBuild.CurrentNode.NodeType == NodeType.BookmarkStart)
                            {
                                listcolumn.Add((docBuild.CurrentNode as BookmarkStart).Name);
                            }
                        }
                        else
                            listcolumn.Add("");

                        widthList.Add(docBuild.CellFormat.Width);

                        CellVerticalAlignment_1.Add((docBuild.CellFormat.VerticalAlignment).ToString());
                        ParagraphAlignment_1.Add((docBuild.ParagraphFormat.Alignment).ToString());

                        count1++;
                    }
                    catch
                    {
                        break;
                    }
                }
                if (tm_id == "13")
                {
                    docBuild.MoveToBookmark("Probe_num");//跳到指定书签
                }
                else
                {
                    docBuild.MoveToBookmark("id");//跳到指定书签
                }
                height = docBuild.RowFormat.Height;
                docBuild.MoveToBookmark("table"); //开始添加值 
                int row_count = 1;
                int CC = 0;
                int testData_total_count = 0;
                if (tm_id == "13")
                {
                    if (testData.Rows.Count < 5)
                    {
                        testData_total_count = 4;
                    }
                }
                else
                {
                    if (testData.Rows.Count < 4)
                    {
                        testData_total_count = 3;
                    }
                }

                #endregion

                #region//写入记录 从第2行开始

                int id_no = 2;
                for (var m = 1; m < testData_total_count; m++)
                {
                    CC = 1;
                    row_count = row_count + 1;
                    for (var i = 0; i < listcolumn.Count; i++)
                    {
                        docBuild.InsertCell(); // 添加一个单元格 
                        //docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;
                        //docBuild.CellFormat.Borders.Color = System.Drawing.Color.Black;
                        //设置单元格宽度
                        docBuild.CellFormat.Width = widthList[i];
                        //设置单元格高度
                        docBuild.RowFormat.Height = height;

                        docBuild.CellFormat.Borders.LineStyle = LineStyle.Single;

                        if (i == 0)
                        {
                            docBuild.CellFormat.Borders.Left.LineStyle = LineStyle.None;
                        }
                        if (i == listcolumn.Count - 1)
                        { docBuild.CellFormat.Borders.Right.LineStyle = LineStyle.None; }

                        docBuild.CellFormat.Borders.Top.LineStyle = LineStyle.None;

                        if (row_count == testData_total_count)
                        {
                            docBuild.CellFormat.Borders.Bottom.LineStyle = LineStyle.None;

                        }

                        //docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                        //docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                        //垂直样式
                        switch (CellVerticalAlignment_1[i])
                        {
                            case "Center": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; break;
                            case "Top": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Top; break;
                            case "Bottom": docBuild.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom; break;

                        }
                        //水平样式
                        switch (ParagraphAlignment_1[i])
                        {
                            case "Center": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Center; break;
                            case "ArabicHighKashida": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicHighKashida; break;
                            case "ArabicLowKashida": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicLowKashida; break;
                            case "ArabicMediumKashida":
                                docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ArabicMediumKashida;
                                break;
                            case "Distributed": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Distributed; break;
                            case "Justify": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Justify; break;
                            case "Left": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Left; break;
                            case "Right": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.Right; break;
                            case "ThaiDistributed": docBuild.ParagraphFormat.Alignment = ParagraphAlignment.ThaiDistributed; break;

                        }

                        docBuild.Font.Size = 9;
                        //是否加粗  
                        docBuild.Bold = false;
                        try
                        {
                            if (testData.Columns.Contains(listcolumn[i].ToString()))
                            {
                                if (tm_id == "5" || tm_id == "10" || tm_id == "11")
                                {
                                    testData.Rows[m][listcolumn[i]].ToString();
                                    if (listcolumn[i].ToString().Trim() == "id")
                                    {
                                        docBuild.Write(id_no.ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                                        id_no++;
                                    }
                                    else
                                    {
                                        docBuild.Write(testData.Rows[m][listcolumn[i]].ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
                                    }

                                }
                                else
                                {
                                    if (listcolumn[i].ToString().Trim() != "id")
                                    {
                                        docBuild.Write(testData.Rows[m][listcolumn[i]].ToString());//docBuild.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;

                                    }
                                }

                            }
                        }
                        catch { }


                    }
                    docBuild.EndRow();
                }
                if (CC == 1)
                {
                    docBuild.EndTable();
                }

                contract_doc.Range.Bookmarks["table"].Text = ""; // 清掉标示 
                try
                {
                    if (tm_id == "13")
                    {
                        contract_doc.Range.Bookmarks["Probe_num"].Text = "key1010";
                    }
                    else
                    {
                        contract_doc.Range.Bookmarks["id"].Text = "key1010";
                    }
                }
                catch
                {

                }

                #endregion

                #region 表格第一行记录
                //填写第一行记录5 10 11 需要序号
                if (tm_id == "5" || tm_id == "10" || tm_id == "11")
                {
                    if (testData.Rows.Count > 0)
                    {
                        foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
                        {
                            switch (mark.Name)
                            {
                                case "id": mark.Text = "1"; break;
                                case "Probe_Manufacture": mark.Text = testData.Rows[0]["Probe_Manufacture"].ToString(); break;
                                case "Probe_type": mark.Text = testData.Rows[0]["Probe_type"].ToString(); break;
                                case "Probe_num": mark.Text = testData.Rows[0]["Probe_num"].ToString(); break;
                                case "Coil_Size": mark.Text = testData.Rows[0]["Coil_Size"].ToString(); break;
                                case "Probe_Length": mark.Text = testData.Rows[0]["Probe_Length"].ToString(); break;
                                case "Cable_Length": mark.Text = testData.Rows[0]["Cable_Length"].ToString(); break;
                                case "Chip_size": mark.Text = testData.Rows[0]["Chip_size"].ToString(); break;
                                case "Probe_frequency": mark.Text = testData.Rows[0]["Probe_frequency"].ToString(); break;
                                case "Mode_L": mark.Text = testData.Rows[0]["Mode_L"].ToString(); break;
                                case "Mode_T": mark.Text = testData.Rows[0]["Mode_T"].ToString(); break;
                                case "Angle": mark.Text = testData.Rows[0]["Angle"].ToString(); break;
                                case "Shoe": mark.Text = testData.Rows[0]["Shoe"].ToString(); break;
                                case "Nom_Angle": mark.Text = testData.Rows[0]["Nom_Angle"].ToString(); break;
                            }
                        }
                    }
                }
                else
                {
                    if (testData.Rows.Count > 0)
                    {
                        foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
                        {
                            switch (mark.Name)
                            {
                                case "Probe_num": mark.Text = testData.Rows[0]["Probe_num"].ToString(); break;
                                case "Probe_type": mark.Text = testData.Rows[0]["Probe_type"].ToString(); break;
                                case "Probe_size": mark.Text = testData.Rows[0]["Probe_size"].ToString(); break;
                                case "Probe_frequency": mark.Text = testData.Rows[0]["Probe_frequency"].ToString(); break;
                            }
                        }
                    }
                }

                #endregion

                #endregion
            }
            #endregion


            //表头信息
            string select_report_title = "select * from dbo.TB_NDT_report_title where id='" + id + "'";
            #region
            String report_name = "";
            String clientele_department = "";
            String application_num = "";
            String Project_name = "";
            String Subassembly_name = "";
            String Material = "";
            String Type_ = "";
            String Chamfer_type = "";
            String Drawing_num = "";
            String Procedure_ = "";
            String Inspection_context = "";
            String Inspection_opportunity = "";
            String circulation_NO = "";
            String procedure_NO = "";
            String apparent_condition = "";
            String manufacturing_process = "";
            String Batch_Num = "";
            String Inspection_NO = "";
            String remarks = "";
            String Inspection_date = "";
            String Inspection_result = "";
            String figure = "";
            String Tubes_Size = "";
            String Tubes_num = "";
            String disable_report_num = "";
            String welding_method = "";
            String Inspection_personnel = "";
            String Work_instruction = "";
            String heat_treatment = "";
            #endregion

            ///设备库信息
            //string equipment_library = "select * from dbo.TB_NDT_equipment_library as el left join dbo.TB_NDT_test_equipment as te on el.id=te.equipment_id where el.report_id='" + id + "'";
            //#region
            //string equipment_name = "";
            //string equipment_Type = "";
            //string equipment_num = "";
            //string Manufacture = "";
            //#endregion

            #region//探头库信息
            //string probe_library = "select * from dbo.TB_NDT_probe_library as pl left join dbo.TB_NDT_test_probe as tp on pl.id=tp.probe_id where pl.report_id ='" + id + "'";

            //string Pquipment_name = "";
            //string Pquipment_type = "";
            //string Pquipment_num = "";
            //string Manufacture_probe = "";
            //string Probe_num = "";
            //string Probe_type = "";
            //string Probe_size = "";
            //string Probe_frequency = "";
            //string Coil_Size = "";
            //string Probe_Length = "";
            //string Cable_Length = "";
            //string Assembly_num = "";
            //string Assembly_type = "";
            //string Remarks = "";
            //string Mode_L = "";
            //string Mode_T = "";
            //string Chip_size = "";
            //string Angle = "";
            #endregion

            //测试数据
            string test_info = "select * from dbo.TB_NDT_test_probereport_data where report_num ='" + id + "' ";
            #region
            string Data1 = "";
            string Data2 = "";
            string Data3 = "";
            string Data4 = "";
            string Data5 = "";
            string Data6 = "";
            string Data7 = "";
            string Data8 = "";
            string Data9 = "";
            string Data10 = "";
            string Data11 = "";
            string Data12 = "";
            string Data13 = "";
            string Data14 = "";
            string Data15 = "";
            string Data16 = "";
            string Data17 = "";
            string Data18 = "";
            string Data19 = "";
            string Data20 = "";
            string Data21 = "";
            string Data22 = "";
            string Data23 = "";
            string Data24 = "";
            string Data25 = "";
            string Data26 = "";
            string Data27 = "";
            string Data28 = "";
            string Data29 = "";
            string Data30 = "";
            string Data31 = "";
            string Data32 = "";
            string Data33 = "";
            string Data34 = "";
            string Data35 = "";
            string Data36 = "";
            string Data37 = "";
            string Data38 = "";
            string Data39 = "";
            string Data40 = "";
            string Data41 = "";
            string Data42 = "";
            string Data43 = "";
            string Data44 = "";
            string Data45 = "";
            string Data46 = "";
            string Data47 = "";
            string Data48 = "";
            string Data49 = "";
            string Data50 = "";
            string Data51 = "";
            string Data52 = "";
            string Data53 = "";
            string Data54 = "";
            string Data55 = "";
            string Data56 = "";
            string Data57 = "";
            string Data58 = "";
            string Data59 = "";
            string Data60 = "";
            string Data61 = "";
            string Data62 = "";
            string Data63 = "";
            string Data64 = "";
            #endregion

            #region 插入设备 判断报告
            #region 定义
            string equipment_1 = "";
            string equipment_2 = "";
            string equipment_3 = "";
            string equipment_4 = "";
            string equipment_5 = "";
            string equipment_6 = "";
            string equipment_7 = "";
            string equipment_8 = "";
            string equipment_9 = "";
            string equipment_10 = "";
            string equipment_11 = "";
            string equipment_12 = "";
            string equipment_13 = "";
            string equipment_14 = "";
            string equipment_15 = "";
            string equipment_16 = "";
            string equipment_17 = "";
            string equipment_18 = "";
            string equipment_19 = "";
            string equipment_20 = "";
            string equipment_21 = "";
            string equipment_22 = "";
            string equipment_23 = "";
            string equipment_24 = "";
            string equipment_25 = "";
            string equipment_26 = "";
            string equipment_27 = "";
            string equipment_28 = "";
            string equipment_29 = "";
            string equipment_30 = "";
            string equipment_31 = "";
            string equipment_32 = "";
            string equipment_33 = "";
            string equipment_34 = "";
            string equipment_35 = "";
            string equipment_36 = "";
            string equipment_37 = "";
            string equipment_38 = "";
            string equipment_39 = "";
            string equipment_40 = "";
            string equipment_41 = "";
            string equipment_42 = "";
            string equipment_43 = "";
            string equipment_44 = "";
            string equipment_45 = "";
            string equipment_46 = "";
            string equipment_47 = "";
            string equipment_48 = "";
            string equipment_49 = "";
            string equipment_50 = "";
            string equipment_51 = "";
            string equipment_52 = "";
            string equipment_53 = "";
            string equipment_54 = "";
            string equipment_55 = "";
            string equipment_56 = "";
            string equipment_57 = "";
            string equipment_58 = "";
            string equipment_59 = "";
            #endregion
            if (tm_id == "4")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)
                    {//扫查器
                        equipment_4 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_5 = dr_1["equipment_num"].ToString().Trim();
                        equipment_6 = dr_1["Remarks"].ToString().Trim();

                    } if (i == 1)//数据采集系统
                    {
                        equipment_1 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_2 = dr_1["equipment_num"].ToString().Trim();
                        equipment_3 = dr_1["Remarks"].ToString().Trim();
                    } if (i == 2)//运动控制器
                    {
                        equipment_7 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_8 = dr_1["equipment_num"].ToString().Trim();
                        equipment_9 = dr_1["Remarks"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();

            } if (tm_id == "5")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//标定样管
                    {
                        equipment_13 = dr_1["equipment_name"].ToString().Trim();
                        equipment_14 = dr_1["equipment_num"].ToString().Trim();

                    } if (i == 1)//涡流仪
                    {
                        equipment_10 = dr_1["Manufacture"].ToString().Trim();
                        equipment_11 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_12 = dr_1["equipment_num"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            } if (tm_id == "6")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//氦浓度仪
                    {
                        equipment_19 = dr_1["equipment_name"].ToString().Trim();
                        equipment_20 = dr_1["equipment_num"].ToString().Trim();

                    } if (i == 1)//检测仪器
                    {
                        equipment_15 = dr_1["equipment_name"].ToString().Trim();
                        equipment_16 = dr_1["equipment_num"].ToString().Trim();
                    }
                    if (i == 2)//湿度计
                    {
                        equipment_21 = dr_1["equipment_name"].ToString().Trim();
                        equipment_22 = dr_1["equipment_num"].ToString().Trim();
                    }
                    if (i == 3)//温度计
                    {
                        equipment_23 = dr_1["equipment_name"].ToString().Trim();
                        equipment_24 = dr_1["equipment_num"].ToString().Trim();
                    }
                    if (i == 4)//压力表
                    {
                        equipment_25 = dr_1["equipment_name"].ToString().Trim();
                        equipment_26 = dr_1["equipment_num"].ToString().Trim();
                    }
                    if (i == 5)//真空计
                    {
                        equipment_17 = dr_1["equipment_name"].ToString().Trim();
                        equipment_18 = dr_1["equipment_num"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            } if (tm_id == "7")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//测温仪
                    {
                        equipment_30 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_31 = dr_1["equipment_num"].ToString().Trim();

                    } if (i == 1)//使用设备
                    {
                        equipment_27 = dr_1["Manufacture"].ToString().Trim();
                        equipment_28 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_29 = dr_1["equipment_num"].ToString().Trim();
                    } if (i == 2)//照度计
                    {
                        equipment_32 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_33 = dr_1["equipment_num"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            } if (tm_id == "8")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//测温仪
                    {
                        equipment_37 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_38 = dr_1["equipment_num"].ToString().Trim();
                    } if (i == 1)//黑光强度剂
                    {
                        equipment_39 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_40 = dr_1["equipment_num"].ToString().Trim();
                    } if (i == 2)//使用设备
                    {
                        equipment_34 = dr_1["Manufacture"].ToString().Trim();
                        equipment_35 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_36 = dr_1["equipment_num"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            } if (tm_id == "10")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//仪器
                    {
                        equipment_41 = dr_1["Manufacture"].ToString().Trim();
                        equipment_42 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_43 = dr_1["equipment_num"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            } if (tm_id == "11")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//仪器
                    {
                        equipment_44 = dr_1["Manufacture"].ToString().Trim();
                        equipment_45 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_46 = dr_1["equipment_num"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            } if (tm_id == "13")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//超声数据采集系统
                    {
                        equipment_47 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_48 = dr_1["equipment_num"].ToString().Trim();
                        equipment_49 = dr_1["Remarks"].ToString().Trim();

                    } if (i == 1)//扫查器
                    {
                        equipment_50 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_51 = dr_1["equipment_num"].ToString().Trim();
                        equipment_52 = dr_1["Remarks"].ToString().Trim();
                    } if (i == 2)//运动控制器
                    {
                        equipment_53 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_54 = dr_1["equipment_num"].ToString().Trim();
                        equipment_55 = dr_1["Remarks"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            } if (tm_id == "26")
            {
                string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
                SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
                int i = 0;
                while (dr_1.Read())
                {
                    if (i == 0)//仪器
                    {
                        equipment_56 = dr_1["equipment_Type"].ToString().Trim();
                        equipment_57 = dr_1["equipment_num"].ToString().Trim();

                    } if (i == 1)//照度计/黑光强度计
                    {
                        equipment_58 = dr_1["equipment_name"].ToString().Trim();
                        equipment_59 = dr_1["equipment_num"].ToString().Trim();
                    }
                    i++;
                }
                dr_1.Close();
            }
            #endregion

            try
            {
                //表头信息
                #region
                SqlDataReader rt_dr = SQLHelper.ExecuteReader(CommandType.Text, select_report_title);
                while (rt_dr.Read())
                {
                    report_name = rt_dr["report_name"].ToString().Trim();
                    //report_num = rt_dr["report_num"].ToString().Trim();
                    clientele_department = rt_dr["clientele_department"].ToString().Trim();
                    application_num = rt_dr["application_num"].ToString().Trim();
                    Project_name = rt_dr["Project_name"].ToString().Trim();
                    Subassembly_name = rt_dr["Subassembly_name"].ToString().Trim();
                    Material = rt_dr["Material"].ToString().Trim();
                    Type_ = rt_dr["Type_"].ToString().Trim();
                    Chamfer_type = rt_dr["Chamfer_type"].ToString().Trim();
                    Drawing_num = rt_dr["Drawing_num"].ToString().Trim();
                    Procedure_ = rt_dr["Procedure_"].ToString().Trim();
                    Inspection_context = rt_dr["Inspection_context"].ToString().Trim();
                    Inspection_opportunity = rt_dr["Inspection_opportunity"].ToString().Trim();
                    circulation_NO = rt_dr["circulation_NO"].ToString().Trim();
                    procedure_NO = rt_dr["procedure_NO"].ToString().Trim();
                    apparent_condition = rt_dr["apparent_condition"].ToString().Trim();
                    manufacturing_process = rt_dr["manufacturing_process"].ToString().Trim();
                    Batch_Num = rt_dr["Batch_Num"].ToString().Trim();
                    Inspection_NO = rt_dr["Inspection_NO"].ToString().Trim();
                    remarks = rt_dr["remarks"].ToString().Trim();
                    Inspection_date = rt_dr["Inspection_date"].ToString().Trim();
                    Inspection_result = rt_dr["Inspection_result"].ToString().Trim();
                    figure = rt_dr["figure"].ToString().Trim();
                    Tubes_Size = rt_dr["Tubes_Size"].ToString().Trim();
                    Tubes_num = rt_dr["Tubes_num"].ToString().Trim();
                    disable_report_num = rt_dr["disable_report_num"].ToString().Trim();
                    welding_method = rt_dr["welding_method"].ToString().Trim();
                    //Inspection_personnel = rt_dr["Inspection_personnel"].ToString().Trim();
                    Work_instruction = rt_dr["Work_instruction"].ToString().Trim();
                    heat_treatment = rt_dr["heat_treatment"].ToString().Trim();

                }
                rt_dr.Close();

                if (!string.IsNullOrEmpty(Inspection_date))
                {
                    DateTime Inspection_date1 = Convert.ToDateTime(Inspection_date);
                    Inspection_date = string.Format("{0:yyyy-MM-dd}", Inspection_date1);
                }
                #endregion
                //设备库信息
                #region
                //SqlDataReader el_dr = SQLHelper.ExecuteReader(CommandType.Text, equipment_library);
                //while (el_dr.Read())
                //{
                //    equipment_name = el_dr["equipment_name"].ToString().Trim();
                //    equipment_Type = el_dr["equipment_Type"].ToString().Trim();
                //    equipment_num = el_dr["equipment_num"].ToString().Trim();
                //    Manufacture = el_dr["Manufacture"].ToString().Trim();

                //}
                //el_dr.Close();
                #endregion
                //探头库信息
                #region
                //SqlDataReader pl_dr = SQLHelper.ExecuteReader(CommandType.Text, probe_library);
                //while (pl_dr.Read())
                //{
                //    Pquipment_name = pl_dr["Pquipment_name"].ToString().Trim();
                //    Pquipment_type = pl_dr["Pquipment_type"].ToString().Trim();
                //    Pquipment_num = pl_dr["Pquipment_num"].ToString().Trim();
                //    Manufacture_probe = pl_dr["Manufacture_probe"].ToString().Trim();
                //    Probe_num = pl_dr["Probe_num"].ToString().Trim();
                //    Probe_type = pl_dr["Probe_type"].ToString().Trim();
                //    Probe_size = pl_dr["Probe_size"].ToString().Trim();
                //    Probe_frequency = pl_dr["Probe_frequency"].ToString().Trim();
                //    Coil_Size = pl_dr["Coil_Size"].ToString().Trim();
                //    Probe_Length = pl_dr["Probe_Length"].ToString().Trim();
                //    Cable_Length = pl_dr["Cable_Length"].ToString().Trim();
                //    Assembly_num = pl_dr["Assembly_num"].ToString().Trim();
                //    Assembly_type = pl_dr["Assembly_type"].ToString().Trim();
                //    Remarks = pl_dr["Remarks"].ToString().Trim();
                //    Mode_L = pl_dr["Mode_L"].ToString().Trim();
                //    Mode_T = pl_dr["Mode_T"].ToString().Trim();
                //    Chip_size = pl_dr["Chip_size"].ToString().Trim();
                //    Angle = pl_dr["Angle"].ToString().Trim();

                //}
                //pl_dr.Close();
                #endregion
                //测试数据
                #region
                SqlDataReader dr = SQLHelper.ExecuteReader(CommandType.Text, test_info);
                while (dr.Read())
                {
                    Data1 = dr["Data1"].ToString().Trim();
                    Data2 = dr["Data2"].ToString().Trim();
                    Data3 = dr["Data3"].ToString().Trim();
                    Data4 = dr["Data4"].ToString().Trim();
                    Data5 = dr["Data5"].ToString().Trim();
                    Data6 = dr["Data6"].ToString().Trim();
                    Data7 = dr["Data7"].ToString().Trim();
                    Data8 = dr["Data8"].ToString().Trim();
                    Data9 = dr["Data9"].ToString().Trim();
                    Data10 = dr["Data10"].ToString().Trim();
                    Data11 = dr["Data11"].ToString().Trim();
                    Data12 = dr["Data12"].ToString().Trim();
                    Data13 = dr["Data13"].ToString().Trim();
                    Data14 = dr["Data14"].ToString().Trim();
                    Data15 = dr["Data15"].ToString().Trim();
                    Data16 = dr["Data16"].ToString().Trim();
                    Data17 = dr["Data17"].ToString().Trim();
                    Data18 = dr["Data18"].ToString().Trim();
                    Data19 = dr["Data19"].ToString().Trim();
                    Data20 = dr["Data20"].ToString().Trim();
                    Data21 = dr["Data21"].ToString().Trim();
                    Data22 = dr["Data22"].ToString().Trim();
                    Data23 = dr["Data23"].ToString().Trim();
                    Data24 = dr["Data24"].ToString().Trim();
                    Data25 = dr["Data25"].ToString().Trim();
                    Data26 = dr["Data26"].ToString().Trim();
                    Data27 = dr["Data27"].ToString().Trim();
                    Data28 = dr["Data28"].ToString().Trim();
                    Data29 = dr["Data29"].ToString().Trim();
                    Data30 = dr["Data30"].ToString().Trim();
                    Data31 = dr["Data31"].ToString().Trim();
                    Data32 = dr["Data32"].ToString().Trim();
                    Data33 = dr["Data33"].ToString().Trim();
                    Data34 = dr["Data34"].ToString().Trim();
                    Data35 = dr["Data35"].ToString().Trim();
                    Data36 = dr["Data36"].ToString().Trim();
                    Data37 = dr["Data37"].ToString().Trim();
                    Data38 = dr["Data38"].ToString().Trim();
                    Data39 = dr["Data39"].ToString().Trim();
                    Data40 = dr["Data40"].ToString().Trim();
                    Data41 = dr["Data41"].ToString().Trim();
                    Data42 = dr["Data42"].ToString().Trim();
                    Data43 = dr["Data43"].ToString().Trim();
                    Data44 = dr["Data44"].ToString().Trim();
                    Data45 = dr["Data45"].ToString().Trim();
                    Data46 = dr["Data46"].ToString().Trim();
                    Data47 = dr["Data47"].ToString().Trim();
                    Data48 = dr["Data48"].ToString().Trim();
                    Data49 = dr["Data49"].ToString().Trim();
                    Data50 = dr["Data50"].ToString().Trim();
                    Data51 = dr["Data51"].ToString().Trim();
                    Data52 = dr["Data52"].ToString().Trim();
                    Data53 = dr["Data53"].ToString().Trim();
                    Data54 = dr["Data54"].ToString().Trim();
                    Data55 = dr["Data55"].ToString().Trim();
                    Data56 = dr["Data56"].ToString().Trim();
                    Data57 = dr["Data57"].ToString().Trim();
                    Data58 = dr["Data58"].ToString().Trim();
                    Data59 = dr["Data59"].ToString().Trim();
                    Data60 = dr["Data60"].ToString().Trim();
                    Data61 = dr["Data61"].ToString().Trim();
                    Data62 = dr["Data62"].ToString().Trim();
                    Data63 = dr["Data63"].ToString().Trim();
                    Data64 = dr["Data64"].ToString().Trim();
                }
                dr.Close();
                #endregion
            }
            catch
            {
            }

            //插入书签
            #region
            foreach (Aspose.Words.Bookmark mark in contract_doc.Range.Bookmarks)
            {
                switch (mark.Name)
                {
                    //表头信息
                    case "report_num": mark.Text = report_num; break;
                    case "clientele_department": mark.Text = clientele_department; break;
                    case "Work_instruction": mark.Text = Work_instruction; break;
                    case "heat_treatment": mark.Text = heat_treatment; break;
                    case "application_num": mark.Text = application_num; break;
                    case "Project_name": mark.Text = Project_name; break;
                    case "Subassembly_name": mark.Text = Subassembly_name; break;
                    case "Material": mark.Text = Material; break;
                    case "Type_": mark.Text = Type_; break;
                    case "Chamfer_type": mark.Text = Chamfer_type; break;
                    case "Drawing_num": mark.Text = Drawing_num; break;
                    case "Procedure_": mark.Text = Procedure_; break;
                    case "Inspection_context": mark.Text = Inspection_context; break;
                    case "Inspection_opportunity": mark.Text = Inspection_opportunity; break;
                    case "circulation_NO": mark.Text = circulation_NO; break;
                    case "procedure_NO": mark.Text = procedure_NO; break;
                    case "apparent_condition": mark.Text = apparent_condition; break;
                    case "manufacturing_process": mark.Text = manufacturing_process; break;
                    case "Batch_Num": mark.Text = Batch_Num; break;
                    case "Inspection_NO": mark.Text = Inspection_NO; break;
                    case "remarks": mark.Text = remarks; break;
                    case "Inspection_date": mark.Text = Inspection_date; break;
                    case "Inspection_personnel": mark.Text = Inspection_personnel; break;
                    case "Tubes_num": mark.Text = Tubes_num; break;
                    case "Tubes_Size": mark.Text = Tubes_Size; break;
                    case "disable_report_num": mark.Text = disable_report_num; break;
                    case "welding_method": mark.Text = welding_method; break;
                    case "Inspection_result_yes": if (Inspection_result.Trim() == "0" || Inspection_result.Trim() == "False")
                        {
                            insert_char163(docBuild, "Inspection_result_yes");
                        }
                        else if (Inspection_result.Trim() == "1" || Inspection_result.Trim() == "True")
                        {
                            insert_char82(docBuild, "Inspection_result_yes");
                        } break;
                    case "Inspection_result_no": if (Inspection_result.Trim() == "0" || Inspection_result.Trim() == "False")
                        {
                            insert_char82(docBuild, "Inspection_result_no");
                        }
                        else if (Inspection_result.Trim() == "1" || Inspection_result.Trim() == "True")
                        {
                            insert_char163(docBuild, "Inspection_result_no");
                        } break;
                    case "figure_yes": if (figure.Trim() == "0" || figure.Trim() == "False")
                        {
                            insert_char163(docBuild, "figure_yes");
                        }
                        else if (figure.Trim() == "1" || figure.Trim() == "True")
                        {
                            insert_char82(docBuild, "figure_yes");
                        } break;
                    case "figure_no": if (figure.Trim() == "0" || figure.Trim() == "False")
                        {
                            insert_char82(docBuild, "figure_no");
                        }
                        else if (figure.Trim() == "1" || figure.Trim() == "True")
                        {
                            insert_char163(docBuild, "figure_no");
                        } break;


                    //表头重复信息 ——水压测试报告
                    case "report_num2": mark.Text = report_num; break;
                    case "report_num3": mark.Text = report_num; break;
                    case "report_num4": mark.Text = report_num; break;
                    case "clientele_department2": mark.Text = clientele_department; break;
                    case "application_num2": mark.Text = application_num; break;
                    case "Project_name2": mark.Text = Project_name; break;
                    case "Subassembly_name2": mark.Text = Subassembly_name; break;
                    case "Material2": mark.Text = Material; break;
                    case "Type_2": mark.Text = Type_; break;
                    case "Chamfer_type2": mark.Text = Chamfer_type; break;
                    case "Drawing_num2": mark.Text = Drawing_num; break;
                    case "Procedure_2": mark.Text = Procedure_; break;
                    case "Inspection_context2": mark.Text = Inspection_context; break;
                    case "Inspection_opportunity2": mark.Text = Inspection_opportunity; break;
                    case "circulation_NO2": mark.Text = circulation_NO; break;
                    case "procedure_NO2": mark.Text = procedure_NO; break;
                    case "apparent_condition2": mark.Text = apparent_condition; break;
                    case "manufacturing_process2": mark.Text = manufacturing_process; break;
                    case "Batch_Num2": mark.Text = Batch_Num; break;
                    case "Inspection_NO2": mark.Text = Inspection_NO; break;
                    case "remarks2": mark.Text = remarks; break;
                    case "Inspection_date2": mark.Text = Inspection_date; break;
                    case "Inspection_personnel2": mark.Text = Inspection_personnel; break;
                    case "Tubes_num2": mark.Text = Tubes_num; break;
                    case "Tubes_Size2": mark.Text = Tubes_Size; break;
                    case "disable_report_num2": mark.Text = disable_report_num; break;
                    case "welding_method2": mark.Text = welding_method; break;
                    //设备表信息
                    //case "equipment_name": mark.Text = equipment_name; break;
                    //case "equipment_Type": mark.Text = equipment_Type; break;
                    //case "equipment_num": mark.Text = equipment_num; break;
                    //case "Manufacture": mark.Text = Manufacture; break;

                    //探头表信息
                    //case "Pquipment_name": mark.Text = Pquipment_name; break;
                    //case "Pquipment_type": mark.Text = Pquipment_type; break;
                    //case "Pquipment_num": mark.Text = Pquipment_num; break;
                    //case "Manufacture_probe": mark.Text = Manufacture_probe; break;
                    //case "Probe_num": mark.Text = Probe_num; break;
                    //case "Probe_type": mark.Text = Probe_type; break;
                    //case "Probe_size": mark.Text = Probe_size; break;
                    //case "Probe_frequency": mark.Text = Probe_frequency; break;
                    //case "Coil_Size": mark.Text = Coil_Size; break;
                    //case "Probe_Length": mark.Text = Probe_Length; break;
                    //case "Cable_Length": mark.Text = Cable_Length; break;
                    //case "Assembly_num": mark.Text = Assembly_num; break;
                    //case "Assembly_type": mark.Text = Assembly_type; break;
                    //case "Remarks": mark.Text = Remarks; break;
                    //case "Mode_L": mark.Text = Mode_L; break;
                    //case "Mode_T": mark.Text = Mode_T; break;
                    //case "Chip_size": mark.Text = Chip_size; break;
                    //case "Angle": mark.Text = Angle; break;

                    //设备信息
                    case "equipment_1": mark.Text = equipment_1; break;
                    case "equipment_2": mark.Text = equipment_2; break;
                    case "equipment_3": mark.Text = equipment_3; break;
                    case "equipment_4": mark.Text = equipment_4; break;
                    case "equipment_5": mark.Text = equipment_5; break;
                    case "equipment_6": mark.Text = equipment_6; break;
                    case "equipment_7": mark.Text = equipment_7; break;
                    case "equipment_8": mark.Text = equipment_8; break;
                    case "equipment_9": mark.Text = equipment_9; break;
                    case "equipment_10": mark.Text = equipment_10; break;
                    case "equipment_11": mark.Text = equipment_11; break;
                    case "equipment_12": mark.Text = equipment_12; break;
                    case "equipment_13": mark.Text = equipment_13; break;
                    case "equipment_14": mark.Text = equipment_14; break;
                    case "equipment_15": mark.Text = equipment_15; break;
                    case "equipment_16": mark.Text = equipment_16; break;
                    case "equipment_17": mark.Text = equipment_17; break;
                    case "equipment_18": mark.Text = equipment_18; break;
                    case "equipment_19": mark.Text = equipment_19; break;
                    case "equipment_20": mark.Text = equipment_20; break;
                    case "equipment_21": mark.Text = equipment_21; break;
                    case "equipment_22": mark.Text = equipment_22; break;
                    case "equipment_23": mark.Text = equipment_23; break;
                    case "equipment_24": mark.Text = equipment_24; break;
                    case "equipment_25": mark.Text = equipment_25; break;
                    case "equipment_26": mark.Text = equipment_26; break;
                    case "equipment_27": mark.Text = equipment_27; break;
                    case "equipment_28": mark.Text = equipment_28; break;
                    case "equipment_29": mark.Text = equipment_29; break;
                    case "equipment_30": mark.Text = equipment_30; break;
                    case "equipment_31": mark.Text = equipment_31; break;
                    case "equipment_32": mark.Text = equipment_32; break;
                    case "equipment_33": mark.Text = equipment_33; break;
                    case "equipment_34": mark.Text = equipment_34; break;
                    case "equipment_35": mark.Text = equipment_35; break;
                    case "equipment_36": mark.Text = equipment_36; break;
                    case "equipment_37": mark.Text = equipment_37; break;
                    case "equipment_38": mark.Text = equipment_38; break;
                    case "equipment_39": mark.Text = equipment_39; break;
                    case "equipment_40": mark.Text = equipment_40; break;
                    case "equipment_41": mark.Text = equipment_41; break;
                    case "equipment_42": mark.Text = equipment_42; break;
                    case "equipment_43": mark.Text = equipment_43; break;
                    case "equipment_44": mark.Text = equipment_44; break;
                    case "equipment_45": mark.Text = equipment_45; break;
                    case "equipment_46": mark.Text = equipment_46; break;
                    case "equipment_47": mark.Text = equipment_47; break;
                    case "equipment_48": mark.Text = equipment_48; break;
                    case "equipment_49": mark.Text = equipment_49; break;
                    case "equipment_50": mark.Text = equipment_50; break;
                    case "equipment_51": mark.Text = equipment_51; break;
                    case "equipment_52": mark.Text = equipment_52; break;
                    case "equipment_53": mark.Text = equipment_53; break;
                    case "equipment_54": mark.Text = equipment_54; break;
                    case "equipment_55": mark.Text = equipment_55; break;
                    case "equipment_56": mark.Text = equipment_56; break;
                    case "equipment_57": mark.Text = equipment_57; break;
                    case "equipment_58": mark.Text = equipment_58; break;
                    case "equipment_59": mark.Text = equipment_59; break;


                    //测试数据信息
                    case "Data1": insert_char82_163(mark, docBuild, mark.Name, Data1); break;
                    case "Data2": insert_char82_163(mark, docBuild, mark.Name, Data2); break;
                    case "Data3": insert_char82_163(mark, docBuild, mark.Name, Data3); break;
                    case "Data4": insert_char82_163(mark, docBuild, mark.Name, Data4); break;
                    case "Data5": insert_char82_163(mark, docBuild, mark.Name, Data5); break;
                    case "Data6": insert_char82_163(mark, docBuild, mark.Name, Data6); break;
                    case "Data7": insert_char82_163(mark, docBuild, mark.Name, Data7); break;
                    case "Data8": insert_char82_163(mark, docBuild, mark.Name, Data8); break;
                    case "Data9": insert_char82_163(mark, docBuild, mark.Name, Data9); break;
                    case "Data10": insert_char82_163(mark, docBuild, mark.Name, Data10); break;
                    case "Data11": insert_char82_163(mark, docBuild, mark.Name, Data11); break;
                    case "Data12": insert_char82_163(mark, docBuild, mark.Name, Data12); break;
                    case "Data13": insert_char82_163(mark, docBuild, mark.Name, Data13); break;
                    case "Data14": insert_char82_163(mark, docBuild, mark.Name, Data14); break;
                    case "Data15": insert_char82_163(mark, docBuild, mark.Name, Data15); break;
                    case "Data16": insert_char82_163(mark, docBuild, mark.Name, Data16); break;
                    case "Data17": insert_char82_163(mark, docBuild, mark.Name, Data17); break;
                    case "Data18": insert_char82_163(mark, docBuild, mark.Name, Data18); break;
                    case "Data19": insert_char82_163(mark, docBuild, mark.Name, Data19); break;
                    case "Data20": insert_char82_163(mark, docBuild, mark.Name, Data20); break;
                    case "Data21": insert_char82_163(mark, docBuild, mark.Name, Data21); break;
                    case "Data22": insert_char82_163(mark, docBuild, mark.Name, Data22); break;
                    case "Data23": insert_char82_163(mark, docBuild, mark.Name, Data23); break;
                    case "Data24": insert_char82_163(mark, docBuild, mark.Name, Data24); break;
                    case "Data25": insert_char82_163(mark, docBuild, mark.Name, Data25); break;
                    case "Data26": insert_char82_163(mark, docBuild, mark.Name, Data26); break;
                    case "Data27": insert_char82_163(mark, docBuild, mark.Name, Data27); break;
                    case "Data28": insert_char82_163(mark, docBuild, mark.Name, Data28); break;
                    case "Data29": insert_char82_163(mark, docBuild, mark.Name, Data29); break;
                    case "Data30": insert_char82_163(mark, docBuild, mark.Name, Data30); break;
                    case "Data31": insert_char82_163(mark, docBuild, mark.Name, Data31); break;
                    case "Data32": insert_char82_163(mark, docBuild, mark.Name, Data32); break;
                    case "Data33": insert_char82_163(mark, docBuild, mark.Name, Data33); break;
                    case "Data34": insert_char82_163(mark, docBuild, mark.Name, Data34); break;
                    case "Data35": insert_char82_163(mark, docBuild, mark.Name, Data35); break;
                    case "Data36": insert_char82_163(mark, docBuild, mark.Name, Data36); break;
                    case "Data37": insert_char82_163(mark, docBuild, mark.Name, Data37); break;
                    case "Data38": insert_char82_163(mark, docBuild, mark.Name, Data38); break;
                    case "Data39": insert_char82_163(mark, docBuild, mark.Name, Data39); break;
                    case "Data40": insert_char82_163(mark, docBuild, mark.Name, Data40); break;
                    case "Data41": insert_char82_163(mark, docBuild, mark.Name, Data41); break;
                    case "Data42": insert_char82_163(mark, docBuild, mark.Name, Data42); break;
                    case "Data43": insert_char82_163(mark, docBuild, mark.Name, Data43); break;
                    case "Data44": insert_char82_163(mark, docBuild, mark.Name, Data44); break;
                    case "Data45": insert_char82_163(mark, docBuild, mark.Name, Data45); break;
                    case "Data46": insert_char82_163(mark, docBuild, mark.Name, Data46); break;
                    case "Data47": insert_char82_163(mark, docBuild, mark.Name, Data47); break;
                    case "Data48": insert_char82_163(mark, docBuild, mark.Name, Data48); break;
                    case "Data49": insert_char82_163(mark, docBuild, mark.Name, Data49); break;
                    case "Data50": insert_char82_163(mark, docBuild, mark.Name, Data50); break;
                    case "Data51": insert_char82_163(mark, docBuild, mark.Name, Data51); break;
                    case "Data52": insert_char82_163(mark, docBuild, mark.Name, Data52); break;
                    case "Data53": insert_char82_163(mark, docBuild, mark.Name, Data53); break;
                    case "Data54": insert_char82_163(mark, docBuild, mark.Name, Data54); break;
                    case "Data55": insert_char82_163(mark, docBuild, mark.Name, Data55); break;
                    case "Data56": insert_char82_163(mark, docBuild, mark.Name, Data56); break;
                    case "Data57": insert_char82_163(mark, docBuild, mark.Name, Data57); break;
                    case "Data58": insert_char82_163(mark, docBuild, mark.Name, Data58); break;
                    case "Data59": insert_char82_163(mark, docBuild, mark.Name, Data59); break;
                    case "Data60": insert_char82_163(mark, docBuild, mark.Name, Data60); break;
                    case "Data61": insert_char82_163(mark, docBuild, mark.Name, Data61); break;
                    case "Data62": insert_char82_163(mark, docBuild, mark.Name, Data62); break;
                    case "Data63": insert_char82_163(mark, docBuild, mark.Name, Data63); break;
                    case "Data64": insert_char82_163(mark, docBuild, mark.Name, Data64); break;

                }
                //else if (mark.Name == "customer_name")
                //    mark.Text = dr["customer_name"].ToString();
            }
            #endregion

            contract_doc.Save(outputPath, Aspose.Words.SaveFormat.Doc);
            //string path = outputPath;
            //System.IO.FileInfo file = new System.IO.FileInfo(path);
            context.Response.Write(return_url);
            context.Response.End();
        }
        //判断是否为打勾书签
        public void insert_char82_163(Aspose.Words.Bookmark mark, Aspose.Words.DocumentBuilder docBuild, string mark_Name, string Data)
        {
            if (Data == "1,flag")//如果为勾选
            {
                insert_char82(docBuild, mark_Name);
            }
            else if (Data == "flag")//如果不为勾选
            {
                insert_char163(docBuild, mark_Name);
            }
            else { mark.Text = Data; }//为文字输入框
        }
        //打勾
        public void insert_char82(Aspose.Words.DocumentBuilder docBuild, string local_x)
        {


            docBuild.MoveToBookmark(local_x);
            docBuild.Font.Name = "Wingdings 2";
            docBuild.Font.Size = 10.0;
            docBuild.Write(char.ConvertFromUtf32(162));

        }
        //不打勾
        public void insert_char163(Aspose.Words.DocumentBuilder docBuild, string local_x)
        {


            docBuild.MoveToBookmark(local_x);
            docBuild.Font.Name = "Wingdings 2";
            docBuild.Font.Size = 10.0;
            docBuild.Write(char.ConvertFromUtf32(163));

        }
        /// 检测报告在线编辑
        public void Get_Report_Url(HttpContext context)
        {
            string id = context.Request.Params.Get("id");
            string report_url = "";
            //判断证书是否存在
            string get_urlSql = "select report_url from dbo.TB_NDT_report_title where id ='" + id + "'";
            report_url = db.ExecuteScalar(get_urlSql).ToString();
            if (report_url == "" || report_url == null)
            {
                context.Response.Write("F");
            }
            else
            {
                context.Response.Write(report_url);

            }

            context.Response.End();
        }
        /// 编制报告--提交审核报告
        public void submit_edit_Report(HttpContext context)
        {
            int state_ = (int)LosslessReportStatusEnum.Audit;

            string date = context.Request.Params.Get("level_date");                    //时间
            string login_user = Convert.ToString(context.Session["loginAccount"]);   //用户 编制人员
            string id = context.Request.Params.Get("id");
            string Inspection_personnel = context.Request.Params.Get("Inspection_personnel");   //检验人
            //string Audit_personnel = context.Request.Params.Get("group");   //审核组id
            String report_num = context.Request.Params.Get("report_num");
            String report_url = context.Request.Params.Get("report_url");
            String level_Inspection = context.Request.Params.Get("level_Inspection");
            String AuditIssueRetrunTime = DateTime.Now.ToString();//第一次签字时间
            String group = context.Request.Params.Get("group");
            Document first_doc = new Document(context.Server.MapPath(report_url));///upload_Folder/word_certificate/0e8b03a4-a7e5-455f-bea1-441bf5076448.doc
            DocumentBuilder first_builder = new DocumentBuilder(first_doc);

            //String select = "select top 1 report_url from dbo.TB_NDT_RevisionsRecord where report_id='" + id + "' order by id desc";
            //string reportLogURl = db.ExecuteScalar(select).ToString();
            //Document record_Logdoc = new Document(context.Server.MapPath(reportLogURl));
            //DocumentBuilder record_Logbuilder = new DocumentBuilder(record_Logdoc);

            string date2 = DateTime.Now.ToString();

            //判断日期是否为空
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }
            //判断级别是否为空
            if (string.IsNullOrEmpty(level_Inspection))
            {
                level_Inspection = "II";
            }

            //复制的记录文档
            string saveName = Guid.NewGuid().ToString() + ".doc"; //保存文件名称
            string new_report_url = "/upload_Folder/Lossless_report_/" + saveName;

            string getDetectionSql = @"select top 1 Signature as detection_image from dbo.TB_user_info where User_count = '" + login_user + "'";
            DataTable detection_dt = db.ExecuteDataTable(getDetectionSql);
            //判断签名是否存在
            if (!File.Exists(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())))
            {
                context.Response.Write("签名不存在");
                context.Response.End();
            }

            if (detection_dt.Rows.Count > 0)
            {
                //写入试验人员签名                    
                try
                {
                    // first_builder.InsertField("", "Hello");

                    first_doc.Range.Bookmarks["Inspection_personnel"].Text = "";
                    first_builder.MoveToBookmark("Inspection_personnel");//跳到指定书签
                    double width = 50, height = 20;
                    Aspose.Words.Drawing.Shape shape = first_builder.InsertImage(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                    shape.Width = width;
                    shape.Height = height;
                    // shape.VerticalAlignment = VerticalAlignment.Center;
                    first_builder.InsertNode(shape);

                    //record_Logdoc.Range.Bookmarks["Inspection_personnel"].Text = "";
                    //record_Logbuilder.MoveToBookmark("Inspection_personnel");//跳到指定书签
                    ////double width = 50, height = 20;
                    //Aspose.Words.Drawing.Shape shapes = record_Logbuilder.InsertImage(context.Server.MapPath(detection_dt.Rows[0]["detection_image"].ToString())); //插入图片：自动控制大小，并不遮挡后面的内容
                    //shapes.Width = width;
                    //shapes.Height = height;
                    //// shape.VerticalAlignment = VerticalAlignment.Center;
                    //record_Logbuilder.InsertNode(shapes);
                }
                catch (Exception ex)
                {

                }
                //写入检验级别
                try
                {
                    first_doc.Range.Bookmarks["level_Inspection"].Text = level_Inspection;//审核级别
                }
                catch (Exception ex)
                {
                }

                //写入试验时间
                try
                {
                    //DateTime new_time = DateTime.Today;
                    //string Inspection_personnel_date = string.Format("{0:yyyy-MM-dd}", new_time);
                    first_doc.Range.Bookmarks["Inspection_personnel_date"].Text = date;//检验人签字时间
                    //record_Logdoc.Range.Bookmarks["Inspection_personnel_date"].Text = date;
                }
                catch (Exception ex)
                {
                    //context.Response.Write(ex);
                    //context.Response.End();
                }
            }

            first_doc.Save(context.Server.MapPath(report_url), Aspose.Words.SaveFormat.Doc);
            first_doc.Save(context.Server.MapPath(new_report_url), Aspose.Words.SaveFormat.Doc);//记录文档
            //判断是不是第一次提交
            string getRetrunTimeSql = @"select AuditIssueRetrunTime from dbo.TB_NDT_report_title where id='" + id + "' and AuditIssueRetrunTime is null";
            DataTable RetrunTime_dt = db.ExecuteDataTable(getRetrunTimeSql);
            string setRetrunTimeSql = "";

            //如果是第一次提交修改第一次编制签名时间
            if (RetrunTime_dt.Rows.Count > 0)
            {
                setRetrunTimeSql = ",AuditIssueRetrunTime='" + AuditIssueRetrunTime + "'";
            }
            string upate_ = "update dbo.TB_NDT_report_title set level_Inspection='" + level_Inspection + "',Inspection_personnel_date='" + date + "',state_='" + state_ + "',Audit_groupid='" + group + "'" + setRetrunTimeSql + " where id='" + id + "' ";



            string insert = "insert into dbo.TB_NDT_RevisionsRecord (report_id,report_url,ReturnNode,add_date,addpersonnel) values ('" + id + "','" + new_report_url + "','EditUpdate','" + date2 + "','" + login_user + "')";
            //添加报告流程记录
            string insertProcessRecord = "insert into dbo.TB_ProcessRecord (ReportID,Operator,OperateDate,NodeResult,NodeId) values ('" + id + "','" + login_user + "','" + AuditIssueRetrunTime + "','" + "pass" + "','" + (int)TB_ProcessRecordNodeIdEnum.EditToReview + "')";

            //////删除退回编制附注图片
            ////string del_sql = "delete dbo.TB_NDT_return_accessory where report_id='" + id + "'";

            //string messageContent = "您有一条 报告编号为：" + report_num + "的无损报告需要审核";
            //string message_type = "待报告审核";

            //string messageSqls = "insert into  dbo.TB_show_message(User_count,message,message_type,create_time,send_time,message_push_personnel) values ('"
            //     + Audit_personnel + "','" + messageContent + "','" + message_type + "','" + date + "','" + date + "','" + login_user + "') ";


            try
            {
                db.BeginTransaction();
                //db.ExecuteNonQueryByTrans(messageSqls);
                //db.ExecuteNonQueryByTrans(del_sql);
                db.ExecuteNonQueryByTrans(insert);
                db.ExecuteNonQueryByTrans(upate_);
                db.ExecuteNonQueryByTrans(insertProcessRecord);
                db.CommitTransacton();

                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "提交无损报告审核", "报告编号：" + report_num);
                //new Send_message().send_usercount(Audit_personnel, messageContent);
                ////发消息
                //Send_message new_message = new Send_message();
                //new_message.send_usercount(Audit_personnel, messageContent);
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
        //加载组
        public void load_professional_department(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select id,Department_name from dbo.TB_department where id>=45";
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
            string strsql = "select u.User_count,u.User_name from dbo.TB_user_info as u "
            + " left join  dbo.TB_user_department as d  on u.User_count=d.User_count  where"
                + " d.User_department= '" + User_department + "' ";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }
        //加载试样设备          COLLATE Chinese_PRC_CI_AI_WS 强制转换字段规则
        public void load_Device_test(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select id,(sample_name COLLATE Chinese_PRC_CI_AI_WS + asset_num) as equipment_nem from dbo.TB_standing_book where customer_name='质量检验部无损检测室' and (management_state='SJ' or management_state='ZY') ";
            DataSet ds = sql2.GetDataSet(strsql, ConfigurationManager.ConnectionStrings["pubsConnectionString2"].ConnectionString);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }
        //加载试验探头
        public void load_Probe_test(HttpContext context)
        {
            int Probe_state = (int)ProbeStateEnum.ZY;
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select * from dbo.TB_NDT_probe_library where Probe_state=" + Probe_state;
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }
        //加载已选试验探头
        public void load_Probe_test2(HttpContext context)
        {
            String report_id = context.Request.Params.Get("report_id");
            SQLHelper sql2 = new SQLHelper();
            string strsql = "select pl.* from dbo.TB_NDT_test_probe as tp left join dbo.TB_NDT_probe_library as pl on tp.probe_id=pl.id where tp.report_id='" + report_id + "'";
            DataSet ds = sql2.GetDataSet(strsql);
            DataTable dt = ds.Tables[0];
            int count = dt.Rows.Count;
            string strJson = Dataset2Json1(ds, count);//DataSet数据转化为Json数据    
            context.Response.Write(strJson);//返回给前台页面    
            context.Response.End();
        }
        //添加试验探头
        private void add_Probe_test(HttpContext context)
        {
            String probe_id = context.Request.Params.Get("probe_id");//探头id
            String report_id = context.Request.Params.Get("report_id");
            String report_num = context.Request.Params.Get("report_num");

            String insert_sql = "";

            string select_info = "select * from dbo.TB_NDT_test_probe where report_id='" + report_id + "' and probe_id='" + probe_id + "'";
            SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_info);
            int flag = 0;
            while (dr_1.Read())
            {
                flag = 1;
            }
            dr_1.Close();
            if (flag == 0)
            {
                insert_sql = "insert into dbo.TB_NDT_test_probe (report_id,probe_id) values ('" + report_id + "','" + probe_id + "')";
            }
            else
            {
                context.Response.Write("D");
                context.Response.End();
            }
            //String insert_sql = "insert into dbo.TB_NDT_test_probe (report_id,probe_id) values ('" + report_id + "','" + probe_id + "')";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(insert_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "添加报告探头信息", "报告编号:" + report_num + "、探头id：" + probe_id);
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
        //删除试验探头
        private void del_Probe_test(HttpContext context)
        {
            String probe_id = context.Request.Params.Get("probe_id");//探头id
            String report_id = context.Request.Params.Get("report_id");
            String report_num = context.Request.Params.Get("report_num");

            String insert_sql = "delete dbo.TB_NDT_test_probe where report_id='" + report_id + "' and probe_id='" + probe_id + "'";

            try
            {
                //SQL语句
                List<string> SQLStringList = new List<string>();
                SQLStringList.Add(insert_sql);
                //事务
                SQLHelper.ExecuteSqlTran(SQLStringList);
                string loginAccount = Convert.ToString(context.Session["loginAccount"]);
                string login_username = Convert.ToString(context.Session["login_username"]);
                Operation_log.operation_log_(loginAccount, login_username, "删除报告探头信息", "报告编号:" + report_num + "、探头id：" + probe_id);
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
        //回显设备信息
        private void load_equipment_info(HttpContext context)
        {
            String id = context.Request.Params.Get("id");//报告id
            String tm_id = context.Request.Params.Get("tm_id");
            string[] info = new string[6];

            string select_1 = "select * from TB_NDT_test_equipment where report_id='" + id + "' order by equipment_name_R ";
            SqlDataReader dr_1 = SQLHelper.ExecuteReader(CommandType.Text, select_1);
            int i = 0;
            while (dr_1.Read())
            {
                info[i] = dr_1["equipment_id"].ToString().Trim() + ";" + dr_1["equipment_name"].ToString().Trim();
                i++;
            }
            dr_1.Close();

            string data = "";
            if (tm_id == "4")
            {
                data = info[1] + "," + info[0] + "," + info[2];
            } if (tm_id == "5")
            {
                data = info[1] + "," + info[0];
            } if (tm_id == "6")
            {
                data = info[1] + "," + info[5] + "," + info[0] + "," + info[2] + "," + info[3] + "," + info[4];
            } if (tm_id == "7")
            {
                data = info[1] + "," + info[0] + "," + info[2];
            } if (tm_id == "8")
            {
                data = info[2] + "," + info[0] + "," + info[1];
            } if (tm_id == "10")
            {
                data = info[0];
            } if (tm_id == "11")
            {
                data = info[0];
            } if (tm_id == "13")
            {
                data = info[0] + "," + info[1] + "," + info[2];
            } if (tm_id == "26")
            {
                data = info[0] + "," + info[1] + "," + info[2];
            }
            context.Response.Write(data);
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