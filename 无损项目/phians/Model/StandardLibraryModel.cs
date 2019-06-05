using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phians.Model
{
    public class StandardLibraryModel
    {
        //试验名称
        public string test_type { get; set; }
       // 样品编号
        public string test_num 
        {
            get 
            {
                return "";
            }
            set { }
        }
        //测试条件
        public string test_condition { get; set; }
        
        //测试温度
        public string test_temperature { get; set; }
        //方法
        public string Method { get; set; }
        //标准数据
        public string PassStandard { get; set; }
        //热处理状态
        public string heat_treat { get; set; }
        //牌号
        public string type_num { get; set; }
        //等级
        public string level_ { get; set; } 
         //样品数量
        public string sample_amount { get; set; }
        //取样位置
        public string sampling_point { get; set; }
        //标准id
        public string id { get; set; }
        
    }
}