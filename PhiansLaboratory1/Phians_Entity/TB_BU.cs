using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{


    /// <summary>
    /// BU绑定
    /// </summary>
    [PetaPoco.TableName("TB_BU")]
    [PetaPoco.PrimaryKey("ID",AutoIncrement=true)]

    public class TB_BU
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
        /// UserId（用户id）
        /// </summary>		
        public Guid UserId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// BUID（BU）
        /// </summary>		
        public string BU
        {
            get;
            set;
        }

    }
}