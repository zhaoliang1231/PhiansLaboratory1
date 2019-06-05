using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //报告错误原因记录
    [PetaPoco.TableName("TB_NDT_error_log")]
    [PetaPoco.PrimaryKey("id", AutoIncrement = true)]

    public class TB_NDT_error_log
    {


        ///<summary>
        /// id
        /// </summary>	
        [Column]
        public int id
        {
            get;
            set;
        }

        ///<summary>
        /// report_id
        /// </summary>	
        [Column]
        public int report_id
        {
            get;
            set;
        }

        ///<summary>
        /// 错误原因
        /// </summary>	
        [Column]
        public string error_remarks
        {
            get;
            set;
        }

        ///<summary>
        /// 报告编制人
        /// </summary>	
        [Column]
        public string constitute_personnel
        {
            get;
            set;
        }

        ///<summary>
        /// 发现人
        /// </summary>	
        [Column]
        public string addpersonnel
        {
            get;
            set;
        }

        ///<summary>
        /// ReturnNode
        /// </summary>	
        [Column]
        public string ReturnNode
        {
            get;
            set;
        }

        ///<summary>
        /// 发现时间
        /// </summary>	
        [Column]
        public DateTime ? add_date
        {
            get;
            set;
        }

        //===============================================================


        ///<summary>
        /// 返回信息
        /// </summary>	
        [ResultColumn]
        public string return_info
        {
            get;
            set;
        }

        ///<summary>
        /// 发现人
        /// </summary>	
        [ResultColumn]
        public string addpersonnel_n
        {
            get;
            set;
        }


    }
}