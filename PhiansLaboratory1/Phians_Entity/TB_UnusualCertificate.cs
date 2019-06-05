using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //异常报告处理
    [PetaPoco.TableName("TB_UnusualCertificate")]
    [PetaPoco.PrimaryKey("Id")]

    public class TB_UnusualCertificate
    {

        [Column]
        ///<summary>
        /// Id
        /// </summary>		
        public Guid Id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// MTRNO(MTRNO)
        /// </summary>		
        public string MTRNO
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// File_forma(证书格式)
        /// </summary>		
        public string FileFormat
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// File_url(证书url)
        /// </summary>		
        public string FileUrl
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Apply_type(申请类型)
        /// </summary>		
        public string ApplyType
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Error_remark(申请信息)
        /// </summary>		
        public string ErrorRemark
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Accept_date(申请人)
        /// </summary>		
        public Guid AcceptPersonnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// AcceptDate(申请时间)
        /// </summary>		
        public DateTime? AcceptDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnusualReviewPersonnel(评审人)
        /// </summary>		
        public Guid UnusualReviewPersonnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnusualReviewDate（评审时间）
        /// </summary>		
        public DateTime? UnusualReviewDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// EditPersonnel(编制人)
        /// </summary>		
        public Guid EditPersonnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// EditDate（编制时间）
        /// </summary>		
        public DateTime? EditDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// ReviewPersonnel(评审人)
        /// </summary>		
        public Guid ReviewPersonnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// ReviewDate（评审时间）
        /// </summary>		
        public DateTime? ReviewDate
        {
            get;
            set;
        }

        [Column]
        /// ApprovedBy(签发人)
        public Guid ApprovedBy
        {
            get;
            set;
        }
        [Column]
        /// ApprovedBy(签发时间)
        public DateTime ? ApprovedDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// AcceptState(处理状态)
        /// </summary>		
        public int AcceptState
        {
            get;
            set;
        }

        [ResultColumn]
        /// Accept_date(申请人)
        public string AcceptPersonnel_n
        {
            get;
            set;
        }
        [ResultColumn]
        /// UnusualReviewPersonnel(评审人)
        public string UnusualReviewPersonnel_n
        {
            get;
            set;
        }
        [ResultColumn]
        /// EditPersonnel(编制人)
        public string EditPersonnel_n
        {
            get;
            set;
        }
        [ResultColumn]
        /// ReviewPersonnel(评审人)
        public string ReviewPersonnel_n
        {
            get;
            set;
        }
          [ResultColumn]
        /// ApprovedBy(签发人)
        public string ApprovedBy_n
        {
            get;
            set;
        }
        
    }
}