using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_NDT_probe_library
    [PetaPoco.TableName("TB_NDT_probe_library")]
    [PetaPoco.PrimaryKey("id",AutoIncrement=true)]

    /// <summary>
    /// 探头库
    /// </summary>
    public class TB_NDT_probe_library
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
        /// 探头名称
        /// </summary>	
        [Column]
        public string Probe_name
        {
            get;
            set;
        }

        ///<summary>
        /// 探头编号
        /// </summary>	
        [Column]
        public string Probe_num
        {
            get;
            set;
        }

        ///<summary>
        /// 探头类型
        /// </summary>	
        [Column]
        public string Probe_type
        {
            get;
            set;
        }

        ///<summary>
        /// 探头尺寸
        /// </summary>	
        [Column]
        public string Probe_size
        {
            get;
            set;
        }

        ///<summary>
        /// 频率
        /// </summary>	
        [Column]
        public string Probe_frequency
        {
            get;
            set;
        }

        ///<summary>
        /// 线圈尺寸
        /// </summary>	
        [Column]
        public string Coil_Size
        {
            get;
            set;
        }

        ///<summary>
        /// 探头长度
        /// </summary>	
        [Column]
        public string Probe_Length
        {
            get;
            set;
        }

        ///<summary>
        /// 探头扩展线缆长度
        /// </summary>	
        [Column]
        public string Cable_Length
        {
            get;
            set;
        }

        ///<summary>
        /// 波型L
        /// </summary>	
        [Column]
        public string Mode_L
        {
            get;
            set;
        }

        ///<summary>
        /// 晶片尺寸
        /// </summary>	
        [Column]
        public string Chip_size
        {
            get;
            set;
        }

        ///<summary>
        /// 角度
        /// </summary>	
        [Column]
        public string Angle
        {
            get;
            set;
        }

        ///<summary>
        /// 标称角度
        /// </summary>	
        [Column]
        public string Nom_Angle
        {
            get;
            set;
        }

        ///<summary>
        /// 楔块
        /// </summary>	
        [Column]
        public string Shoe
        {
            get;
            set;
        }

        ///<summary>
        /// 制造商
        /// </summary>	
        [Column]
        public string Probe_Manufacture
        {
            get;
            set;
        }

        ///<summary>
        /// 状态
        /// </summary>	
        [Column]
        public int Probe_state
        {
            get;
            set;
        }

        ///<summary>
        /// 波型X
        /// </summary>	
        [Column]
        public string Mode_T
        {
            get;
            set;
        }     
        ///<summary>
        /// 单S/双口
        /// </summary>	
        [Column]
        public string DoublePort
        {
            get;
            set;
        }

        ///<summary>
        /// 聚焦
        /// </summary>	
        [Column]
        public string FocalLength
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
        /// 曲面
        /// </summary>	
        [Column]
        public string CurvedSurface
        {
            get;
            set;
        }

        ///<summary>
        /// 周向/轴向
        /// </summary>	
        [Column]
        public string Circumferential
        {
            get;
            set;
        }

        ///<summary>
        /// 位置
        /// </summary>	
        [Column]
        public string Position
        {
            get;
            set;
        }

        ///<summary>
        /// 半径
        /// </summary>	
        [Column]
        public string Radius
        {
            get;
            set;
        }

        ///<summary>
        /// 说明
        /// </summary>	
        [Column]
        public string remarks
        {
            get;
            set;
        }        

    }
}