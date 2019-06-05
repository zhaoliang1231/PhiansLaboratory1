using PetaPoco;
using Phans_DAL_INTERFACE.ManageQuality;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Phians_DAL
{
    class EnvironmentManageDAL : IEnvironmentManageDAL
    {
        #region 获取树节点
        public List<TB_DictionaryManagement> LoadPageTree(Guid id)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_DictionaryManagement where id=@0", id);
            sql.Append(" or Parent_id =@0", id);
            sql.Append(" and DictionaryState !=@0", 0);
            sql.Append(" order by Sort_num");
            //sql.Append(" where Parent_Id=@0  ", PageId);

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                //返回单条数据model
                List<TB_DictionaryManagement> model = db.Fetch<TB_DictionaryManagement>(sql);
                return model;
            }
        }
        #endregion



        #region 添加树节点
        public bool AddPageTree(TB_DictionaryManagement model, string pageName, Guid UserId)
        {

            string OperationType = GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddFileManagement);//"添加文件";
            string OperationInfo = pageName + "添加文件类型：" + model.Project_name;

            bool flag = false;
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    db.BeginTransaction();
                    var sql = PetaPoco.Sql.Builder;
                    sql.Append("select * ");
                    sql.Append(" from TB_DictionaryManagement where Parent_id=@0", model.Parent_id);

                    sql.Append(" and Project_name=@0", model.Project_name);
                    //sql.Append(" where Parent_Id=@0  ", PageId);

                   
                        //返回单条数据model
                    List<TB_DictionaryManagement> modelDouble = db.Fetch<TB_DictionaryManagement>(sql);
                    if (modelDouble.Count > 0)
                    {
                        flag = false;
                    }
                    else {
                        Guid NodeId = new Guid(db.Insert(model).ToString());
                        //系统日志
                        string operation_log_sql = CommonDAL.operation_log_(UserId, OperationType, OperationInfo);
                        int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                        //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table));
                        db.CompleteTransaction();
                        flag = true;
                    }
                   
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            return flag;
        }
        public List<TB_FileManagement> GetFileTypeList()
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_Organization ");

            using (var db = DbInstance.CreateDataBase())
            {
                //返回数据model
                List<TB_FileManagement> model = db.Fetch<TB_FileManagement>(sql);
                return model;
            }
        }
        #endregion

        #region 获取文件列表
        public List<TB_FileManagement> GetFileManagement(int pageIndex, int pageSize, out int totalRecord, string file_type, string search, string key, string sortName, string sortOrder)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select a.id,Dic.Project_name as FileType_n,a.FileNum,a.FileNewName,a.state, a.FileDate,a.Remarks, a.FileUrl,a.FileName,a.FileRemark,a.FileFormat,b.UserName as FileUserName,c.UserName as ReviewName, d.UserName as IssuePersonnelName from dbo.TB_FileManagement a");
            sql.Append(" left join TB_UserInfo b on a.FilePersonnel=b.UserId");
            sql.Append(" left join TB_UserInfo c on a.Personnel=c.UserId");
            sql.Append(" left join TB_UserInfo d on a.IssuePersonnel=d.UserId");
            sql.Append(" left join TB_DictionaryManagement Dic on a.FileType=Dic.id");
           // sql.Append(" left join dbo.TB_DictionaryManagement e on a.FileType=e.id");
            sql.Append(" where  a.FileType=@0", file_type);
            //if (!string.IsNullOrEmpty(file_type))
            //{
            //    sql.Append("and filetype='" + file_type + "'");
            //}
            if (!string.IsNullOrEmpty(search))
            {
                if(search == "FilePersonnel")
                    sql.Append(" and b.UserName like '%"+key+"%'");
                else if (search == "AuditPersonnel")
                    sql.Append(" and c.UserName like '%" + key + "%'");
                else if (search == "IssuePersonnel")
                    sql.Append(" and d.UserName like '%" + key + "%'");
                else
                    sql.Append(" and " + search + " like '%" + key + "%'");
            }
           if( sortName!=null){
               sql.OrderBy(sortName+" "+sortOrder);
           }
            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Page<TB_FileManagement>(pageIndex, pageSize, sql);
                totalRecord = (int)result.TotalItems;
                return result.Items;
            }
        }
        #endregion

        #region 添加文件
        //添加文件
        public bool AddFileManagement(TB_FileManagement model,out int errortype)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AddFileManagement)).Split('%');// "添加文件";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.FileName;
            model.FileDate = DateTime.Now.ToString();

            //判断文件编号是否存在
            var exist = PetaPoco.Sql.Builder;
            exist.Append("select * from TB_FileManagement where FileNum=@0 and state=@1 and FileType=@2", model.FileNum, 1,model.FileType);

            bool flag = false;
            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    if (db.FirstOrDefault<TB_FileManagement>(exist) != null)
                    {
                        errortype = 2;//文件编号已存在
                        return false;
                    }
                    db.Insert(model).ToString();
                    
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.FilePersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table));
                    db.CompleteTransaction();
                    flag = true;
                }
                catch (Exception e)
                {
                    db.AbortTransaction();
                }
            }
            errortype = 3;//插入成功
            return flag;

        }
        #endregion

        #region 修改文件
        //修改文件
        public bool UpdateFileManagement(TB_FileManagement model, Guid loginer,string FileUrl, out int errortype)
        {
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.UpdateFileManagement)).Split('%');// "修改文件";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + FileUrl;

            dynamic table = new ExpandoObject();
            table.id=model.id;
            table.FileName = model.FileName;
            table.FileNum = model.FileNum;
            table.FileType = model.FileType;
            table.FileNewName = model.FileNewName;
          //  table.FileFormat = model.FileFormat;
            table.FileRemark = model.FileRemark;
            table.Remarks = model.Remarks;
            table.state = model.state;
            ////判断文件编号是否存在
            //var exist = PetaPoco.Sql.Builder;
            //exist.Append("select * from TB_FileManagement where FileNum=@0 and FileType!=@1", model.FileNum, model.FileType);

            bool flag = false;
           // using (var db = DbInstance.CreateDataBase())
            using (PetaPoco.Database db=new PetaPoco.Database())
            {
                try
                {
                    db.BeginTransaction();
                    //if (db.FirstOrDefault<TB_FileManagement>(exist) != null)
                    //{
                    //    errortype = 2;//文件编号已存在
                    //    return false;
                    //}   
                    object obj = null;
                    obj = db.Update("TB_FileManagement","id", table);
                    if (obj == null)
                    {
                        errortype = 0;//修改失败
                        return false;
                    }

                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(loginer, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    //int id = Convert.ToInt32(db.Insert("TB_OperationLog", "id", table));
                    db.CompleteTransaction();
                    flag = true;
                }
                catch(Exception e)
                {
                    db.AbortTransaction();
                }
            }
            errortype = 3;//修改成功
            return flag;
        }
        #endregion

        #region 删除文件
        public bool DelFileManagement(TB_FileManagement model, Guid loginer) 
        {
            bool flag = false;

            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.DelFileManagement)).Split('%');// "删除文件";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + model.FileName;

            var sql = PetaPoco.Sql.Builder;
            sql.Append("update TB_FileManagement set state=0 where id=@0", model.id);
            using (var db = DbInstance.CreateDataBase())
            {
                try
                {
                    //db.Execute(sql);
                    db.Delete(model);
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(loginer, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    flag = true;
                }
                catch
                {
                    flag = false;
                }
            }
            return flag;
        }
        #endregion

        #region 审核文件
        //审核文件
        public bool AuditFileManagement(TB_FileManagement model, out int state, string FileUrl)
        {
            state=0;
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.AuditFileManagement)).Split('%');// "审核文件";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + FileUrl;

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select state from TB_FileManagement where id=@0", model.id);
            bool flag = false;
            model.AuditDate = DateTime.Now.ToString();
            dynamic table = new ExpandoObject();
            table.id = model.id;
            table.AuditDate = model.AuditDate;
            table.Personnel = model.Personnel;
            table.state=model.state;

            using (PetaPoco.Database db= new PetaPoco.Database())
            {
                try
                {
                    if(db.ExecuteScalar<int>(sql)==2)
                    {
                        state = 2;
                        return false;
                    }
                    object obj = new object();
                    obj = db.Update("TB_FileManagement", "id", table);
                    if (table == null)
                        return false;
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.Personnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    flag = true;
                }
                catch
                {
                    flag = false;
                }
                return flag;
            }
        }
        #endregion

        #region 签发文件
        //签发文件
        public bool IssuedFileManagement(TB_FileManagement model, out int state, string FileUrl)
        {
            state = 0;
            string[] AllInfo = (GetOperationType.GetOperationTypeEnum((int)OperationTypeEnum.IssuedFileManagement)).Split('%');// "签发文件";
            string OperationType = AllInfo[0];
            string OperationInfo = AllInfo[1] + FileUrl;

            var sql = PetaPoco.Sql.Builder;
            sql.Append("select state from TB_FileManagement where id=@0", model.id);
            bool flag = false;
            model.IssueDate = DateTime.Now.ToString();
            dynamic table = new ExpandoObject();
            table.id = model.id;
            table.IssueDate = model.IssueDate;
            table.IssuePersonnel = model.IssuePersonnel;
            table.state = model.state;

            using (PetaPoco.Database db = new PetaPoco.Database())
            {
                try
                {
                    if (db.ExecuteScalar<int>(sql) == 3)
                    {
                        state = 3;
                        return false;
                    }
                    object obj = new object();
                    obj = db.Update("TB_FileManagement", "id", table);
                    if (table == null)
                        return false;
                    //系统日志
                    string operation_log_sql = CommonDAL.operation_log_(model.IssuePersonnel, OperationType, OperationInfo);
                    int operation_log_id = Convert.ToInt32(db.Execute(operation_log_sql));
                    flag = true;
                }
                catch
                {
                    flag = false;
                }
                return flag;
            }
        }
        #endregion

        #region 导出列表
        //导出列表
        public List<TB_FileManagement> export(string search, string key, int type, string ids)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select a.*, b.UserName as FileUserName,c.UserName as ReviewName, d.UserName as IssuePersonnelName from dbo.TB_FileManagement a");
            sql.Append(" left join TB_UserInfo b on a.FilePersonnel=b.UserId");
            sql.Append(" left join TB_UserInfo c on a.Personnel=c.UserId");
            sql.Append(" left join TB_UserInfo d on a.IssuePersonnel=d.UserId");
            sql.Append(" where a.state!=1 ");
            if(type==0)//全部导出
            {
                if (!string.IsNullOrEmpty(search))
                {
                    if (search == "FilePersonnel")
                        sql.Append(" and b.UserName like '%" + key + "%'");
                    else if (search == "AuditPersonnel")
                        sql.Append(" and c.UserName like '%" + key + "%'");
                    else if (search == "IssuePersonnel")
                        sql.Append(" and d.UserName like '%" + key + "%'");
                    else
                        sql.Append(" and " + search + " like '%" + key + "%'");
                }
            }
            else if(type==1)//选择导出
            {
                sql.Append(" and a.id in(" + ids + ")");
            }

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                var result = db.Fetch<TB_FileManagement>(sql);
                return result;
            }
        }
        #endregion

        public List<TB_FileManagement> GetFile(int id, string pageName)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_"+pageName+" where id=@1", pageName,id);
            //sql.Append(" where Parent_Id=@0  ", PageId);

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                List<TB_FileManagement> list= db.Fetch<TB_FileManagement>(sql);
                return list;
            }
        }


     
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public TB_FileManagement GetFileModel(int id)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_FileManagement where id=@0",id);
            //sql.Append(" where Parent_Id=@0  ", PageId);

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                TB_FileManagement list = db.FirstOrDefault<TB_FileManagement>(sql);
                return list;
            }
        }


        #region 获取文件信息By FileID
        /// <summary>
        /// 获取文件信息By FileID
        /// </summary>
        /// <param name="FileID">文件FileID</param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public TB_FileManagement GetFileModelByFileID(Guid FileID)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select * ");
            sql.Append(" from TB_FileManagement where FileID=@0", FileID);
            //sql.Append(" where Parent_Id=@0  ", PageId);

            using (var db = DbInstance.CreateDataBase())
            {
                //PetaPoco框架自带分页
                TB_FileManagement list = db.FirstOrDefault<TB_FileManagement>(sql);
                return list;
            }
        }
        #endregion
        
    }
}
