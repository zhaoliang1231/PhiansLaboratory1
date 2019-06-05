using PetaPoco;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using Phians_Entity.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Phians_DAL
{
    class PersonalManagementDAL : IPersonalManagementDAL
    {

        #region 加载列表
        public List<TB_Organization> GetDepartmentList()
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_Organization ");

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                List<TB_Organization> model = db.Fetch<TB_Organization>(sql);
                return model;
            }
        }
        #endregion

        #region 人员操作

        #region 加载部门人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_UserInfo> GetDepPerList(TPageModel PageModel, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select UI.*,UI2.UserName AS CreatePersonnel_N ");
            sql.Append(" from TB_UserInfo UI  ");
            sql.Append(" LEFT JOIN TB_UserInfo UI2 ON UI.CreatePersonnel=UI2.UserId ");
            sql.Append(" where UI.CountState !=0  ");
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {

                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }

            sql.OrderBy("UI." + PageModel.SortName + " " + PageModel.SortOrder);


            // sql.Append(" ORDER BY @0 @1", PageModel.SortName,PageModel.SortOrder);


            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_UserInfo>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 加载停用人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_UserInfo> GetCancelPerson(TPageModel PageModel, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select UI.*,UI2.UserName AS CreatePersonnel_N ");
            sql.Append(" from TB_UserInfo UI  ");
            sql.Append(" LEFT JOIN TB_UserInfo UI2 ON UI.CreatePersonnel=UI2.UserId ");

            sql.Append(" where UI.CountState=@0  ", 0);
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }

            sql.OrderBy("UI." + PageModel.SortName + " " + PageModel.SortOrder);
            // sql.Append(" ORDER BY @0 @1", PageModel.SortName,PageModel.SortOrder);


            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_UserInfo>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 加载人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="PageModel">条件model</param>     
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_UserInfo> GetPersonnelList(TPageModel PageModel, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select UI.*,UI2.UserName AS CreatePersonnel_N ");
            sql.Append(" from TB_UserInfo UI ");
            //sql.Append(" left join TB_User_department ud on ui.UserId=ud.User_id ");
            sql.Append(" LEFT JOIN TB_UserInfo UI2 ON UI.CreatePersonnel=UI2.UserId ");
            sql.Append(" where 1=1 and UI.CountState !=0 ");
            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search) && !string.IsNullOrEmpty(PageModel.SearchList_[0].Key))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_UserInfo>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 添加人员
        public ReturnDALResult AddPersonnel(TB_UserInfo TB_UserInfo, TB_groupAuthorization TB_groupAuthorization, TB_User_department TB_User_department)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelAdd)).Split('%');// "添加人员";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + TB_UserInfo.UserName;

         
        
            var CheckBuilder = PetaPoco.Sql.Builder;
            CheckBuilder.Append("select * from TB_UserInfo where UserCount=@0 ", TB_UserInfo.UserCount);
            ReturnDALResult Resultmodel = new ReturnDALResult();
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    //添加新人员

                    //将人员添加到组

                    //
                    TB_UserInfo TB_UserInfo2 = new TB_UserInfo();
                    //判断用户是否已经纯在
                    TB_UserInfo2 = db.SingleOrDefault<TB_UserInfo>(CheckBuilder);
                    if (TB_UserInfo2 == null)
                    {
                        Guid UserId = db.GetGuid(db.Insert(TB_UserInfo));//添加人员
                        TB_User_department.User_id = UserId;
                        db.Insert(TB_User_department);//添加部门
                        TB_groupAuthorization.UserId = UserId;
                        db.Insert(TB_groupAuthorization);//添加人员到组
                       
                        //系统日志
                        string operation_log_sql = CommonDAL.operation_log_(TB_UserInfo.CreatePersonnel, OperationType, OperationInfo);
                        int id = Convert.ToInt32(db.Execute(operation_log_sql));
                        Resultmodel.Success = 1;
                    }
                    else
                    {
                        Resultmodel.Success = 0;
                        Resultmodel.returncontent = "该账号已经被使用！";
                    }

                    db.CompleteTransaction();

                }
                catch (Exception e)
                {
                    Resultmodel.Success = 2;
                    Resultmodel.returncontent = e.ToString(); ;
                    db.AbortTransaction();
                }
            }
            return Resultmodel;

        }

        #endregion

        #region 修改人员
        public bool EditPersonnel(TB_UserInfo TB_UserInfo, dynamic TempTable)
        {
            dynamic table = new ExpandoObject();

            table.UserId = TB_UserInfo.UserId;
            table.UserName = TB_UserInfo.UserName;
            table.UserNsex = TB_UserInfo.UserNsex;
            table.Tel = TB_UserInfo.Tel;
            table.Phone = TB_UserInfo.Phone;
            table.Fax = TB_UserInfo.Fax;
            table.Email = TB_UserInfo.Email;
            table.Postcode = TB_UserInfo.Postcode;
            table.QQ = TB_UserInfo.QQ;
            table.Address = TB_UserInfo.Address;
            table.Remarks = TB_UserInfo.Remarks;
            table.JobNum = TB_UserInfo.JobNum;
            table.Profession = TB_UserInfo.Profession;
            table.Job = TB_UserInfo.Job;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelEdit)).Split('%');//"修改人员";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + TB_UserInfo.UserName;

            using (var db = DbInstance.CreateDataBase())
            {
                int temp = 0;
                try
                {
                    db.BeginTransaction();

                    //修改人员信息
                    temp = db.Update("TB_UserInfo", "UserId", table);
                        
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(TempTable.CreatePersonnel, OperationType, OperationInfo);
                    int id = Convert.ToInt32(db.Execute(operation_log_sql));
               
                  

                    db.CompleteTransaction();
                    //flag = true;
                }
                catch (Exception e)
                {
                    temp = 0;
                    db.AbortTransaction();
                }
                if (temp > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }


        }

        #endregion

        #region 人员停用/启用
        public bool DelPersonnel(Guid UserId, string UserName, Guid CreatePersonnel, int del_flag)
        {
            bool flag = false;

            string[] AllInfo = new string[10];
            string OperationType = "";
            string OperationInfo = "";

            var sql = PetaPoco.Sql.Builder;
            if (del_flag == 1)//del_flag 为1 将人员状态改为1 启用  ； del_flag 为0 将人员状态改为0 停用  ；
            {
                sql.Append("update TB_UserInfo set CountState=1 where UserId=@0 ", UserId);
                AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelEnable)).Split('%');//"启用人员";
                OperationType = AllInfo[0];
                OperationInfo = AllInfo[1] + UserName;
            }
            else if (del_flag == 0)
            {
                sql.Append("update TB_UserInfo set CountState=0 where UserId=@0 ", UserId);
                AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelDisable)).Split('%');//"停用人员";
                OperationType = AllInfo[0];
                OperationInfo = AllInfo[1] + UserName;
            }
            else
            {
                return flag;
            }

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    //人员停用/启用
                    string num = db.Execute(sql).ToString();

                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(CreatePersonnel, OperationType, OperationInfo);
                    int id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }

        #endregion

        #region 人员重置密码
        public bool ResetPerPwd(Guid UserId, string UserName, Guid CreatePersonnel, string UserPwd)
        {
            bool flag = false;
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.ResetPerPwd)).Split('%');//"启用人员";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + UserName;

            string insertSql = "update TB_UserInfo set UserPwd=@0 where UserId=@1";//也可以拼接sql

            Sql sql = Sql.Builder.Append(insertSql, new object[] { UserPwd, UserId });//拼接sql 这一步就不需要了

            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    //人员停用/启用
                    string num = db.Execute(sql).ToString();

                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(CreatePersonnel, OperationType, OperationInfo);
                    int id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
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
        public bool UpdateUserImg(dynamic model)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonnelAddImg)).Split('%');//"修改人员";
            //string OperationInfo = "人员名称：" + TB_UserInfo.UserName;

            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.UserName + " " + model.UserId2;

            dynamic table = new ExpandoObject();
            table.Signature = model.Signature;
            table.UserId = model.UserId2;
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

        #region 上传头像

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="model">用户实体</param>
        /// <returns>是否成功bool</returns>
        public bool UploadUserPortrait(dynamic model)
        {
            //日志
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.UploadUserPortrait)).Split('%');

            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.UserName + " " + model.UserId;

            dynamic table = new ExpandoObject();
            table.Portrait = model.Portrait;
            table.UserId = model.UserId2;
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
     
        #endregion

        #region  删除人员
        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="Model">用户model</param>
        /// <returns></returns>
        public ReturnDALResult DeletePerson(TB_UserInfo Model)
        {
            ReturnDALResult Returnmodel = new ReturnDALResult();
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    //人员删除
                    int num = db.Delete(Model);
                    if (num > 0)
                    {
                        Returnmodel.Success = 1;
                       
                    }
                    else
                    {
                        Returnmodel.Success = 0;
                        Returnmodel.returncontent = "OPerare Fail";
                     
                    }
                    db.CompleteTransaction();

                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                    Returnmodel.Success = 0;
                    Returnmodel.returncontent = e.ToString();
                }
                return Returnmodel;
            }
        }
           #endregion

        #region 组操作
        #region 加载组树
        public List<TB_group> GetGroupTree(string GroupParentId)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_group ");
            sql.OrderBy(" id ");

            using (var db = DbInstance.CreateDataBase())
            {
                //返回多条数据model
                List<TB_group> model = db.Fetch<TB_group>(sql);
                return model;
            }
        }
        #endregion

        #region 组信息加载——回显
        public TB_group LoadGroupInfo(Guid GroupId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_group ");
            sql.Append(" where GroupId=@0  ", GroupId);

            using (var db = DbInstance.CreateDataBase())
            {
                //model
                TB_group model = db.SingleOrDefault<TB_group>(sql);
                return model;
            }

        }
        #endregion

        #region 添加组
        public TB_group AddGroup(TB_group TB_group)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.GroupAdd)).Split('%');//"添加组";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + TB_group.GroupName;

            TB_group GroupInfo = new TB_group();
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    string GroupId = db.Insert(TB_group).ToString();//添加组

                    var sql = PetaPoco.Sql.Builder;

                    sql.Append(" select * from TB_group where GroupId=@0 ", GroupId);

                    GroupInfo = db.FirstOrDefault<TB_group>(sql);

                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(TB_group.CreatePersonnel, OperationType, OperationInfo);
                    int id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();

                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                    throw;
                }
            }
            return GroupInfo;

        }

        #endregion

        #region 修改组
        public bool EditGroup(TB_group TB_group, Guid CreatePersonnel)
        {
            dynamic table = new ExpandoObject();

            //table.NodeId = Organization.NodeId;
            //table.ParentId = Organization.ParentId;
            table.GroupName = TB_group.GroupName;
            table.Remarks = TB_group.Remarks;
            table.GroupId = TB_group.GroupId;

            ////系统日志
            //OperationLog table2 = new OperationLog();

            //table2.UserId = TB_group.CreatePersonnel;
            //table2.OperationDate = TB_group.CreateDate;
            //table2.OperationType = "修改组";
            //table2.OperationInfo = "修改组：" + TB_group.GroupName;
            //table2.OperationIp = GetLocalIP();

            //string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.GroupEdit);//"修改组";
            //string OperationInfo = "组名称：" + TB_group.GroupName;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.GroupEdit)).Split('%');//"添加组";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + TB_group.GroupName;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    string temp = db.Update("TB_group", "GroupId", table).ToString();

                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(CreatePersonnel, OperationType, OperationInfo);
                    int id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }


        }

        #endregion

        #region 删除组
        public bool DelGroup(Guid GroupId, string GroupName, Guid CreatePersonnel)
        {
            ////系统日志
            //OperationLog table = new OperationLog();

            //table.UserId = CreatePersonnel;
            //table.OperationDate = DateTime.Now;
            //table.OperationType = "删除部门";
            //table.OperationInfo = "删除部门：" + GroupName;
            //table.OperationIp = GetLocalIP();

            //string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.GroupDel);//"删除组";
            //string OperationInfo = "组名称：" + GroupName;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.GroupDel)).Split('%');//"添加组";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + GroupName;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    string num = db.Delete<TB_group>(GroupId).ToString();
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(CreatePersonnel, OperationType, OperationInfo);
                    int id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }

        #endregion
        #endregion

        #region 人员-组操作

        #region 显示人员授权组
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_group> GetPerGroupTree(Guid UserId, int pageIndex, int pageSize, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select * from (");
            sql.Append(" select ga.id,ga.GroupLeader, g.GroupId, g.GroupParentId, g.GroupName, g.CreateDate, g.CreatePersonnel, g.Remarks ");
            sql.Append(" from TB_group g left join TB_groupAuthorization ga on g.GroupId=ga.GroupId where ga.UserId=@0 ) as a ", UserId);
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_group>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion


        #region 显示组授权人员lsit

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupId">组id</param>
        /// <param name="model">model传参</param>
        /// <param name="totalRecord">返回总数</param>
        /// <returns></returns>
        public List<TB_UserInfo> GetPernonnelGroup(Guid GroupId, TPageModel model, out int totalRecord)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select TB_A.*,TB_U.id as Authorization_id,TB_U.GroupId,TB_U.GroupLeader,TB_g.GroupName,TB_UI.UserName AS CreatePersonnel_n from TB_UserInfo as TB_A ");
            sql.Append(" left join TB_groupAuthorization TB_U on TB_U.UserId=TB_A.UserId");//组权限
            sql.Append(" left join TB_group TB_g on TB_g.GroupId=TB_U.GroupId");//组
            sql.Append(" left join TB_UserInfo TB_UI on TB_A.CreatePersonnel=TB_UI.UserId");//组
            sql.Append(" where TB_g.GroupId=@0 and TB_A.CountState !=0", GroupId);

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_UserInfo>(model.PageIndex, model.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion


        #region 添加人员到组
        public string AddPerToGroup(TB_groupAuthorization model, dynamic TempTable)
        {
            //系统日志
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddPerToGroup)).Split('%');//

            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + TempTable.UserName + "" + AllInfo[2] + TempTable.GroupName;

            //添加人员到组
            string insertSql = "insert into TB_groupAuthorization (Group_id,GroupId,UserId,GroupLeader) values(@0,@1,@2,@3)";
            Sql sql = Sql.Builder.Append(insertSql, new object[] { model.Group_id, model.GroupId, model.UserId, model.GroupLeader });


            string flag = "false";
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();

                    //搜索组信息
                    var selectGroupPerson = PetaPoco.Sql.Builder;
                    selectGroupPerson.Append(" select * from dbo.TB_group where GroupId=@0 ", model.GroupId);//搜索组信息

                    TB_group TB_group =db.FirstOrDefault<TB_group>(selectGroupPerson);

                    //判断是否可以添加人员到组
                    var selectAuth = PetaPoco.Sql.Builder;

                    //判断该组是否属于无损检测室，检查控制室，综合室，如果属于 则判断人是否存在这三个室的任何组，存在则不可再添加
                    if (TB_group.GroupParentId.ToString().ToLower() == "8cff8e9f-f539-41c9-80ce-06a97f481393" || TB_group.GroupParentId.ToString().ToLower() == "5cfdf201-8dd4-499c-ad0b-34f77920b036" || TB_group.GroupParentId.ToString().ToLower() == "158d233d-1e20-4a9f-b3cf-716b248d59d0" || TB_group.GroupParentId.ToString().ToLower() == "0d652422-8338-41e0-952a-67c23bd15dad")
                    {
                        selectAuth.Append(" select * from TB_groupAuthorization ");
                        selectAuth.Append(" where UserId=@0 and GroupId in(select GroupId from TB_group where GroupParentId=@1 or GroupParentId=@2 or GroupParentId=@3 or GroupParentId=@4) ", model.UserId, "8cff8e9f-f539-41c9-80ce-06a97f481393", "5cfdf201-8dd4-499c-ad0b-34f77920b036", "158d233d-1e20-4a9f-b3cf-716b248d59d0", "0d652422-8338-41e0-952a-67c23bd15dad");
                    }
                    else {
                        selectAuth.Append(" select * from TB_groupAuthorization where UserId=@0 and GroupId=@1 ", model.UserId, model.GroupId);//@0和@1判断是否人存在该组；

                    }

                    if (db.ExecuteScalar<string>(selectAuth) != null)
                    {
                        db.CompleteTransaction();
                        return "该人员已在组内";
                    }


                    //添加
                    string temp = db.Execute(sql).ToString();
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(TempTable.CreatePersonnel, OperationType, OperationInfo);
                    int id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();


                    flag = "true";
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            return flag;

        }
        #endregion

        #region 从组删除人员
        public bool DelPerToGroup(int id, string UserName, string GroupName, Guid CreatePersonnel)
        {
            ////系统日志
            //OperationLog table = new OperationLog();

            //table.UserId = CreatePersonnel;
            //table.OperationDate = DateTime.Now;
            //table.OperationType = "从组删除人员";
            //table.OperationInfo = "从组:" + GroupName + " 删除人员:" + UserName;
            //table.OperationIp = GetLocalIP();

            string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelPerToGroup);//"从组删除人员";
            string OperationInfo = "从组:" + GroupName + " 删除人员:" + UserName;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    string num = db.Delete<TB_groupAuthorization>(id).ToString();
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(CreatePersonnel, OperationType, OperationInfo);
                    int ids = Convert.ToInt32(db.Execute(operation_log_sql));
                      
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }

        #endregion

        #endregion

        #region 证书操作

        #region 获取用户证书列表
        public List<TB_CertificateManagement> GetUserCertificate(int pageIndex, int pageSize, Guid UserId, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append(" select TU.UserName AS UserName_n,TC.CertificateNum,TC.Id,TC.CertificateName,TD.Project_name AS CertificateType_n,TC.IssuingUnit,TC.IssueDate,TC.ValidDate,TC.Profession,TC.Quarters,TC.Grade,TC.CertificateSate,TC.CreateDate,TU2.UserName AS CreatePersonnel_n,TC.remarks from TB_CertificateManagement TC ");
            sql.Append("  LEFT JOIN TB_DictionaryManagement TD ON TC.CertificateType=TD.id ");
            sql.Append("  LEFT JOIN TB_UserInfo TU ON TC.UserId=TU.UserId ");
            sql.Append("LEFT JOIN TB_UserInfo TU2 ON TC.CreatePersonnel=TU2.UserId ");
            sql.Append("WHERE TC.UserId=@0", UserId);

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_CertificateManagement>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 添加证书列表
        public bool AddCertificateData(TB_CertificateManagement Certificate_model, Guid UserId, out int errortype)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.Add_Certificate)).Split('%');// "添加仓库位置";
            string OperationType = AllInfo[0];
            string OperationInfo = UserId + AllInfo[1] + Certificate_model.CertificateName;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * from dbo.TB_CertificateManagement");
                    sql.Append(" where CertificateNum=@0 and CertificateName=@1 and UserId=@2", Certificate_model.CertificateNum, Certificate_model.CertificateName, Certificate_model.UserId);

                    //返回数据modelList
                    List<TB_CertificateManagement> modelDouble = db.Fetch<TB_CertificateManagement>(sql);
                    if (modelDouble.Count > 0)
                    {
                        db.CompleteTransaction();

                        errortype = 3;
                        return flag;
                    }
                    else
                    {

                        db.GetGuid(db.Insert(Certificate_model));

                        //系统日志
                        string operation_log_sql = CommonDAL.operation_log_(UserId, OperationType, OperationInfo);
                        int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                        errortype = 1;
                        db.CompleteTransaction();
                        flag = true;
                    }
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            errortype = 1;
            return flag;
        }
        #endregion

        #region 加载证书类别
        public List<ComboboxEntity> GetDictionaryList()
        {
            Guid Parent_id = new Guid("9b4592f3-b65e-4744-92b6-34b43b794f32");
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select id  as Value,Project_name as Text  ");
            sql.Append(" from TB_DictionaryManagement where Parent_id=@0", Parent_id);

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                List<ComboboxEntity> model = db.Fetch<ComboboxEntity>(sql);
                return model;
            }
        }
        #endregion

        #region 加载证书附件
        public List<TB_CertificateAppendix> GetCertificateAppendixList(Phians_Entity.Common.TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select tc.Id,tc.CertificateId,tc.DocumentName,tc.DocumentUrl,tc.DocumentFormat,tc.CreateDate,tu.UserName as CreatePersonnel_n from TB_CertificateAppendix as tc left join TB_UserInfo as tu on tc.CreatePersonnel=tu.UserId  WHERE 1=1");


            if (!string.IsNullOrEmpty(PageModel.SearchList_[0].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[0].Search + " like @0 ", "%" + PageModel.SearchList_[0].Key + "%");
            }
            //ID
            if (!string.IsNullOrEmpty(PageModel.SearchList_[1].Search))
            {
                sql.Append(" and " + PageModel.SearchList_[1].Search + "= @0", PageModel.SearchList_[1].Key);
            }
            using (var db = DbInstance.CreateDataBase())
            {
                var result = db.Page<TB_CertificateAppendix>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }


        }

        #endregion

        #region 添加证书附件
        /// <summary>
        /// 添加证书附件
        /// </summary>
        /// <param name="model">添加实体</param>
        /// <param name="errortype"></param>
        /// <returns></returns>
        public bool AddCertificateFile(TB_CertificateAppendix model, out int errortype)
        {
            //dynamic table = new ExpandoObject();
            //table.Id = model.Id;
            //table.CertificateId = model.CertificateId;
            //table.DocumentName = model.DocumentName;
            //table.DocumentUrl = model.DocumentUrl;
            //table.DocumentFormat = model.DocumentFormat;
            //table.CreateDate = model.CreateDate;
            //table.CreatePersonnel = model.CreatePersonnel;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.EditDepartment)).Split('%');// "添加设备";
            string OperationType = AllInfo[0];
            string OperationInfo = model.CreatePersonnel + AllInfo[1] + model.DocumentName;
            //判断文件名称是否重复
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from TB_CertificateAppendix WHERE CertificateId=@0 and DocumentName=@1", model.CertificateId, model.DocumentName);
            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    List<TB_CertificateAppendix> list_module = db.Fetch<TB_CertificateAppendix>(sql);
                    if (list_module.Count > 0)
                    {
                        db.CompleteTransaction();

                        errortype = 1;
                        return flag;
                    }
                    string NodeId = db.Insert(model).ToString();
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.CreatePersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));

                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            errortype = 0;
            return flag;
        }
        #endregion

        #region 删除证书
        public bool DelCertificateAppendix(TB_CertificateManagement model, Guid UserId)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.Del_Certificate)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = UserId + AllInfo[1] + model.UserName_n + model.CertificateName;
            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {

                    db.BeginTransaction();
                    string num = db.Delete<TB_CertificateManagement>(model).ToString();
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }
        #endregion

        #region 删除证书附件
        public bool DelFileManagement(TB_CertificateAppendix model)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelFileManagement)).Split('%');
            string OperationType = AllInfo[0];
            string OperationInfo = model.CreatePersonnel + AllInfo[1] + model.DocumentName;
            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {

                    db.BeginTransaction();
                    string num = db.Delete<TB_CertificateAppendix>(model).ToString();
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
                return flag;
            }
        }
        #endregion
        #endregion
    }
}
