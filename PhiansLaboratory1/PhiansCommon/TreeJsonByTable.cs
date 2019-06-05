using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhiansCommon
{
    public class TreeJsonByTable
    {

        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源table</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
        private StringBuilder result = new StringBuilder();
        private StringBuilder result2 = new StringBuilder();
        private StringBuilder sb = new StringBuilder();

        #region table转树
        public  StringBuilder GetTreeJsonByTable(DataTable tabel, string id, string idCol, string txtCol, string rela, object pId, string state_)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string new_ = pId.ToString();
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string nn =row[rela].ToString();
                        if (nn == new_)
                        {
                            sb.Append("{\"id\":\"" + row[idCol] + "\",\"id_\":\"" + row[id] + "\",\"text\":\"" + row[txtCol] + "\",\"ParendID\":\"" + row[txtCol] + "\",\"state\":\"" + row[state_] + "\"");
                            result.Append(sb.ToString());
                            sb.Clear();
                        }

                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
            return result;
        }


        //public StringBuilder GetTreeJsonByTable2(DataTable tabel, string idColumn, string textColumn)
        //{
        //    result.Append(sb.ToString());
        //    sb.Clear();
        //    if (tabel.Rows.Count > 0)
        //    {
        //        sb.Append("[");
        //        string new_ = pId.ToString();
        //        string filer = string.Format("{0}='{1}'", rela, pId);
        //        DataRow[] rows = tabel.Select(filer);
        //        if (rows.Length > 0)
        //        {
        //            foreach (DataRow row in rows)
        //            {
        //                string nn = row[rela].ToString();
        //                if (nn == new_)
        //                {
        //                    sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"" + row[state_] + "\"");
        //                    result.Append(sb.ToString());
        //                    sb.Clear();
        //                }

        //                result.Append(sb.ToString());
        //                sb.Clear();
        //                sb.Append("},");
        //            }
        //            sb = sb.Remove(sb.Length - 1, 1);
        //        }
        //        sb.Append("]");
        //        result.Append(sb.ToString());
        //        sb.Clear();
        //    }
        //    return result;
        //}


        #endregion

        #region  查询树用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabel"></param>
        /// <param name="idCol"></param>
        /// <param name="txtCol"></param>
        /// <param name="rela"></param>
        /// <param name="pId"></param>
        /// <param name="state_"></param>
        /// <param name="list_"></param>
        /// <returns></returns>

        public  StringBuilder GetTreeJsonByTable_s(DataTable tabel, string idCol, string txtCol, string rela, object pId, string state_, List<string> list_)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string new_ = pId.ToString();
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string nn = row[idCol].ToString();
                        string nnn = row[state_].ToString();

                        if (list_.Contains(row[idCol].ToString()) || list_.Contains(row[rela].ToString()))
                        {
                            sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"" + row[state_] + "\"");
                            if (nnn == "open")
                            {
                                if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                                {
                                    sb.Append(",\"children\":");
                                    GetTreeJsonByTable_s(tabel, idCol, txtCol, rela, row[idCol], state_, list_);
                                    result.Append(sb.ToString());
                                    sb.Clear();
                                }
                            }
                            result.Append(sb.ToString());
                            sb.Clear();
                            sb.Append("},");
                        }
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
            return result;
        }

        #endregion

        #region 权限表格树

        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idColumn">ID列</param>
        /// <param name="textColumn">Text列</param>
        /// <param name="ParentId">关系字段</param>
        /// <param name="Idindex">父ID 入口</param>
        StringBuilder treeresult = new StringBuilder();
        //StringBuilder sb = new StringBuilder();
        public StringBuilder GetTreeJsonByTable(DataTable tabel, string idColumn, string textColumn, string ParentId, object Idindex, string NodeType, string IconCls, string U_url, string SortNum, string Remarks, string PermissionFlag, string isParent, string id)
        {

            treeresult.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                //string new_ = Idindex.ToString();
                string filer = string.Format("{0}='{1}'", ParentId, Idindex);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {


                        sb.Append("{\"id\":\"" + row[id] + "\",\"PageId\":\"" + row[idColumn] + "\",\"ModuleName\":\"" + row[textColumn] + "\",\"ParentId\":\"" + row[ParentId] + "\",\"NodeType\":\"" + row[NodeType] + "\",\"IconCls\":\"" + row[IconCls] + "\",\"U_url\":\"" + row[U_url] + "\",\"SortNum\":\"" + row[SortNum] + "\",\"Remarks\":\"" + (row[Remarks].ToString()).Trim() + "\",\"PermissionFlag\":\"" + row[PermissionFlag] + "\",\"isParent\":\"" + row[isParent] + "\"");

                        if (tabel.Select(string.Format("{0}='{1}'", ParentId, row[idColumn])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idColumn, textColumn, ParentId, row[idColumn], NodeType, IconCls, U_url, SortNum, Remarks, PermissionFlag, isParent,id);
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

        #endregion

        #region 权限表格树

        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="TaskId">ID列</param>
        /// <param name="textColumn">Text列</param>
        /// <param name="ParentId">关系字段</param>
        /// <param name="Idindex">父ID 入口</param>
        StringBuilder Tasktreeresult = new StringBuilder();
        //StringBuilder sb = new StringBuilder();
        public StringBuilder GetTaskTreeJsonByTable(DataTable tabel, string TaskId, string ProjectName, string ParentID, object Idindex,string id)
        {

            Tasktreeresult.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                //string new_ = Idindex.ToString();
                string filer = string.Format("{0}='{1}'", ParentID, Idindex);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        sb.Append("{\"TaskId\":\"" + row[TaskId] + "\",\"ProjectName\":\"" + row[ProjectName] + "\",\"ParentID\":\"" + row[ParentID] + "\"");
                        if (tabel.Select(string.Format("{0}='{1}'", ParentID, row[TaskId])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTaskTreeJsonByTable(tabel, TaskId, ProjectName, ParentID, row[TaskId], id);
                            Tasktreeresult.Append(sb.ToString());
                            sb.Clear();
                        }
                        Tasktreeresult.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                Tasktreeresult.Append(sb.ToString());
                sb.Clear();
            }
            return Tasktreeresult;
        }

        #endregion

        #region 同步树 排除子项目 保留子项目
        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
        /// <param name="Isparent">是否为主项目</param>
        /// <param name="ProjectId">项目id</param>
        /// <param name="MotoNum">电机编号</param>

        public StringBuilder GetSynchTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId, string Isparent, string ProjectId, string SampleNo, string SampleQty, string TemplateID_id)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                     string txtCol2=  row[txtCol].ToString();
                     txtCol2 = txtCol2.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                     sb.Append("{\"id\":\"" + row[idCol] + "\",\"ParentID\":\"" + row[rela] + "\",\"text\":\"" + txtCol2 + "\",\"Isparent\":\"" + row[Isparent] + "\",\"ProjectId\":\"" + row[ProjectId] + "\",\"SampleNo\":\"" + row[SampleNo] + "\",\"SampleQty\":\"" + row[SampleQty] + "\",\"TemplateID_id\":\"" + row[TemplateID_id] + "\",\"state\":\"open\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetSynchTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol], Isparent, ProjectId, SampleNo, SampleQty, TemplateID_id);
                            result.Append(sb.ToString());
                            sb.Clear();
                        }
                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
            return result;
        }

        public StringBuilder GetSynchTreeJsonByTablecommon(DataTable tabel, string idCol, string txtCol, string rela, object pId, string Isparent, string ProjectId, string SampleNo, string SampleQty, string TemplateID_id)
        {
            result.Append(sb.ToString());
            sb.Clear();
            if (tabel.Rows.Count > 0)
            {
                sb.Append("[");
                string filer = string.Format("{0}='{1}'", rela, pId);
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string txtCol2 = row[txtCol].ToString();
                        txtCol2 = txtCol2.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        sb.Append("{\"id\":\"" + row[idCol] + "\",\"ParentID\":\"" + row[rela] + "\",\"text\":\"" + txtCol2 + "\",\"" + Isparent + "\":\"" + row[Isparent] + "\",\"" + ProjectId + "\":\"" + row[ProjectId] + "\",\""
                            + SampleNo + "\":\"" + row[SampleNo] + "\",\"" + SampleQty + "\":\"" + row[SampleQty] + "\",\"" + TemplateID_id + "\":\"" + row[TemplateID_id] + "\",\"state\":\"open\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetSynchTreeJsonByTablecommon(tabel, idCol, txtCol, rela, row[idCol], Isparent, ProjectId, SampleNo, SampleQty, TemplateID_id);
                            result.Append(sb.ToString());
                            sb.Clear();
                        }
                        result.Append(sb.ToString());
                        sb.Clear();
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                result.Append(sb.ToString());
                sb.Clear();
            }
            return result;
        }
        #endregion

        #region 下拉框
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="total">行数</param>
        /// <returns></returns>
        public string Dataset2Json1(DataSet ds, int total = -1)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {

                json.Append("[");

                json.Append(DataTable2Json(dt));
                json.Append("]");
            }
            return json.ToString();
        }
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
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

            return jsonBuilder.ToString();
        }

        #endregion

    }
}
