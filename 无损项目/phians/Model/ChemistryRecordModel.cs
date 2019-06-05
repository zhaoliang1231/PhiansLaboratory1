using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.Model
{
    public class ChemistryRecordModel
    {
        public string id { get; set; }

        public string Standard_id { get; set; }

        //样品编号
        public string sample_num { get; set; }
        //取样位置 记录
        public string sampling_point { get; set; }
        //取样位置 标准
        public string sampling_point_s { get; set; }
        // 实测值
        public string test_data { get; set; }
        //任务编号
        public string task_num { get; set; }
        //元素名称
        public string elements_name { get; set; }

        public string test1 { get; set; }

        private string _requireData = "";
        //最低
        public string data1 { get; set; }
        //最高
        public string data2 { get; set; }
        //是否提供数据
        public string data3 { get; set; }
        //单位
        public string data4 { get; set; }
        //结论
        public string conclusion { get; set; }

        public string requireData
        {
            get
            {

                if (data3 == "True")
                {
                    return "Info";
                }
                else
                {
                    if (data1 != "" && data2 != "")
                        return data1 + "~" + data2;
                    if (data1 != "" && data2 == "")
                        return "≥" + data1;
                    if (data1 == "" && data2 != "")
                        return "≤" + data2;
                    else
                        return _requireData;
                }


            }
            set
            {
                _requireData = value;
            }
        }

    }
}