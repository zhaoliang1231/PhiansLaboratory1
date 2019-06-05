using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [PetaPoco.TableName("TB_UserInfo")]
    [PetaPoco.PrimaryKey("UserId", AutoIncrement = true)]

    public class TB_UserInfo
    {
        [ResultColumn]
        ///<summary>
        /// id
        /// </summary>		
        public int? id
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
        /// 用户帐户
        /// </summary>		
        public string UserCount
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 用户密码
        /// </summary>		
        public string UserPwd
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 用户名称
        /// </summary>		
        public string UserName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 用户性别
        /// </summary>		
        public bool UserNsex
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 电话
        /// </summary>		
        public string Tel
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 手机
        /// </summary>		
        public string Phone
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 传真
        /// </summary>		
        public string Fax
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 电子邮件
        /// </summary>		
        public string Email
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 邮编
        /// </summary>		
        public string Postcode
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// QQ
        /// </summary>		
        public string QQ
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 地址
        /// </summary>		
        public string Address
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 签名
        /// </summary>		
        public string Signature
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 用户头像
        /// </summary>		
        public string Portrait
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 说明
        /// </summary>		
        public string Remarks
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 工号
        /// </summary>		
        public string JobNum
        {
            get;
            set;
        }

        [ResultColumn]
        ///<summary>
        /// 排序号
        /// </summary>		
        public int? sort_num
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// CreateDate
        /// </summary>		
        public DateTime? CreateDate
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
        /// 状态
        /// </summary>		
        public int CountState
        {
            get;
            set;
        }
        /// <summary>
        /// Profession 专业
        /// </summary>
        [Column]
        public string Profession
        {
            get;
            set;
        }
        /// <summary>
        /// Job (Title) 职位 
        /// </summary>
        [Column]
        public string Job
        {
            get;
            set;
        }

        #region 新加

        /// <summary>
        /// 部门 
        /// </summary>
        [Column]
        public string department
        {
            get;
            set;
        }

        /// <summary>
        /// 部门代码
        /// </summary>
        [Column]
        public string department_code
        {
            get;
            set;
        }

        /// <summary>
        /// 用户部门 
        /// </summary>
        [Column]
        public string User_department
        {
            get;
            set;
        }

        /// <summary>
        /// 职责
        /// </summary>
        [Column]
        public string User_duties
        {
            get;
            set;
        }

        /// <summary>
        ///  MSN
        /// </summary>
        [Column]
        public string MSN
        {
            get;
            set;
        }

        /// <summary>
        /// 签名格式
        /// </summary>
        [Column]
        public string Signature_format
        {
            get;
            set;
        }
        /// <summary>
        /// 使用状态
        /// </summary>
        [Column]
        public string User_count_state
        {
            get;
            set;
        }

        #endregion

        [ResultColumn]
        ///<summary>
        /// 组授权id
        /// </summary>		
        public int Authorization_id
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 组id
        /// </summary>		
        public Guid GroupId
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// 部门id
        /// </summary>		
        public string GroupName
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// Group Leader(组长标识)0非组长，1组长　默认0
        /// </summary>		
        public bool GroupLeader
        {
            get;
            set;
        }

        [ResultColumn]
        public string NodeName
        {
            get;
            set;
        }
        /// <summary>
        ///   职位
        /// </summary>
        [ResultColumn]

        public string Position
        {
            get;
            set;
        }

        /// <summary>
        ///   创建人
        /// </summary>
        [ResultColumn]
        public string CreatePersonnel_n
        {
            get;
            set;
        }
        /// <summary>
        /// BU
        /// </summary>
        [ResultColumn]

        public string BU
        {
            get;
            set;
        }

        [ResultColumn]
        public DateTime? HireDate
        {
            get;
            set;
        }
        ///<summary>
        /// 离职时间
        /// </summary>

        [ResultColumn]
        public DateTime? LeaveDate
        {
            get;
            set;
        }
        ///<summary>
        /// 设备组合名（）
        /// </summary>

        [ResultColumn]
        public string EquipmentManager
        {
            get;
            set;
        }
    }
}