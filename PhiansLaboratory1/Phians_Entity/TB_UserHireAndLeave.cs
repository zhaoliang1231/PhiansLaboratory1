using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
      /// <summary>
      ///  入职和离职日期管理
      /// </summary>
    [PetaPoco.TableName("TB_UserHireAndLeave")]
    [PetaPoco.PrimaryKey("UserId",AutoIncrement=false)]

    public class TB_UserHireAndLeave
    {

        ///<summary>
        /// 用户id
        /// </summary>

        [Column]
        public Guid UserId
        {
            get;
            set;
        }
        ///<summary>
        /// 入职时间
        /// </summary>

        [Column]
        public DateTime ? HireDate
        {
            get;
            set;
        }
        ///<summary>
        /// 离职时间
        /// </summary>

        [Column]
        public DateTime ? LeaveDate
        {
            get;
            set;
        }

    }
}