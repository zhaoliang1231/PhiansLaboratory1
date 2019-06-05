using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class AbnormalApplyReviewDAL : IAbnormalApplyReviewDAL
    {

        #region 加载异常申请信息
        /// <summary>
        /// 加载异常申请信息
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="PageModel">页面传参</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>返回项目信息实体集</returns>
        public List<TB_NDT_error_Certificate> LoadUnusualCertificateList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            //sql.Append("select * from TB_UnusualCertificate WHERE 1=1 ");

            sql.Append(" select uc.*,ui.user_name as review_personnel_n ,ui2.user_name as  accept_personnel_n ,ui3.user_name as constitute_personnel_n ,ui4.user_name as review_personnel_word_n ,TB_NDT_R.report_num,TB_NDT_R.report_name,TB_NDT_R.clientele,TB_NDT_R.clientele_department FROM TB_NDT_error_Certificate as  uc");
            sql.Append("left join tb_user_info as  ui on uc.review_personnel = ui.user_count");
            sql.Append("left join tb_user_info as  ui2 on uc.accept_personnel = ui2.user_count");
            sql.Append("left join tb_user_info as  ui3 on uc.constitute_personnel = ui3.user_count");
            sql.Append("left join TB_NDT_report_title as  TB_NDT_R on TB_NDT_R.id = uc.report_id");
            sql.Append("left join tb_user_info as  ui4 on uc.review_personnel_word = ui4.user_count");
            sql.Append(" where 1=1");


            //sql.Append(" where StatusFlag!=1 ");
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }
            //历史审核
            if (PageModel.SearchList_[1].Key == "1")
            {
                sql.Append("and (uc.accept_state != @0 and uc.accept_state != @1) and uc.review_personnel=@2", PageModel.SearchList_[2].Key, PageModel.SearchList_[3].Key, PageModel.SearchList_[4].Key);

            }
            //待审核
            if (PageModel.SearchList_[1].Key == "0")
            {
                sql.Append("and (uc.accept_state = @0 or uc.accept_state = @1) and uc.review_personnel=@2", PageModel.SearchList_[2].Key, PageModel.SearchList_[3].Key, PageModel.SearchList_[4].Key);

            }

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_NDT_error_Certificate>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }

        }

        #endregion

        #region 申请通过审核
        /// <summary>
        /// 申请通过审核
        /// </summary>
        /// <param name="Model">异常流程记录表</param>
        /// <param name="MTRNO">MTR单号</param>
        /// <param name="AcceptState">异常申请状态</param>
        /// <param name="currentState">当前任务状态</param>
        /// <returns></returns>
        public ReturnDALResult PassUnusualApply(TB_NDT_error_Certificate Model, TB_ProcessRecord TB_ProcessRecord, int type, int flag, Guid Operator, int state_)
        {

            string[] AllInfo = new string[3];// 审核报告
            string OperationType = "";
            string OperationInfo = "";

            TB_NDT_report_title reportTitle = new TB_NDT_report_title();
            reportTitle.id = Convert.ToInt32(Model.report_id);

            if (type == 1) //通过
            {
                if (flag == 0)
                {
                    AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ScrapReportAgree)).Split('%');//报废完成
                    reportTitle.state_ = state_;//完成状态
                    reportTitle.IsScrap = true;//报废状态

                }
                else
                {
                    AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ErrorReportAgree)).Split('%');//异常编制
                    reportTitle.state_ = 5;//异常流程
                    reportTitle.IsScrap = false;//非报废状态
                }
            }
            else//拒绝
            {
                reportTitle.state_ = 4;//完成状态
                if (flag == 1)
                {
                    AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ScrapReportNoAgree)).Split('%');//报废拒绝

                }
                else
                {
                    AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ErrorReportNoAgree)).Split('%');//申请拒绝
                }
            }
            ReturnDALResult ReturnDALResult = new ReturnDALResult();
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from TB_NDT_error_Certificate where accept_state=@0 and Id=@1", Model.accept_state, Model.id);
            using (PetaPoco.Database db = new PetaPoco.Database())
            {


                try
                {
                    db.BeginTransaction();

                    #region 获取报告信息

                    var selectReport = PetaPoco.Sql.Builder;
                    selectReport.Append("select * from TB_NDT_report_title where id = @0 ", Model.report_id);

                    TB_NDT_report_title TB_NDT_report_title = db.FirstOrDefault<TB_NDT_report_title>(selectReport);

                    OperationType = AllInfo[0];
                    OperationInfo = AllInfo[0] + AllInfo[1] + TB_NDT_report_title.report_num;

                    #endregion


                    var UpdateSal = PetaPoco.Sql.Builder;
                    TB_NDT_error_Certificate UnusualCertificatemodel = db.FirstOrDefault<TB_NDT_error_Certificate>(sql);
                    //判断任务是否存在
                    if (UnusualCertificatemodel == null)
                    {

                        //更新状态异常申请 AcceptState
                        string[] UpdateSql = { "review_date", "accept_state", "review_personnel", "false_review_remarks", "review_remarks" };
                        db.Update(Model, UpdateSql);

                        //UpdateSal.Append(" update TB_NDT_error_Certificate set  review_date=@0,accept_state=@1 where Id=@2",Model.review_date,Model.accept_state,Model.id );
                        //db.Execute(UpdateSal);//影响行数

                        string[] reportModuleColumns = { "state_", "IsScrap" };
                        db.Update(reportTitle, reportModuleColumns);

                        #region 添加报告流程记录

                        db.Insert(TB_ProcessRecord);

                        #endregion

                        #region 搜索编制人员的UserId

                        var select = PetaPoco.Sql.Builder;
                        sql.Append("select * from TB_UserInfo where UserCount = @0 ", TB_NDT_report_title.Inspection_personnel);

                        Guid UserId = db.FirstOrDefault<TB_UserInfo>(sql).UserId;

                        #endregion

                        #region 工作消息

                        string Message = "你有一个新待编制的异常报告：" + TB_NDT_report_title.report_num;
                        string MessageType = "待编制异常报告";
                        string CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var InsertMessage = PetaPoco.Sql.Builder;
                        //任务消息
                        InsertMessage.Append("insert into  dbo.TB_Message(UserId, MessageType, Message, CreateTime, PushPersonnel) values (@0,@1,@2,@3,@4)", UserId, MessageType, Message, CreateTime, Operator);
                        db.Execute(InsertMessage);

                        #endregion

                        //添加日志
                        OperationLog operation_log = CommonDAL.operation_log(Operator, OperationType, OperationInfo);
                        db.Insert(operation_log);
                        db.CompleteTransaction();
                        ReturnDALResult.Success = 1;
                    }
                    //任务不在
                    else
                    {
                        db.AbortTransaction();
                        ReturnDALResult.Success = 0;
                        ReturnDALResult.returncontent = "任务不在请刷新页面";
                    }



                }
                catch (Exception E)
                {

                    ReturnDALResult.Success = 2;
                    ReturnDALResult.returncontent = E.ToString();
                    db.AbortTransaction();


                }

            }
            return ReturnDALResult;


        }

        #endregion




    }
}
