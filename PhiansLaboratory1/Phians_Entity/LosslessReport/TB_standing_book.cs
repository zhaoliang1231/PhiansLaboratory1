using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_standing_book
    [PetaPoco.TableName("TB_standing_book")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_standing_book
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
        /// 器具名称
        /// </summary>	
        [Column]
        public string sample_name
        {
            get;
            set;
        }

        ///<summary>
        /// 型号规格
        /// </summary>	
        [Column]
        public string sample_type
        {
            get;
            set;
        }

        ///<summary>
        /// measuring_range
        /// </summary>	
        [Column]
        public string measuring_range
        {
            get;
            set;
        }

        ///<summary>
        /// 制造厂商
        /// </summary>	
        [Column]
        public string manufacturers
        {
            get;
            set;
        }

        ///<summary>
        /// factory_date
        /// </summary>	
        [Column]
        public DateTime factory_date
        {
            get;
            set;
        }

        ///<summary>
        /// 出厂编号
        /// </summary>	
        [Column]
        public string factory_num
        {
            get;
            set;
        }

        ///<summary>
        /// 内部编号
        /// </summary>	
        [Column]
        public string asset_num
        {
            get;
            set;
        }

        ///<summary>
        /// 条形编码
        /// </summary>	
        [Column]
        public string bar_coding
        {
            get;
            set;
        }

        ///<summary>
        /// using_place
        /// </summary>	
        [Column]
        public string using_place
        {
            get;
            set;
        }

        ///<summary>
        /// 管理人员
        /// </summary>	
        [Column]
        public string management
        {
            get;
            set;
        }

        ///<summary>
        /// 管理状态
        /// </summary>	
        [Column]
        public string management_state
        {
            get;
            set;
        }

        ///<summary>
        /// 生效日期
        /// </summary>	
        [Column]
        public DateTime effective_date
        {
            get;
            set;
        }

        ///<summary>
        /// 单位名称
        /// </summary>	
        [Column]
        public string customer_name
        {
            get;
            set;
        }

        ///<summary>
        /// customer_code
        /// </summary>	
        [Column]
        public string customer_code
        {
            get;
            set;
        }

        ///<summary>
        /// 单位权属
        /// </summary>	
        [Column]
        public string customer_ownership
        {
            get;
            set;
        }

        ///<summary>
        /// 管理类别
        /// </summary>	
        [Column]
        public string management_type
        {
            get;
            set;
        }

        ///<summary>
        /// 监控类别
        /// </summary>	
        [Column]
        public string monitor_type
        {
            get;
            set;
        }

        ///<summary>
        /// 计量类别
        /// </summary>	
        [Column]
        public string meterage_type
        {
            get;
            set;
        }

        ///<summary>
        /// 溯源单位
        /// </summary>	
        [Column]
        public string tracing_name
        {
            get;
            set;
        }

        ///<summary>
        /// 溯源方式
        /// </summary>	
        [Column]
        public string tracing_method
        {
            get;
            set;
        }

        ///<summary>
        /// Accuracy
        /// </summary>	
        [Column]
        public string Accuracy
        {
            get;
            set;
        }

        ///<summary>
        /// 证书编号
        /// </summary>	
        [Column]
        public string certificate_num
        {
            get;
            set;
        }

        ///<summary>
        /// 检定日期
        /// </summary>	
        [Column]
        public DateTime verification_date
        {
            get;
            set;
        }

        ///<summary>
        /// 确认间隔
        /// </summary>	
        [Column]
        public int verification_cycle
        {
            get;
            set;
        }

        ///<summary>
        /// 有效日期
        /// </summary>	
        [Column]
        public DateTime verification_effective_date
        {
            get;
            set;
        }

        ///<summary>
        /// 检定单位
        /// </summary>	
        [Column]
        public string verification_unit
        {
            get;
            set;
        }

        ///<summary>
        /// 检定人员
        /// </summary>	
        [Column]
        public string verification_personnel
        {
            get;
            set;
        }

        ///<summary>
        /// 检定结论
        /// </summary>	
        [Column]
        public string verification_result
        {
            get;
            set;
        }

        ///<summary>
        /// count
        /// </summary>	
        [Column]
        public int count
        {
            get;
            set;
        }

        ///<summary>
        /// 服务价格
        /// </summary>	
        [Column]
        public int charge
        {
            get;
            set;
        }

        ///<summary>
        /// material_code
        /// </summary>	
        [Column]
        public string material_code
        {
            get;
            set;
        }

        ///<summary>
        /// 备注
        /// </summary>	
        [Column]
        public string remarks
        {
            get;
            set;
        }

        ///<summary>
        /// 备注
        /// </summary>	
        [Column]
        public string remarks1
        {
            get;
            set;
        }

        ///<summary>
        /// 备注
        /// </summary>	
        [Column]
        public string remarks2
        {
            get;
            set;
        }

        ///<summary>
        /// exception_date
        /// </summary>	
        [Column]
        public DateTime exception_date
        {
            get;
            set;
        }

        ///<summary>
        /// instrument_state
        /// </summary>	
        [Column]
        public bool instrument_state
        {
            get;
            set;
        }

    }
}