using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //消息注册服务
    [PetaPoco.TableName("TB_MessageRegister")]
    [PetaPoco.PrimaryKey("ID", AutoIncrement = true)]

    public class TB_MessageRegister
    {

        [Column]
        ///<summary>
        /// ID
        /// </summary>		
        public int ID
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
        /// ConnectionId(消息ID)
        /// </summary>		
        public Guid ConnectionId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// RegisterDate(上线时间)
        /// </summary>		
        public DateTime RegisterDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// RegisterFlag
        /// </summary>		
        public bool RegisterFlag
        {
            get;
            set;
        }

    }
}