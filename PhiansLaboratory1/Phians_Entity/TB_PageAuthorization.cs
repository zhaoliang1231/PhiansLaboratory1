using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 页面授权
    /// </summary>
    [PetaPoco.TableName("TB_PageAuthorization")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_PageAuthorization
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
        /// 用户id
        /// </summary>		
        public Guid GroupId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// SystemId
        /// </summary>		
        public Guid SystemId
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
        //[Column]
        /////<summary>
        ///// 被授权帐户名称
        ///// </summary>		
        //public string username
        //{
        //    get;
        //    set;
        //}
        [Column]
        ///<summary>
        /// 页面id
        /// </summary>		
        public Guid PageId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 页面id
        /// </summary>		
        public bool Flag
        {
            get;
            set;
        }



    }
}