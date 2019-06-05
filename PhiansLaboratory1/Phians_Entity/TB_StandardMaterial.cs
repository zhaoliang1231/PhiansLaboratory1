using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 标准物质
    /// </summary>
    [PetaPoco.TableName("TB_StandardMaterial")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_StandardMaterial
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
        /// manufacturers(物料编号)
        /// </summary>		
        public string MaterialNum
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// MaterialName(物料名字)
        /// </summary>		
        public string MaterialName
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// MurchaseDate(购进时间)
        /// </summary>		
        public string MurchaseDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// manufacturers（制造厂商）
        /// </summary>		
        public string Manufacturers
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Type（规格）
        /// </summary>		
        public string MateriaType
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// DetectionProject(检测项目)
        /// </summary>		
        public string DetectionProject
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Manager(管理人)
        /// </summary>		
        public Guid Manager
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Operator（使用人）
        /// </summary>		
        public Guid Operator
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Maintenance Frequency(保养时间)
        /// </summary>		
        public int MaintenanceFrequency
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Location（存放位置）
        /// </summary>		
        public Guid Location
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Inspection Unit（检定单位）
        /// </summary>		
        public string InspectionUnit
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Inspection Date（检定日期）
        /// </summary>		
        public string InspectionDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Valid Date（有效日期）
        /// </summary>		
        public string ValidDate
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Inspection Result(检定结果)
        /// </summary>		
        public string InspectionResult
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Certificaten Num（证书编号）
        /// </summary>		
        public string CertificatenNum
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// Use Times(使用次数)
        /// </summary>		
        public int UseTimes
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// MaterialState（状态）
        /// </summary>		
        public int MaterialState
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// remarks
        /// </summary>		
        public string Remarks
        {
            get;
            set;
        }
        [Column]
        ///<summary>
        /// MRS
        /// </summary>		
        public string MRS
        {
            get;
            set;
        }

        [ResultColumn]
        ///<summary>
        /// 管理者姓名
        /// </summary>		
        public string ManagerName
        {
            get;
            set;
        }

        [ResultColumn]
        ///<summary>
        /// 操作人姓名
        /// </summary>		
        public string OperatorName
        {
            get;
            set;
        }
        [ResultColumn]
        ///<summary>
        /// Location_n(仓库url)
        /// </summary>		
        public string Location_n
        {
            get;
            set;
        }

    }
}