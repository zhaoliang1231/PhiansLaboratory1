using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 组授权
    /// </summary>
    [PetaPoco.TableName("TB_groupAuthorization")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_groupAuthorization
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
        /// 组id
        /// </summary>		
        public int Group_id
        {
            get;
            set;
        }
        [Column]
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
        /// 用户id
        /// </summary>		
        public Guid UserId
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// Group Leader(组长标识)0非组长，1组长　默认0
        /// </summary>		
        public bool GroupLeader
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// UserCount 用户账户
        /// </summary>		
        public string UserCount
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// UserName 用户名
        /// </summary>		
        public string UserName
        {
            get;
            set;
        }
    }
}