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
    public class MainfromBLL
    {
        IMainformDAL dal = DALFactory.GetMainform();

        #region 返回用户菜单DataTable
        //public DataTable GetMenuData(Guid UserId)
        //{


        //    return dal.GetMenuData(UserId);
        //}
        #endregion

        #region 返回用户菜单string
        //public string GetMenuData_s(Guid UserId)
        //{
        //    DataTable DataTable_ = dal.GetMenuData(UserId);
         
          
        //    return DataTable2Json(DataTable_);
        //}

        public string GetMenutree(Guid UserId)
        {
            DataTable new_table = ListToDataTable.ListToDataTable_(dal.GetMenuData(UserId));

            string json = GetTreeJsonByTable(new_table, "PageId", "ModuleName", "ParentId", "2147efda-9ce3-4f56-b24c-cf2f16cabc52", "IconCls", "U_url").ToString();
           return json;
        }


        #endregion
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            int table_rowcount = dt.Rows.Count;//table行数
            jsonBuilder.Append("{\"total\":");

            jsonBuilder.Append(table_rowcount);
            
            jsonBuilder.Append(",\"rows\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    string text_ = dt.Rows[i][j].ToString().Trim().Replace("\\", "\\\\");
                    text_ = text_.Replace("\r", "");
                    text_ = text_.Replace("\n", "");
                    //\r\n
                    jsonBuilder.Append(text_);
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            jsonBuilder.Append("]}");
            return jsonBuilder.ToString();
        }



        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idColumn">ID列</param>
        /// <param name="textColumn">Text列</param>
        /// <param name="ParentId">关系字段</param>
        /// <param name="Idindex">父ID 入口</param>
        StringBuilder treeresult = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        private StringBuilder GetTreeJsonByTable(DataTable tabel, string idColumn, string textColumn, string ParentId, object Idindex, string IconCls, string U_url)
        {

            treeresult.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}='{1}'", ParentId, Idindex);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        sb.Append("{\"id\":\"" + row[idColumn] + "\",\"text\":\"" + row[textColumn] + "\",\"icon\":\"" + row[IconCls] + "\",\"url\":\"" + row[U_url] + "\"");
                        if (tabel.Select(string.Format("{0}='{1}'", ParentId, row[idColumn])).Length > 0)
                        {
                            sb.Append(",\"menus\":");
                            GetTreeJsonByTable(tabel, idColumn, textColumn, ParentId, row[idColumn], IconCls, U_url);
                            treeresult.Append(sb.ToString());
                            sb.Clear();
                        }
                        treeresult.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                treeresult.Append(sb.ToString());
                sb.Clear();
            }
            return treeresult;
        }
        

    }
}
