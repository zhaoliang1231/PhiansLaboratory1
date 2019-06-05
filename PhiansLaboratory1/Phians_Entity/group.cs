using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 组管理
    /// </summary>
    [PetaPoco.TableName("TB_group")]
    [PetaPoco.PrimaryKey("GroupId")]

    public class TB_group
    {

        [ResultColumn]
        ///<summary>
        /// id
        /// </summary>		
        public int id
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 用户id
        /// </summary>		
        public Guid GroupId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 父id
        /// </summary>		
        public Guid GroupParentId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 部门id
        /// </summary>		
        public string GroupName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// CreateDate
        /// </summary>		
        public DateTime CreateDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// CreatePersonnel
        /// </summary>		
        public Guid CreatePersonnel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 公司id
        /// </summary>		
        public string Remarks
        {
            get;
            set;
        }
       [ResultColumn]
        ///<summary>
        /// 是否为组长
        /// </summary>		
        public bool GroupLeader
        {
            get;
            set;
        }
        

    }
}