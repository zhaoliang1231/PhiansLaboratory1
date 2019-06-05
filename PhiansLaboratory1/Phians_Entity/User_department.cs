using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 用户部门
    /// </summary>
    [PetaPoco.TableName("TB_User_department")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_User_department
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
        public Guid User_id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 部门id
        /// </summary>		
        public Guid Departmentid
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

    }
}