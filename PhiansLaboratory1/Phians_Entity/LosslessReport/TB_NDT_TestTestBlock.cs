using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //试验试块
    [PetaPoco.TableName("TB_NDT_TestTestBlock")]
    //[PetaPoco.PrimaryKey("article_id")]

    public class TB_NDT_TestTestBlock
    {

        ///<summary>
        /// 序号
        /// </summary>

        [Column]
        public int ID
        {
            get;
            set;
        }
        ///<summary>
        /// 报告ID
        /// </summary>

        [Column]
        public int ReportID
        {
            get;
            set;
        }
        ///<summary>
        /// 试验探头ID
        /// </summary>

        [Column]
        public int ProbeID
        {
            get;
            set;
        }
        ///<summary>
        /// 实测角度
        /// </summary>

        [Column]
        public string Angle
        {
            get;
            set;
        }
        ///<summary>
        /// 前沿
        /// </summary>

        [Column]
        public string TFront
        {
            get;
            set;
        }
        ///<summary>
        /// 试块guid
        /// </summary>

        [Column]
        public Guid CalBlockID
        {
            get;
            set;
        }
        ///<summary>
        /// 试块编号
        /// </summary>

        [Column]
        public string CalBlockNum
        {
            get;
            set;
        }
        ///<summary>
        /// 校验试块
        /// </summary>

        [Column]
        public string CalBlock
        {
            get;
            set;
        }
        ///<summary>
        /// 校验表面
        /// </summary>

        [Column]
        public string C_S
        {
            get;
            set;
        }
        ///<summary>
        /// 仪器设置
        /// </summary>

        [Column]
        public string InstrumentSet
        {
            get;
            set;
        }
        ///<summary>
        /// 参考反射体
        /// </summary>

        [Column]
        public string Reflector
        {
            get;
            set;
        }

    }
}