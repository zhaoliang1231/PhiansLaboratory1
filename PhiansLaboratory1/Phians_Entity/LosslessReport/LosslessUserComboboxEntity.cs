using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;

namespace Phians_Entity.LosslessReport
{
    public class LosslessUserComboboxEntity
    {
        [Column]
        ///<summary>
        /// value
        /// </summary>		
        public string Value
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Text
        /// </summary>		
        public string Text
        {
            get;
            set;
        }
    }
}
