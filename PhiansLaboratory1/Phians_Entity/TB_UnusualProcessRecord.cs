using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //异常流程记录
    [PetaPoco.TableName("TB_UnusualProcessRecord")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_UnusualProcessRecord
    {

        [ResultColumn]
        ///<summary>
        /// id
        /// </summary>		
        public Guid id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnusualCertificateID（流程Id）
        /// </summary>		
        public Guid UnusualCertificateID
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Operator（操作人）
        /// </summary>		
        public Guid Operator
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// OperateDate（操作时间）
        /// </summary>		
        public DateTime? OperateDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// NodeResult（流程节点操作结果　pass或者fail）
        /// </summary>		
        public string NodeResult
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// NodeId(流程节点id)
        /// </summary>		
        public int NodeId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Remark（说明）
        /// </summary>		
        public string Remark
        {
            get;
            set;
        }

    }
}