using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;

namespace Phians_Entity
{
    //TB_user_info
    [PetaPoco.TableName("TB_user_info")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_user_info
    {


        ///<summary>
        /// id
        /// </summary>	
        [Column]
        public int id
        {
            get;
            set;
        }

        ///<summary>
        /// 用户
        /// </summary>	
        [Column]
        public string User_count
        {
            get;
            set;
        }

        ///<summary>
        /// 密码
        /// </summary>	
        [Column]
        public string User_pwd
        {
            get;
            set;
        }

        ///<summary>
        /// 姓名
        /// </summary>	
        [Column]
        public string User_name
        {
            get;
            set;
        }

        ///<summary>
        /// 性别
        /// </summary>	
        [Column]
        public string User_sex
        {
            get;
            set;
        }

        ///<summary>
        /// department
        /// </summary>	
        [Column]
        public string department
        {
            get;
            set;
        }

        ///<summary>
        /// department_code
        /// </summary>	
        [Column]
        public string department_code
        {
            get;
            set;
        }

        ///<summary>
        /// 部门
        /// </summary>	
        [Column]
        public int User_department
        {
            get;
            set;
        }

        ///<summary>
        /// 排序号
        /// </summary>	
        [Column]
        public int sort_num
        {
            get;
            set;
        }

        ///<summary>
        /// 职务
        /// </summary>	
        [Column]
        public string User_job
        {
            get;
            set;
        }

        ///<summary>
        /// 职责
        /// </summary>	
        [Column]
        public string User_duties
        {
            get;
            set;
        }

        ///<summary>
        /// 座机
        /// </summary>	
        [Column]
        public string Tel
        {
            get;
            set;
        }

        ///<summary>
        /// 手机
        /// </summary>	
        [Column]
        public string Phone
        {
            get;
            set;
        }

        ///<summary>
        /// 传真
        /// </summary>	
        [Column]
        public string Fax
        {
            get;
            set;
        }

        ///<summary>
        /// Email
        /// </summary>	
        [Column]
        public string Email
        {
            get;
            set;
        }

        ///<summary>
        /// QQ
        /// </summary>	
        [Column]
        public string QQ
        {
            get;
            set;
        }

        ///<summary>
        /// MSN
        /// </summary>	
        [Column]
        public string MSN
        {
            get;
            set;
        }

        ///<summary>
        /// Address
        /// </summary>	
        [Column]
        public string Address
        {
            get;
            set;
        }

        ///<summary>
        /// Postcode
        /// </summary>	
        [Column]
        public string Postcode
        {
            get;
            set;
        }

        ///<summary>
        /// 签名图样
        /// </summary>	
        [Column]
        public string Signature
        {
            get;
            set;
        }

        ///<summary>
        /// 签名格式
        /// </summary>	
        [Column]
        public string Signature_format
        {
            get;
            set;
        }

        ///<summary>
        /// 附注
        /// </summary>	
        [Column]
        public string Remarks
        {
            get;
            set;
        }

        ///<summary>
        /// 账户状态（启用停用）
        /// </summary>	
        [Column]
        public string User_count_state
        {
            get;
            set;
        }

    }
}