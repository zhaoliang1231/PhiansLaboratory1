using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 功能模块
    /// </summary>
    [PetaPoco.TableName("TB_FunctionalModule")]
    [PetaPoco.PrimaryKey("PageId", AutoIncrement = false)]

    public class TB_FunctionalModule
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
        /// PageId
        /// </summary>		
        public Guid PageId
        {
            get;
            set;
        }
      
        ///<summary>
        /// 父id
        /// </summary>	
          [Column]
        public Guid ParentId
        {
            get;
            set;
        }
       
        ///<summary>
        /// 节点类型
        /// </summary>		
         [Column]
        public int NodeType
        {
            get;
            set;
        }
      
        ///<summary>
        /// 图标
        /// </summary>	
         [Column]
        public string IconCls
        {
            get;
            set;
        }
    
        ///<summary>
        /// 页面ulr
        /// </summary>		
          [Column]
        public string U_url
        {
            get;
            set;
        }
      
        ///<summary>
        /// 显示名称
        /// </summary>	
        [Column]
        public string ModuleName
        {
            get;
            set;
        }
      
        ///<summary>
        /// sort_num
        /// </summary>	
        [Column]
        public int SortNum
        {
            get;
            set;
        }
    
        ///<summary>
        /// remarks
        /// </summary>	
         [Column]
        public string Remarks
        {
            get;
            set;
        }
      
        ///<summary>
        /// 是否需要详细配置权限
        /// </summary>
        [Column]
        public bool PermissionFlag
        {
            get;
            set;
        }
         ///<summary>
        /// 是否父节点
        /// </summary>		
        public bool isParent
        {
            get;
            set;
        }

   
        ///<summary>
        /// 是否父节点
        /// </summary>		
        /// 
        //public Guid _parentId
        //{
        //    get;
        //    set;
        //}
      
        ///<summary>
        /// 返回按钮是否已经授权——用于给前端判断，未授权则为null
        /// </summary>		
      [ResultColumn]
        public int IdFlag
        {
            get;
            set;
        }
    }
}