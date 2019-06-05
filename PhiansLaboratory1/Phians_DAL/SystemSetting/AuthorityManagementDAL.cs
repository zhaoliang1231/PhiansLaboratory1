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
    class AuthorityManagementDAL : IAuthorityManagementDAL
    {
        #region 加载页面树
        public List<TB_FunctionalModule> LoadPageTree(Guid ParentId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_FunctionalModule ");
            sql.Append(" where NodeType !=3");
            sql.Append(" order by SortNum");
            //sql.Append(" where ParentId=@0  ", PageId);

            using (var db = DbInstance.CreateDataBase())
            {
                //返回多条数据model
                List<TB_FunctionalModule> model = db.Fetch<TB_FunctionalModule>(sql);
                return model;
            }

        }
        #endregion

        #region 加载页面表格树
        //public List<TB_FunctionalModule> LoadPageTbleTree(string ParentId, int pageIndex, int pageSize, out int totalRecord)
        //{
        //    var sql = PetaPoco.Sql.Builder;
        //    sql.Append("select *  ");//,ParentId as _parentId
        //    sql.Append(" from TB_FunctionalModule");
        //    sql.Append(" where ParentId=@0  ", ParentId);
        //    sql.Append(" or ParentId IN  ");
        //    sql.Append(" ( SELECT PageId FROM TB_FunctionalModule WHERE ParentId=@0) ", ParentId);
          
        //    using (var db = DbInstance.CreateDataBase())
        //    {
        //        //PetaPoco框架自带分页
        //        var result = db.Page<TB_FunctionalModule>(pageIndex, pageSize, sql);
        //        totalRecord = (int)result.TotalItems;
        //        return result.Items;
        //    }

        //}
        #endregion

        #region 加载组树
        public List<TB_group> GetGroupTree(string GroupParentId)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_group ");

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                List<TB_group> model = db.Fetch<TB_group>(sql);
                return model;
            }
        }
        #endregion

        #region 加载组人员信息
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_UserInfo> GetGroupPerList(Guid GroupId, int pageIndex, int pageSize, out int totalRecord, string search, string key)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * from (select ui.* from TB_UserInfo ui left join TB_groupAuthorization ga on ui.UserId=ga.UserId where ga.GroupId=@0 ) as a ", GroupId);
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(key))
            {
                sql.Append("where " + search + " like '%" + key + "%'");
            }

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_UserInfo>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 组/人员 授权与取消授权
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">页面model</param>
        /// <param name="ModuleNames"></param>
        /// <param name="GroupName"></param>
        /// <param name="CreatePersonnel"></param>
        /// <param name="flag2"></param>
        /// <param name="flag3"></param>
        /// <param name="ButtonNames"></param>
        /// <returns></returns>
        public bool GroupAuthority(List<TB_PageAuthorization> Model, string ModuleNames, string GroupName, Guid CreatePersonnel, bool flag2, bool flag3, string ButtonNames)
        {

            //string inse  rtSql = "INSERT INTO TB_PageAuthorization (GroupId, SystemId, PageId) SELECT @0,@1,PageId FROM TB_FunctionalModule WHERE PageId IN (@2)";

            #region 日志信息
            string AllInfo = "";
            string OperationType = "";
            string OperationInfo = "";

            if (Model[0].Flag)
            {
                if (flag3)//标识 按钮操作false  页面操作为true   //主要作用于写日志区别
                {
                    AllInfo = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonAuthorityPage);
                    string[] AllInfos = AllInfo.Split('%');
                    OperationType = AllInfos[0];
                    OperationInfo = AllInfos[1] + GroupName + AllInfos[2] + ModuleNames;//"给组：" + GroupName + "授权页面：" + ModuleNames;
                }
                else
                {
                    AllInfo = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.PersonAuthorityButton);
                    string[] AllInfos = AllInfo.Split('%');
                    OperationType = AllInfos[0];
                    OperationInfo = AllInfos[1] + GroupName + AllInfos[2] + ModuleNames + AllInfos[3] + ButtonNames;//"给组：" + GroupName + "授权页面：" + ModuleNames + "按钮授权：" + ButtonNames;
                }
            }
            else
            {
                if (flag3)//标识 按钮操作false  页面操作为true   //主要作用于写日志区别
                {
                    AllInfo = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.GroupAuthorityPage);
                    string[] AllInfos = AllInfo.Split('%');
                    OperationType = AllInfos[0];
                    OperationInfo = AllInfos[1] + GroupName + AllInfos[2] + ModuleNames;//"给组：" + GroupName + "授权页面：" + ModuleNames;
                }
                else
                {
                    AllInfo = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.GroupAuthorityButton);
                    string[] AllInfos = AllInfo.Split('%');
                    OperationType = AllInfos[0];
                    OperationInfo = AllInfos[1] + GroupName + AllInfos[2] + ModuleNames + AllInfos[3] + ButtonNames;//"给组：" + GroupName + "授权页面：" + ModuleNames + "按钮授权：" + ButtonNames;
                }
            }

            #endregion


            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {

                try
                {
                    db.BeginTransaction();

                    for (int i = 0; i < Model.Count; i++)//将传过来的数据分成一个一个的权限进行授权
                    {
                        if (flag2)//flag2如果为true 则为添加权限
                        {

                            if (Model[i].Flag)//按人授权
                            {
                                #region //检查该页面的权限是否存在 ——此块代码仅仅判断页面的权限 （按钮不做判断）
                                if (flag3)//排除按钮的授权操作 flag 按钮操作false  页面操作为true  
                                {//
                                    var checkModuleAuth = PetaPoco.Sql.Builder;
                                    checkModuleAuth.Append("select * from TB_PageAuthorization  where UserId=@0 and PageId =(select ParentId from TB_FunctionalModule where PageId=@1 ) ", Model[i].UserId, Model[i].PageId);//检查即将授权的页面所属模块是否存在权限

                                    //否存在权限 如果不存在 则添加
                                    if (db.FirstOrDefault<TB_PageAuthorization>(checkModuleAuth) == null)//权限不存在 需要添加功能模块
                                    {
                                        //获取该页面所属模块的PageId
                                        var checkModulePId = PetaPoco.Sql.Builder;
                                        checkModulePId.Append("select ParentId from TB_FunctionalModule  where PageId=@0", Model[i].PageId);
                                        Guid PageId = db.ExecuteScalar<Guid>(checkModulePId);

                                        TB_PageAuthorization modeTemp = new TB_PageAuthorization();
                                        //mode1.id = Convert.ToInt32(id[i]);
                                        modeTemp.GroupId = Model[i].GroupId;
                                        modeTemp.UserId = Model[i].UserId;
                                        //modeTemp.username = mode[i].username;
                                        modeTemp.SystemId = Model[i].SystemId;
                                        modeTemp.PageId = PageId;
                                        modeTemp.Flag = true;
                                        db.Insert(modeTemp);
                                        //InsertModuleAuth = "insert into TB_PageAuthorization (GroupId, SystemId, UserId, username, PageId, Flag) values ('" + mode[i].GroupId + "','" + mode[i].SystemId + "','" + mode[i].UserId + "','" + mode[i].username + "','" + PageId + "',True) ";
                                    }
                                }

                                #endregion

                                #region 页面 and 按钮授权

                                var selectAuth = PetaPoco.Sql.Builder;
                                //查看页面/按钮是否存在权限
                                selectAuth.Append("select * from TB_PageAuthorization where UserId=@0 and PageId=@1", Model[i].UserId, Model[i].PageId);
                                //sql = "select count(*) from TB_PageAuthorization where UserId='" + mode[i].UserId + "' and PageId='" + mode[i].PageId + "'";
                                //num = db.ExecuteScalar<int>(sql);
                                if (db.FirstOrDefault<TB_PageAuthorization>(selectAuth) == null)
                                {//如果该权限不存在 则进行insert
                                    db.Insert(Model[i]);
                                }

                                #endregion

                            }
                            else
                            {//按组授权

                                #region //检查该页面的模块权限是否存在 ——此块代码仅仅判断页面的权限 （按钮不做判断）
                                if (flag3)//排除按钮的授权操作   flag 按钮操作false  页面操作为true  
                                {//
                                    var checkModuleAuth = PetaPoco.Sql.Builder;
                                    checkModuleAuth.Append("select * from TB_PageAuthorization  where GroupId=@0 and PageId =(select ParentId from TB_FunctionalModule where PageId=@1 )", Model[i].GroupId, Model[i].PageId);//检查即将授权的页面所属模块是否存在权限

                                    //如果不存在 则添加
                                    if (db.FirstOrDefault<TB_PageAuthorization>(checkModuleAuth) == null)
                                    {
                                        //获取该页面所属模块的PageId
                                        var checkModulePId = PetaPoco.Sql.Builder;
                                        checkModulePId.Append("select ParentId from TB_FunctionalModule where PageId=@0", Model[i].PageId);
                                        Guid PageId = db.ExecuteScalar<Guid>(checkModulePId);

                                        TB_PageAuthorization modeTemp = new TB_PageAuthorization();
                                        //mode1.id = Convert.ToInt32(id[i]);
                                        modeTemp.GroupId = Model[i].GroupId;
                                        modeTemp.UserId = Model[i].UserId;
                                        modeTemp.SystemId = Model[i].SystemId;
                                        modeTemp.PageId = PageId;
                                        modeTemp.Flag = false;

                                        db.Insert(modeTemp);

                                        //InsertModuleAuth = "insert into TB_PageAuthorization (GroupId, SystemId, UserId, username, PageId, Flag) values ('" + mode[i].GroupId + "','" + mode[i].SystemId + "','" + mode[i].UserId + "','" + mode[i].username + "','" + PageId + "',True) ";
                                    }
                                }

                                #endregion

                                #region 页面 and 按钮授权

                                var selectAuth = PetaPoco.Sql.Builder;
                                selectAuth.Append("select * from TB_PageAuthorization where GroupId=@0 and PageId=@1", Model[i].GroupId, Model[i].PageId);
                                // num = db.ExecuteScalar<int>(sql);
                                if (db.FirstOrDefault<TB_PageAuthorization>(selectAuth) == null)
                                {//如果该权限不存在 则进行insert
                                    db.Insert(Model[i]);
                                }

                                #endregion

                            }
                        }
                        else//删除权限
                        {
                            if (Model[i].Flag)//按人授权
                            {
                                #region //检查该页面是否为该模块的最后一个页面 此块代码仅仅判断页面的权限 （按钮不做判断）
                                if (flag3)//排除按钮的授权操作   flag 按钮操作false  页面操作为true  
                                {
                                    var checkModuleAuth = PetaPoco.Sql.Builder;
                                    checkModuleAuth.Append("select * from TB_PageAuthorization where PageId in (select PageId from TB_FunctionalModule where ParentId = (select ParentId from TB_FunctionalModule where PageId =@0)) and PageId !=@1 and UserId=@2", Model[i].PageId, Model[i].PageId, Model[i].UserId);//索搜该页面模块下已授权的所有除了该页面之外的页面

                                    //如果该页面为该模块的最后一个页面 则删除模块的授权
                                    if (db.FirstOrDefault<TB_PageAuthorization>(checkModuleAuth) == null)
                                    {
                                        //获取该页面所属模块的PageId
                                        var checkModulePId = PetaPoco.Sql.Builder;
                                        checkModulePId.Append("select ParentId from TB_FunctionalModule  where PageId=@0", Model[i].PageId);
                                        Guid PageId = db.ExecuteScalar<Guid>(checkModulePId);

                                        db.Delete<TB_PageAuthorization>(" where UserId=@0 and PageId=@1", new object[] { Model[i].UserId, PageId });
                                    }
                                }

                                #endregion

                                #region 页面 and 按钮取消授权

                                var selectAuth = PetaPoco.Sql.Builder;
                                selectAuth.Append("select * from TB_PageAuthorization where UserId=@0 and PageId=@1", Model[i].UserId, Model[i].PageId);
                                //num = db.ExecuteScalar<int>(sql);
                                if (db.FirstOrDefault<TB_PageAuthorization>(selectAuth) != null)
                                {//如果该权限存在 则进行Delete
                                    db.Delete<TB_PageAuthorization>(" where UserId=@0 and PageId=@1", new object[] { Model[i].UserId, Model[i].PageId });
                                }


                                #endregion

                            }
                            else
                            {//按组授权

                                #region //检查该页面是否为该模块的最后一个页面 此块代码仅仅判断页面的权限 （按钮不做判断）
                                if (flag3)//排除按钮的授权操作   flag 按钮操作false  页面操作为true  
                                {
                                    var checkModuleAuth = PetaPoco.Sql.Builder;
                                    checkModuleAuth.Append("select * from TB_PageAuthorization where PageId in (select PageId from TB_FunctionalModule where ParentId = (select ParentId from TB_FunctionalModule where PageId =@0)) and PageId !=@1 and GroupId=@2", Model[i].PageId, Model[i].PageId, Model[i].GroupId);//索搜该页面模块下已授权的所有除了该页面之外的页面

                                    //如果该页面为该模块的最后一个页面 则删除模块的授权
                                    if (db.FirstOrDefault<TB_PageAuthorization>(checkModuleAuth) == null)
                                    {
                                        //获取该页面所属模块的PageId
                                        var checkModulePId = PetaPoco.Sql.Builder;
                                        checkModulePId.Append("select ParentId from TB_FunctionalModule  where PageId=@0", Model[i].PageId);
                                        Guid PageId = db.ExecuteScalar<Guid>(checkModulePId);

                                        db.Delete<TB_PageAuthorization>(" where GroupId=@0 and PageId=@1", new object[] { Model[i].GroupId, PageId });
                                    }
                                }

                                #endregion

                                #region 页面 and 按钮取消授权

                                var selectAuth = PetaPoco.Sql.Builder;
                                selectAuth.Append("select * from TB_PageAuthorization where GroupId=@0 and PageId=@1", Model[i].GroupId, Model[i].PageId);
                                // num = db.ExecuteScalar<int>(sql);
                                if (db.FirstOrDefault<TB_PageAuthorization>(selectAuth) != null)
                                {//如果该权限存在 则进行Delete
                                    db.Delete<TB_PageAuthorization>(" where GroupId=@0 and PageId=@1", new object[] { Model[i].GroupId, Model[i].PageId });//mode[i].id为主键
                                } 
                                #endregion

                            }
                        }

                    }

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
            }
            return flag;

        }

        #endregion

        #region 组/人员 授权回显
        public List<TB_FunctionalModule> ShowAuthorizedPage(dynamic table)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select f.* ");
            sql.Append(" from TB_FunctionalModule f ");
            sql.Append(" left join TB_PageAuthorization p on f.PageId= p.PageId ");
            sql.Append(" where f.NodeType !=3  and SystemId=@0", table.PageId);
           

            if (table.flag)
            {
                sql.Append("  and  UserId=@0 ", table.UserId);
            }
            else {
                sql.Append("  and GroupId=@0 ", table.GroupId);
            }
            sql.Append(" order by f.SortNum");
            //sql.Append(" where ParentId=@0  ", PageId);

            using (var db = DbInstance.CreateDataBase())
            {
                //返回多条数据model
                List<TB_FunctionalModule> model = db.Fetch<TB_FunctionalModule>(sql);
                return model;
            }

        }

        #endregion

        #region <<<<<<<<<<<<<<按钮权限操作

        #region 加载按钮权限表格
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalRecord">输出总记录</param>
        /// <returns>查询返回的列表</returns>
        public List<TB_FunctionalModule> GetButtonAuthorityList(TPageModel PageModel, out int totalRecord)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select fm.*, case when  pa.id >=0 then 1 else 0 end as IdFlag ");
            sql.Append(" from TB_FunctionalModule fm ");
            sql.Append(" left join dbo.TB_PageAuthorization pa on fm.PageId = pa.PageId and pa." + PageModel.SearchList_[2].Search + "= @0", PageModel.SearchList_[2].Key);//判断选择的组或人的id
            sql.Append(" where fm.NodeType =3");
            
            sql.Append(" and fm." + PageModel.SearchList_[1].Search + "= @0", PageModel.SearchList_[1].Key);//判断页面的id   sql.Append(" and fm.ParentId =@0", table.ParentId);

            sql.Append(" order by fm.SortNum");//排序

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_FunctionalModule>(PageModel.PageIndex, PageModel.Pagesize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 回显按钮权限表格
        public List<TB_FunctionalModule> ShowButtonAuthorityList(dynamic table, int pageIndex, int pageSize, out int totalRecord)
        {
            bool flag = table.flag;//标识 组授权为false  人员授权true   

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select f.* ");
            sql.Append(" from TB_FunctionalModule f left join TB_PageAuthorization p on f.PageId = p.PageId ");
            sql.Append(" where f.NodeType =3 ");
            sql.Append(" and f.ParentId =@0", table.ParentId);
         

            if (flag)//人员授权true  
            {
                sql.Append(" and p.UserId =@0", table.UserId);
            }
            else//组授权为false
            {
                sql.Append(" and p.GroupId =@0", table.GroupId);
            }
            sql.Append(" order by f.SortNum");
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_FunctionalModule>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #endregion

        #region 返回系统列表
        /// <summary>
       /// 返回系统列表
       /// </summary>
       /// <returns></returns>
        public List<ComboboxEntity> GetSystemList()
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select PageId as Value ,ModuleName as Text ");
            sql.Append(" from TB_FunctionalModule ");
            sql.Append(" where NodeType=0 ");
            sql.Append(" and PageId=@0 ", "2147EFDA-9CE3-4F56-B24C-CF2F16CABC52");//无损系统只需要显示实验室系统
            using (var db = DbInstance.CreateDataBase())
            {
                //返回list model
                List<ComboboxEntity> Newmodel = db.Fetch<ComboboxEntity>(sql);
                return Newmodel;
            }
        }
        #endregion

    }
}
