using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //报告流程记录
    [PetaPoco.TableName("TB_ProcessRecord")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_ProcessRecord
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
        /// ReportID
        /// </summary>	
        [Column]
        public int ReportID
        {
            get;
            set;
        }

        ///<summary>
        /// Operator（操作人）
        /// </summary>	
        [Column]
        public string Operator
        {
            get;
            set;
        }

        ///<summary>
        /// OperateDate（操作时间）
        /// </summary>	
        [Column]
        public DateTime OperateDate
        {
            get;
            set;
        }

        ///<summary>
        /// NodeResult（流程节点操作结果　pass或者fail）
        /// </summary>	
        [Column]
        public string NodeResult
        {
            get;
            set;
        }

        ///<summary>
        /// NodeId(流程节点id)
        /// </summary>	
        [Column]
        public int NodeId
        {
            get;
            set;
        }

        ///<summary>
        /// Remark（返回原因）
        /// </summary>	
        [Column]
        public string Remark
        {
            get;
            set;
        }

        ///<summary>
        /// OverdueSetup（逾期设置）
        /// </summary>	
        [Column]
        public int OverdueSetup
        {
            get;
            set;
        }

        ///<summary>
        /// IsOverdue（是否逾期）
        /// </summary>	
        [Column]
        public bool IsOverdue
        {
            get;
            set;
        }

        ///<summary>
        /// OverdueTime(逾期时间)
        /// </summary>	
        [Column]
        public float OverdueTime
        {
            get;
            set;
        }

        ///<summary>
        /// TimeConsuming(耗时)
        /// </summary>	
        [Column]
        public float TimeConsuming
        {
            get;
            set;
        }

        ///<summary>
        /// TakeBack（拉回标识)
        /// </summary>	
        [Column]
        public bool TakeBack
        {
            get;
            set;
        }

        ///<summary>
        /// 报告创建时间
        /// </summary>	
        [ResultColumn]
        public DateTime ReportCreationTime
        {
            get;
            set;
        }

    }
}