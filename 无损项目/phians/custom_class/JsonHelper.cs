using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace phians
{
    public class JsonHelper
    {
        /// <summary>
        /// dataset 转 json
        /// </summary>
        /// <param name="ds">包含总数的dataset</param>
        /// <returns></returns>
        public string DataSetToJson(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"total\":");
            if (ds != null)
                json.Append(ds.Tables[1].Rows[0][0]);
            else
                json.Append("0");
            json.Append(",\"rows\":[");
            if (ds != null)
                json.Append(DataTableToJson(ds.Tables[0]));
            json.Append("]}");
            return json.ToString();
        }

        /// <summary>   
        /// dataTable转换成Json格式   
        /// </summary>   
        /// <paramname="dt"></param>   
        ///<returns></returns>   
        public string DataTableToJson(DataTable dt)
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
                    string text_ = dt.Rows[i][j].ToString().Trim().Replace("\\", "\\\\");
                    text_ = text_.Replace("\r", "");
                    text_ = text_.Replace("\n", "");
                    text_ = text_.Replace("\"", "'");
                    text_ = text_.Trim();
                    text_ = text_.Replace("\t", "");
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

            return jsonBuilder.ToString();
        }
        public string DataTable2ToJson(DataTable dt)
        {

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
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
                    text_ = text_.Replace("\"", "'");

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
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }
    }
}