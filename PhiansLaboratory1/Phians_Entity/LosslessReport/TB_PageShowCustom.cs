using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //页面字段显示自定义
    [PetaPoco.TableName("TB_PageShowCustom")]
    [PetaPoco.PrimaryKey("id", AutoIncrement = true)]

    public class TB_PageShowCustom
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
        /// PageId（页面id）
        /// </summary>	
        [Column]
        public int PageId
        {
            get;
            set;
        }

        ///<summary>
        /// FieldName（字段）
        /// </summary>	
        [Column]
        public string FieldName
        {
            get;
            set;
        }

        ///<summary>
        /// Title（字段名称）
        /// </summary>	
        [Column]
        public string Title
        {
            get;
            set;
        }

        ///<summary>
        /// FieldSort（字段顺序）
        /// </summary>	
        [Column]
        public int FieldSort
        {
            get;
            set;
        }

        ///<summary>
        /// hidden（是否显示）
        /// </summary>	
        [Column]
        public bool hidden
        {
            get;
            set;
        }

        ///<summary>
        /// Sortable（是否排序）
        /// </summary>	
        [Column]
        public bool Sortable
        {
            get;
            set;
        }

        ///<summary>
        /// Operator（定义人）
        /// </summary>	
        [Column]
        public string Operator
        {
            get;
            set;
        }

        ///<summary>
        /// OperateDate（定义时间）
        /// </summary>	
        [Column]
        public DateTime ? OperateDate
        {
            get;
            set;
        }

        ///<summary>
        /// Remark（备注）
        /// </summary>	
        [Column]
        public string Remark
        {
            get;
            set;
        }

    }
}