using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //人员证书管理
    [PetaPoco.TableName("TB_CertificateManagement")]
    [PetaPoco.PrimaryKey("Id", AutoIncrement = true)]

    public class TB_CertificateManagement
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
        /// UserId（用户id）
        /// </summary>		
        public Guid UserId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Certificate Num(证书编号)
        /// </summary>		
        public string CertificateNum
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Certificate Name（证书名字）
        /// </summary>		
        public string CertificateName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Certificate Type(证书类型)
        /// </summary>		
        public Guid CertificateType
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Issuing Unit（发证单位）
        /// </summary>		
        public string IssuingUnit
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Issue Date（发证时间）
        /// </summary>		
        public DateTime? IssueDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Valid Date（有效时间）
        /// </summary>		
        public DateTime? ValidDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Profession(专业）
        /// </summary>		
        public string Profession
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Quarters(岗位）
        /// </summary>		
        public string Quarters
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Grade(等级)
        /// </summary>		
        public string Grade
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// CertificateSate(证书状态）
        /// </summary>		
        public int CertificateSate
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
        [Column]
        ///<summary>
        /// remarks(说明)
        /// </summary>		
        public string remarks
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 用户名
        /// </summary>		
        public string UserName_n
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 证书名
        /// </summary>		
        public string CertificateType_n
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 添加人
        /// </summary>		
        public string CreatePersonnel_n
        {
            get;
            set;
        }
    }
}