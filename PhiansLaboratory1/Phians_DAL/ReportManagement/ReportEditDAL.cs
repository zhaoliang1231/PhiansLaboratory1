using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using Phians_Entity.LosslessReport;
using PhiansCommon.Enum;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class ReportEditDAL : IReportEditDAL
    {
        /// <summary>
        /// 计量数据库
        /// </summary>
        string connectionstring = "pubsConnectionString";

        #region 获取报告编制列表
        /// <summary>
        /// 获取报告编制列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="history_flag">历史信息or待编制信息</param>
        /// <param name="Inspection_personnel">登录账号</param>
        /// <param name="ReportEditStatus">报告编制状态</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_report_title> LoadReportEditList(int pageIndex, int pageSize, out int totalRecord, string search, string key, string history_flag, string Inspection_personnel, int ReportEditStatus)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rt.*, tp.id as tp_id, tp.report_num as tp_report_num, tp.tm_id as tp_tm_id, tp.Data1, tp.Data2, tp.Data3, tp.Data4, tp.Data5, tp.Data6, tp.Data7, tp.Data8, tp.Data9, tp.Data10, tp.Data11, tp.Data12, tp.Data13, tp.Data14, tp.Data15, tp.Data16, tp.Data17, tp.Data18, tp.Data19, tp.Data20, tp.Data21, tp.Data22, tp.Data23, tp.Data24, tp.Data25, tp.Data26, tp.Data27, tp.Data28, tp.Data29, tp.Data30, tp.Data31, tp.Data32, tp.Data33, tp.Data34, tp.Data35, tp.Data36, tp.Data37, tp.Data38, tp.Data39, tp.Data40, tp.Data41, tp.Data42, tp.Data43, tp.Data44, tp.Data45, tp.Data46, tp.Data47, tp.Data48, tp.Data49, tp.Data50, tp.Data51, tp.Data52, tp.Data53, tp.Data54, tp.Data55, tp.Data56, tp.Data57, tp.Data58, tp.Data59, tp.Data60, tp.Data61, tp.Data62, tp.Data63, tp.Data64 ");
            sql.Append(" from TB_NDT_report_title as rt ");
            sql.Append(" left join TB_NDT_test_probereport_data as tp on rt.id = tp.report_num ");
            sql.Append(" where 1=1 ");

            //搜索
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sql.Append(" and rt." + search + " like @0 ", "%" + key + "%");
            }

            //历史信息
            if (history_flag == "1")
            {
                sql.Append(" and rt.state_ !=@0 and rt.Inspection_personnel=@1 ", ReportEditStatus, Inspection_personnel);
                sql.OrderBy(" rt.Inspection_personnel_date desc");
            }
            //待编制信息
            else if (history_flag == "0")
            {
                sql.Append(" and rt.state_ =@0 and rt.Inspection_personnel=@1 ", ReportEditStatus, Inspection_personnel);
            }
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_report_title>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 页面字段显示List
        /// <summary>
        /// 页面字段显示List
        /// </summary>
        /// <param name="PageId">页面id</param>
        /// <returns></returns>
        public List<TB_PageShowCustom> loadPageShowSetting(string PageId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from");
            sql.Append(" TB_PageShowCustom ");
            sql.Append(" where PageId=@0 ", PageId);
            sql.OrderBy(" FieldSort ");

            using (var db = DbInstance.CreateDataBase())
            {
                return db.Fetch<TB_PageShowCustom>(sql);
            }
        }

        #endregion

        #region 模板下拉框
        /// <summary>
        /// 模板下拉框
        /// </summary>
        /// <returns></returns>
        public List<LosslessComboboxEntityss> LoadTemplateCombobox()
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select id as 'Value',FileName as 'Text' ");
            sql.Append(" from TB_TemplateFile where state!=0 ");

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                List<LosslessComboboxEntityss> model = db.Fetch<LosslessComboboxEntityss>(sql);
                return model;
            }
        }

        #endregion

        #region 写入报告信息

        #region 拷贝报告
        /// <summary>
        /// 拷贝报告
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="id">拷贝报告的id </param>
        /// <returns>结果类</returns>
        public string CopyReport(int id)
        {
            string ReturnInfo = "";
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {


                    //判断报告编号是否重复
                    var selectUrl = PetaPoco.Sql.Builder;
                    selectUrl.Append("select * from dbo.TB_NDT_report_title where id= @0", id);
                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(selectUrl);

                    if (TB_NDT_report_title != null)
                    {
                        ReturnInfo = TB_NDT_report_title.report_url;
                    }

                }
                catch (Exception e)
                {
                    throw;

                }
            }
            return ReturnInfo;


        }

        #endregion

        #region 写入报告信息
        /// <summary>
        /// 写入报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="flag">判断是否为复制的报告 为“1”则复制报告 </param>
        /// <returns>结果类</returns>
        public ReturnDALResult AddReportBaseInfo(TB_NDT_report_title model, int Finish, Guid LogPersonnel)
        {
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddReportBaseInfo)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;

            //判断报告编号是否重复
            var exist = PetaPoco.Sql.Builder;
            exist.Append("select * from TB_NDT_report_title where report_num=@0 and IsScrap != 1 ", model.report_num, Finish);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //判断报告编号是否重复
                    if (db.FirstOrDefault<TB_NDT_report_title>(exist) != null)
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "报告编号已经存在！";
                        return ReturnDALResult;

                    }
                    #region 判断是否有资质创建

                    var sqlQualification = PetaPoco.Sql.Builder;
                    sqlQualification.Append("select * from TB_PersonnelQualification");
                    sqlQualification.Append("where TemplateID=(select id from TB_TemplateFile where FileName=@0) and  UserId=@1 and AuthorizationType=2", model.report_name, LogPersonnel);

                    TB_PersonnelQualification PersonnelQualification = db.FirstOrDefault<TB_PersonnelQualification>(sqlQualification);
                    if (PersonnelQualification == null)
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "你没有资质操作！";
                        return ReturnDALResult;
                    }
                    #endregion
                    db.Insert(model);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ReturnDALResult.Success = 1;
                    ReturnDALResult.returncontent = "添加成功！";
                }
                catch (Exception e)
                {
                    db.AbortTransaction();

                    throw;

                }
            }

            return ReturnDALResult;

        }

        #endregion

        #endregion

        #region ---复制报告

        #region 获取复制报告列表
        /// 模板下拉框
        /// </summary>
        /// <returns></returns>
        public List<TB_NDT_report_title> loadReportCopy(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rt.*, tp.id as tp_id, tp.report_num as tp_report_num, tp.tm_id as tp_tm_id, tp.Data1, tp.Data2, tp.Data3, tp.Data4, tp.Data5, tp.Data6, tp.Data7, tp.Data8, tp.Data9, tp.Data10, tp.Data11, tp.Data12, tp.Data13, tp.Data14, tp.Data15, tp.Data16, tp.Data17, tp.Data18, tp.Data19, tp.Data20, tp.Data21, tp.Data22, tp.Data23, tp.Data24, tp.Data25, tp.Data26, tp.Data27, tp.Data28, tp.Data29, tp.Data30, tp.Data31, tp.Data32, tp.Data33, tp.Data34, tp.Data35, tp.Data36, tp.Data37, tp.Data38, tp.Data39, tp.Data40, tp.Data41, tp.Data42, tp.Data43, tp.Data44, tp.Data45, tp.Data46, tp.Data47, tp.Data48, tp.Data49, tp.Data50, tp.Data51, tp.Data52, tp.Data53, tp.Data54, tp.Data55, tp.Data56, tp.Data57, tp.Data58, tp.Data59, tp.Data60, tp.Data61, tp.Data62, tp.Data63, tp.Data64 ");
            sql.Append(" from TB_NDT_report_title as rt ");
            sql.Append(" left join TB_NDT_test_probereport_data as tp on rt.id = tp.report_num ");
            sql.Append(" WHERE 1=1 and rt.IsUnderLine!=1 ");

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                if ((PageModel.SearchList_[0].Search == "Inspection_personnel" || PageModel.SearchList_[0].Search == "Audit_personnel" || PageModel.SearchList_[0].Search == "issue_personnel") && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
                {
                    var select_count = PetaPoco.Sql.Builder;
                    select_count.Append(" select * from TB_UserInfo where UserName =@0 ", PageModel.SearchList_[0].Key);
                    TB_UserInfo TB_UserInfo = DbInstance.CreateDataBase().FirstOrDefault<TB_UserInfo>(select_count);
                    if (TB_UserInfo != null)
                    {
                        if (TB_UserInfo.UserCount == "")
                        {
                            sql.Append(" and 1=0 ");
                        }
                        else
                        {
                            sql.Append(" and rt." + PageModel.SearchList_[0].Search + " like '%" + TB_UserInfo.UserCount + "%' ");
                        }
                    }
                }
                else
                {
                    sql.Append(" and rt." + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
                }

            }

            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search))
            {
                if ((PageModel.SearchList_[1].Search == "Inspection_personnel" || PageModel.SearchList_[1].Search == "Audit_personnel" || PageModel.SearchList_[1].Search == "issue_personnel") && !string.IsNullOrEmpty(PageModel.SearchList_[1].Key))
                {
                    var select_count = PetaPoco.Sql.Builder;
                    select_count.Append(" select * from TB_UserInfo where UserName =@0 ", PageModel.SearchList_[1].Key);
                    TB_UserInfo TB_UserInfo = DbInstance.CreateDataBase().FirstOrDefault<TB_UserInfo>(select_count);
                    if (TB_UserInfo != null)
                    {
                        if (TB_UserInfo.UserCount == "")
                        {
                            sql.Append(" and 1=0 ");
                        }
                        else
                        {
                            sql.Append(" and rt." + PageModel.SearchList_[1].Search + " like '%" + TB_UserInfo.UserCount + "%' ");
                        }
                    }
                }
                else
                {
                    sql.Append(" and rt." + PageModel.SearchList_[1].Search + " like '%" + PageModel.SearchList_[1].Key + "%'");
                }

            }

            sql.OrderBy(" rt.id desc ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_report_title>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }

        }

        #endregion

        #region 回显复制报告信息
        /// 回显复制报告信息
        /// </summary>
        /// <returns></returns>
        public List<TB_NDT_report_title> ReportCopyShow(TPageModel PageModel)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * ");
            sql.Append(" from TB_NDT_report_title ");
            sql.Append(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
            }

            //report_num报告编号
            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[1].Search + " = @0", PageModel.SearchList_[1].Key);

            }

            //state_报告状态
            if (!string.IsNullOrEmpty(PageModel.SearchList_[2].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[2].Search + " != @0", PageModel.SearchList_[2].Key);

            }

            sql.OrderBy(" id desc ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Fetch<TB_NDT_report_title>(sql);
                return result;
            }
        }

        #endregion

        #endregion

        #region 修改报告信息
        /// <summary>
        /// 修改报告信息
        /// </summary>
        /// <param name="model">报告基本信息实体</param>
        /// <param name="LogPersonnel">操作人 </param>
        /// <returns>结果类</returns>
        public ReturnDALResult EditReportBaseInfo(TB_NDT_report_title TB_NDT_report_title, Guid LogPersonnel)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditReportBaseInfo)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + TB_NDT_report_title.report_num;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    string[] updatefiled = { "Job_num", "tm_id", "report_name", "disable_report_num", "Work_instruction", "heat_treatment", "welding_method", "Tubes_num",
                                               "Tubes_Size", "figure", "Inspection_result", "clientele", "clientele_department", "application_num", "Project_name", "Subassembly_name", "Material", 
                                               "Type_", "Chamfer_type", "Drawing_num", "Procedure_", "Inspection_context", "Inspection_opportunity", "circulation_NO", "procedure_NO", "apparent_condition", 
                                               "manufacturing_process", "Batch_Num", "Inspection_NO", "remarks", "Inspection_date" };
                    db.Update(TB_NDT_report_title, updatefiled);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "修改成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    ResultModel.Success = 0;
                    ResultModel.returncontent = E.ToString();
                    throw;

                    //throw;
                }

            }

            return ResultModel;
        }

        #endregion

        #region 添加检测数据
        /// <summary>
        /// 添加检测数据
        /// </summary>
        /// <param name="model">检测数据实体数据</param>
        /// <param name="report_num">报告编号</param>
        /// <param name="report_name">报告名称</param>
        /// <param name="date">添加日期</param>
        /// <param name="LogPersonnel">添加人</param>
        /// <param name="equipment_id">设备id</param>
        /// <param name="equipment_name">设备名称</param>
        /// <param name="equipment_name_R">label名称s</param>
        /// <returns></returns>
        public ReturnDALResult AddTextData(TB_NDT_test_probereport_data model, string report_num, string report_name, DateTime date, Guid LogPersonnel, string equipment_id, string equipment_name, string equipment_name_R)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddTextData)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + report_num;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {

                    db.BeginTransaction();

                    //判断数据是否已经存在，存在则修改，不存在则添加
                    var exist = PetaPoco.Sql.Builder;
                    exist.Append("select * from dbo.TB_NDT_test_probereport_data where report_num = @0 ", model.report_num);

                    if (db.FirstOrDefault<TB_NDT_test_probereport_data>(exist) == null)
                    {
                        db.Insert("TB_NDT_test_probereport_data", "id", true, model);
                    }
                    else
                    {
                        string[] updatefiled = {   "Data1", "Data2", "Data3", "Data4", "Data5", "Data6", "Data7", "Data8", "Data9", 
                                                   "Data10", "Data11", "Data12", "Data13", "Data14", "Data15", "Data16", "Data17", "Data18", "Data19", 
                                                   "Data20", "Data21", "Data22", "Data23", "Data24", "Data25", "Data26", "Data27", "Data28", "Data29",  
                                                   "Data30", "Data31", "Data32", "Data33", "Data34", "Data35", "Data36", "Data37", "Data38", "Data39", 
                                                   "Data40", "Data41", "Data42", "Data43", "Data44", "Data45", "Data46", "Data47", "Data48", "Data49", 
                                                   "Data50", "Data51", "Data52", "Data53", "Data54", "Data55", "Data56", "Data57", "Data58", "Data59",
                                                   "Data60", "Data61", "Data62", "Data63", "Data64" };
                        db.Update(model, updatefiled);
                    }

                    //设备表格信息填入
                    if (model.tm_id != "27")
                    {
                        string[] equipment_ids = equipment_id.Split(',');
                        string[] equipment_names = equipment_name.Split(',');
                        string[] equipment_name_Rs = equipment_name_R.Split(',');

                        for (int j = 0; j < equipment_ids.Length; j++)
                        {
                            //查找该报告是否已经存在该设备
                            var SelectEqupment = PetaPoco.Sql.Builder;
                            SelectEqupment.Append("select * from dbo.TB_NDT_test_equipment where report_id = @0 and equipment_id = @1 and  equipment_name_R = @2 ", model.report_num, equipment_ids[j], equipment_name_Rs[j]);
                            TB_NDT_test_equipment TB_NDT_test_equipment = db.FirstOrDefault<TB_NDT_test_equipment>(SelectEqupment);

                            int id = 0;
                            string sample_type;
                            string asset_num;
                            string measuring_range;
                            string manufacturers;
                            DateTime verification_effective_date;
                            string remarks1;
                            //检索计量台帐库
                            using (var db1 = DbInstance.CreateDataBase(connectionstring))
                            {
                                var SelectEqupmentInfo = PetaPoco.Sql.Builder;
                                SelectEqupmentInfo.Append("select * from dbo.TB_standing_book where id = @0 ", equipment_ids[j]);
                                TB_standing_book TB_standing_book = db1.FirstOrDefault<TB_standing_book>(SelectEqupmentInfo);

                                id = TB_standing_book.id;
                                sample_type = TB_standing_book.sample_type;
                                asset_num = TB_standing_book.asset_num;
                                measuring_range = TB_standing_book.measuring_range;
                                manufacturers = TB_standing_book.manufacturers;
                                verification_effective_date = TB_standing_book.verification_effective_date;
                                remarks1 = TB_standing_book.remarks1;
                            }

                            if (TB_NDT_test_equipment == null)
                            {
                                TB_NDT_test_equipment TB_NDT_test_equipmentTemp = new TB_NDT_test_equipment();
                                TB_NDT_test_equipmentTemp.equipment_id = id.ToString();
                                TB_NDT_test_equipmentTemp.report_id = model.report_num.ToString();
                                TB_NDT_test_equipmentTemp.equipment_name_R = equipment_name_Rs[j];
                                TB_NDT_test_equipmentTemp.equipment_name = equipment_names[j];
                                TB_NDT_test_equipmentTemp.equipment_Type = sample_type;
                                TB_NDT_test_equipmentTemp.equipment_num = asset_num;
                                TB_NDT_test_equipmentTemp.range_ = measuring_range;
                                TB_NDT_test_equipmentTemp.Manufacture = manufacturers;
                                TB_NDT_test_equipmentTemp.effective = verification_effective_date;
                                TB_NDT_test_equipmentTemp.Remarks = remarks1;

                                db.Insert(TB_NDT_test_equipmentTemp);

                            }
                            else
                            {

                                TB_NDT_test_equipment.equipment_id = id.ToString();
                                TB_NDT_test_equipment.report_id = model.report_num.ToString();
                                TB_NDT_test_equipment.equipment_name_R = equipment_name_Rs[j];
                                TB_NDT_test_equipment.equipment_name = equipment_names[j];
                                TB_NDT_test_equipment.equipment_Type = sample_type;
                                TB_NDT_test_equipment.equipment_num = asset_num;
                                TB_NDT_test_equipment.range_ = measuring_range;
                                TB_NDT_test_equipment.Manufacture = manufacturers;
                                TB_NDT_test_equipment.effective = verification_effective_date;
                                TB_NDT_test_equipment.Remarks = remarks1;






                                db.Execute("update dbo.TB_NDT_test_equipment set equipment_id='" + equipment_ids[j] + "', report_id='" + model.report_num + "', equipment_name_R='"
                                    + equipment_name_Rs[j] + "', equipment_name='" + equipment_names[j] + "', equipment_Type='" + sample_type + "', equipment_num='" + asset_num + "', range_='" + measuring_range + "', Manufacture='" + manufacturers + "', effective='" + verification_effective_date + "', Remarks='" + remarks1 + "' where equipment_name_R='" + equipment_name_Rs[j] + "' and report_id='" + model.report_num + "'");

                            }

                        }

                    }


                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "添加/修改成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    ResultModel.Success = 0;
                    ResultModel.returncontent = E.ToString();
                    throw;

                    // throw;
                }

            }

            return ResultModel;
        }

        #endregion

        #region 删除信息
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">删除的id</param>
        /// <param name="report_num">报告编号</param>
        /// <param name="LogPersonnel">操作人</param>
        /// <returns></returns>
        public ReturnDALResult DataDel(int id, string report_num, Guid LogPersonnel)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DataDel)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + report_num;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    db.Delete<TB_NDT_report_title>("where id=@0 ", id);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "删除成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    ResultModel.Success = 0;
                    ResultModel.returncontent = E.ToString();
                    throw;

                    //throw;                    throw;

                }

            }

            return ResultModel;
        }

        #endregion

        #region 获取设备
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <returns></returns>
        public List<ComboboxEntity_ss> GetEquipmentInfo()
        {

            using (var db = DbInstance.CreateDataBase("pubsConnectionString"))
            {
                try
                {
                    var Checksql = Sql.Builder;
                    Checksql.Append("select  id as Value ,asset_num as Text from TB_standing_book where customer_name='质量检验部无损检测室' and (management_state='SJ' or management_state='ZY') ");
                    return db.Fetch<ComboboxEntity_ss>(Checksql);

                }
                catch (Exception E)
                {

                    throw;
                }

            }


        }

        #endregion

        #region 获取报告设备
        /// <summary>
        /// 获取报告设备
        /// </summary>
        /// <returns></returns>
        public List<TB_NDT_test_equipment> GetReportEquipmentInfo(int report_id)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    var Checksql = Sql.Builder;
                    Checksql.Append("select *from TB_NDT_test_equipment where report_id=@0 ", report_id);
                    Checksql.OrderBy("equipment_name_R");

                    return db.Fetch<TB_NDT_test_equipment>(Checksql);

                }
                catch (Exception E)
                {

                    throw;
                }

            }


        }

        #endregion

        #region --探头库

        #region 获取探头库
        /// <summary>
        /// 获取探头库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="Probe_state">探头状态</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_probe_library> GetProbeTest(int pageIndex, int pageSize, out int totalRecord, string search, string key, int Probe_state)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from dbo.TB_NDT_probe_library where Probe_state=@0", Probe_state);
            //搜索
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sql.Append(" and " + search + " like @0 ", "%" + key + "%");
            }
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_probe_library>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 获取已经添加获取探头库
        /// <summary>
        /// 获取已经添加获取探头库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="ReportId">报告ID</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_test_probe> GetRrportProbe(int pageIndex, int pageSize, out int totalRecord, string search, string key, int ReportId)
        {
            var sql = PetaPoco.Sql.Builder;

            sql.Append("select pl.* from dbo.TB_NDT_test_probe as tp left join dbo.TB_NDT_probe_library as pl on tp.probe_id=pl.id where tp.report_id=@0", ReportId);
            //搜索
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sql.Append(" and " + search + " like @0 ", "%" + key + "%");
            }
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_test_probe>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 添加探头到报告
        /// <summary>
        /// 添加探头报报告
        /// </summary>
        /// <param name="ReportId">报告ID</param>
        /// <param name="probe_id">探头ID</param>
        /// <returns></returns>
        public ReturnDALResult add_Probe_test(string ReportId, string probe_id)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                ReturnDALResult ReturnDALResult = new ReturnDALResult();
                //PetaPoco框架自带分页
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_test_probe where report_id=@0 and  probe_id=@1", ReportId, probe_id);
                var result = db.Fetch<TB_NDT_test_probe>(sql);
                if (result.Count > 0)
                {
                    ReturnDALResult.Success = 0;
                    ReturnDALResult.returncontent = "设备已经存在";
                }
                else
                {
                    var probesql = PetaPoco.Sql.Builder;
                    probesql.Append("select * from dbo.TB_NDT_probe_library where id=@0  ", probe_id);
                    TB_NDT_probe_library ProbeLibraryModel = db.FirstOrDefault<TB_NDT_probe_library>(probesql);

                    TB_NDT_test_probe model = new TB_NDT_test_probe();
                    model.probe_id = probe_id;
                    model.report_id = ReportId;
                    model.Probe_name = ProbeLibraryModel.Probe_name;

                    model.Probe_name = ProbeLibraryModel.Probe_name;
                    model.Probe_num = ProbeLibraryModel.Probe_num;
                    model.Probe_type = ProbeLibraryModel.Probe_type;
                    model.Probe_size = ProbeLibraryModel.Probe_size;
                    model.Probe_frequency = ProbeLibraryModel.Probe_frequency;
                    model.Coil_Size = ProbeLibraryModel.Coil_Size;
                    model.Probe_Length = ProbeLibraryModel.Probe_Length;
                    model.Cable_Length = ProbeLibraryModel.Cable_Length;
                    model.Mode_L = ProbeLibraryModel.Mode_L;
                    model.Chip_size = ProbeLibraryModel.Chip_size;
                    model.Angle = ProbeLibraryModel.Angle;
                    model.Nom_Angle = ProbeLibraryModel.Nom_Angle;
                    model.Shoe = ProbeLibraryModel.Shoe;
                    model.Probe_Manufacture = ProbeLibraryModel.Probe_Manufacture;
                    // model.Probe_state = ProbeLibraryModel.Probe_state;
                    model.Mode_T = ProbeLibraryModel.Mode_T;
                    model.DoublePort = ProbeLibraryModel.DoublePort;
                    model.FocalLength = ProbeLibraryModel.FocalLength;
                    model.TFront = ProbeLibraryModel.TFront;
                    model.CurvedSurface = ProbeLibraryModel.CurvedSurface;
                    model.Circumferential = ProbeLibraryModel.Circumferential;
                    model.Position = ProbeLibraryModel.Position;
                    model.Radius = ProbeLibraryModel.Radius;



                    db.Insert(model);
                    ReturnDALResult.Success = 1;
                }
                return ReturnDALResult;
            }
        }

        #endregion

        #region 删除已经添加探头
        /// <summary>
        /// 删除已经添加探头
        /// </summary>
        /// <param name="id">已经选择探头标识</param>     
        /// <returns></returns>
        public ReturnDALResult Delete_Probe_test(string id)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                ReturnDALResult ReturnDALResult = new ReturnDALResult();
                //PetaPoco框架自带分页
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_test_probe where id=@0", id);
                TB_NDT_test_probe result = db.FirstOrDefault<TB_NDT_test_probe>(sql);
                if (result == null)
                {
                    ReturnDALResult.Success = 0;
                    ReturnDALResult.returncontent = "设备已经被删除，不需要再次删除";
                }
                else
                {

                    db.Delete(result);
                    ReturnDALResult.Success = 1;
                }
                return ReturnDALResult;
            }
        }

        #endregion

        #endregion

        #region --试块库

        #region 获取试块库
        /// <summary>
        /// 获取试块库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="state">试块状态</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_TestBlockLibrary> GetTestBlockLibrary(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from dbo.TB_NDT_TestBlockLibrary where State=@0", PageModel.SearchList_[1].Key);
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
            }
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_TestBlockLibrary>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 获取已经添加获取试块库
        /// <summary>
        /// 获取已经添加获取试块库
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <param name="search">搜索字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="ReportId">报告ID</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_TestTestBlock> GetTestTestBlock(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;

            sql.Append("select * from TB_NDT_TestTestBlock where  ReportID=@0 and ProbeID=@1", PageModel.SearchList_[1].Search, PageModel.SearchList_[1].Key);
            //搜索
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like '%" + PageModel.SearchList_[0].Key + "%'");
            }
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_TestTestBlock>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 添加试块到报告
        /// <summary>
        /// 添加探头报报告
        /// </summary>
        /// <param name="ReportId">报告ID</param>
        /// <param name="CalBlockID">试块ID</param>
        /// <param name="ProbeID">探头ID</param>
        /// <returns></returns>
        public ReturnDALResult Add_TestTestBlock(string CalBlockID, string ProbeID, string ReportID)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                ReturnDALResult ReturnDALResult = new ReturnDALResult();
                //PetaPoco框架自带分页
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_TestTestBlock where ReportID=@0 and  ProbeID=@1 and CalBlockID=@2", ReportID, ProbeID, CalBlockID);
                var result = db.Fetch<TB_NDT_TestTestBlock>(sql);
                if (result.Count > 0)
                {
                    ReturnDALResult.Success = 0;
                    ReturnDALResult.returncontent = "试块已经添加";
                }
                else
                {
                    var Probesql = PetaPoco.Sql.Builder;
                    Probesql.Append("select * from dbo.TB_NDT_probe_library where id=@0 ", ProbeID);


                    var CalBlocksql = PetaPoco.Sql.Builder;
                    CalBlocksql.Append("select * from dbo.TB_NDT_TestBlockLibrary where CalBlockID=@0 ", CalBlockID);

                    TB_NDT_probe_library probeModule = db.FirstOrDefault<TB_NDT_probe_library>(Probesql);//探头
                    TB_NDT_TestBlockLibrary CalBlockModule = db.FirstOrDefault<TB_NDT_TestBlockLibrary>(CalBlocksql);//试块

                    if (!string.IsNullOrEmpty(probeModule.id.ToString()) && !string.IsNullOrEmpty(CalBlockModule.ID.ToString()))
                    {
                        TB_NDT_TestTestBlock module = new TB_NDT_TestTestBlock();
                        module.ReportID = Convert.ToInt32(ReportID);
                        module.ProbeID = probeModule.id;
                        module.Angle = probeModule.Angle;
                        module.TFront = probeModule.TFront;
                        module.CalBlockID = CalBlockModule.CalBlockID;
                        module.CalBlockNum = CalBlockModule.CalBlockNum;
                        module.CalBlock = CalBlockModule.CalBlock;
                        module.C_S = CalBlockModule.C_S;
                        module.InstrumentSet = CalBlockModule.InstrumentSet;
                        module.Reflector = CalBlockModule.Reflector;
                        db.Insert(module);
                        ReturnDALResult.Success = 1;
                    }
                    else
                    {
                        ReturnDALResult.Success = 2;
                        ReturnDALResult.returncontent = "探头或试块已经删除！";
                    }
                }
                return ReturnDALResult;
            }
        }

        #endregion

        #region 删除已经添加试块
        /// <summary>
        /// 删除已经添加探头
        /// </summary>
        /// <param name="id">已经选择探头标识</param>     
        /// <returns></returns>
        public ReturnDALResult Delete_TestTestBlock(string id)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                ReturnDALResult ReturnDALResult = new ReturnDALResult();
                //PetaPoco框架自带分页
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_TestTestBlock where id=@0", id);
                TB_NDT_TestTestBlock result = db.FirstOrDefault<TB_NDT_TestTestBlock>(sql);
                if (result == null)
                {
                    ReturnDALResult.Success = 0;
                    ReturnDALResult.returncontent = "试块已经被删除，不需要再次删除";
                }
                else
                {

                    db.Delete(result);
                    ReturnDALResult.Success = 1;
                }
                return ReturnDALResult;
            }
        }

        #endregion

        #endregion

        #region 获取探头数据
        /// <summary>
        /// 获取探头数据
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        public TB_NDT_test_probereport_data Getreport_probe(int id)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页

                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_test_probereport_data where report_num ='" + id + "' ", id);

                var result = db.FirstOrDefault<TB_NDT_test_probereport_data>(sql);

                return result;
            }
        }

        #endregion

        #region 获取报告表头
        /// <summary>
        /// 获取报告表头
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        public TB_NDT_report_title Getreport_title(int id)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页

                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_report_title where id=@0", id);

                var result = db.FirstOrDefault<TB_NDT_report_title>(sql);

                return result;
            }
        }

        #endregion

        #region 获取探头
        /// <summary>
        /// 获取探头
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_test_probe> GetReportProbeLibrary(int id)
        {

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页

                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_test_probe where report_id=@0", id);

                var result = db.Fetch<TB_NDT_test_probe>(sql);

                return result;
            }
        }

        #endregion

        #region 获取试验试块
        /// <summary>
        /// 获取试验试块
        /// </summary>
        /// <param name="id">报告ID</param>     
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_TestTestBlock> GetReportTestTestBlock(int id)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页

                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_TestTestBlock where ReportID=@0", id);

                var result = db.Fetch<TB_NDT_TestTestBlock>(sql);

                return result;
            }

        }
        #endregion

        #region 保存报告Url
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">报告id</param>
        /// <param name="ReportUrl">报告地址</param>
        /// <returns></returns>
        public ReturnDALResult SaveReportUrl(int id, string ReportUrl)
        {
            ReturnDALResult ReturnDALResult_ = new ReturnDALResult();
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页

                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_report_title where id=@0", id);

                TB_NDT_report_title result = db.FirstOrDefault<TB_NDT_report_title>(sql);

                if (result != null)
                {
                    result.report_url = ReportUrl;
                    result.Condition = 1;//报告编制已开始状态
                    string[] dd = { "report_url", "Condition" };
                    db.Update(result, dd);
                    ReturnDALResult_.Success = 1;
                }
                else
                {
                    ReturnDALResult_.Success = 0;
                    ReturnDALResult_.returncontent = "报告不存在";
                }



                return ReturnDALResult_;
            }
        }
        #endregion

        #region 获取附件列表
        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="report_id">报告id</param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_report_accessory> GetReportAccessoryList(TPageModel PageModel, int report_id, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from TB_NDT_report_accessory where 1=1 and report_id=@0", report_id);
            ////查询
            //if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            //{
            //    sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            //}
            //排序
            if (!string.IsNullOrEmpty(PageModel.SortName) && !string.IsNullOrEmpty(PageModel.SortOrder))
            {

                sql.OrderBy(PageModel.SortName + " " + PageModel.SortOrder);
            }

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_report_accessory>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 保存报告附件
        /// <summary>
        /// 保存报告附件
        /// </summary>
        /// <param name="TB_NDT_report_accessory">报告实体内容</param>     
        /// <returns></returns>
        public ReturnDALResult SaveReportAccessory(string report_num, Guid LogPersonnel, TB_NDT_report_accessory model)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.SaveReportAccessory)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + report_num;

            ReturnDALResult ReturnDALResult_ = new ReturnDALResult();
            using (var db = DbInstance.CreateDataBase())
            {

                try
                {
                    db.BeginTransaction();

                    db.Insert(model);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    ReturnDALResult_.Success = 1;
                    ReturnDALResult_.returncontent = "操作成功！";

                    db.CompleteTransaction();

                }
                catch (Exception)
                {
                    db.AbortTransaction();
                    throw;
                }


                return ReturnDALResult_;
            }
        }
        #endregion

        #region 删除报告附件
        /// <summary>
        /// 删除报告附件
        /// </summary>
        /// <param name="AccessoryID">附件ID</param>     
        /// <returns></returns>
        public ReturnDALResult DelAccessory(int AccessoryID, string report_num, Guid LogPersonnel)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelAccessory)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + report_num + AllInfo[2] + AccessoryID;

            ReturnDALResult ReturnDALResult_ = new ReturnDALResult();
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from TB_NDT_report_accessory where id=@0", AccessoryID);
                    TB_NDT_report_accessory model = db.FirstOrDefault<TB_NDT_report_accessory>(sql);

                    if (model == null)
                    {
                        ReturnDALResult_.Success = 0;
                        ReturnDALResult_.returncontent = "文件不存在";
                        return ReturnDALResult_;
                    }

                    db.Delete(model);

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    ReturnDALResult_.Success = 1;
                    ReturnDALResult_.returncontent = model.accessory_url;

                    db.CompleteTransaction();

                }
                catch (Exception)
                {
                    db.AbortTransaction();
                    throw;
                }

            }
            return ReturnDALResult_;

        }
        #endregion

        #region --提交审核报告

        #region 获取用户信息，判断签名是否存在
        /// <summary>
        /// 获取用户信息，判断签名是否存在
        /// </summary>
        /// <param name="login_user">用户</param>     
        /// <returns></returns>
        public TB_user_info getDetection(string login_user)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select top 1 * from dbo.TB_user_info where User_count=@0", login_user);
                return db.FirstOrDefault<TB_user_info>(sql);

            }
        }
        #endregion

        #region 获取报告信息信息
        /// <summary>
        /// 获取报告信息信息
        /// </summary>
        /// <param name="id">报告id</param>     
        /// <returns></returns>
        public TB_NDT_report_title getNDTReportInfo(int id)
        {
            using (var db = DbInstance.CreateDataBase())
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append("select * from dbo.TB_NDT_report_title where id=@0", id);
                return db.FirstOrDefault<TB_NDT_report_title>(sql);

            }
        }
        #endregion

        #region 报告审核
        /// <summary>
        /// 报告审核
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="login_user"></param>
        /// <returns></returns>
        public ReturnDALResult SubmitEditReport(TB_NDT_report_title model, Guid LogPersonnel, string login_user, string new_report_url, TB_NDT_RevisionsRecord TB_NDT_RevisionsRecord, TB_ProcessRecord TB_ProcessRecord, int EditState)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.SubmitEditReport)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;

            DateTime NowDateTime = DateTime.Now;
            model.Inspection_personnel_date = NowDateTime;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    #region 判断报告是否已提交

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from dbo.TB_NDT_report_title where id=@0", model.id);
                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(sql);


                    if (TB_NDT_report_title.state_ != EditState)
                    {
                        db.AbortTransaction();

                        ResultModel.Success = 0;
                        ResultModel.returncontent = "该报告已提交！";
                        return ResultModel;
                    }

                    #endregion

                    #region 更改报告信息

                    model.Condition = 0;//提交到报告审核，报告节点为审核；报告状态未开始
                    //更改报告信息
                    string[] updatefiled = { "level_Inspection", "Inspection_personnel_date", "state_", "Condition", "Audit_groupid" };
                    db.Update(model, updatefiled);

                    #endregion

                    #region 添加报告文档版本记录信息

                    db.Insert(TB_NDT_RevisionsRecord);

                    #endregion

                    #region 添加报告流程记录

                    #region 获取逾期时间设置 && 判断该环节是否逾期

                    //获取报告创建时间
                    var SelectReportInfo = PetaPoco.Sql.Builder;
                    SelectReportInfo.Append("select * from TB_NDT_report_title where id=@0", model.id);//获取报告信息
                    TB_NDT_report_title ReportInfo = db.FirstOrDefault<TB_NDT_report_title>(SelectReportInfo);

                    #region 判断报告是否提交过

                    TB_DictionaryManagement TB_DictionaryManagement = new TB_DictionaryManagement();

                    //获取报告编制提交到审核流程
                    //var SelectReportProcess = PetaPoco.Sql.Builder;
                    //SelectReportProcess.Append("select * from TB_ProcessRecord where ReportID=@0 and NodeId = 0 ", model.id);//获取报告流程中 报告编制提交到报告审核的信息
                    //TB_ProcessRecord ProcessRecord = db.FirstOrDefault<TB_ProcessRecord>(SelectReportProcess);

                    DateTime ReportCreationTime = Convert.ToDateTime(ReportInfo.ReportCreationTime);//报告创建时间


                    //报告初始编制
                    if (TB_NDT_report_title.return_flag == 0)
                    {
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.EditToReview;//第一次从报告编制提交到报告审核
                        //获取字典逾期时间设置
                        var SelectTime = PetaPoco.Sql.Builder;
                        SelectTime.Append("select * from TB_DictionaryManagement where id=@0", "E3C74A69-C31C-4EE0-BD4D-EDA33789D89F");//报告初始编制
                        TB_DictionaryManagement = db.FirstOrDefault<TB_DictionaryManagement>(SelectTime);

                        #region 流程耗时

                        TimeSpan ts1 = NowDateTime - ReportCreationTime;
                        double Days1 = ts1.TotalDays;
                        TB_ProcessRecord.TimeConsuming = (float)Days1;//耗时

                        #endregion
                    }
                    else//报告退回后编制的时间
                    {
                        TB_ProcessRecord.NodeId = (int)TB_ProcessRecordNodeIdEnum.AgainEditToReview;//再次从报告编制提交到报告审核
                        //获取字典逾期时间设置
                        var SelectTime = PetaPoco.Sql.Builder;
                        SelectTime.Append("select * from TB_DictionaryManagement where id=@0", "E653849C-44FD-45A5-8D80-F8059655599A");//报告退回后编制的时间
                        TB_DictionaryManagement = db.FirstOrDefault<TB_DictionaryManagement>(SelectTime);

                        #region 获取流程耗时

                        //获取上一个流程的信息
                        var SelectProcessInfo = PetaPoco.Sql.Builder;
                        SelectProcessInfo.Append("select top 1* from TB_ProcessRecord where ReportID=@0 and (NodeId=@1 or NodeId=@2) ", model.id, (int)TB_ProcessRecordNodeIdEnum.ReviewToEdit, (int)TB_ProcessRecordNodeIdEnum.IssueToEdit);//报告初始编制 上一条为审核退回或签发退回
                        SelectProcessInfo.OrderBy(" OperateDate desc ");
                        TB_ProcessRecord AfProcessRecord = db.FirstOrDefault<TB_ProcessRecord>(SelectProcessInfo);

                        #region 流程耗时

                        TimeSpan ts1 = NowDateTime - AfProcessRecord.OperateDate;
                        double Days1 = ts1.TotalDays;
                        TB_ProcessRecord.TimeConsuming = (float)Days1;//耗时

                        #endregion

                        #endregion

                    }

                    #endregion


                    try
                    {
                        TB_ProcessRecord.OverdueSetup = Convert.ToInt32(TB_DictionaryManagement.Project_value);//逾期时间设置

                        //判断是否逾期
                        if (ReportCreationTime.AddDays(TB_ProcessRecord.OverdueSetup) < NowDateTime)
                        {
                            TB_ProcessRecord.IsOverdue = true;
                            TimeSpan ts = NowDateTime - ReportCreationTime.AddDays(TB_ProcessRecord.OverdueSetup);
                            double Days = ts.TotalDays;
                            TB_ProcessRecord.OverdueTime = (float)Days;//逾期时间
                        }
                        else
                        {
                            TB_ProcessRecord.IsOverdue = false;
                        }

                    }
                    catch (Exception)
                    {
                        db.AbortTransaction();
                        throw;
                    }

                    #endregion

                    db.Insert(TB_ProcessRecord);

                    #endregion

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "提交成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    throw;

                    //throw;
                }

            }

            return ResultModel;
        }

        #endregion

        #endregion

        #region 查看退回原因
        /// <summary>
        /// 查看退回原因
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_error_log> LoadErrorInfo(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select el.*,rt.return_info,ui.User_name as addpersonnel_n from TB_NDT_error_log as el ");
            sql.Append(" left join dbo.TB_NDT_report_title rt on el.report_id=rt.id ");
            sql.Append(" left join dbo.TB_user_info ui on el.addpersonnel=ui.User_count ");
            sql.Append(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }
            //报告编号id
            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[1].Search + "= @0", PageModel.SearchList_[1].Key);
            }

            sql.OrderBy(" el.id desc ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_error_log>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 查看修改记录
        /// <summary>
        /// 查看修改记录
        /// </summary>
        /// <param name="PageModel"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_NDT_RevisionsRecord> ReadRecord(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select rr.*,ui.User_name as addpersonnel_n from TB_NDT_RevisionsRecord rr ");
            sql.Append(" left join dbo.TB_user_info ui on rr.addpersonnel=ui.User_count ");
            sql.Append(" WHERE 1=1 ");

            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }
            //报告编号id
            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search))
            {
                sql.Append(" and rr." + PageModel.SearchList_[1].Search + "= @0", PageModel.SearchList_[1].Key);
            }

            sql.OrderBy(" rr.id desc ");

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_RevisionsRecord>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }

        #endregion

        #region 获取模板信息
        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TB_TemplateFile LoadTemplateFile(int id)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from TB_TemplateFile where id=@0", id);

            using (var db = DbInstance.CreateDataBase())
            {
                return db.FirstOrDefault<TB_TemplateFile>(sql);
            }
        }

        #endregion

        #region 将报告状态更改成已开始
        /// <summary>
        /// 将报告状态更改成已开始
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnDALResult ReportCondition(TB_NDT_report_title model)
        {

            ReturnDALResult ResultModel = new ReturnDALResult();

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    #region 更改报告信息

                    //更改报告信息
                    string[] updatefiled = { "Condition" };
                    db.Update(model, updatefiled);

                    #endregion


                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    throw;
                }

            }

            return ResultModel;
        }

        #endregion

        #region 编制人员自己退回未开始审核报告
        /// <summary>
        /// 编制人员自己退回未开始审核报告
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogPersonnel"></param>
        /// <param name="TB_ProcessRecord"></param>
        /// <returns></returns>
        public ReturnDALResult TakeBackEditReport(TB_NDT_report_title model, Guid LogPersonnel, TB_ProcessRecord TB_ProcessRecord)
        {
            ReturnDALResult ResultModel = new ReturnDALResult();

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.TakeBackEditReport)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.report_num;

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    #region 判断报告是否为审核状态

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from dbo.TB_NDT_report_title where id=@0", model.id);
                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(sql);


                    if (TB_NDT_report_title.state_ != (int)LosslessReportStatusEnum.Audit)//报告审核状态
                    {
                        db.AbortTransaction();

                        ResultModel.Success = 0;
                        ResultModel.returncontent = "该报告不为审核状态！";
                        return ResultModel;
                    }

                    #endregion

                    #region 判断报告开始状态

                    if (TB_NDT_report_title.Condition == 1)//报告为已开始状态
                    {
                        db.AbortTransaction();

                        ResultModel.Success = 0;
                        ResultModel.returncontent = "该报告已经开始审核！";
                        return ResultModel;
                    }

                    #endregion

                    #region 更改报告信息

                    model.Condition = 0;//报告状态未开始；
                    //更改报告信息
                    string[] updatefiled = { "level_Inspection", "Inspection_personnel_date", "state_", "Condition", "Audit_groupid" };
                    db.Update(model, updatefiled);

                    #endregion

                    #region 添加报告流程记录

                    #region 将已有的(报告编制提交到报告审核的)流程更改成（历史的）

                    //获取上一个流程的信息
                    var SelectProcessInfo = PetaPoco.Sql.Builder;
                    SelectProcessInfo.Append("select top 1* from TB_ProcessRecord where ReportID=@0 and (NodeId=@1 or NodeId=@2) ", model.id, (int)TB_ProcessRecordNodeIdEnum.EditToReview, (int)TB_ProcessRecordNodeIdEnum.AgainEditToReview);//拉回到报告编制 上一条为初始报告编制到审核或再次报告编制到审核
                    SelectProcessInfo.OrderBy(" OperateDate desc ");
                    TB_ProcessRecord AfProcessRecord = db.FirstOrDefault<TB_ProcessRecord>(SelectProcessInfo);

                    //保证所有的（TB_ProcessRecordNodeIdEnum.EditToReview）的耗时（自己拉回的耗时算进编制）
                    if (AfProcessRecord.NodeId == (int)TB_ProcessRecordNodeIdEnum.EditToReview)
                    {
                        AfProcessRecord.TakeBack = true;//将上一次提交操作标识为已被自己退回
                    }
                    //保证所有的（TB_ProcessRecordNodeIdEnum.AgainEditToReview）的耗时（自己拉回的耗时算进编制）
                    else if (AfProcessRecord.NodeId == (int)TB_ProcessRecordNodeIdEnum.AgainEditToReview) 
                    {
                        AfProcessRecord.TakeBack = true; //将上一次提交操作标识为已被自己退回
                    }

                    string[] TempUpdate = { "TakeBack" };
                    db.Update(AfProcessRecord, TempUpdate);//更改上一个流程的记录


                    #endregion

                    db.Insert(TB_ProcessRecord);

                    #endregion

                    //添加日志
                    string operation_log_sql = CommonDAL.operation_log_(LogPersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    ResultModel.Success = 1;
                    ResultModel.returncontent = "操作成功！";
                }
                catch (Exception E)
                {
                    db.AbortTransaction();
                    throw;
                }

            }

            return ResultModel;
        }

        #endregion
    }
}
