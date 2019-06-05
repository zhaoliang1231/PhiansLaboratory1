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
    class OperationUserDAL : IOperationUserDAL
    {
        #region 获取用户资料
        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="id">用户Userid</param>
        /// <returns></returns>
        public TB_UserInfo GetUserByParam(Guid id)
        {
            string sqlStr = "select OT.NodeName,UU.* from dbo.TB_UserInfo as UU left join dbo.TB_User_department as UD on UU.UserId=UD.User_id left join dbo.TB_Organization as OT on UD.Departmentid=OT.NodeId WHERE UU.UserId=@0";//可简化为"where id = @0 and username = @1"

            Sql sql = Sql.Builder.Append(sqlStr, new object[] { id });

            using (var db = DbInstance.CreateDataBase())
            {
                return db.FirstOrDefault<TB_UserInfo>(sql);
            }
        }
        #endregion

        #region 更改信息
        /// <summary>
        /// 更改信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(TB_UserInfo model)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelEditInfo)).Split('%');//"修改人员";
            //string OperationInfo = "人员名称：" + TB_UserInfo.UserName;

            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.UserName;

            dynamic table = new ExpandoObject();
            table.UserId = model.UserId;
            table.UserName = model.UserName;
            table.Tel = model.Tel;
            table.Phone = model.Phone;
            table.Fax = model.Fax;
            table.Email = model.Email;
            table.Postcode = model.Postcode;
            table.QQ = model.QQ;
            table.Address = model.Address;
            table.Remarks = model.Remarks;
            //table.sort_num = model.sort_num;
            table.UserNsex = model.UserNsex;
             table.Profession =   model.Profession;
             table.Job = model.Job;
         
            //table.Position = model.Position;

            bool flag = false;
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //更新用户信息
                    db.Update("TB_UserInfo", "UserId", table);
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.UserId, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table));
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }

        #endregion

        #region 修改密码
        /// <summary>
        ///修改密码
        /// </summary>
        /// <param name="model"> 用户信息model</param>
        /// <returns></returns>
        public int ChangePassword(TB_UserInfo model,string Original_password)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelEditInfo)).Split('%');//"修改人员";
            //string OperationInfo = "人员名称：" + TB_UserInfo.UserName;

            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.UserName;

            dynamic table = new ExpandoObject();
            table.UserId = model.UserId;
            table.UserPwd = model.UserPwd;

            //table.Position = model.Position;

            int flag = 0;



            Sql sql = Sql.Builder.Append("select * from TB_UserInfo  WHERE  UserId=@0 and UserPwd =@1 ", table.UserId, Original_password);

            TB_UserInfo TB_UserInfo;
            using (var db = DbInstance.CreateDataBase())
            {
                TB_UserInfo = db.FirstOrDefault<TB_UserInfo>(sql);
               
            }
            if (TB_UserInfo==null)

                return 1;

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //更新用户信息
                    db.Update("TB_UserInfo", "UserId", table);
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.UserId, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table));
                    db.CompleteTransaction();
                    flag = 2;
                }
                catch (Exception)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }
     

        #endregion

        #region 上传签名
        /// <summary>
        /// 上传签名
        /// </summary>
        /// <param name="model">用户实体</param>
        /// <returns>是否成功bool</returns>
        public bool UpdateUserImg(TB_UserInfo model)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelAddImg)).Split('%');//"修改人员";
            //string OperationInfo = "人员名称：" + TB_UserInfo.UserName;

            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.UserName + " " + model.UserId;

            dynamic table = new ExpandoObject();  
            table.Signature = model.Signature;
            table.UserId = model.UserId;
            bool flag = false;
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    int NodeId = db.Update("TB_UserInfo", "UserId", table);
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.UserId, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table));
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }
        #endregion

        #region 上传头像
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="model">用户实体</param>
        /// <returns>是否成功bool</returns>
        public bool UploadUserPortrait(TB_UserInfo model)
        {
            //日志
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.UploadUserPortrait)).Split('%');

            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.UserName;

            dynamic table = new ExpandoObject();
            table.Portrait = model.Portrait;
            table.UserId = model.UserId;
            bool flag = false;
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    int NodeId = db.Update("TB_UserInfo", "UserId", table);
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.UserId, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }

        #endregion



        #region 获取当前用户的页面权限
        /// <summary>
        /// 获取当前用户的页面权限
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <returns></returns>
        public List<TB_FunctionalModule> GetFunctionalModuleList(Guid UserId)
        {
            string sqlStr = @" 
                WITH  
                 A AS (select PageId from TB_PageAuthorization where GroupId in (
                select GroupId from TB_groupAuthorization where UserId=@0)),
                B AS ( select * from TB_FunctionalModule where ModuleName in ('样品领取','样品接收','报告编制','报告审核','报告签发','异常报告编制','异常报告审核','异常报告签发','培训计划审批','培训计划执行','监督计划审批','监督计划执行')) 
                select B.* from A,B where A.PageId=B.PageId";

            Sql sql = Sql.Builder.Append(sqlStr, new object[] { UserId });

            using (var db = DbInstance.CreateDataBase())
            {
                var result = db.Fetch<TB_FunctionalModule>(sql);
                return result;
            }
        }
        #endregion

        #region 获取当前用户所有代办事项
        /// <summary>
        /// 获取当前用户所有代办事项
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <returns></returns>
        public List<TB_PendingTransaction> GetPendingTransactionList(Guid UserId)
        {
            string sqlStr = @"
                select '样品接收' as TransactionName, COUNT(*) as TransactionCount  from dbo.TB_Motortbaseinfo as TM left join dbo.TB_MTRInfo as TB_MT on TB_MT.MTRNO=TM.MTRNO where MTRState=2
                    UNION
                    select '样品领取' as TransactionName, COUNT(*) as TransactionCount from TB_Motortbaseinfo where SampleStatus=1
                    UNION
                    select '报告编制' as TransactionName, COUNT(*) as TransactionCount from TB_MTRInfo where (MTRState =1 or MTRState =2 or MTRState =3 or MTRState =4 or MTRState =6 or MTRState =8)
                    UNION
                    select '报告审核' as TransactionName, COUNT(*) as TransactionCount from TB_MTRInfo TB_MI left join TB_TotalReport TB_TR ON TB_MI.MTRNO = TB_TR.MTRNo WHERE  TB_MI.MTRState=5 and TB_TR.CheckedBy=@0
                    UNION
                    select '报告签发' as TransactionName, COUNT(*) as TransactionCount from TB_MTRInfo TB_MI left join TB_TotalReport TB_TR ON TB_MI.MTRNO = TB_TR.MTRNo WHERE TB_MI.MTRState=7 and TB_TR.ApprovedBy=@1
                    UNION
                    select '异常报告编制' as TransactionName, COUNT(*) as TransactionCount from TB_UnusualCertificate WHERE (AcceptState=4 or AcceptState=3 or AcceptState=2)
                    UNION
                    select '异常报告审核' as TransactionName, COUNT(*) as TransactionCount from TB_UnusualCertificate WHERE AcceptState=5
                    UNION
                    select '异常报告签发' as TransactionName, COUNT(*) as TransactionCount from TB_UnusualCertificate WHERE AcceptState=6
                    UNION
                    select '培训计划审批' as TransactionName, COUNT(*) as TransactionCount from dbo.TB_TrainingManagement where state=2 and  ApprovedBy=@2
                    UNION
                    select '培训计划执行' as TransactionName, COUNT(*) as TransactionCount from dbo.TB_TrainingManagement where state=3 and  ApprovedBy=@3
                    UNION
                    select '监督计划审批' as TransactionName, COUNT(*) as TransactionCount from TB_SupervisionManagement WHERE state=2 and ReviewPersonnel=@4
                    UNION
                    select '监督计划执行' as TransactionName, COUNT(*) as TransactionCount from TB_SupervisionManagement WHERE state=3 and ReviewPersonnel=@5";
            Sql sql = Sql.Builder.Append(sqlStr, new object[] { UserId, UserId, UserId, UserId, UserId, UserId });
            using (var db = DbInstance.CreateDataBase())
            {
                var result = db.Fetch<TB_PendingTransaction>(sql);
                return result;
            }
        }
        #endregion
    }
}
