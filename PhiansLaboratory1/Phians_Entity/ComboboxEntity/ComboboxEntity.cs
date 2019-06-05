using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;

namespace Phians_Entity
{
    public class ComboboxEntity
    {
        [Column]
        ///<summary>
        /// value
        /// </summary>		
        public Guid Value
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
