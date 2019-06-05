using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //版本报告储存
    [PetaPoco.TableName("TB_NDT_EditionCertificate")]
    [PetaPoco.PrimaryKey("id", AutoIncrement = true)]

    public class TB_NDT_EditionCertificate
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
        /// 报告id
        /// </summary>	
        [Column]
        public int ReportId
        {
            get;
            set;
        }

        ///<summary>
        /// 报告编号
        /// </summary>	
        [Column]
        public string ReportNum
        {
            get;
            set;
        }

        ///<summary>
        /// 报告格式
        /// </summary>	
        [Column]
        public string CertificateFormat
        {
            get;
            set;
        }

        ///<summary>
        /// 报告url
        /// </summary>	
        [Column]
        public string CertificateUrl
        {
            get;
            set;
        }

        ///<summary>
        /// 存储人
        /// </summary>	
        [Column]
        public string Operator
        {
            get;
            set;
        }

        ///<summary>
        /// 存储时间
        /// </summary>	
        [Column]
        public DateTime? OperationDate
        {
            get;
            set;
        }

        ///<summary>
        /// 说明
        /// </summary>	
        [Column]
        public string Remarks
        {
            get;
            set;
        }

    }
}