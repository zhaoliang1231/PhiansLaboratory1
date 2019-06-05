using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 消息
    /// </summary>
    [PetaPoco.TableName("TB_Message")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_Message
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
        public Guid UserId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 消息类型
        /// </summary>		
        public string MessageType
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 消息内容
        /// </summary>		
        public string Message
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 创建时间
        /// </summary>		
        public DateTime? CreateTime
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 确认时间
        /// </summary>		
        public DateTime ? ConfirmTime
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 发送人
        /// </summary>		
        public Guid PushPersonnel
        {
            get;
            set;
        }
        //用户表（TB_UserInfo） 用户名字
          [ResultColumn]
        public string UserName
        {
            get;
            set;
        }
        //用户表（TB_UserInfo） 创建人名字
          [ResultColumn]
        public string PushPersonname
        {
            get;
            set;
        }

    }
}