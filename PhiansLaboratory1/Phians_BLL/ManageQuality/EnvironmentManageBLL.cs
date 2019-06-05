using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE.ManageQuality;
using Phians_Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PhiansCommon;
using System.Data;
using Windows.Excel;

namespace Phians_BLL.ManageQuality
{
    public class EnvironmentManageBLL
    {
        IEnvironmentManageDAL dal = DALFactory.GetManageQuality();

        #region 加载树
        public string LoadPageTree(Guid id, Guid Parent_id)
        {
            //if (string.IsNullOrEmpty(ParentId))
            //{
            //    ParentId = "52e00f9a-bbcd-4216-9cb8-0feea30f8664";//默认最高的父的ParentId
            //}
            DataTable new_table = ListToDataTable.ListToDataTable_(dal.LoadPageTree(id));

            new_table.Columns.Add("state_", typeof(string));//表格添加一个state_字段

            foreach (DataRow dr in new_table.Rows)
            {
                int i = 0;
                foreach (DataRow drs in new_table.Rows)
                {
                    if (dr["id"].ToString() == drs["Parent_id"].ToString())
                    {
                        dr["state_"] = "closed";
                        i = 1;
                        break;
                    }
                }

                if (i == 0)
                {
                    dr["state_"] = "open";
                }
            }
            TreeJsonByTable newTree = new TreeJsonByTable();
            string json = newTree.GetTreeJsonByTable(new_table,"id", "id", "Project_name", "Parent_id", Parent_id, "state_").ToString();
            return json;
        }
        #endregion

        //#region 添加树
        //public bool AddPageTree(TB_DictionaryManagement model,string pageName, Guid UserId)
        //{
        //    return dal.AddPageTree(model,pageName,UserId); ;
        //}
        //#endregion

        #region 加载文件类型
        public string GetFileTypeList(string ParentId)
        {
            if (string.IsNullOrEmpty(ParentId))
            {
                ParentId = "8cff8e9f-f539-41c9-80ce-06a97f481390";
            }
            DataTable new_table = ListToDataTable.ListToDataTable_(dal.GetFileTypeList());

            new_table.Columns.Add("state_", typeof(string));//表格添加一个state_字段

            foreach (DataRow dr in new_table.Rows)
            {
                int i = 0;
                foreach (DataRow drs in new_table.Rows)
                {
                    if (dr["NodeId"].ToString() == drs["ParentId"].ToString())
                    {
                        dr["state_"] = "closed";
                        i = 1;
                        break;
                    }
                }

                if (i == 0)
                {
                    dr["state_"] = "open";
                }
            }

            TreeJsonByTable newTree = new TreeJsonByTable();
            string json = newTree.GetTreeJsonByTable(new_table,"id" ,"NodeId", "NodeName", "ParentId", ParentId, "state_").ToString();
            return json;
        }
        #endregion

        #region 获取文件列表
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <param name="file_type">获取文件类型</param>
        /// <returns></returns>
        public List<TB_FileManagement> GetFileManagement(int pageIndex, int pageSize, out int totalRecord,string file_type, string search,string key,string sortName,string sortOrder)
        {
            return dal.GetFileManagement(pageIndex, pageSize, out totalRecord, file_type, search, key, sortName, sortOrder);
        }

        #endregion

        #region 新增文件
        /// <summary>
        /// 新增文件
        /// </summary>
        /// <param name="files"></param>
        /// <param name="model"></param>
        /// <param name="MapPath">文件绝对路径</param>
        /// <returns></returns>
        public bool AddFileManagement(HttpFileCollection files, TB_FileManagement model, string MapPath, out int errortype)
        {
            bool flag = false;
            if (files != null && files.Count > 0)
            {
                string fileName = Path.GetFileName(files[0].FileName);          //文件名称
                string fileExtension = Path.GetExtension(fileName).ToLower();  //文件扩展名
                string filePath = MapPath;//保存路径
                model.FileNewName += fileExtension;

                if (fileExtension != ".doc" && fileExtension != ".docx" && fileExtension != ".xls" && fileExtension != ".xlsx" && fileExtension != ".pdf")
                {
                    errortype = 1;
                    return flag;
                }
                model.FileFormat = fileExtension;

                //判断路径是否存在
                if (!System.IO.File.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                //string fileName2 = model.FileName.ToString();//文件名
                //string update_url = model.FileUrl+ fileName2 + fileExtension;
                model.FileUrl = filePath + model.FileNewName;
                files[0].SaveAs(System.Web.HttpContext.Current.Server.MapPath(filePath) + model.FileNewName);//保存路径+文件名+格式
                //修改数据库保存路径
                try
                {
                    flag = dal.AddFileManagement(model, out errortype);
                    if (flag)
                    {
                        return true;//返回执行结果 ，添加成功可以换成需要的内容
                    }
                    else
                    {
                        return flag;
                    }
                }
                catch (Exception e)
                {
                    errortype = 0;
                    return flag;
                }
            }
            else
            {
                errortype = 0;
                return flag;
            }
        }

        #endregion

        #region 修改文件
        //修改文件
        public bool UpdateFileManagement(TB_FileManagement model, Guid loginer,string FileUrl, out int errortype)
        {
            bool flag = false;
            flag = dal.UpdateFileManagement(model,loginer, FileUrl, out errortype);
            return flag;
        }
        #endregion

        #region 删除文件
        //删除文件
        public bool DelFileManagement(TB_FileManagement model,Guid loginer)
        {
            return dal.DelFileManagement(model,loginer);
        }

        #endregion

        #region 审核文件
        //审核文件
        public bool AuditFileManagement(TB_FileManagement model, out int state, string FileUrl)
        {
            return dal.AuditFileManagement(model,out state, FileUrl);
        }
        #endregion

        #region 签发文件
        //签发文件
        public bool IssuedFileManagement(TB_FileManagement model, out int state, string FileUrl)
        {
            return dal.IssuedFileManagement(model,out state, FileUrl);
        }

        #endregion

        #region 导出列表
        //导出列表
        public bool export(string search, string key, string tempFileName, string tempFilePath, int type, string ids)
        {
            List<TB_FileManagement> list=dal.export( search, key, type, ids);
            try
            {
                ExcelExport(tempFilePath, tempFileName, list);
                return true;
            }
            catch
            {
                return false;
            }           
        }

        #endregion

        public List<TB_FileManagement> GetFile(int id, string pageName)
        {
            return dal.GetFile(id,pageName);
        }


        #region 获取文件信息
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id">文件id</param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public TB_FileManagement GetFileModelBLL(int id)
        {

          return  dal.GetFileModel(id);
        
        }
        #endregion

        #region 获取文件信息By FileID
        /// <summary>
        /// 获取文件信息By FileID
        /// </summary>
        /// <param name="FileID">文件FileID</param>
        public TB_FileManagement GetFileModelByFileIDBLL(Guid FileID)
        {

            return dal.GetFileModelByFileID(FileID);

        }
        #endregion
        public static void ExcelExport(string tempFilePath, string tempFileName, List<TB_FileManagement> list)
        {
            //创建新的EXCELSheet
            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            Cells cells = ws.Cells;

            //插入内容
            //第一行
            cells.Merge(0, 0, 1, 13);
            cells[0, 0].PutValue("环境文件列表");
            //cells[0, 0].SetStyle(style);
            cells.SetRowHeight(0, 20.5);//行高
            //cells.SetColumnWidth(0, 3.5);//列宽

            cells[1, 0].PutValue("序号");
            cells[1, 1].PutValue("文件编号");
            cells[1, 2].PutValue("文件类别");
            cells[1, 3].PutValue("文件名称");
            cells[1, 4].PutValue("文件格式");
            cells[1, 5].PutValue("文件说明");
            cells[1, 6].PutValue("存档人员");
            cells[1, 7].PutValue("存档日期");
            cells[1, 8].PutValue("评审人员");
            cells[1, 9].PutValue("评审日期");
            cells[1, 10].PutValue("签发人员");
            cells[1, 11].PutValue("签发日期");
            cells[1, 12].PutValue("说明");

            int i = 0;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    cells[2 + i, 0].PutValue(i + 1);
                    cells[2 + i, 1].PutValue(item.FileNum);
                    cells[2 + i, 2].PutValue(item.FileType);
                    cells[2 + i, 3].PutValue(item.FileName);
                    cells[2 + i, 4].PutValue(item.FileFormat);
                    cells[2 + i, 5].PutValue(item.FileRemark);
                    cells[2 + i, 6].PutValue(item.FileUserName);
                    cells[2 + i, 7].PutValue(item.FileDate);
                    cells[2 + i, 8].PutValue(item.ReviewName);
                    cells[2 + i, 9].PutValue(item.AuditDate);
                    cells[2 + i, 10].PutValue(item.IssuePersonnelName);
                    cells[2 + i, 11].PutValue(item.IssueDate);
                    cells[2 + i, 12].PutValue(item.Remarks);
                    i++;
                }
            }

            ws.Name = "环境文件列表导出";
            ws.AutoFitColumns();//让各列自适应宽度，这个很有用
            //string path = Path.GetTempFileName();
            //Session["FailMsg"] = path;
            wb.Save(tempFilePath + tempFileName);
        }

    }
}
