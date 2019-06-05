using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace phians.test
{
    /// <summary>
    /// tree_json_test 的摘要说明
    /// </summary>
    public class tree_json_test : IHttpHandler
    {
       
        public void ProcessRequest(HttpContext context)
        {
            SQLHelper sql2 = new SQLHelper();
           // DataTable dt= sql2.ExecuteDataTable("select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info");
            string strsql = "select M_num,Project_num,S_num from dbo.TB_laboratory_Qualification_info";
            DataSet ds1 = sql2.GetDataSet(strsql);
            DataTable dt = ds1.Tables[0];
            GetTreeJsonByTable(dt, "M_num", "Project_name", "S_num", "0");
          string content1 = result.ToString();
          context.Response.Write(content1);
        }

        /// <summary>
        /// 根据DataTable生成Json树结构
        /// </summary>
        /// <param name="tabel">数据源</param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela">关系字段</param>
        /// <param name="pId">父ID</param>
        StringBuilder result = new StringBuilder();
        StringBuilder sb = new StringBuilder(); 
        private void GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId)
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
                        sb.Append("{\"id\":\"" + row[idCol] + "\",\"text\":\"" + row[txtCol] + "\",\"state\":\"open\"");
                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            sb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol]);
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
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}