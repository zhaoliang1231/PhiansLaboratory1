using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Phians_Entity
{
    public class MESInfo
    {

        ///<summary>
        /// 流转卡号/ST+工序号
        /// </summary>	
        public string WORKID
        {
            get;
            set;
        }
        ///<summary>
        /// 图号
        /// </summary>	
        public string DRAWING_NUM
        {
            get;
            set;
        }
        ///<summary>
        /// 订单号
        /// </summary>	
        public string APPLICATION_NUM
        {
            get;
            set;
        }
        ///<summary>
        /// 部件名称
        /// </summary>	
        public string SUBASSEMBLY_NAME
        {
            get;
            set;
        }
        ///<summary>
        /// 检验内容
        /// </summary>	
        public string INSPECTION_CONTEXT
        {
            get;
            set;
        }
        ///<summary>
        /// 项目代号
        /// </summary>	
        public string PROJECT_NUM
        {
            get;
            set;
        }
        ///<summary>
        /// 检验规程
        /// </summary>	
        public string PROCEDURE_
        {
            get;
            set;
        }


    }
}