using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
   /// <summary>
   /// 报告使用的探头
   /// </summary>
    [PetaPoco.TableName("TB_NDT_test_probe")]
    [PetaPoco.PrimaryKey("id",AutoIncrement=true)]

    public class TB_NDT_test_probe
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
        public string report_id
        {
            get;
            set;
        }
        ///<summary>
        /// probe_id
        /// </summary>

        [Column]
        public string probe_id
        {
            get;
            set;
        }
          ///<summary>
        /// 探头名字
        /// </summary>

        [ResultColumn]
        public string Probe_name
        {
            get;
            set;
        }
        ///<summary>
        /// 探头编号
        /// </summary>

        [ResultColumn]
        public string Probe_num
        {
            get;
            set;
        }
        ///<summary>
        /// 探头类型
        /// </summary>		
        public string Probe_type
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 探头尺寸
        /// </summary>		
        public string Probe_size
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 频率
        /// </summary>		
        public string Probe_frequency
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 线圈尺寸
        /// </summary>		
        public string Coil_Size
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 探头长度
        /// </summary>		
        public string Probe_Length
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 探头扩展线缆长度
        /// </summary>		
        public string Cable_Length
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 波型L
        /// </summary>		
        public string Mode_L
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 晶片尺寸
        /// </summary>		
        public string Chip_size
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 角度
        /// </summary>		
        public string Angle
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 标称角度
        /// </summary>		
        public string Nom_Angle
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 楔块
        /// </summary>		
        public string Shoe
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 制造商
        /// </summary>		
        public string Probe_Manufacture
        {
            get;
            set;
        }
        //[Column]
        /////<summary>
        ///// 状态
        ///// </summary>		
        //public int Probe_state
        //{
        //    get;
        //    set;
        //}
        [Column]
        ///<summary>
        /// 波型X
        /// </summary>		
        public string Mode_T
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 单S/双口
        /// </summary>		
        public string DoublePort
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 聚焦
        /// </summary>		
        public string FocalLength
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 前沿
        /// </summary>		
        public string TFront
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 曲面
        /// </summary>		
        public string CurvedSurface
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 周向/轴向
        /// </summary>		
        public string Circumferential
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 位置
        /// </summary>		
        public string Position
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 半径
        /// </summary>		
        public string Radius
        {
            get;
            set;
        }        
    }
}