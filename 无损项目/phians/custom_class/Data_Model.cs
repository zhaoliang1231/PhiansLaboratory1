using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.custom_class
{
    //表格填充数据
    public class Data_Model2
    {

        //零件id
        public string S_Number { get; set; }


        //检测项目
        public string Dimensioning { get; set; }

        //加工尺寸
        public string DimSizeGoal { get; set; }
        //频次
        public string MeasureFrequency { get; set; }
        //测量值
        public string MeasureValue { get; set; }


    }

}