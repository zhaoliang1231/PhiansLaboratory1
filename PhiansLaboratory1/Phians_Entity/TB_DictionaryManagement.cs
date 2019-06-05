using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 字典管理
    /// </summary>
    [PetaPoco.TableName("TB_DictionaryManagement")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_DictionaryManagement
    {

        [Column]
        ///<summary>
        /// id
        /// </summary>		
        public Guid id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Parent_id
        /// </summary>		
        public Guid Parent_id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// NodeType
        /// </summary>		
        public string NodeType
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Project_name
        /// </summary>		
        public string Project_name
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Project_value
        /// </summary>		
        public string Project_value
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Sort_num
        /// </summary>		
        public int Sort_num
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// remarks
        /// </summary>		
        public string remarks
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// DictionaryState(状态)
        /// </summary>		
        public int DictionaryState
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// Project_name
        /// </summary>		
        public string ProjectNameValue
        {
            get;
            set;
        }
    }
}