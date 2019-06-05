using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_NDT_equipment_library
    [PetaPoco.TableName("TB_NDT_equipment_library")]
    [PetaPoco.PrimaryKey("id", AutoIncrement = true)]

    public class TB_NDT_equipment_library
    {

        [Column]
        ///<summary>
        /// id
        /// </summary>		
        public int id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// report_id
        /// </summary>		
        public int report_id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// equipment_nem
        /// </summary>		
        public string equipment_nem
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// equipment_Type
        /// </summary>		
        public string equipment_Type
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// equipment_num
        /// </summary>		
        public string equipment_num
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// range_
        /// </summary>		
        public string range_
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Manufacture
        /// </summary>		
        public string Manufacture
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// effective
        /// </summary>		
        public DateTime ? effective
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// E_state
        /// </summary>		
        public int E_state
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Remarks
        /// </summary>		
        public string Remarks
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// personnel_
        /// </summary>		
        public string personnel_
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// personnel_time
        /// </summary>		
        public DateTime? personnel_time
        {
            get;
            set;
        }

    }
}