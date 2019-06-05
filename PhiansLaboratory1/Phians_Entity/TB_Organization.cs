using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 组织机构
    /// </summary>
    [PetaPoco.TableName("TB_Organization")]
    [PetaPoco.PrimaryKey("NodeId")]

    public class TB_Organization
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
        [Column]
        ///<summary>
        /// 公司id
        /// </summary>		
        public Guid NodeId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 父id
        /// </summary>		
        public Guid ParentId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 节点名称
        /// </summary>		
        public string NodeName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 组织机构代码
        /// </summary>		
        public string OrganizationCode
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
        /// 节点类型
        /// </summary>		
        public int NodeType
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
        /// 电话
        /// </summary>		
        public string Phone
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 邮编
        /// </summary>		
        public string PostCode
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 排序号
        /// </summary>		
        public int SortNum
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

    }
}