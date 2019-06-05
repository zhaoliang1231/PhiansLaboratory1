using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    public class TB_PendingTransaction
    {
        [Column]
        ///<summary>
        /// 待处理事务名称
        /// </summary>		
        public string TransactionName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 页面id
        /// </summary>		
        public string PageId
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 事务url
        /// </summary>		
        public string TransactionUrl
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 事务个数
        /// </summary>		
        public int TransactionCount
        {
            get;
            set;
        }
    }
}
