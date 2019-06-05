using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //报告文档版本记录表
    [PetaPoco.TableName("TB_NDT_RevisionsRecord")]
    [PetaPoco.PrimaryKey("id", AutoIncrement = true)]

    public class TB_NDT_RevisionsRecord
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
        public string report_url
        {
            get;
            set;
        }

        ///<summary>
        /// ReturnNode
        /// </summary>	
        [Column]
        public int ReturnNode
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

        ///<summary>
        /// addpersonnel
        /// </summary>	
        [Column]
        public string addpersonnel
        {
            get;
            set;
        }

        //===========================================================================================

        ///<summary>
        /// addpersonnel
        /// </summary>	
        [ResultColumn]
        public string addpersonnel_n
        {
            get;
            set;
        }

    }
}