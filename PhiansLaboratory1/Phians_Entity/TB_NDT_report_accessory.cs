using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_NDT_report_accessory
    [PetaPoco.TableName("TB_NDT_report_accessory")]
    [PetaPoco.PrimaryKey("id",AutoIncrement=true)]

    public class TB_NDT_report_accessory
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
        /// accessory_name
        /// </summary>

        [Column]
        public string accessory_name
        {
            get;
            set;
        }
        ///<summary>
        /// accessory_url
        /// </summary>

        [Column]
        public string accessory_url
        {
            get;
            set;
        }
        ///<summary>
        /// add_personnel
        /// </summary>

        [Column]
        public string add_personnel
        {
            get;
            set;
        }
        ///<summary>
        /// add_date
        /// </summary>

        [Column]
        public DateTime add_date
        {
            get;
            set;
        }
        ///<summary>
        /// accessory_format
        /// </summary>

        [Column]
        public string accessory_format
        {
            get;
            set;
        }
        ///<summary>
        /// remarks
        /// </summary>

        [Column]
        public string remarks
        {
            get;
            set;
        }

    }
}