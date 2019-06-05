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

namespace Phians_BLL
{
    public class FunctionalModuleBLL
    {
        IFunctionalModuleDAL dal = DALFactory.GetFunctionalModule();
        #region 加载页面
        public string FunctionalModuleLoad(string ParentId)
        {

            if (string.IsNullOrEmpty(ParentId))
            {
                ParentId = "2147efda-9ce3-4f56-b24c-cf2f16cabc66";
            }
            DataTable new_table = ListToDataTable.ListToDataTable_(dal.FunctionalModuleLoad(ParentId));

            new_table.Columns.Add("state_", typeof(string));//表格添加一个state_字段

            foreach (DataRow dr in new_table.Rows)
            {
                int i = 0;
                foreach (DataRow drs in new_table.Rows)
                {
                    if (dr["PageId"].ToString() == drs["ParentId"].ToString())
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
            string json = newTree.GetTreeJsonByTable(new_table,"id", "PageId", "ModuleName", "ParentId", ParentId, "state_").ToString();
            return json;
        }
        #endregion

        #region 获取页面信息
        public TB_FunctionalModule FunctionalModuleLoadPageInfo(Guid PageId)
        {
            return dal.FunctionalModuleLoadPageInfo(PageId);
        }
        #endregion

        #region 模块信息加载
        /// <summary>
        /// /
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<TB_FunctionalModule> LoadModuleInfo(Guid PageId, int pageIndex, int pageSize, out int totalRecord)
        {
            return dal.LoadModuleInfo(PageId, pageIndex, pageSize, out totalRecord);
        }
        #endregion

        #region 添加页面
        public Guid FunctionalModuleAdd(TB_FunctionalModule model)
        {
            return dal.FunctionalModuleAdd(model);
        }
        #endregion

        #region 修改页面信息
        public bool FunctionalModuleEdit(TB_FunctionalModule model)
        {
            return dal.FunctionalModuleEdit(model);
        }
        #endregion

        #region 删除页面
        public bool FunctionalModuleDel(Guid PageId)
        {
            return dal.FunctionalModuleDel(PageId);
        }
        #endregion
    }
}
