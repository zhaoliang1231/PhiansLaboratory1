using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_Unit
    [PetaPoco.TableName("TB_Unit")]
    //[PetaPoco.PrimaryKey("article_id")]

    public class TB_Unit
    {

        [Column]
        ///<summary>
        /// ID
        /// </summary>		
        public Guid ID
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Type
        /// </summary>		
        public Guid Type
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnitSource
        /// </summary>		
        public string UnitSource
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnitSourceName
        /// </summary>		
        public string UnitSourceName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnitConversion
        /// </summary>		
        public decimal UnitConversion
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnitTarget
        /// </summary>		
        public string UnitTarget
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// UnitTargetName
        /// </summary>		
        public string UnitTargetName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// SortNum
        /// </summary>		
        public int SortNum
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 类型名称
        /// </summary>		
        public string Type_n
        {
            get;
            set;
        }

    }
}