using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Phians_BLL;
using Phians_BLL.ReportManagement;
using Phians_Entity;
using Phians_Entity.Common;
using phians_laboratory.custom_class;
using phians_laboratory.custom_class.ActionFilters;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhiansCommon.Enum;
using System.IO;
using System.Text;
using Phians_Entity.LosslessReport;
using System.Net;
using Newtonsoft.Json.Linq;

namespace phians_laboratory.Controllers
{
    public class ReportManagementController : BaseController
    {
        //
        // GET: /ReportManagement/


        #region =====================  报告流程

        #region 报告编制
        [Authentication]
        public ActionResult ReportPreparation()
        {
            return View();
        }

        #region 页面字段显示list
        public string loadPageShowSetting()
        {
            string PageId = Convert.ToString(Request.Params.Get("PageId"));//报告编制页面PageId=101，报告审核页面PageId=102，报告签发页面PageId=103，报告管理页面PageId=104，报告监控页面PageId=113

            try
            {
                string list_model = new ReportEditBLL().loadPageShowSetting(PageId);

                return JsonConvert.SerializeObject(new ExecuteResult(true, list_model));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 获取报告编制列表
        public string LoadReportEditList()//获取列表
        {
            int pageIndex = Convert.ToInt32(Request.Params.Get("page"));
            int pagesize = Convert.ToInt32(Request.Params.Get("rows"));
            string search = Convert.ToString(Request.Params.Get("search"));
            string key = Convert.ToString(Request.Params.Get("key"));
            string history_flag = Convert.ToString(Request.Params.Get("history_flag"));//history_flag=1 为历史信息； history_flag=0 为待编制信息；
            string Inspection_personnel = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//报告编制人

            try
            {
                int totalRecord;
                List<TB_NDT_report_title> list_model = new ReportEditBLL().LoadReportEditList(pageIndex, pagesize, out totalRecord, search, key, history_flag, Inspection_personnel);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region ---添加报告信息

        #region 添加报告信息
        public string AddReportBaseInfo(FormCollection FormCollection)
        {
            try
            {
                TB_NDT_report_title model = new TB_NDT_report_title();
                model.welding_method = Convert.ToString(FormCollection["welding_method"]);
                model.Job_num = Convert.ToString(FormCollection["Job_num"]);
                model.disable_report_num = Convert.ToString(FormCollection["disable_report_num"]);
                model.Tubes_num = Convert.ToString(FormCollection["Tubes_num"]);
                model.Tubes_Size = Convert.ToString(FormCollection["Tubes_Size"]);
                if (!string.IsNullOrEmpty(FormCollection["figure"]))
                {
                    model.figure = Convert.ToBoolean(FormCollection["figure"]);
                }
                else
                {
                    model.figure = false;

                }

                model.Inspection_result = Convert.ToString(FormCollection["Inspection_result"]);
                model.clientele = Convert.ToString(FormCollection["clientele"]);
                model.clientele_department = Convert.ToString(FormCollection["clientele_department"]);
                model.application_num = Convert.ToString(FormCollection["application_num"]);
                model.Project_name = Convert.ToString(FormCollection["Project_name"]);
                model.Subassembly_name = Convert.ToString(FormCollection["Subassembly_name"]);
                model.Material = Convert.ToString(FormCollection["Material"]);
                model.Type_ = Convert.ToString(FormCollection["Type_"]);
                model.ModelNum = Convert.ToString(FormCollection["ModelNum"]);
                model.Chamfer_type = Convert.ToString(FormCollection["Chamfer_type"]);
                model.Drawing_num = Convert.ToString(FormCollection["Drawing_num"]);
                model.Procedure_ = Convert.ToString(FormCollection["Procedure_"]);
                model.Inspection_context = Convert.ToString(FormCollection["Inspection_context"]);
                model.Inspection_opportunity = Convert.ToString(FormCollection["Inspection_opportunity"]);
                model.circulation_NO = Convert.ToString(FormCollection["circulation_NO"]);
                model.procedure_NO = Convert.ToString(FormCollection["procedure_NO"]);
                model.apparent_condition = Convert.ToString(FormCollection["apparent_condition"]);
                model.manufacturing_process = Convert.ToString(FormCollection["manufacturing_process"]);
                model.Batch_Num = Convert.ToString(FormCollection["Batch_Num"]);
                model.Inspection_NO = Convert.ToString(FormCollection["Inspection_NO"]);
                model.remarks = Convert.ToString(FormCollection["remarks"]);
                model.Inspection_date = Convert.ToDateTime(FormCollection["Inspection_date"]);
                model.Work_instruction = Convert.ToString(FormCollection["Work_instruction"]);
                model.heat_treatment = Convert.ToString(FormCollection["heat_treatment"]);
                model.report_num = Convert.ToString(FormCollection["report_num"]);
                model.ReportCreationTime = DateTime.Now;

                model.state_ = (int)LosslessReportStatusEnum.Edit;//报告节点为报告编制
                model.Condition = 0;//报告状态为未开始

                int Finish = (int)LosslessReportStatusEnum.Finish;

                model.tm_id = Convert.ToString(FormCollection["tm_id"]);
                //model.report_format = Convert.ToString(FormCollection["report_format"]);
                //model.report_name = Convert.ToString(FormCollection["report_name"]);
                model.Inspection_personnel = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//报告编制人
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//报告编制人


                int id = 0;
                String flag = Convert.ToString(FormCollection["flag"]); //判断是否为复制的报告 为“1”则复制报告 
                if (flag == "1")
                {

                    id = Convert.ToInt32(FormCollection["id"]);//复制报告信息 需要已完成的报告id
                }

                #region 新报告的url

                //定义报告新文件名
                string new_doc_name = Guid.NewGuid().ToString() + ".doc";
                //定义保存位置Lossless_report_certificate_E
                string save_url = ConfigurationManager.AppSettings["Lossless_report_"].ToString();
                //证书保存路径
                string outputPath = save_url + new_doc_name;

                #endregion

                ReturnDALResult ReturnDALResult = new ReportEditBLL().AddReportBaseInfo(model, flag, Finish, id, outputPath, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }
        #endregion

        #region 模板下拉框
        public JsonResult LoadTemplateCombobox()
        {
            try
            {
                //链接
                List<LosslessComboboxEntityss> mode = new ReportEditBLL().LoadTemplateCombobox();
                return Json(mode);
            }
            catch (Exception e)
            {
                return Json("{'total':0,'rows':[]}");//异常返回无数据
            }
        }

        #endregion

        #region 获取SAp订单号

        public string GetOrderNUM(FormCollection FormCollection)
        {
            try
            {
                string OrderNUM = Convert.ToString(FormCollection["OrderNUM"]);

                string retrun_OrderNUM = "";

                get_interface.phians_webservice_N1SoapClient new_client = new get_interface.phians_webservice_N1SoapClient();
                retrun_OrderNUM = new_client.GetOrderNUM(OrderNUM);

                return JsonConvert.SerializeObject(new ExecuteResult(true, retrun_OrderNUM));

            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 获取mes系统信息

        public string GET_MESlist(FormCollection FormCollection)
        {
            try
            {
                string circulation_NO = Convert.ToString(FormCollection["key"]);

                string return_info = "";

                int page = Convert.ToInt32(FormCollection["page"]);
                int pagesize = Convert.ToInt32(FormCollection["rows"]);

                string order = Convert.ToString(FormCollection["order"]);
                string sortby = Convert.ToString(FormCollection["sort"]);

                MESINTERFACE.MES_INTERFACESoapClient new_client = new MESINTERFACE.MES_INTERFACESoapClient();
                return_info = new_client.GET_MESINFO2(circulation_NO, page, pagesize, order, sortby);

                //throw new Exception("错误");
                //return_info = "{\"total\":20,\"rows\":[{\"WORKID\":\"11065SG-TSD-ST1410-0190\",\"APPLICATION_NUM\":\"11065.AN/001\",\"INSPECTION_CONTEXT\":\"ST14100190$划线及标记移植检查\",\"SUBASSEMBLY_NAME\":\"一次侧接管密封环座\",\"DRAWING_NUM\":\"ACP1000S-SG-DR310003\"},{\"WORKID\":\"11065SG-TSD-ST1410-0190\",\"APPLICATION_NUM\":\"11065.AN/00122\",\"INSPECTION_CONTEXT\":\"ST14100190$划线及标记移植检查\",\"SUBASSEMBLY_NAME\":\"一次侧接管密封环座\",\"DRAWING_NUM\":\"ACP1000S-SG-DR310003\"}]}";


                //return_info = "Exception Message:Index was outside the bounds of the array.";

               // string TaskData = jObject["Data"].ToString();
                try
                {
                    JObject jObject = (JObject)JsonConvert.DeserializeObject(return_info);

                    string MESInfo = jObject["rows"].ToString();
                    List<MESInfo> TaskModel = JsonConvert.DeserializeObject<List<MESInfo>>(MESInfo);
                    string total = jObject["total"].ToString();
                    int total_ = Convert.ToInt32(total);
                    PagedResult<MESInfo> pagelist = new PagedResult<MESInfo>(total_, TaskModel, true);//转换成easyui json
                    var iso = new IsoDateTimeConverter();
                    iso.DateTimeFormat = "yyyy-MM-dd";

                    return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI jso

                }
                catch (Exception)
                {
                    List<MESInfo> list = new List<MESInfo>();
                    PagedResult<MESInfo> pagelist = new PagedResult<MESInfo>(0, list, false, return_info);//转换成easyui json
                    var iso = new IsoDateTimeConverter();
                    iso.DateTimeFormat = "yyyy-MM-dd";

                    return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI jso
                }      


            }
            catch (Exception e)
            {
                List<MESInfo> list = new List<MESInfo>();
                PagedResult<MESInfo> pagelist = new PagedResult<MESInfo>(0, list, false, e.Message);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI jso

            }
        }

     

        #endregion

        #region 根据流转卡号和工序号查方法

        public string Getmethod(FormCollection FormCollection)
        {
            try
            {
                string circulation_NO = Convert.ToString(FormCollection["circulation_NO"]);
                string procedure_NO = Convert.ToString(FormCollection["procedure_NO"]);

                string retrun_OrderNUM = "";

                get_interface.phians_webservice_N1SoapClient new_client = new get_interface.phians_webservice_N1SoapClient();
                retrun_OrderNUM = new_client.Getmethod(circulation_NO, procedure_NO);
                //Response.Write(retrun_OrderNUM);

                return JsonConvert.SerializeObject(new ExecuteResult(true, retrun_OrderNUM));

            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #endregion

        #region ---复制报告

        #region 获取复制报告列表
        public string loadReportCopy(FormCollection FormCollection)//获取列表
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search1"]) ? null : FormCollection["search1"],
                Key = string.IsNullOrEmpty(FormCollection["key1"]) ? null : FormCollection["key1"]
            });

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_report_title> list_model = new ReportEditBLL().loadReportCopy(PageModel, out totalRecord);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion

        #region 回显复制报告信息
        public string ReportCopyShow(FormCollection FormCollection)//获取列表
        {
            string report_num = Convert.ToString(Request.Params.Get("report_num"));
            int state_ = (int)LosslessReportStatusEnum.Scrap;

            TPageModel PageModel = new TPageModel();

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            //报告id
            SearchList.Add(new SearchList { Search = "report_num", Key = report_num });
            SearchList.Add(new SearchList { Search = "state_", Key = state_ });

            PageModel.SearchList_ = SearchList;

            try
            {
                List<TB_NDT_report_title> list_model = new ReportEditBLL().ReportCopyShow(PageModel);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(1, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion

        #endregion

        #region 修改报告信息
        public string EditReportBaseInfo(FormCollection FormCollection)
        {
            try
            {
                TB_NDT_report_title model = new TB_NDT_report_title();
                model.report_num = Convert.ToString(FormCollection["report_num"]);
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.Job_num = Convert.ToString(FormCollection["Job_num"]);
                try
                {
                    int tm_id = Convert.ToInt32(FormCollection["tm_id"]);
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "选择非法模版"));
                }
                model.tm_id = Convert.ToString(FormCollection["tm_id"]);
                //model.report_format = Convert.ToString(FormCollection["report_format"]);
                model.report_name = Convert.ToString(FormCollection["report_name"]);
                model.disable_report_num = Convert.ToString(FormCollection["disable_report_num"]);
                model.Work_instruction = Convert.ToString(FormCollection["Work_instruction"]);
                model.heat_treatment = Convert.ToString(FormCollection["heat_treatment"]);
                model.welding_method = Convert.ToString(FormCollection["welding_method"]);
                model.Tubes_num = Convert.ToString(FormCollection["Tubes_num"]);
                model.Tubes_Size = Convert.ToString(FormCollection["Tubes_Size"]);
                model.figure = Convert.ToBoolean(FormCollection["figure"]);
                model.Inspection_result = Convert.ToString(FormCollection["Inspection_result"]);
                model.clientele = Convert.ToString(FormCollection["clientele"]);
                model.clientele_department = Convert.ToString(FormCollection["clientele_department"]);
                model.application_num = Convert.ToString(FormCollection["application_num"]);
                model.Project_name = Convert.ToString(FormCollection["Project_name"]);
                model.Subassembly_name = Convert.ToString(FormCollection["Subassembly_name"]);
                model.Material = Convert.ToString(FormCollection["Material"]);
                model.Type_ = Convert.ToString(FormCollection["Type_"]);
                model.ModelNum = Convert.ToString(FormCollection["ModelNum"]);
                model.Chamfer_type = Convert.ToString(FormCollection["Chamfer_type"]);
                model.Drawing_num = Convert.ToString(FormCollection["Drawing_num"]);
                model.Procedure_ = Convert.ToString(FormCollection["Procedure_"]);
                model.Inspection_context = Convert.ToString(FormCollection["Inspection_context"]);
                model.Inspection_opportunity = Convert.ToString(FormCollection["Inspection_opportunity"]);
                model.circulation_NO = Convert.ToString(FormCollection["circulation_NO"]);
                model.procedure_NO = Convert.ToString(FormCollection["procedure_NO"]);
                model.apparent_condition = Convert.ToString(FormCollection["apparent_condition"]);
                model.manufacturing_process = Convert.ToString(FormCollection["manufacturing_process"]);
                model.Batch_Num = Convert.ToString(FormCollection["Batch_Num"]);
                model.Inspection_NO = Convert.ToString(FormCollection["Inspection_NO"]);
                model.remarks = Convert.ToString(FormCollection["remarks"]);
                model.Inspection_date = Convert.ToDateTime(FormCollection["Inspection_date"]);

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//报告编制人

                ReturnDALResult ReturnDALResult = new ReportEditBLL().EditReportBaseInfo(model, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 添加检测数据
        public string AddTextData(FormCollection FormCollection)
        {
            try
            {
                string report_num = Convert.ToString(FormCollection["report_num"]);
                string report_name = Convert.ToString(FormCollection["report_name"]);
                string equipment_id = Convert.ToString(FormCollection["equipment_id"]);
                string equipment_name = Convert.ToString(FormCollection["equipment_name"]);
                string equipment_name_R = Convert.ToString(FormCollection["equipment_name_R"]);//label名称s
                DateTime date = DateTime.Now;
                //string personnel = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//报告编制人

                #region 数据获取  (因为需要存储"1,flag"标识,所以不使用直接传model的形式 )
                TB_NDT_test_probereport_data model2 = new TB_NDT_test_probereport_data();

                model2.Data1 = Convert.ToString(FormCollection["Data1"]);
                model2.Data2 = Convert.ToString(FormCollection["Data2"]);
                model2.Data3 = Convert.ToString(FormCollection["Data3"]);
                model2.Data4 = Convert.ToString(FormCollection["Data4"]);
                model2.Data5 = Convert.ToString(FormCollection["Data5"]);
                model2.Data6 = Convert.ToString(FormCollection["Data6"]);
                model2.Data7 = Convert.ToString(FormCollection["Data7"]);
                model2.Data8 = Convert.ToString(FormCollection["Data8"]);
                model2.Data9 = Convert.ToString(FormCollection["Data9"]);
                model2.Data10 = Convert.ToString(FormCollection["Data10"]);
                model2.Data11 = Convert.ToString(FormCollection["Data11"]);
                model2.Data12 = Convert.ToString(FormCollection["Data12"]); ;
                model2.Data13 = Convert.ToString(FormCollection["Data13"]); ;
                model2.Data14 = Convert.ToString(FormCollection["Data14"]);
                model2.Data15 = Convert.ToString(FormCollection["Data15"]);
                model2.Data16 = Convert.ToString(FormCollection["Data16"]); ;
                model2.Data17 = Convert.ToString(FormCollection["Data17"]); ;
                model2.Data18 = Convert.ToString(FormCollection["Data18"]); ;
                model2.Data19 = Convert.ToString(FormCollection["Data19"]);
                model2.Data20 = Convert.ToString(FormCollection["Data20"]);
                model2.Data21 = Convert.ToString(FormCollection["Data21"]); ;
                model2.Data22 = Convert.ToString(FormCollection["Data22"]);
                model2.Data23 = Convert.ToString(FormCollection["Data23"]);
                model2.Data24 = Convert.ToString(FormCollection["Data24"]);
                model2.Data25 = Convert.ToString(FormCollection["Data25"]);
                model2.Data26 = Convert.ToString(FormCollection["Data26"]);
                model2.Data27 = Convert.ToString(FormCollection["Data27"]);
                model2.Data28 = Convert.ToString(FormCollection["Data28"]);
                model2.Data29 = Convert.ToString(FormCollection["Data29"]);
                model2.Data30 = Convert.ToString(FormCollection["Data30"]);
                model2.Data31 = Convert.ToString(FormCollection["Data31"]);
                model2.Data32 = Convert.ToString(FormCollection["Data32"]);
                model2.Data33 = Convert.ToString(FormCollection["Data33"]);
                model2.Data34 = Convert.ToString(FormCollection["Data34"]);
                model2.Data35 = Convert.ToString(FormCollection["Data35"]);
                model2.Data36 = Convert.ToString(FormCollection["Data36"]);
                model2.Data37 = Convert.ToString(FormCollection["Data37"]);
                model2.Data38 = Convert.ToString(FormCollection["Data38"]);
                model2.Data39 = Convert.ToString(FormCollection["Data39"]);
                model2.Data40 = Convert.ToString(FormCollection["Data40"]);
                model2.Data41 = Convert.ToString(FormCollection["Data41"]);
                model2.Data42 = Convert.ToString(FormCollection["Data42"]);
                model2.Data43 = Convert.ToString(FormCollection["Data43"]);
                model2.Data44 = Convert.ToString(FormCollection["Data44"]);
                model2.Data45 = Convert.ToString(FormCollection["Data45"]);
                model2.Data46 = Convert.ToString(FormCollection["Data46"]);
                model2.Data47 = Convert.ToString(FormCollection["Data47"]);
                model2.Data48 = Convert.ToString(FormCollection["Data48"]);
                model2.Data49 = Convert.ToString(FormCollection["Data49"]);
                model2.Data50 = Convert.ToString(FormCollection["Data50"]);
                model2.Data51 = Convert.ToString(FormCollection["Data51"]);
                model2.Data52 = Convert.ToString(FormCollection["Data52"]);
                model2.Data53 = Convert.ToString(FormCollection["Data53"]);
                model2.Data54 = Convert.ToString(FormCollection["Data54"]);
                model2.Data55 = Convert.ToString(FormCollection["Data55"]);
                model2.Data56 = Convert.ToString(FormCollection["Data56"]);
                model2.Data57 = Convert.ToString(FormCollection["Data57"]);
                model2.Data58 = Convert.ToString(FormCollection["Data58"]);
                model2.Data59 = Convert.ToString(FormCollection["Data59"]);
                model2.Data60 = Convert.ToString(FormCollection["Data60"]);
                model2.Data61 = Convert.ToString(FormCollection["Data61"]);
                model2.Data62 = Convert.ToString(FormCollection["Data62"]);
                model2.Data63 = Convert.ToString(FormCollection["Data63"]);
                model2.Data64 = Convert.ToString(FormCollection["Data64"]);

                #endregion

                #region 数据
                model2.report_num = Convert.ToInt32(FormCollection["report_id"]);
                model2.tm_id = Convert.ToString(FormCollection["tm_id"]);
                model2.id = Convert.ToInt32(FormCollection["tp_id"]);

                #endregion

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//报告编制人

                ReturnDALResult ReturnDALResult = new ReportEditBLL().AddTextData(model2, report_num, report_name, date, LogPersonnel, equipment_id, equipment_name, equipment_name_R);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 删除信息
        public string DataDel(FormCollection FormCollection)
        {
            try
            {
                int id = Convert.ToInt32(FormCollection["id"]);
                String report_num = Convert.ToString(FormCollection["report_num"]);


                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//报告编制人

                ReturnDALResult ReturnDALResult = new ReportEditBLL().DataDel(id, report_num, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 载入报告
        public string Filling_report(FormCollection FormCollection)
        {
            try
            {


                int id = Convert.ToInt32(FormCollection["id"]);//报告ID
                string report_num = FormCollection["report_num"];
                string field_fomat = FormCollection["report_format"];//模板格式
                int tm_id = Convert.ToInt32(FormCollection["tm_id"]);

                //定义报告新文件名
                string new_doc_name = Guid.NewGuid().ToString() + ".doc";
                string save_url = ConfigurationManager.AppSettings["Lossless_report_"].ToString();
                //证书保存路径
                string outputPath = Server.MapPath(save_url + new_doc_name);
                //返回的证书路径
                string return_url = save_url + new_doc_name;
                ReturnDALResult ReturnDALResult = new ReportEditBLL().Filling_report(report_num, id, outputPath, tm_id, return_url);

                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, return_url));
                }
                else if (ReturnDALResult.Success == 2)
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region -- 报告附件

        #region 获取报告附件list
        public string load_accessory(FormCollection FormCollection)//获取列表
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortOrder = FormCollection["order"];
            PageModel.SortName = FormCollection["sort"];
            //List<SearchList> SearchList = new List<SearchList>();
            ////下拉框内容
            //SearchList.Add(new SearchList
            //{
            //    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
            //    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            //});
            int report_id = Convert.ToInt32(FormCollection["report_id"]);
            try
            {
                int totalRecord;
                List<TB_NDT_report_accessory> list_model = new ReportEditBLL().GetReportAccessoryListBLL(PageModel, report_id, out totalRecord);
                PagedResult<TB_NDT_report_accessory> pagelist = new PagedResult<TB_NDT_report_accessory>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }


        #endregion

        #region 上传附件
        public string upload_accessory(FormCollection Collection)
        {
            try
            {
                Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                TB_NDT_report_accessory model = new TB_NDT_report_accessory();
                // 获取上传文件文件
                string report_num = Collection["report_num"];
                model.report_id = Collection["report_id"];
                model.accessory_name = Collection["accessory_name"];
                model.remarks = Collection["remarks"];
                model.add_date = DateTime.Now;                    //保存时间
                model.add_personnel = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value); //用户

                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//报告编制人

                //文件保存路径
                model.accessory_url = ConfigurationManager.AppSettings["Lossless_report_accessory"].ToString() + Guid.NewGuid().ToString(); //保存文件名称

                ReturnDALResult ReturnDALResult = new ReportEditBLL().AddFileManagement(report_num, LogPersonnel, files, model, ConfigurationManager.AppSettings["Lossless_report_accessory"].ToString());

                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region 删除报告附件


        public string DelAccessory(FormCollection FormCollection)
        {
            try
            {
                int id = Convert.ToInt32(FormCollection["id"]);
                string report_num = Convert.ToString(FormCollection["report_num"]);

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//报告编制人

                ReturnDALResult ReturnDALResult = new ReportEditBLL().DelAccessoryBLL(id, report_num, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "删除成功"));
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }
        #endregion

        #endregion

        #region 提交审核报告

        public string SubmitEditReport(FormCollection FormCollection)
        {
            try
            {
                TB_NDT_report_title model = new TB_NDT_report_title();
                model.state_ = (int)LosslessReportStatusEnum.Audit;
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.Inspection_personnel = Convert.ToString(FormCollection["Inspection_personnel"]);
                model.Audit_groupid = Convert.ToInt32(FormCollection["group"]);
                model.Condition = 0;//报告状态为未开始

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                #region 判断日期和检验级别是否为空

                //判断日期是否为空
                if (string.IsNullOrEmpty(FormCollection["level_date"]))
                {
                    model.Inspection_personnel_date = DateTime.Now;
                }
                else
                {
                    model.Inspection_personnel_date = Convert.ToDateTime(FormCollection["level_date"]);
                }
                //判断级别是否为空
                if (string.IsNullOrEmpty(FormCollection["level_Inspection"]))
                {
                    model.level_Inspection = "II";
                }
                else
                {
                    model.level_Inspection = Convert.ToString(FormCollection["level_Inspection"]);
                }
                #endregion

                #region 复制的记录文档

                string saveName = Guid.NewGuid().ToString() + ".doc"; //保存文件名称
                string new_report_url = ConfigurationManager.AppSettings["Lossless_report_"].ToString() + saveName;

                #endregion


                ReturnDALResult ReturnDALResult = new ReportEditBLL().SubmitEditReport(model, LogPersonnel, login_user, new_report_url);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 获取设备
        public string GetEquipmentInfo()
        {
            try
            {

                List<ComboboxEntity_ss> EquipmentInfo = new ReportEditBLL().GetEquipmentInfoBLL();
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）

                // return JsonConvert.SerializeObject(new ListExecuteResult<ComboboxEntity_ss>(true, "Success", EquipmentInfo));
                return JsonConvert.SerializeObject(EquipmentInfo);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;

            }

        }


        #endregion

        #region 回显设备信息
        //回显设备信息
        public string GetReportEquipmentInfo(FormCollection FormCollection)
        {

            try
            {

                int id = Convert.ToInt32(FormCollection["id"]);//报告id
                String tm_id = FormCollection["tm_id"];
                string[] info = new string[6];
                List<TB_NDT_test_equipment> EquipmentInfo = new ReportEditBLL().GetReportEquipmentInfoBLL(id);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）

                return JsonConvert.SerializeObject(new ListExecuteResult<TB_NDT_test_equipment>(true, "Success", EquipmentInfo));

            }
            catch (Exception e)
            {
                throw;

            }

        }
        #endregion

        #region 探头信息

        #region 加载试验探头

        public string load_Probe_test(FormCollection FormCollection)
        {


            // int Probe_state = (int)ProbeStateEnum.ZY;
            int Probe_state = 1;
            try
            {
                int pageIndex = Convert.ToInt32(FormCollection["page"]);
                int pagesize = Convert.ToInt32(FormCollection["rows"]);
                string search = Convert.ToString(Request.Params.Get("search"));
                string key = Convert.ToString(Request.Params.Get("key"));
                int totalRecord;
                List<TB_NDT_probe_library> list_model = new ReportEditBLL().GetProbeTestBLL(pageIndex, pagesize, out totalRecord, search, key, Probe_state);
                PagedResult<TB_NDT_probe_library> pagelist = new PagedResult<TB_NDT_probe_library>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 加载已选试验探头

        public string load_Probe_test2(FormCollection FormCollection)
        {
            int report_id = Convert.ToInt32(FormCollection["report_id"]);

            try
            {
                int pageIndex = Convert.ToInt32(FormCollection["page"]);
                int pagesize = Convert.ToInt32(FormCollection["rows"]);
                string search = Convert.ToString(Request.Params.Get("search"));
                string key = Convert.ToString(Request.Params.Get("key"));
                int totalRecord;
                List<TB_NDT_test_probe> list_model = new ReportEditBLL().GetRrportProbeBLL(pageIndex, pagesize, out totalRecord, search, key, report_id);
                PagedResult<TB_NDT_test_probe> pagelist = new PagedResult<TB_NDT_test_probe>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion

        #region 添加试验探头
        public string add_Probe_test(FormCollection FormCollection)
        {
            try
            {


                string probe_id = FormCollection["probe_id"];//探头id
                string report_id = FormCollection["report_id"];
                // String report_num = FormCollection["report_num"];
                ReturnDALResult ReturnDALResult = new ReportEditBLL().add_Probe_testBLL(report_id, probe_id);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }



        }

        #endregion

        #region 删除试验探头
        public string del_Probe_test(FormCollection FormCollection)
        {
            try
            {

                string ID = FormCollection["ID"];//探头id               

                ReturnDALResult ReturnDALResult = new ReportEditBLL().Delete_Probe_testBLL(ID);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }


        }
        #endregion

        #endregion

        #region 试块信息

        #region 加载试块
        /// <summary>
        /// 获取所有试块
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string GetTestBlockLibrary(FormCollection FormCollection)
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            SearchList.Add(new SearchList
            {
                Key = 1//在用状态
            });

            PageModel.SearchList_ = SearchList;
            //int Probe_state = 1;
            try
            {

                int totalRecord;
                List<TB_NDT_TestBlockLibrary> list_model = new ReportEditBLL().GetTestBlockLibrary(PageModel, out totalRecord);
                PagedResult<TB_NDT_TestBlockLibrary> pagelist = new PagedResult<TB_NDT_TestBlockLibrary>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 加载已选试验试块

        public string GetTestTestBlock(FormCollection FormCollection)
        {
            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["ReportID"]) ? null : FormCollection["ReportID"],
                Key = string.IsNullOrEmpty(FormCollection["ProbeID"]) ? null : FormCollection["ProbeID"]
            });

            PageModel.SearchList_ = SearchList;

            try
            {
                int pageIndex = Convert.ToInt32(FormCollection["page"]);
                int pagesize = Convert.ToInt32(FormCollection["rows"]);
                string search = Convert.ToString(Request.Params.Get("search"));
                string key = Convert.ToString(Request.Params.Get("key"));
                int totalRecord;
                List<TB_NDT_TestTestBlock> list_model = new ReportEditBLL().GetTestTestBlock(PageModel, out totalRecord);
                PagedResult<TB_NDT_TestTestBlock> pagelist = new PagedResult<TB_NDT_TestTestBlock>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion

        #region 添加试验试块
        public string Add_TestTestBlock(FormCollection FormCollection)
        {
            try
            {
                string CalBlockID = FormCollection["CalBlockID"];
                string ProbeID = FormCollection["ProbeID"];
                string ReportID = FormCollection["ReportID"];



                ReturnDALResult ReturnDALResult = new ReportEditBLL().Add_TestTestBlock(CalBlockID, ProbeID, ReportID);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }



        }

        #endregion

        #region 删除试验试块
        public string Delete_TestTestBlock(FormCollection FormCollection)
        {
            try
            {

                string ID = FormCollection["ID"];//探头id               
                ReturnDALResult ReturnDALResult = new ReportEditBLL().Delete_TestTestBlock(ID);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }


        }
        #endregion

        #endregion

        #region 查看退回原因
        public string LoadErrorInfo(FormCollection FormCollection)//获取列表
        {
            int id = Convert.ToInt32(Request.Params.Get("id"));

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            //报告id
            SearchList.Add(new SearchList { Search = "report_id", Key = id });
            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_error_log> list_model = new ReportEditBLL().LoadErrorInfo(PageModel, out totalRecord);
                PagedResult<TB_NDT_error_log> pagelist = new PagedResult<TB_NDT_error_log>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 查看修改记录
        public string ReadRecord(FormCollection FormCollection)//获取列表
        {
            int id = Convert.ToInt32(Request.Params.Get("id"));

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            //报告id
            SearchList.Add(new SearchList { Search = "report_id", Key = id });
            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_RevisionsRecord> list_model = new ReportEditBLL().ReadRecord(PageModel, out totalRecord);
                PagedResult<TB_NDT_RevisionsRecord> pagelist = new PagedResult<TB_NDT_RevisionsRecord>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }


        #endregion

        #region 报告编码逻辑
        public string ReportCodingLogic(FormCollection FormCollection)
        {
            try
            {
                string ReportType = Convert.ToString(FormCollection["ReportType"]);//报告类型
                string circulation_NO = Convert.ToString(FormCollection["circulation_NO"]);//流转卡号
                string procedure_NO = Convert.ToString(FormCollection["procedure_NO"]);//工序号

                string report_url = "";

                if (ReportType == "产品理化试验报告")
                {
                    if (circulation_NO == "")
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(false, "流转卡号为空！"));
                    }
                    string JobNum = circulation_NO.Substring(0, 5);//流转卡前五位数——工号
                    string ChapterNum = circulation_NO.Split(new string[] { "-ST" }, StringSplitOptions.RemoveEmptyEntries)[1];//流转卡章节号
                    report_url = "FQ-" + JobNum + "-" + ChapterNum + "-" + procedure_NO;
                }
                else if (ReportType == "工艺评定理化试验报告")
                {
                    if (circulation_NO == "")
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(false, "流转卡号为空！"));
                    }
                    //项目代号[没有]
                    string EvaluateNum = circulation_NO.Split(new string[] { "-TSD-" }, StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 5);//评定编号或评定流水号
                    string ChapterNum = circulation_NO.Split(new string[] { "-ST" }, StringSplitOptions.RemoveEmptyEntries)[1];//.Substring(0,2);//流转卡章节号后2位
                    report_url = "DFHM-GYPD-" + EvaluateNum + "-" + ChapterNum + "-" + procedure_NO;
                }
                else if (ReportType == "工艺试验检验报告")
                {
                    report_url = "DFHM-GYSY-";
                }
                else if (ReportType == "焊材验收检验报告")
                {
                    report_url = "DFHM-HCYS-";
                }
                else if (ReportType == "原材料验收检验报告")
                {
                    report_url = "DFHM-YCL-";
                }
                else if (ReportType == "焊工考试检验报告")
                {
                    report_url = "DFHM-HGKS-";
                }
                else if (ReportType == "工艺临时更改通知单检验报告")
                {
                    report_url = "DFHM-LG-";
                }

                return JsonConvert.SerializeObject(new ExecuteResult(true, report_url));

            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ExecuteResult(false, "请输入正确的流转卡号，或选择正确的报告类型！"));

                //throw;

            }
        }

        #endregion

        #region 编制人员自行退回未开始审核报告
        /// <summary>
        /// 编制人员自己退回未开始审核报告
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string TakeBackEditReport(FormCollection FormCollection)
        {
            try
            {
                TB_NDT_report_title model = new TB_NDT_report_title();
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.state_ = (int)LosslessReportStatusEnum.Edit;
                model.Inspection_personnel = Convert.ToString(FormCollection["Inspection_personnel"]);
                model.Audit_groupid = 0;//审核组置零
                model.Inspection_personnel_date = null;
                model.level_Inspection = null;
                model.Condition = 0;//报告状态为未开始

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                //判断编制人员
                if (model.Inspection_personnel != login_user)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "只有该报告编制人员可以退回报告！"));

                }

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = model.id;
                TB_ProcessRecord.Operator = login_user;
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "TakeBack";
                TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.EditTakeBack;

                #endregion

                ReturnDALResult ReturnDALResult = new ReportEditBLL().TakeBackEditReport(model, LogPersonnel, TB_ProcessRecord);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #endregion

        #region 报告审核

        #region 报告审核view
        /// <summary>
        /// 报告审核view
        /// </summary>
        /// <returns></returns>
        [Authentication]
        public ActionResult ReportReview()
        {
            return View();
        }

        #endregion

        #region 加载报告审核
        public string LoadReportReviewList(FormCollection FormCollection)//获取列表
        {
            string history_flag = Convert.ToString(Request.Params.Get("history_flag"));//history_flag=1 为历史信息； history_flag=0 为待编制信息；

            if (history_flag == "" || history_flag == null)
            {
                return JsonConvert.SerializeObject(new SingleExecuteResult<TB_NDT_report_title>(false, "false", null));
            }

            string loginAccount = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//报告编制人
            Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
            int state_ = (int)LosslessReportStatusEnum.Audit;

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortName = FormCollection["sort"];
            PageModel.SortOrder = FormCollection["order"];

            List<SearchList> SearchList = new List<SearchList>();
            SearchList.Add(new SearchList
            {
                Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            });

            //报告id
            SearchList.Add(new SearchList { Search = "UserId", Key = LogPersonnel });
            SearchList.Add(new SearchList { Search = "state_", Key = state_ });
            SearchList.Add(new SearchList { Search = "history_flag", Key = history_flag });
            //逾期时间判断
            SearchList.Add(new SearchList { Search = "DaySetting", Key = string.IsNullOrEmpty(FormCollection["DaySetting"]) ? null : FormCollection["DaySetting"] });
            SearchList.Add(new SearchList { Search = "loginAccount", Key = loginAccount });//审核人员验证

            PageModel.SearchList_ = SearchList;

            try
            {
                int totalRecord;
                List<TB_NDT_report_title> list_model = new ReportReviewBLL().LoadReportReviewList(PageModel, out totalRecord);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 退回报告编制
        public string BackReviewReport(FormCollection FormCollection)
        {
            try
            {
                #region 报告信息

                TB_NDT_report_title model = new TB_NDT_report_title();
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.return_info = Convert.ToString(FormCollection["return_info"]);
                model.report_url = Convert.ToString(FormCollection["report_url"]);
                model.report_num = Convert.ToString(FormCollection["report_num"]);
                model.Inspection_personnel_date = null;
                model.return_flag = 1;
                model.Audit_personnel = null;
                model.Audit_groupid = 0;
                model.level_Inspection = null;
                model.state_ = (int)LosslessReportStatusEnum.Edit;
                model.Condition = 0;//报告状态为未开始

                #endregion

                #region 错误信息

                string Inspection_personnel = Convert.ToString(FormCollection["Inspection_personnel"]);
                string loginAccount = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                string error_remark = Convert.ToString(FormCollection["error_remarks"]);//错误原因
                List<TB_NDT_error_log> TB_NDT_error_log = new List<TB_NDT_error_log>();

                if (error_remark != "")
                {
                    string[] error_remarks = error_remark.Split(',');
                    for (int i = 0; i < error_remarks.Length; i++)
                    {
                        TB_NDT_error_log model1 = new TB_NDT_error_log();
                        model1.error_remarks = error_remarks[i];
                        model1.constitute_personnel = Inspection_personnel;
                        model1.addpersonnel = loginAccount;
                        model1.add_date = DateTime.Now;
                        model1.report_id = model.id;
                        model1.ReturnNode = "无损报告审核退回";
                        TB_NDT_error_log.Add(model1);
                    }
                }

                #endregion

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = model.id;
                TB_ProcessRecord.Operator = loginAccount;
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "pass";
                TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ReviewToEdit;

                #endregion

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人


                ReturnDALResult ReturnDALResult = new ReportReviewBLL().BackReviewReport(model, LogPersonnel, TB_NDT_error_log, TB_ProcessRecord);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    //发送消息
                    SendMessage new_message = new SendMessage();
                    new_message.client_message(ReturnDALResult.MessagePerson, "你有一个新待编制的报告;报告编号:" + model.report_num);
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 提交审核报告
        public string SubmitReviewReport(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                int tm_id = Convert.ToInt32(FormCollection["tm_ids"]);

                #region 报告信息

                TB_NDT_report_title model = new TB_NDT_report_title();
                model.id = Convert.ToInt32(FormCollection["ids"]);
                // model.state_ = state_;
                model.issue_personnel = Convert.ToString(FormCollection["group"]);
                model.Audit_personnel = login_user;
                model.Condition = 0;//报告状态为未开始

                #region 判断日期和级别是否为空

                //判断日期是否为空
                if (string.IsNullOrEmpty(FormCollection["level_Audit"]))
                {
                    model.level_Audit = "II";
                }
                else
                {
                    model.level_Audit = Convert.ToString(FormCollection["level_Audit"]);
                }
                //判断级别是否为空
                if (string.IsNullOrEmpty(FormCollection["level_date"]))
                {
                    model.Audit_date = DateTime.Now;
                }
                else
                {
                    model.Audit_date = Convert.ToDateTime(FormCollection["level_date"]);
                }
                #endregion

                #endregion

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = model.id;
                TB_ProcessRecord.Operator = login_user;
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "pass";
                //if (model.state_ == 3)
                //{
                //    TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ReviewToIssue;
                //}
                //else if (model.state_ == 4)
                //{
                //    TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ReviewToManage;
                //}
                //else
                //{
                //    return JsonConvert.SerializeObject(new ExecuteResult(false, "tm_id不存在！"));
                //}

                #endregion



                ReturnDALResult ReturnDALResult = new ReportReviewBLL().SubmitReviewReport(model, TB_ProcessRecord, LogPersonnel, tm_id);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region --退回原因

        #region 加载全部退回原因
        public string AllErrorInfo(FormCollection FormCollection)//获取列表
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortOrder = FormCollection["order"];
            PageModel.SortName = FormCollection["sort"];
            //List<SearchList> SearchList = new List<SearchList>();
            ////下拉框内容
            //SearchList.Add(new SearchList
            //{
            //    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
            //    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            //});

            try
            {
                int totalRecord;
                List<TB_DictionaryManagement> list_model = new ReportReviewBLL().AllErrorInfo(PageModel, out totalRecord);
                PagedResult<TB_DictionaryManagement> pagelist = new PagedResult<TB_DictionaryManagement>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 添加退回原因
        public string AddErrorInfo(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string loginAccount = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人
                String report_num = Convert.ToString(FormCollection["report_num"]);

                TB_NDT_error_log model = new TB_NDT_error_log();
                model.report_id = Convert.ToInt32(FormCollection["id"]);
                model.error_remarks = Convert.ToString(FormCollection["error_remarks"]);
                model.constitute_personnel = Convert.ToString(FormCollection["constitute_personnel"]);
                model.add_date = DateTime.Now;
                model.addpersonnel = loginAccount;

                ReturnDALResult ReturnDALResult = new ReportReviewBLL().AddErrorInfo(model, report_num, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }



        }

        #endregion

        #region 加载已选退回原因
        public string ReturnErrorInfo(FormCollection FormCollection)//获取列表
        {

            TPageModel PageModel = new TPageModel();
            PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
            PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
            PageModel.SortOrder = FormCollection["order"];
            PageModel.SortName = FormCollection["sort"];
            //List<SearchList> SearchList = new List<SearchList>();
            ////下拉框内容
            //SearchList.Add(new SearchList
            //{
            //    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
            //    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
            //});

            int report_id = Convert.ToInt32(FormCollection["id"]);//报告id

            try
            {
                int totalRecord;
                List<TB_NDT_error_log> list_model = new ReportReviewBLL().ReturnErrorInfo(PageModel, out totalRecord, report_id);
                PagedResult<TB_NDT_error_log> pagelist = new PagedResult<TB_NDT_error_log>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region 二级还是三级审批
        public string LoadLevel(FormCollection FormCollection)
        {
            try
            {
                int tm_id = Convert.ToInt32(FormCollection["tm_id"]);

                TB_TemplateFile list_model = new ReportEditBLL().LoadTemplateFile(tm_id);

                #region 二级还是三级审批

                //int state_ = 4;
                //if (tm_id == "1")
                //{
                //    state_ = (int)NDE_report_review.Heat_treatment;
                //}
                //if (tm_id == "2")
                //{
                //    state_ = (int)NDE_report_review.VT;
                //}
                //else if (tm_id == "3")
                //{
                //    state_ = (int)NDE_report_review.DT;
                //}
                //else if (tm_id == "4" || tm_id == "5")
                //{
                //    state_ = (int)NDE_report_review.ECT;
                //}
                //else if (tm_id == "6")
                //{
                //    state_ = (int)NDE_report_review.LT;
                //}
                //else if (tm_id == "7" || tm_id == "8")
                //{
                //    state_ = (int)NDE_report_review.MT;
                //}
                //else if (tm_id == "10" || tm_id == "11")
                //{
                //    state_ = (int)NDE_report_review.UT;
                //}
                //else if (tm_id == "12")
                //{
                //    state_ = (int)NDE_report_review.Water_pressure;
                //}
                //else if (tm_id == "26")
                //{
                //    state_ = (int)NDE_report_review.PT;
                //}
                //else if (tm_id == "27")
                //{
                //    state_ = (int)NDE_report_review.RT;
                //}
                //else
                //{
                //    state_ = 4;
                //}
                #endregion

                return JsonConvert.SerializeObject(new ExecuteResult(true, list_model.ReviewLevel.ToString()));
            }
            catch (Exception e)
            {
                throw;

            }

        }
        #endregion

        #region 审核人员自己退回未开始签发的报告
        /// <summary>
        /// 审核人员自己退回未开始签发的报告
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string TakeBackReviewReport(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                #region 报告信息

                TB_NDT_report_title model = new TB_NDT_report_title();
                model.id = Convert.ToInt32(FormCollection["id"]);//报告id
                model.Audit_personnel = Convert.ToString(FormCollection["Inspection_personnel"]);//
                model.state_ = (int)LosslessReportStatusEnum.Audit;
                model.issue_personnel = null;//
                model.Condition = 0;//报告状态为未开始
                model.level_Audit = null;//
                model.Audit_date = null;//

                #endregion

                //判断编制人员
                if (model.Audit_personnel != login_user)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "只有该报告审核人员可以退回报告！"));

                }

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = model.id;
                TB_ProcessRecord.Operator = login_user;
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "TakeBack";
                TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ReviewtTakeBack;

                #endregion

                ReturnDALResult ReturnDALResult = new ReportReviewBLL().TakeBackReviewReport(model, TB_ProcessRecord, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 判断人员资质
        /// <summary>
        /// 判断人员资质
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string JudgingPersonnelQualifications(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                #region 人员资质信息

                TB_PersonnelQualification model = new TB_PersonnelQualification();
                model.TemplateID = Convert.ToInt32(FormCollection["TemplateID"]);//模板id
                model.UserId = LogPersonnel;
                model.AuthorizationType = Convert.ToInt32(FormCollection["AuthorizationType"]);//授权类型（0审核；1签发；2编制）

                #endregion

                ReturnDALResult ReturnDALResult = new ReportReviewBLL().JudgingPersonnelQualifications(model);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #endregion

        #region 报告签发

        #region 报告签发view
        [Authentication]
        public ActionResult ReportIssued()
        {
            return View();
        }

        #endregion

        #region 加载报告签发

        public string LoadReportIssueList(FormCollection FormCollection)//获取列表
        {
            try
            {
                string history_flag = Convert.ToString(Request.Params.Get("history_flag"));//history_flag=1 为历史信息； history_flag=0 为待编制信息；

                if (history_flag == "" || history_flag == null)
                {
                    return JsonConvert.SerializeObject(new SingleExecuteResult<TB_NDT_report_title>(false, "false", null));
                }

                string loginAccount = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//报告编制人
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                int state_ = (int)LosslessReportStatusEnum.Issue;

                TPageModel PageModel = new TPageModel();
                PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
                PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
                PageModel.SortName = FormCollection["sort"];
                PageModel.SortOrder = FormCollection["order"];

                List<SearchList> SearchList = new List<SearchList>();
                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
                });

                //报告id
                SearchList.Add(new SearchList { Search = "state_", Key = state_ });
                SearchList.Add(new SearchList { Search = "history_flag", Key = history_flag });
                SearchList.Add(new SearchList { Search = "Audit_personnel", Key = loginAccount });
                SearchList.Add(new SearchList { Search = "UserId", Key = LogPersonnel });

                PageModel.SearchList_ = SearchList;


                int totalRecord;
                List<TB_NDT_report_title> list_model = new ReportApprovalBLL().LoadReportIssueList(PageModel, out totalRecord);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 退回报告编制
        public string BackIssueReport(FormCollection FormCollection)
        {
            try
            {
                #region 报告信息

                TB_NDT_report_title model = new TB_NDT_report_title();
                model.id = Convert.ToInt32(FormCollection["id"]);
                model.return_info = Convert.ToString(FormCollection["return_info"]);
                model.report_url = Convert.ToString(FormCollection["report_url"]);
                model.report_num = Convert.ToString(FormCollection["report_num"]);
                model.Inspection_personnel_date = null;
                model.return_flag = 1;
                model.Audit_personnel = null;
                model.Audit_groupid = 0;
                model.level_Inspection = null;
                model.issue_personnel = null;
                model.issue_date = null;
                model.level_Audit = null;
                model.Audit_date = null;
                model.state_ = (int)LosslessReportStatusEnum.Edit;
                model.Condition = 0;//报告状态为未开始

                #endregion

                #region 错误信息

                string Inspection_personnel = Convert.ToString(FormCollection["Inspection_personnel"]);
                string loginAccount = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                string error_remark = Convert.ToString(FormCollection["error_remarks"]);//错误原因
                List<TB_NDT_error_log> TB_NDT_error_log = new List<TB_NDT_error_log>();

                if (error_remark != "")
                {
                    string[] error_remarks = error_remark.Split(',');
                    for (int i = 0; i < error_remarks.Length; i++)
                    {
                        TB_NDT_error_log model1 = new TB_NDT_error_log();
                        model1.error_remarks = error_remarks[i];
                        model1.constitute_personnel = Inspection_personnel;
                        model1.addpersonnel = loginAccount;
                        model1.add_date = DateTime.Now;
                        model1.report_id = model.id;
                        model1.ReturnNode = "无损报告签发退回";
                        TB_NDT_error_log.Add(model1);
                    }
                }

                #endregion

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = model.id;
                TB_ProcessRecord.Operator = loginAccount;
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "pass";
                TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.IssueToEdit;

                #endregion

                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人


                ReturnDALResult ReturnDALResult = new ReportApprovalBLL().BackIssueReport(model, LogPersonnel, TB_NDT_error_log, TB_ProcessRecord);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    //发送消息
                    SendMessage new_message = new SendMessage();
                    new_message.client_message(ReturnDALResult.MessagePerson, "你有一个新待编制的报告;报告编号:" + model.report_num);
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 提交签发报告
        public string SubmitIssueReport(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                #region 报告信息

                TB_NDT_report_title model = new TB_NDT_report_title();
                model.id = Convert.ToInt32(FormCollection["ids"]);
                model.state_ = (int)LosslessReportStatusEnum.Finish;
                model.issue_date = DateTime.Now;
                model.issue_personnel = login_user;
                model.Condition = 0;//报告状态为未开始

                #endregion

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = model.id;
                TB_ProcessRecord.Operator = login_user;
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "pass";
                TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.IssueToManage;

                #endregion

                ReturnDALResult ReturnDALResult = new ReportApprovalBLL().SubmitIssueReport(model, TB_ProcessRecord, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #endregion

        #region 报告管理

        #region 报告管理view
        [Authentication]
        public ActionResult ReportManagement()
        {
            return View();
        }

        #endregion

        #region 加载报告管理信息
        public string LoadReportManageList(FormCollection FormCollection)//获取列表
        {
            try
            {
                string loginAccount = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//报告编制人
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                int state_ = (int)LosslessReportStatusEnum.Finish;//完成
                int state_2 = (int)LosslessReportStatusEnum.Abnormal;//异常申请

                TPageModel PageModel = new TPageModel();
                PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
                PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
                PageModel.SortName = FormCollection["sort"];
                PageModel.SortOrder = FormCollection["order"];

                List<SearchList> SearchList = new List<SearchList>();

                #region 搜索条件

                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
                });

                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search1"]) ? null : FormCollection["search1"],
                    Key = string.IsNullOrEmpty(FormCollection["key1"]) ? null : FormCollection["key1"]
                });
                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search2"]) ? null : FormCollection["search2"],
                    Key = string.IsNullOrEmpty(FormCollection["key2"]) ? null : FormCollection["key2"]
                });
                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search3"]) ? null : FormCollection["search3"],
                    Key = string.IsNullOrEmpty(FormCollection["key3"]) ? null : FormCollection["key3"]
                });
                //SearchList.Add(new SearchList
                //{
                //    Search = string.IsNullOrEmpty(FormCollection["search4"]) ? null : FormCollection["search4"],
                //    Key = string.IsNullOrEmpty(FormCollection["key4"]) ? null : FormCollection["key4"]
                //});

                #endregion

                //报告id
                SearchList.Add(new SearchList { Search = "state_", Key = state_ });//报告完成状态
                SearchList.Add(new SearchList { Search = "state_2", Key = state_2 });//异常申请状态

                PageModel.SearchList_ = SearchList;

                int totalRecord;
                List<TB_NDT_report_title> list_model = new ReportManagementBLL().LoadReportManageList(PageModel, out totalRecord);
                PagedResult<TB_NDT_report_title> pagelist = new PagedResult<TB_NDT_report_title>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss:ffff";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 添加线下报告信息
        public string AddUnderLineReportInfo(FormCollection FormCollection)
        {
            try
            {
                TB_NDT_report_title model = new TB_NDT_report_title();
                model.report_num = Convert.ToString(FormCollection["report_num"]);
                model.report_name = Convert.ToString(FormCollection["report_name"]);//报告类别
                model.Job_num = Convert.ToString(FormCollection["Job_num"]);
                model.procedure_NO = Convert.ToString(FormCollection["procedure_NO"]);
                model.circulation_NO = Convert.ToString(FormCollection["circulation_NO"]);
                model.remarks = Convert.ToString(FormCollection["remarks"]);
                model.tm_id = Convert.ToString(FormCollection["tm_id"]);

                model.IsUnderLine = true;//线下报告
                model.ReportCreationTime = DateTime.Now;
                model.state_ = (int)LosslessReportStatusEnum.Finish;//报告节点为报告完成
                model.Condition = 0;//报告状态为未开始
                model.Inspection_personnel = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//报告编制人
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//报告编制人

                #region 线下报告上传

                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files != null && files.Count > 0)
                {
                    string tempPath = ConfigurationManager.AppSettings["Lossless_report_"].ToString();//相对路径

                    string[] Extension = { ".doc", ".docx", ".pdf" };//支持文件格式
                    Resultinfo Resultinfo = new FileOperate().Filesave(files, tempPath, Extension);//保存文件并输出文件信息等
                    if (!Resultinfo.Sucess)
                    {
                        return JsonConvert.SerializeObject(new ExecuteResult(false, Resultinfo.ReturnContent));
                    }
                    model.report_url = Resultinfo.FileInfo[0].FileRelativeUrl;//文件相对路径
                    model.report_format = Resultinfo.FileInfo[0].FileFormat;//文件类型


                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "请上传文件！"));
                }

                #endregion


                ReturnDALResult ReturnDALResult = new ReportManagementBLL().AddUnderLineReportInfo(model, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }
        #endregion

        #region 异常报告申请
        [Authentication]
        public string SubmitAbnormalReport(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                string flag = Convert.ToString(FormCollection["flag"]);//判断是否为报废申请
                string report_num = Convert.ToString(FormCollection["report_num"]);//report_num
                string File_url = Convert.ToString(FormCollection["File_url"]);

                string message = "";

                #region 报告信息
                TB_NDT_report_title model = new TB_NDT_report_title();

                model.id = Convert.ToInt32(FormCollection["report_id"]);
                model.report_num = report_num;
                model.state_ = (int)LosslessReportStatusEnum.Abnormal;//报告异常申请状态

                #endregion

                //报告流程记录
                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                //异常报告信息（错误报告）
                TB_NDT_error_Certificate TB_NDT_error_Certificate = new TB_NDT_error_Certificate();

                #region 拷贝一份报告到异常流程

                //原报告路径
                string reportPath = Server.MapPath(File_url);
                //定义错误报告路径
                string temp_url = ConfigurationManager.AppSettings["Lossless_report_certificate_E"].ToString();
                string file_name = Guid.NewGuid().ToString() + ".doc";
                //错误证书保存位置
                string path = Server.MapPath(temp_url + file_name);
                //hpf.SaveAs(path);

                //复制模板
                try
                {
                    System.IO.File.Copy(reportPath, path, true);
                }
                catch
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "报告已丢失！"));

                }

                #endregion


                if (flag == "Scrap")//报废流程
                {
                    message = "你有一个待审核的异常报废报告;报告编号:" + report_num;

                    #region 添加报告流程记录

                    TB_ProcessRecord.ReportID = model.id;
                    TB_ProcessRecord.Operator = login_user;
                    TB_ProcessRecord.OperateDate = DateTime.Now;
                    TB_ProcessRecord.NodeResult = "pass";
                    TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ManageToBFApply;

                    #endregion

                    #region 添加异常报告信息

                    TB_NDT_error_Certificate.report_id = model.id.ToString();
                    TB_NDT_error_Certificate.File_format = ".doc";
                    TB_NDT_error_Certificate.File_url = temp_url + file_name;
                    TB_NDT_error_Certificate.error_remark = Convert.ToString(FormCollection["error_remark"]);
                    TB_NDT_error_Certificate.accept_personnel = login_user;//申请人
                    TB_NDT_error_Certificate.accept_date = DateTime.Now;
                    TB_NDT_error_Certificate.review_personnel = Convert.ToString(FormCollection["review_personnel"]);//审核人
                    TB_NDT_error_Certificate.other_remarks = Convert.ToString(FormCollection["other_remarks"]);//其他原因
                    TB_NDT_error_Certificate.accept_state = (int)LosslessReport_EditApplyEnum.BFSH;//报废申请
                    TB_NDT_error_Certificate.constitute_personnel = Convert.ToString(FormCollection["constitute_personnel"]);//编制人，为原报告编制人

                    #endregion
                }
                else
                {
                    message = "你有一个审核的异常修改报告;报告编号:" + report_num;

                    #region 添加报告流程记录

                    TB_ProcessRecord.ReportID = model.id;
                    TB_ProcessRecord.Operator = login_user;
                    TB_ProcessRecord.OperateDate = DateTime.Now;
                    TB_ProcessRecord.NodeResult = "pass";
                    TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ManageToApply;

                    #endregion

                    #region 添加报告流程记录

                    TB_NDT_error_Certificate.report_id = model.id.ToString();
                    TB_NDT_error_Certificate.File_format = ".doc";
                    TB_NDT_error_Certificate.File_url = temp_url + file_name;
                    TB_NDT_error_Certificate.error_remark = Convert.ToString(FormCollection["error_remark"]);
                    TB_NDT_error_Certificate.accept_personnel = login_user;//申请人
                    TB_NDT_error_Certificate.accept_date = DateTime.Now;
                    TB_NDT_error_Certificate.review_personnel = Convert.ToString(FormCollection["review_personnel"]);//审核人
                    TB_NDT_error_Certificate.other_remarks = Convert.ToString(FormCollection["other_remarks"]);//其他原因
                    TB_NDT_error_Certificate.accept_state = (int)LosslessReport_EditApplyEnum.SQSH;//申请审核
                    TB_NDT_error_Certificate.constitute_personnel = Convert.ToString(FormCollection["constitute_personnel"]);//编制人，为原报告编制人

                    #endregion

                }

                ReturnDALResult ReturnDALResult = new ReportManagementBLL().SubmitAbnormalReport(model, TB_ProcessRecord, TB_NDT_error_Certificate, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    //发送消息
                    SendMessage new_message = new SendMessage();
                    new_message.client_message(ReturnDALResult.MessagePerson, message);

                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else if (ReturnDALResult.Success == 2)
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 批量下载

        public string Alldownload(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                string DownloadCheck = Convert.ToString(FormCollection["DownloadCheck"]);//下载模式

                string search = Convert.ToString(FormCollection["search"]);//search
                string search1 = Convert.ToString(FormCollection["search1"]);
                string search2 = Convert.ToString(FormCollection["search2"]);
                string search3 = Convert.ToString(FormCollection["search3"]);
                string key = Convert.ToString(FormCollection["key"]);
                string key1 = Convert.ToString(FormCollection["key1"]);
                string key2 = Convert.ToString(FormCollection["key2"]);
                string key3 = Convert.ToString(FormCollection["key3"]);

                string ids = "";

                //下载条件 0选择下载 1搜索下载
                if (DownloadCheck == "0")
                {
                    ids = Convert.ToString(FormCollection["ids"]);//选择的行s

                }

                //临时存放需要复制的文件夹
                string rootUrl = ConfigurationManager.AppSettings["view_temp_Folder"].ToString() + DateTime.Now.ToString("yyyymmddhhmmss");

                //临时存放压缩包
                string savePath = ConfigurationManager.AppSettings["view_temp_Folder"].ToString() + DateTime.Now.ToString("yyyymmdd") + ".zip";

                dynamic model = new ExpandoObject();
                model.search = search;
                model.search1 = search1;
                model.search2 = search2;
                model.search3 = search3;
                model.key = key;
                model.key1 = key1;
                model.key2 = key2;
                model.key3 = key3;
                model.ids = ids;
                model.rootUrl = rootUrl;
                model.savePath = savePath;
                model.DownloadCheck = DownloadCheck;


                ReturnDALResult ReturnDALResult = new ReportManagementBLL().Alldownload(model);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }


        #endregion

        #region 下载报告
        public string download_word(FormCollection FormCollection)
        {
            try
            {
                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人
                string login_user = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value));//操作人

                int id = Convert.ToInt32(FormCollection["id"]);//报告id

                TB_NDT_report_title Model = new ReportManagementBLL().LoadReportInfo(id);
                string sourcefile_Url = Model.report_url;
                string report_num = Model.report_num;//报告编号
                string report_format = Model.report_format;//文件格式

                string PathUrl = ConfigurationManager.AppSettings["view_temp_Folder"].ToString();

                #region 去除报告名称的非法字符

                report_num = report_num.Replace('/', '_');
                report_num = report_num.Replace('\\', '_');
                report_num = report_num.Replace(':', '_');
                report_num = report_num.Replace('*', '_');
                report_num = report_num.Replace('?', '_');
                report_num = report_num.Replace('>', '_');
                report_num = report_num.Replace('<', '_');
                report_num = report_num.Replace('|', '_');

                #endregion

                //拷贝文件(路径+copy的文件,拷贝到的路径+新文件名)
                if (!string.IsNullOrEmpty(sourcefile_Url) || !string.IsNullOrEmpty(report_num))
                {
                    System.IO.File.Copy(Server.MapPath(sourcefile_Url), Server.MapPath(PathUrl + report_num + report_format), true);
                    return JsonConvert.SerializeObject(new ExecuteResult(true, PathUrl + report_num + report_format));
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "失败！"));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion

        #region 导出报告
        public string Report_ExportExcl(FormCollection FormCollection)
        {
            try
            {
                string search3 = FormCollection["search3"];
                string key3 = FormCollection["key3"];
                string search2 = FormCollection["search2"];
                string key2 = FormCollection["key2"];
                string search1 = FormCollection["search1"];
                string key1 = FormCollection["key1"];
                string search = FormCollection["search"];
                string key = FormCollection["key"];

                int type = Convert.ToInt32(FormCollection["type"]);
                string ids = FormCollection["ids"];

                //string tempFileName = "报告导出.xls";
                string tempFilePath = ConfigurationManager.AppSettings["View_Temp_Folder"].ToString();
                ReturnDALResult ReturnResult = new ReportManagementBLL().Report_ExportExcl(search3, key3, search2, key2, search1, key1, search, key, tempFilePath, type, ids);
                if (ReturnResult.Success == 1)
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnResult.returncontent));
                else
                    return JsonConvert.SerializeObject(new ExecuteResult(false, "导出失败"));

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #endregion

        #region =====================  异常报告

        #region 异常报告处理View
        [Authentication]
        public ActionResult AbnormalReportDeal()
        {
            return View();
        }
        #endregion

        #region ====异常申请审核

        #region 异常申请审核View
        [Authentication]
        public ActionResult AbnormalApplyReview()
        {
            return View();
        }
        #endregion

        #region 加载异常申请信息
        /// <summary>
        /// 加载异常申请信息
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string LoadUnusualCertificateList(FormCollection FormCollection)
        {
            try
            {

                ReportReviewBLL ReportReviewBLL = new ReportReviewBLL();
                TPageModel PageModel = new TPageModel();
                PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
                PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
                //Guid UserId = new Guid(Session["UserId"].ToString());

                List<SearchList> SearchList = new List<SearchList>();
                //下拉框内容
                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
                });
                //状态　异常审核状态
                SearchList.Add(new SearchList { Search = "history_flag", Key = FormCollection["history_flag"] });
                //状态　异常审核状态
                SearchList.Add(new SearchList { Search = "accept_state", Key = LosslessReport_EditApplyEnum.SQSH });
                //状态　报废申请审核
                SearchList.Add(new SearchList { Search = "accept_state", Key = LosslessReport_EditApplyEnum.BFSH });
                //用户账号
                SearchList.Add(new SearchList { Search = "UserCount", Key = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value) });


                PageModel.SearchList_ = SearchList;


                int totalRecord;
                List<TB_NDT_error_Certificate> list_model = new AbnormalApplyReviewBLL().LoadUnusualCertificateList(PageModel, out totalRecord);
                PagedResult<TB_NDT_error_Certificate> pagelist = new PagedResult<TB_NDT_error_Certificate>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion

        #region 申请通过审核
        /// <summary>
        /// 申请通过审核
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string PassUnusualApply(FormCollection FormCollection)
        {
            try
            {
                Guid Operator = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));
                //Guid Id = new Guid(FormCollection["Id"]);//选中的id

                int type = Convert.ToInt32(FormCollection["type"]);//操作类型 1通过 0拒绝
                int flag = Convert.ToInt32(FormCollection["accept_state"]);//"0"为报废，"1"为修改


                TB_NDT_error_Certificate Model = new TB_NDT_error_Certificate();
                Model.id = Convert.ToInt32(FormCollection["id"]);
                Model.report_id = FormCollection["report_id"];
                Model.review_personnel = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = Convert.ToInt32(Model.report_id);
                TB_ProcessRecord.Operator = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "pass";

                #endregion


                if (type == 1) //通过
                {
                    if (flag == 0)//报废
                    {
                        Model.accept_state = (int)LosslessReport_EditApplyEnum.BFWC;//报废完成
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.BFApplyToFinish;

                    }
                    else//修改
                    {
                        Model.accept_state = (int)LosslessReport_EditApplyEnum.YCBZ;//异常编制
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ApplyToAbnEdit;
                    }
                }
                else if (type == 0)//不通过
                {
                    Model.false_review_remarks = FormCollection["return_info"];
                    Model.review_remarks = FormCollection["remarks"];
                    //Model.review_personnel = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                    if (flag == 0)//报废
                    {
                        Model.accept_state = (int)LosslessReport_EditApplyEnum.BFTH;//报废拒绝
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.BFApplyToManage;
                    }
                    else//修改
                    {
                        Model.accept_state = (int)LosslessReport_EditApplyEnum.SQTH;//申请拒绝
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.ApplyToManage;
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "请选择通过或退回！"));
                }


                //Model.review_personnel = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                Model.review_date = DateTime.Now;
                ReturnDALResult DALResult = new AbnormalApplyReviewBLL().PassUnusualApplyBLL(Model, TB_ProcessRecord, type, flag, Operator);

                if (DALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "操作成功"));
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, DALResult.returncontent));
                }
            }
            catch (Exception E)
            {
                throw;
            }


        }

        #endregion

        #endregion

        #region ====异常报告编制

        #region 异常报告编制View
        [Authentication]
        public ActionResult AbnormalReportEdit()
        {
            return View();
        }
        #endregion

        #region 加载异常报告
        /// <summary>
        /// 加载异常报告编制页面
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string LoadUnusualCertificateEditList(FormCollection FormCollection)
        {
            try
            {

                ReportReviewBLL ReportReviewBLL = new ReportReviewBLL();
                TPageModel PageModel = new TPageModel();
                PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
                PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
                //Guid UserId = new Guid(Session["UserId"].ToString());

                List<SearchList> SearchList = new List<SearchList>();
                //下拉框内容
                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
                });

                string PageType = FormCollection["PageType"];//页面类型
                string history_flag = FormCollection["history_flag"];//历史还是待
                SearchList.Add(new SearchList { Search = "history_flag", Key = history_flag });
                SearchList.Add(new SearchList { Search = "PageType", Key = PageType });
                switch (PageType)
                {
                    case "Edit"://编制页面 
                        //历史编制
                        if (history_flag == "1")
                        {
                            SearchList.Add(new SearchList { Search = "state0", Key = Convert.ToInt32(LosslessReport_EditApplyEnum.YCSH) });//
                            SearchList.Add(new SearchList { Search = "state1", Key = Convert.ToInt32(LosslessReport_EditApplyEnum.YCWC) });
                        }
                        //待编制
                        else
                        {
                            SearchList.Add(new SearchList { Search = "state0", Key = Convert.ToInt32(LosslessReport_EditApplyEnum.YCBZ) });
                            SearchList.Add(new SearchList { Search = "state1", Key = Convert.ToInt32(LosslessReport_EditApplyEnum.YCTH) });
                        }
                        break;
                    case "Audit"://审核页面
                        SearchList.Add(new SearchList { Search = "state0", Key = Convert.ToInt32(LosslessReport_EditApplyEnum.YCSH) });//
                        break;
                    default:
                        break;
                }
                ////1 历史编制 0
                //SearchList.Add(new SearchList { Search = "history_flag", Key = FormCollection["history_flag"] });
                ////状态 历史 4 6 待编3 5
                //SearchList.Add(new SearchList { Search = "state0", Key = FormCollection["state"] });
                //SearchList.Add(new SearchList { Search = "state1", Key = FormCollection["state"] });
                //用户账号
                SearchList.Add(new SearchList { Search = "UserCount", Key = Convert.ToString(DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value)) });
                PageModel.SearchList_ = SearchList;
                int totalRecord;
                List<TB_NDT_error_Certificate> list_model = new AbnormalReportEditBLL().GetUnusualCertificateListBLL(PageModel, out totalRecord);
                PagedResult<TB_NDT_error_Certificate> pagelist = new PagedResult<TB_NDT_error_Certificate>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion

        #region 异常报告提交
        /// <summary>
        /// 异常报告提交审核 
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string SubmitAbnormalReportReview(FormCollection FormCollection)
        {
            try
            {
                Guid Operator = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));

                TB_NDT_error_Certificate Model = new TB_NDT_error_Certificate();//异常报告表
                TB_NDT_report_title reportModule = new TB_NDT_report_title(); //报告表

                #region 添加报告流程记录
                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.Operator = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "pass";

                #endregion

                string PageType = FormCollection["PageType"];//页面类型

                switch (PageType)
                {
                    case "Edit"://编制页面 
                        Model.id = Convert.ToInt32(FormCollection["id"]);
                        Model.report_id = FormCollection["report_id"];
                        Model.accept_state = (int)LosslessReport_EditApplyEnum.YCSH;
                        Model.constitute_date = DateTime.Now;
                        Model.constitute_personnel = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                        Model.review_personnel_word = FormCollection["review_personnel_word"];
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.AbnEditToAbnReview;
                        break;
                    case "Audit":
                        Model.id = Convert.ToInt32(FormCollection["id"]);
                        Model.report_id = FormCollection["report_id"];
                        Model.accept_state = (int)LosslessReport_EditApplyEnum.YCWC;
                        Model.review_personnel_word_date = DateTime.Now;
                        reportModule.id = Convert.ToInt32(Model.report_id);
                        reportModule.state_ = Convert.ToInt32(LosslessReportStatusEnum.Finish);
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.AbnReviewToAbnManage;
                        break;
                    //case "Issue":
                    //    break;

                    default:
                        break;
                }

                TB_ProcessRecord.ReportID = Convert.ToInt32(Model.report_id); ;

                ReturnDALResult DALResult = new AbnormalReportEditBLL().SubmitAbnormalReportReview(Model, TB_ProcessRecord, reportModule, Operator, PageType);

                if (DALResult.Success == 1)
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(true, "操作成功!"));
                }
                else
                {
                    return JsonConvert.SerializeObject(new ExecuteResult(false, DALResult.returncontent));
                }
            }
            catch (Exception E)
            {
                throw;
            }

        }

        #endregion

        #region 退回报告编制
        public string BackAbnormalReviewReport(FormCollection FormCollection)
        {
            try
            {
                TB_NDT_error_Certificate module = new TB_NDT_error_Certificate();
                module.id = Convert.ToInt32(FormCollection["id"]);
                module.report_id = FormCollection["report_id"];
                module.review_personnel_word_date = DateTime.Now;
                module.accept_state = (int)LosslessReport_EditApplyEnum.YCTH;
                module.review_remarks_word = FormCollection["review_remarks_word"];

                #region 添加报告流程记录

                TB_ProcessRecord TB_ProcessRecord = new TB_ProcessRecord();
                TB_ProcessRecord.ReportID = Convert.ToInt32(module.report_id);
                TB_ProcessRecord.Operator = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                TB_ProcessRecord.OperateDate = DateTime.Now;
                TB_ProcessRecord.NodeResult = "pass";

                #endregion


                Guid LogPersonnel = new Guid(DES_.StringDecrypt(CookiesHelper.GetCookie("UserId").Value));//操作人

                module.review_personnel_word = DES_.StringDecrypt(CookiesHelper.GetCookie("UserCount").Value);
                ReturnDALResult ReturnDALResult = new AbnormalReportEditBLL().BackAbnormalReviewReport(module, TB_ProcessRecord, LogPersonnel);
                //ReturnDALResult.Success==1表示成功（ReturnDALResult.Success==0表示失败）
                if (ReturnDALResult.Success == 1)
                {
                    //发送消息
                    SendMessage new_message = new SendMessage();
                    new_message.client_message(ReturnDALResult.MessagePerson, " 报告编号为：" + module.report_id + "的无损异常报告需要编制;");
                    return JsonConvert.SerializeObject(new ExecuteResult(true, ReturnDALResult.returncontent));
                }
                else
                {

                    return JsonConvert.SerializeObject(new ExecuteResult(false, ReturnDALResult.returncontent));
                }
            }
            catch (Exception e)
            {
                throw;

            }
        }

        #endregion
        #endregion

        #region ====异常报告管理
        [Authentication]
        public ActionResult AbnormalReportManagement()
        {
            return View();
        }

        #region 加载异常报告
        /// <summary>
        /// 加载异常报告编制页面
        /// </summary>
        /// <param name="FormCollection"></param>
        /// <returns></returns>
        public string LoadUnusualCertificateManagementList(FormCollection FormCollection)
        {
            try
            {

                ReportReviewBLL ReportReviewBLL = new ReportReviewBLL();
                TPageModel PageModel = new TPageModel();
                PageModel.PageIndex = Convert.ToInt32(FormCollection["page"]);
                PageModel.Pagesize = Convert.ToInt32(FormCollection["rows"]);
                //Guid UserId = new Guid(Session["UserId"].ToString());

                List<SearchList> SearchList = new List<SearchList>();
                //异常完成状态
                SearchList.Add(new SearchList { Search = "accept_state", Key = Convert.ToInt32(LosslessReport_EditApplyEnum.YCWC) });
                SearchList.Add(new SearchList { Search = "accept_state", Key = Convert.ToInt32(LosslessReport_EditApplyEnum.BFWC) });
                //下拉框内容
                SearchList.Add(new SearchList
                {
                    Search = string.IsNullOrEmpty(FormCollection["search"]) ? null : FormCollection["search"],
                    Key = string.IsNullOrEmpty(FormCollection["key"]) ? null : FormCollection["key"]
                });


                PageModel.SearchList_ = SearchList;
                int totalRecord;
                List<TB_NDT_error_Certificate> list_model = new AbnormalReportManagementBLL().GetUnusualCertificateListBLL(PageModel, out totalRecord);
                PagedResult<TB_NDT_error_Certificate> pagelist = new PagedResult<TB_NDT_error_Certificate>(totalRecord, list_model, true);//转换成easyui json
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd";

                return JsonConvert.SerializeObject(pagelist, iso);//返回easyUI json
            }
            catch (Exception e)
            {
                throw;
            }

        }

        #endregion
        #endregion

        #endregion
   

    }
}
