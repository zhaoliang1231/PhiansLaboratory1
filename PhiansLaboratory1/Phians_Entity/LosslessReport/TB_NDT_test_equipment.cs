using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_NDT_test_equipment
    [PetaPoco.TableName("TB_NDT_test_equipment")]
    [PetaPoco.PrimaryKey("id",AutoIncrement=true)]

    public class TB_NDT_test_equipment
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
        /// equipment_id
        /// </summary>	
        [Column]
        public string equipment_id
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
        /// 设备类型
        /// </summary>	
        [Column]
        public string equipment_name_R
        {
            get;
            set;
        }

        ///<summary>
        /// equipment_name
        /// </summary>	
        [Column]
        public string equipment_name
        {
            get;
            set;
        }

        ///<summary>
        /// equipment_Type
        /// </summary>	
        [Column]
        public string equipment_Type
        {
            get;
            set;
        }

        ///<summary>
        /// equipment_num
        /// </summary>	
        [Column]
        public string equipment_num
        {
            get;
            set;
        }

        ///<summary>
        /// range_
        /// </summary>	
        [Column]
        public string range_
        {
            get;
            set;
        }

        ///<summary>
        /// Manufacture
        /// </summary>	
        [Column]
        public string Manufacture
        {
            get;
            set;
        }

        ///<summary>
        /// effective
        /// </summary>	
        [Column]
        public DateTime ? effective
        {
            get;
            set;
        }

        ///<summary>
        /// Remarks
        /// </summary>	
        [Column]
        public string Remarks
        {
            get;
            set;
        }

    }
}