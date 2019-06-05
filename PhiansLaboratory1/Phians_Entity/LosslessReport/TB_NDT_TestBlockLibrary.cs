using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //试块库
    [PetaPoco.TableName("TB_NDT_TestBlockLibrary")]
    [PetaPoco.PrimaryKey("ID", AutoIncrement = true)]

    public class TB_NDT_TestBlockLibrary
    {

        [Column]
        ///<summary>
        /// 序号
        /// </summary>		
        public int ID
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 试块guid
        /// </summary>		
        public Guid CalBlockID
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 试块编号
        /// </summary>		
        public string CalBlockNum
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 校验试块
        /// </summary>		
        public string CalBlock
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 校验表面
        /// </summary>		
        public string C_S
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 仪器设置
        /// </summary>		
        public string InstrumentSet
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 参考反射体
        /// </summary>		
        public string Reflector
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 添加人
        /// </summary>		
        public string CreatePerson
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 添加时间
        /// </summary>		
        public DateTime CreateTime
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// 状态（1在用；0停用）
        /// </summary>		
        public int State
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

    }
}