using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    public class HandleData
    {
        public List<StandardData> GetAllData(DataTable dt)
        {
            List<StandardData> list = new List<StandardData>();


            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 1; i < 11; i++)
                {
                    StandardData item = new StandardData();
                    item.name = "requireData" + i;
                    //是否测试试验指标
                    if (dr["test" + i].ToString() == "True")
                    {
                       
                        if (dr["data" + (i * 4)].ToString() == "True")
                        {
                            item.value = "info.";
                        }
                        else
                        {
                            if (dr["data" + (2 + (i - 1) * 4)].ToString() != "" && dr["data" + (3 + (i - 1) * 4)].ToString() != "")
                                item.value = dr["data" + (2 + (i - 1) * 4)] + "-" + dr["data" + (3 + (i - 1) * 4)];
                            if (dr["data" + (2 + (i - 1) * 4)].ToString() != "" && dr["data" + (3 + (i - 1) * 4)].ToString() == "")
                                item.value = "≥" + dr["data" + (2 + (i - 1) * 4)];
                            if (dr["data" + (2 + (i - 1) * 4)].ToString() == "" && dr["data" + (3 + (i - 1) * 4)].ToString() != "")
                                item.value = "≤" + dr["data" + (3 + (i - 1) * 4)];
                        }

                      
                    }
                        //不测试的试验指标默认是/N/A
                    else {
                        item.value = "N/A";
                    }

                    list.Add(item);
                }
            }
            return list;
        }

        public  List<T> ConvertIListToList<T>(System.Collections.IList gbList) where T : class
        {
            if (gbList != null && gbList.Count > 0)
            {
                List<T> list = new List<T>();
                for (int i = 0; i < gbList.Count; i++)
                {
                    T temp = gbList[i] as T;
                    if (temp != null)
                        list.Add(temp);
                }
                return list;
            }
            return null;
        }


    }

    public class StandardData
    {
        public string name { get; set; }

        public string value { get; set; }
    }

}