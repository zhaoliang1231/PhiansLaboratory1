using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [PetaPoco.TableName("TB_OperationLog")]
    [PetaPoco.PrimaryKey("id")]

    public class OperationLog
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
        [ResultColumn]
        ///<summary>
        ///  临时操作时间
        /// </summary>		
        public string OperationDate_
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 操作时间
        /// </summary>		
        public string OperationDate
        {
            get
            {
                if (!string.IsNullOrEmpty(OperationDate_))
                {
                    OperationDate_ = Convert.ToDateTime(OperationDate_.Trim()).ToString("yyyy-MM-dd hh:mm:ss").ToString();
                    return OperationDate_;
                }
                else { return OperationDate_; }
            }
            set
            {
                OperationDate_ = value;
            }
        }
        [Column]
        ///<summary>
        /// 操作类型
        /// </summary>		
        public string OperationType
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 操作内容
        /// </summary>		
        public string OperationInfo
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 操作ip
        /// </summary>		
        public string OperationIp
        {
            get;
            set;
        }
     
     
      
 
         ///<summary>
        /// UserName 用户表 User_info 用户名字
        /// </summary>		
        /// 
          [ResultColumn]
        public string UserName
        {
            get;
            set;
        }
        

    }
}