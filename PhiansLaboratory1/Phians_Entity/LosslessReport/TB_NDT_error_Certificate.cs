using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_NDT_error_Certificate
    [PetaPoco.TableName("TB_NDT_error_Certificate")]
    [PetaPoco.PrimaryKey("id", AutoIncrement = true)]

    public class TB_NDT_error_Certificate
    {

        [Column]
        ///<summary>
        /// id
        /// </summary>		
        public int id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// report_id
        /// </summary>		
        public string report_id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// File_format
        /// </summary>		
        public string File_format
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// File_url
        /// </summary>		
        public string File_url
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// error_remark
        /// </summary>		
        public string error_remark
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// accept_personnel
        /// </summary>		
        public string accept_personnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// accept_date
        /// </summary>		
        public DateTime ? accept_date
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// review_personnel
        /// </summary>		
        public string review_personnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// review_date
        /// </summary>		
        public DateTime ? review_date
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// review_remarks
        /// </summary>		
        public string review_remarks
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// false_review_remarks
        /// </summary>		
        public string false_review_remarks
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// constitute_personnel
        /// </summary>		
        public string constitute_personnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// constitute_date
        /// </summary>		
        public DateTime ? constitute_date
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// collate_personnel
        /// </summary>		
        public string collate_personnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// collate_date
        /// </summary>		
        public DateTime ? collate_date
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// review_personnel_word
        /// </summary>		
        public string review_personnel_word
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// review_personnel_word_date
        /// </summary>		
        public DateTime ? review_personnel_word_date
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// accept_state
        /// </summary>		
        public int accept_state
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// other_remarks
        /// </summary>		
        public string other_remarks
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// review_remarks_word
        /// </summary>		
        public string review_remarks_word
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 评审人
        /// </summary>		
        public string review_personnel_n
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 申请人
        /// </summary>		
        public string accept_personnel_n
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 报告编号
        /// </summary>		
        public string report_num
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 报告名称
        /// </summary>		
        public string report_name
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 委托人
        /// </summary>		
        public string clientele
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 委托部门
        /// </summary>		
        public string clientele_department
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 报告编制人
        /// </summary>		
        public string constitute_personnel_n
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 报告评审人
        /// </summary>		
        public string review_personnel_word_n
        {
            get;
            set;
        }

        [ResultColumn]
        ///<summary>
        /// 报告id
        /// </summary>		
        public int ReportId
        {
            get;
            set;
        }

        

    }
}