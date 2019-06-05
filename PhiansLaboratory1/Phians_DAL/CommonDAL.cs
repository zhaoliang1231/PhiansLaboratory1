using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Phians_Entity.LosslessReport;

namespace Phians_DAL
{
    public class CommonDAL : ICommonDAL
    {
        #region 日志系列

        #region 操作日志
        /// <summary>
        /// 返回系统日志sql
        /// </summary>
        /// <param name="UserId">操作用户ID</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="OperationInfo">操作内容</param>
        /// <returns></returns>
        public static string operation_log_(Guid UserId, string OperationType, string OperationInfo)
        {

            string OperationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string OperationIp = GetIP4Address();


            string sql1 = "insert into TB_OperationLog (UserId,OperationDate,OperationType,OperationInfo,OperationIp) values('" + UserId + "','" + OperationDate + "','" + OperationType + "','" + OperationInfo + "','" + OperationIp + "')";

            return sql1;
        }


        #endregion

        #region 操作日志
        /// <summary>
        /// 返回系统日志sql
        /// </summary>
        /// <param name="UserId">操作用户ID</param>
        /// <param name="OperationType">操作类型</param>
        /// <param name="OperationInfo">操作内容</param>
        /// <returns></returns>
        public static OperationLog operation_log(Guid UserId, string OperationType, string OperationInfo)
        {

            string OperationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string OperationIp = GetIP4Address();
            var insert = PetaPoco.Sql.Builder;
            OperationLog model = new OperationLog();
            model.UserId = UserId;
            model.OperationDate = OperationDate;
            model.OperationIp = OperationIp;
            model.OperationType = OperationType;
            model.OperationInfo = OperationInfo;

            return model;
        }


        #endregion

        #region 获取本地Ip
        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            // foreach (IPAddress IPA in Dns.GetHostAddresses(System.Web.HttpContext.Current.Request.UserHostAddress))
            string hostName = Dns.GetHostName();
            foreach (IPAddress IPA in Dns.GetHostAddresses(hostName))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
        #endregion

        #endregion

        #region 权限相关

        #region 获取表里面的某一个属性的记录
        /// <summary>
        /// 获取表里面的某一个属性的记录
        /// </summary>
        /// <param name="U_url">模块url</param>
        /// <returns>username string</returns>
        public int GetFunctionalModule(string U_url)
        {
            string sqlStr = "select count(*) from TB_FunctionalModule where U_url =@0";//也可以拼接sql

            Sql sql = Sql.Builder.Append(sqlStr, new object[] { U_url });//拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                return db.ExecuteScalar<int>(sql);//db.ExecuteScalar 后的<> 根据你需要的类型获取选择 现在是string的
            }
        }

        #endregion

        #region 检查人员权限

        /// <summary>
        /// //检查人员权限
        /// </summary>
        /// <param name="U_url">模块url</param>
        /// <param name="UserId">用户id</param>
        /// <returns>username string</returns>
        public int GetAuthorization(Guid UserId, string U_url)
        {

            var Get_Authorizatio_Builder = PetaPoco.Sql.Builder;
            Get_Authorizatio_Builder.Append(" SELECT COUNT(*) FROM TB_PageAuthorization AS TB_P LEFT JOIN TB_FunctionalModule AS TB_F ON TB_F.PageId = TB_P.PageId WHERE ");
            Get_Authorizatio_Builder.Append(" TB_F.U_url=@0 and( TB_P.UserId=@1 or TB_P.GroupId IN ( SELECT GroupId FROM TB_groupAuthorization WHERE UserId = @2 ))", U_url, UserId, UserId);
            using (var db = DbInstance.CreateDataBase())
            {
                return db.ExecuteScalar<int>(Get_Authorizatio_Builder);//db.ExecuteScalar 后的<> 根据你需要的类型获取选择 
            }
        }
        #endregion

        #endregion

        #region 消息系列

        #region 注册消息
        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="model">注册消息实体</param>
        /// <returns></returns>
        public bool RegisterSignalR(TB_MessageRegister model)
        {

            bool successflag = false;
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database())
                {

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select *from TB_MessageRegister where UserId =@0 ", model.UserId);
                    TB_MessageRegister model3 = db.FirstOrDefault<TB_MessageRegister>(sql);
                    //判断设备编号是否存在
                    if (model3 != null)
                    {
                        dynamic model2 = new ExpandoObject();
                        model2.RegisterDate = model.RegisterDate;
                        model2.ConnectionId = model.ConnectionId;
                        model2.RegisterFlag = model.RegisterFlag;
                        model2.ID = model3.ID;
                        db.Update("TB_MessageRegister", "ID", model2);
                        successflag = true;

                    }
                    else
                    {
                        db.Insert(model);
                        successflag = true;
                    }


                }
            }
            catch (Exception e)
            {

                throw;
            }


            return successflag;

        }
        #endregion

        #region 获取注册消息
        /// <summary>
        /// 获取注册消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public TB_MessageRegister getRegisterSignalRInfo(Guid UserId)
        {

            TB_MessageRegister model2 = new TB_MessageRegister();
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database())
                {

                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select *from TB_MessageRegister where UserId =@0 ", UserId);
                    model2 = db.FirstOrDefault<TB_MessageRegister>(sql);
                }
            }
            catch (Exception e)
            {

                
            }

            return model2;
        }
        #endregion

        #endregion

        #region 组、人员组合选择系列

        #region 室下拉框
        /// <summary>
        /// 室下拉框
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        public List<ComboboxEntity> LoadRoomCombobox(dynamic TempTable)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select GroupId as 'Value',GroupName as 'Text' ");
            sql.Append(" from TB_group ");
            sql.Append(" WHERE GroupParentId = @0 ", TempTable.GroupId);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                List<ComboboxEntity> model = db.Fetch<ComboboxEntity>(sql);
                return model;
            }
        }
        #endregion

        #region 组下拉框
        /// <summary>
        /// 组下拉框
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        public List<LosslessComboboxEntity> LoadGroupCombobox(dynamic TempTable) 
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select id as 'Value',GroupName as 'Text' ");
            sql.Append(" from TB_group ");
            sql.Append(" WHERE GroupParentId = @0 ", TempTable.GroupId);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                List<LosslessComboboxEntity> model = db.Fetch<LosslessComboboxEntity>(sql);
                return model;
            }
        }
        #endregion

        #region 根据组选择人员
        /// <summary>
        /// 根据组选择人员
        /// </summary>
        /// <param name="TempTable">TempTable.Group = 组id</param>
        /// <returns></returns>
        public List<LosslessUserComboboxEntity> LoadPersonnelCombobox(int id)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select TB_UI.UserCount as Value,TB_UI.UserName as Text");
            sql.Append(" from TB_group TB_G");
            sql.Append(" left join TB_groupAuthorization TB_GA on TB_G.GroupId = TB_GA.GroupId ");
            sql.Append(" left join TB_UserInfo TB_UI on TB_UI.UserId = TB_GA.UserId ");
            sql.Append(" WHERE TB_G.id = @0 ", id);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                List<LosslessUserComboboxEntity> model = db.Fetch<LosslessUserComboboxEntity>(sql);
                return model;
            }
        }
        #endregion

        #region 加载提交CNAS人员下拉框
        /// <summary>
        /// 加载提交CNAS人员下拉框
        /// </summary>
        /// <param name="TempTable">传递信息</param>
        /// <returns></returns>
        public List<ComboboxEntity> LoadCNASPersonnel(dynamic TempTable)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select TB_UI.UserId as 'Value',TB_UI.UserName as 'Text' ");
            sql.Append(" from TB_group as TB_G ");
            sql.Append(" left join TB_groupAuthorization as TB_GA on TB_G.GroupId=TB_GA.GroupId ");
            sql.Append(" left join TB_UserInfo as TB_UI on TB_GA.UserId=TB_UI.UserId ");
            sql.Append(" left join TB_CNASAuthorization as TB_C on TB_C.UserId=TB_UI.UserId ");
            
            sql.Append(" where TB_G.GroupId =@0 ", TempTable.GroupId);//报告组
            sql.Append(" and TB_UI.CountState !=0  ");//排除停用人员
            sql.Append("  and TB_C.CNASLogo=1 ");//CNAS人员

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                List<ComboboxEntity> model = db.Fetch<ComboboxEntity>(sql);
                return model;
            }

        }
        #endregion


        #endregion

        #region 逾期时间下拉框
        /// <summary>
        /// 逾期时间下拉框
        /// </summary>
        /// <param name="Parent_id">逾期时间下拉框 字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntityString> LoadDaySetting(Guid Parent_id)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select Project_value as Value,Project_name as Text  ");
            sql.Append(" from TB_DictionaryManagement where Parent_id=@0", Parent_id);
            sql.OrderBy(" Sort_num ");
            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据listmodel
                List<ComboboxEntityString> model = db.Fetch<ComboboxEntityString>(sql);
                return model;
            }
        }
        #endregion

        #region 报告类型下拉框——报告模板
        /// <summary>
        /// 报告类型下拉框——报告模板
        /// </summary>
        /// <param name="Parent_id">报告类型下拉框 字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntity> LoadReportType(Guid Parent_id)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select id as Value,Project_name as Text  ");
            sql.Append(" from TB_DictionaryManagement where Parent_id=@0", Parent_id);
            sql.OrderBy(" Sort_num ");
            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据listmodel
                List<ComboboxEntity> model = db.Fetch<ComboboxEntity>(sql);
                return model;
            }
        }
        #endregion

        #region 字典 

        #region 字典 获取下拉框
        #region 通用字典获取  
        /// <summary>
        ///  通用字典获取  Project_value  Project_name
        /// </summary>
        /// <param name="Parent_id">字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntityString> GetDictionaryList(Guid Parent_id)
        {
            
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select Project_value  as Value,Project_name as Text  ");
            sql.Append(" from TB_DictionaryManagement where Parent_id=@0", Parent_id);

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据listmodel
                List<ComboboxEntityString> model = db.Fetch<ComboboxEntityString>(sql);
                return model;
            }
        }
        #endregion


        #region 通用字典获取  id  Project_name
        /// <summary>
        ///  通用字典获取  id  Project_name
        /// </summary>
        /// <param name="Parent_id">字典父id</param>
        /// <returns></returns>
        public List<ComboboxEntityString> GetDictionaryListId(Guid Parent_id)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select   cast(id as varchar(40)) as Value,Project_name as Text  ");
            sql.Append(" from TB_DictionaryManagement where Parent_id=@0", Parent_id);

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据listmodel
                List<ComboboxEntityString> model = db.Fetch<ComboboxEntityString>(sql);
                return model;
            }
        }
        #endregion


        #region 通用字典获取  id　获取的字典内容
        /// <summary>
        /// 通用字典获取  id　获取的字典内容
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TB_DictionaryManagement GetDicitionaryContent(Guid ID)
        {
            
            var sql = PetaPoco.Sql.Builder;
          
            sql.Append(" select * from TB_DictionaryManagement where id=@0", ID);

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据listmodel
                TB_DictionaryManagement model = db.FirstOrDefault<TB_DictionaryManagement>(sql);
                return model;
            }
        }
        #endregion

        #endregion

        #endregion

    }
}