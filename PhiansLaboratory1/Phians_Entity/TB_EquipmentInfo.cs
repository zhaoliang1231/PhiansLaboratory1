using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace Phians_Entity
{
    /// <summary>
    /// 设备管理
    /// </summary>
    [PetaPoco.TableName("TB_EquipmentInfo")]
    [PetaPoco.PrimaryKey("NO",AutoIncrement=true)]
    public class TB_EquipmentInfo
    {
        [Column]
        ///<summary>
        /// 序号
        /// </summary>		
        public int NO
        {
            get;
            set;
        }


        [Column]
        ///<summary>
        /// 设备编号
        /// </summary>		
        public string EquipmentCode
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 设备名称
        /// </summary>		
        public string EquipmentName
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 制造厂商
        /// </summary>		
        public string Manufactor
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 型号规格
        /// </summary>		
        public string EquipmentType
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 出厂编号
        /// </summary>		
        public string ManufactoryNum
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 资产编号
        /// </summary>		
        public string AsetNo
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 检测对象
        /// </summary>		
        public string TestObjective
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 使用范围
        /// </summary>		
        public string UsageRange
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 测量范围
        /// </summary>		
        public string TestRange
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 溯源方法
        /// </summary>		
        public string TraceabilityType
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 送检价格
        /// </summary>		
        public string Price
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 校准周期
        /// </summary>		
        public string CalibrationPeroid
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 证书编号
        /// </summary>		
        public string CertificateNum
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 校准公司
        /// </summary>		
        public string CalibrationCompany
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 校准时间
        /// </summary>		
        public DateTime? CalibrationDate
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 下次校准时间
        /// </summary>		
        public DateTime? DueDate
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 使用投入时间
        /// </summary>		
        public DateTime? DateOfLaunch
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 使用位置
        /// </summary>		
        public Guid Address
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 状况
        /// </summary>		
        public string Status
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 管理人
        /// </summary>		
        public Guid Manager
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// 状态
        /// </summary>		
        public int StatusFlag
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// IO
        /// </summary>		
        public string IO
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// IDN
        /// </summary>		
        public string IDN
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// CalibrationResult
        /// </summary>		
        public string CalibrationResult
        {
            get;
            set;
        }

        [Column]
        ///<summary>
        /// OwnerEquipment  设备归属者
        /// </summary>		
        public Guid OwnerEquipment
        {
            get;
            set;
        }

         [Column]
        ///<summary>
        /// DataLocation  资料位置
        /// </summary>		
        public string DataLocation
        {
            get;
            set;
        }
         [Column]
         ///<summary>
         /// Remark  说明
         /// </summary>		
         public string Remark
         {
             get;
             set;
         }
        [ResultColumn]
        ///<summary>
        /// OwnerEquipment 设备归属者姓名
        /// </summary>		
        public string OwnerEquipmentName
        {
            get;
            set;
        }
         [ResultColumn]
        ///<summary>
        /// Manager_n 设备管理人
        /// </summary>		
        public string Manager_n
        {
            get;
            set;
        }  
        [ResultColumn]
        ///<summary>
         /// Address_n 地址名
        /// </summary>		
         public string Address_n
        {
            get;
            set;
        } 
    }
}
