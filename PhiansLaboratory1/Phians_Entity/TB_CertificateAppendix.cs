using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //证书附件
    [PetaPoco.TableName("TB_CertificateAppendix")]
    [PetaPoco.PrimaryKey("Id",AutoIncrement=false)]

    public class TB_CertificateAppendix
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
        /// CertificateId
        /// </summary>		
        public Guid CertificateId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// DocumentName
        /// </summary>		
        public string DocumentName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// DocumentUrl
        /// </summary>		
        public string DocumentUrl
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// DocumentFormat
        /// </summary>		
        public string DocumentFormat
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// CreateDate(创建时间)
        /// </summary>		
        public DateTime? CreateDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// CreatePersonnel(创建)
        /// </summary>		
        public Guid CreatePersonnel
        {
            get;
            set;
        }

        [ResultColumn]
        ///<summary>
        /// CreatePersonnel(创建)
        /// </summary>		
        public string CreatePersonnel_n
        {
            get;
            set;
        }

    }
}