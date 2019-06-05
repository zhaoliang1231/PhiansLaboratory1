using Phans_DAL_DALFactory;
using Phans_DAL_INTERFACE;
using Phians_Entity;
using PhiansCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL.SystemSetting
{
    public class DictionaryManagementBLL
    {
        IDictionaryManagementDAL dal = DALFactory.GetDictionaryManagement();

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

        #region 添加字典列表
        public bool AddPageTree(TB_DictionaryManagement model, Guid UserId, out Guid NodeId)
        {
            return dal.AddPageTree(model,UserId,out NodeId);
        }

        #endregion 

        #region 修改字典
        public bool EditDictionary(TB_DictionaryManagement model, Guid UserId, out int errorType)
        {
            return dal.EditDictionary(model, UserId, out errorType); ;
        }

        #endregion 

        #region 删除字典列表
        public bool DelDicitionaryData(TB_DictionaryManagement model, Guid UserId)
        {
            return dal.DelDicitionaryData(model, UserId); ;
        }
        #endregion

        #region 加载字典列表
        public List<TB_DictionaryManagement> LoadDicitionaryData(int pageIndex, int pageSize, out int totalRecord, Guid nodeid, string search, string key)
        {
            return dal.LoadDicitionaryData(pageIndex, pageSize, out totalRecord, nodeid, search, key);
        }
        #endregion

        #region 启用停用字典状态
        public bool EditDictionaryState(TB_DictionaryManagement model, Guid UserId)
        {
            return dal.EditDictionaryState(model, UserId); ;
        }
        #endregion
    }
}
