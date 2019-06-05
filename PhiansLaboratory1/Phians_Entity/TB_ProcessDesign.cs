using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
   /// <summary>
    ///  测试流程设计
   /// </summary>
    [PetaPoco.TableName("TB_ProcessDesign")]
    [PetaPoco.PrimaryKey("id",AutoIncrement=true)]

    public class TB_ProcessDesign
    {

        [Column]
        ///<summary>
        /// id
        /// </summary>		
        public Guid id
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// MTRNO（单号）
        /// </summary>		
        public string MTRNO
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// ProcessPicUrl （保存路径）
        /// </summary>		
        public string Url
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// ProcessContent（保存内容）
        /// </summary>		
        public string ProcessContent
        {
            get;
            set;
        }

    }
}