using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //TB_NDT_report_title
    [PetaPoco.TableName("TB_NDT_report_title")]
    [PetaPoco.PrimaryKey("id",AutoIncrement=true)]

    public class TB_NDT_report_title
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
        /// clientele_department
        /// </summary>	
        [Column]
        public string clientele_department
        {
            get;
            set;
        }

        ///<summary>
        /// clientele
        /// </summary>	
        [Column]
        public string clientele
        {
            get;
            set;
        }

        ///<summary>
        /// application_num
        /// </summary>	
        [Column]
        public string application_num
        {
            get;
            set;
        }

        ///<summary>
        /// Project_name
        /// </summary>	
        [Column]
        public string Project_name
        {
            get;
            set;
        }

        ///<summary>
        /// Subassembly_name
        /// </summary>	
        [Column]
        public string Subassembly_name
        {
            get;
            set;
        }

        ///<summary>
        /// Material
        /// </summary>	
        [Column]
        public string Material
        {
            get;
            set;
        }

        ///<summary>
        /// Type_
        /// </summary>	
        [Column]
        public string Type_
        {
            get;
            set;
        }

        ///<summary>
        /// Chamfer_type
        /// </summary>	
        [Column]
        public string Chamfer_type
        {
            get;
            set;
        }

        ///<summary>
        /// Drawing_num
        /// </summary>	
        [Column]
        public string Drawing_num
        {
            get;
            set;
        }

        ///<summary>
        /// Procedure_
        /// </summary>	
        [Column]
        public string Procedure_
        {
            get;
            set;
        }

        ///<summary>
        /// Inspection_context
        /// </summary>	
        [Column]
        public string Inspection_context
        {
            get;
            set;
        }

        ///<summary>
        /// Inspection_opportunity
        /// </summary>	
        [Column]
        public string Inspection_opportunity
        {
            get;
            set;
        }

        ///<summary>
        /// circulation_NO
        /// </summary>	
        [Column]
        public string circulation_NO
        {
            get;
            set;
        }

        ///<summary>
        /// procedure_NO
        /// </summary>	
        [Column]
        public string procedure_NO
        {
            get;
            set;
        }

        ///<summary>
        /// apparent_condition
        /// </summary>	
        [Column]
        public string apparent_condition
        {
            get;
            set;
        }

        ///<summary>
        /// manufacturing_process
        /// </summary>	
        [Column]
        public string manufacturing_process
        {
            get;
            set;
        }

        ///<summary>
        /// Batch_Num
        /// </summary>	
        [Column]
        public string Batch_Num
        {
            get;
            set;
        }

        ///<summary>
        /// Inspection_NO
        /// </summary>	
        [Column]
        public string Inspection_NO
        {
            get;
            set;
        }

        ///<summary>
        /// Inspection_date
        /// </summary>	
        [Column]
        public DateTime ? Inspection_date
        {
            get;
            set;
        }

        ///<summary>
        /// Inspection_personnel_date
        /// </summary>	
        [Column]
        public DateTime? Inspection_personnel_date
        {
            get;
            set;
        }

        ///<summary>
        /// Inspection_personnel
        /// </summary>	
        [Column]
        public string Inspection_personnel
        {
            get;
            set;
        }

        ///<summary>
        /// level_Inspection
        /// </summary>	
        [Column]
        public string level_Inspection
        {
            get;
            set;
        }

        ///<summary>
        /// Audit_personnel
        /// </summary>	
        [Column]
        public string Audit_personnel
        {
            get;
            set;
        }

        ///<summary>
        /// Audit_date
        /// </summary>	
        [Column]
        public DateTime? Audit_date
        {
            get;
            set;
        }

        ///<summary>
        /// return_info
        /// </summary>	
        [Column]
        public string return_info
        {
            get;
            set;
        }

        ///<summary>
        /// level_Audit
        /// </summary>	
        [Column]
        public string level_Audit
        {
            get;
            set;
        }

        ///<summary>
        /// issue_personnel
        /// </summary>	
        [Column]
        public string  issue_personnel
        {
            get;
            set;
        }

        ///<summary>
        /// issue_date
        /// </summary>	
        [Column]
        public DateTime ? issue_date
        {
            get;
            set;
        }

        ///<summary>
        /// laboratorian_
        /// </summary>	
        [Column]
        public string laboratorian_
        {
            get;
            set;
        }

        ///<summary>
        /// laboratorian_date
        /// </summary>	
        [Column]
        public DateTime? laboratorian_date
        {
            get;
            set;
        }

        ///<summary>
        /// figure
        /// </summary>	
        [Column]
        public bool figure
        {
            get;
            set;
        }

        ///<summary>
        /// disable_report_num
        /// </summary>	
        [Column]
        public string  disable_report_num
        {
            get;
            set;
        }

        ///<summary>
        /// report_num
        /// </summary>	
        [Column]
        public string report_num
        {
            get;
            set;
        }

        ///<summary>
        /// report_format
        /// </summary>	
        [Column]
        public string report_format
        {
            get;
            set;
        }

        ///<summary>
        /// report_name
        /// </summary>	
        [Column]
        public string report_name
        {
            get;
            set;
        }

        ///<summary>
        /// report_url
        /// </summary>	
        [Column]
        public string report_url
        {
            get;
            set;
        }

        ///<summary>
        /// Inspection_result
        /// </summary>	
        [Column]
        public string Inspection_result
        {
            get;
            set;
        }

        ///<summary>
        /// remarks
        /// </summary>	
        [Column]
        public string remarks
        {
            get;
            set;
        }

        ///<summary>
        /// 节点
        /// </summary>	
        [Column]
        public int state_
        {
            get;
            set;
        }

        ///<summary>
        /// tm_id
        /// </summary>	
        [Column]
        public string tm_id
        {
            get;
            set;
        }

        ///<summary>
        /// Tubes_Size
        /// </summary>	
        [Column]
        public string Tubes_Size
        {
            get;
            set;
        }

        ///<summary>
        /// Tubes_num
        /// </summary>	
        [Column]
        public string Tubes_num
        {
            get;
            set;
        }

        ///<summary>
        /// welding_method
        /// </summary>	
        [Column]
        public string welding_method
        {
            get;
            set;
        }

        ///<summary>
        /// return_flag
        /// </summary>	
        [Column]
        public int return_flag
        {
            get;
            set;
        }

        ///<summary>
        /// Job_num
        /// </summary>	
        [Column]
        public string Job_num
        {
            get;
            set;
        }

        ///<summary>
        /// heat_treatment
        /// </summary>	
        [Column]
        public string heat_treatment
        {
            get;
            set;
        }

        ///<summary>
        /// Work_instruction
        /// </summary>	
        [Column]
        public string Work_instruction
        {
            get;
            set;
        }

        ///<summary>
        /// 型号
        /// </summary>	
        [Column]
        public string ModelNum
        {
            get;
            set;
        }

        ///<summary>
        /// Audit_groupid
        /// </summary>	
        [Column]
        public int Audit_groupid
        {
            get;
            set;
        }

        ///<summary>
        /// 报告创建时间
        /// </summary>	
        [Column]
        public DateTime ? ReportCreationTime
        {
            get;
            set;
        }

        ///<summary>
        /// 状态(1开始；0未开始)
        /// </summary>	
        [Column]
        public int Condition
        {
            get;
            set;
        }

        ///<summary>
        /// 是否逾期
        /// </summary>	
        [Column]
        public bool IsOverdue
        {
            get;
            set;
        }

        ///<summary>
        /// 是否报废 true为报废状态
        /// </summary>	
        [Column]
        public bool IsScrap
        {
            get;
            set;
        }

        ///<summary>
        /// 是否线下 true为线下报告
        /// </summary>	
        [Column]
        public bool IsUnderLine
        {
            get;
            set;
        }

        //=====报告数据========================================================================================

                ///<summary>
        /// id
        /// </summary>	
        [ResultColumn]
        public int tp_id
        {
            get;
            set;
        }

        ///<summary>
        /// report_num
        /// </summary>	
        [ResultColumn]
        public int tp_report_num
        {
            get;
            set;
        }

        ///<summary>
        /// tm_id
        /// </summary>	
        [ResultColumn]
        public string tp_tm_id
        {
            get;
            set;
        }

        ///<summary>
        /// Data1
        /// </summary>	
        [ResultColumn]
        public string Data1
        {
            get;
            set;
        }

        ///<summary>
        /// Data2
        /// </summary>	
        [ResultColumn]
        public string Data2
        {
            get;
            set;
        }

        ///<summary>
        /// Data3
        /// </summary>	
        [ResultColumn]
        public string Data3
        {
            get;
            set;
        }

        ///<summary>
        /// Data4
        /// </summary>	
           [ResultColumn]
        public string Data4
        {
            get;
            set;
        }

        ///<summary>
        /// Data5
        /// </summary>	
        [ResultColumn]
        public string Data5
        {
            get;
            set;
        }

        ///<summary>
        /// Data6
        /// </summary>	
        [ResultColumn]
        public string Data6
        {
            get;
            set;
        }

        ///<summary>
        /// Data7
        /// </summary>	
        [ResultColumn]
        public string Data7
        {
            get;
            set;
        }

        ///<summary>
        /// Data8
        /// </summary>	
        [ResultColumn]
        public string Data8
        {
            get;
            set;
        }

        ///<summary>
        /// Data9
        /// </summary>	
        [ResultColumn]
        public string Data9
        {
            get;
            set;
        }

        ///<summary>
        /// Data10
        /// </summary>	
        [ResultColumn]
        public string Data10
        {
            get;
            set;
        }

        ///<summary>
        /// Data11
        /// </summary>	
        [ResultColumn]
        public string Data11
        {
            get;
            set;
        }

        ///<summary>
        /// Data12
        /// </summary>	
        [ResultColumn]
        public string Data12
        {
            get;
            set;
        }

        ///<summary>
        /// Data13
        /// </summary>	
        [ResultColumn]
        public string Data13
        {
            get;
            set;
        }

        ///<summary>
        /// Data14
        /// </summary>	
        [ResultColumn]
        public string Data14
        {
            get;
            set;
        }

        ///<summary>
        /// Data15
        /// </summary>	
        [ResultColumn]
        public string Data15
        {
            get;
            set;
        }

        ///<summary>
        /// Data16
        /// </summary>	
        [ResultColumn]
        public string Data16
        {
            get;
            set;
        }

        ///<summary>
        /// Data17
        /// </summary>	
        [ResultColumn]
        public string Data17
        {
            get;
            set;
        }

        ///<summary>
        /// Data18
        /// </summary>	
        [ResultColumn]
        public string Data18
        {
            get;
            set;
        }

        ///<summary>
        /// Data19
        /// </summary>	
        [ResultColumn]
        public string Data19
        {
            get;
            set;
        }

        ///<summary>
        /// Data20
        /// </summary>	
        [ResultColumn]
        public string Data20
        {
            get;
            set;
        }

        ///<summary>
        /// Data21
        /// </summary>	
        [ResultColumn]
        public string Data21
        {
            get;
            set;
        }

        ///<summary>
        /// Data22
        /// </summary>	
        [ResultColumn]
        public string Data22
        {
            get;
            set;
        }

        ///<summary>
        /// Data23
        /// </summary>	
        [ResultColumn]
        public string Data23
        {
            get;
            set;
        }

        ///<summary>
        /// Data24
        /// </summary>	
        [ResultColumn]
        public string Data24
        {
            get;
            set;
        }

        ///<summary>
        /// Data25
        /// </summary>	
        [ResultColumn]
        public string Data25
        {
            get;
            set;
        }

        ///<summary>
        /// Data26
        /// </summary>	
        [ResultColumn]
        public string Data26
        {
            get;
            set;
        }

        ///<summary>
        /// Data27
        /// </summary>	
        [ResultColumn]
        public string Data27
        {
            get;
            set;
        }

        ///<summary>
        /// Data28
        /// </summary>	
        [ResultColumn]
        public string Data28
        {
            get;
            set;
        }

        ///<summary>
        /// Data29
        /// </summary>	
        [ResultColumn]
        public string Data29
        {
            get;
            set;
        }

        ///<summary>
        /// Data30
        /// </summary>	
        [ResultColumn]
        public string Data30
        {
            get;
            set;
        }

        ///<summary>
        /// Data31
        /// </summary>	
        [ResultColumn]
        public string Data31
        {
            get;
            set;
        }

        ///<summary>
        /// Data32
        /// </summary>	
        [ResultColumn]
        public string Data32
        {
            get;
            set;
        }

        ///<summary>
        /// Data33
        /// </summary>	
        [ResultColumn]
        public string Data33
        {
            get;
            set;
        }

        ///<summary>
        /// Data34
        /// </summary>	
        [ResultColumn]
        public string Data34
        {
            get;
            set;
        }

        ///<summary>
        /// Data35
        /// </summary>	
        [ResultColumn]
        public string Data35
        {
            get;
            set;
        }

        ///<summary>
        /// Data36
        /// </summary>	
        [ResultColumn]
        public string Data36
        {
            get;
            set;
        }

        ///<summary>
        /// Data37
        /// </summary>	
        [ResultColumn]
        public string Data37
        {
            get;
            set;
        }

        ///<summary>
        /// Data38
        /// </summary>	
        [ResultColumn]
        public string Data38
        {
            get;
            set;
        }

        ///<summary>
        /// Data39
        /// </summary>	
        [ResultColumn]
        public string Data39
        {
            get;
            set;
        }

        ///<summary>
        /// Data40
        /// </summary>	
        [ResultColumn]
        public string Data40
        {
            get;
            set;
        }

        ///<summary>
        /// Data41
        /// </summary>	
        [ResultColumn]
        public string Data41
        {
            get;
            set;
        }

        ///<summary>
        /// Data42
        /// </summary>	
        [ResultColumn]
        public string Data42
        {
            get;
            set;
        }

        ///<summary>
        /// Data43
        /// </summary>	
        [ResultColumn]
        public string Data43
        {
            get;
            set;
        }

        ///<summary>
        /// Data44
        /// </summary>	
        [ResultColumn]
        public string Data44
        {
            get;
            set;
        }

        ///<summary>
        /// Data45
        /// </summary>	
        [ResultColumn]
        public string Data45
        {
            get;
            set;
        }

        ///<summary>
        /// Data46
        /// </summary>	
        [ResultColumn]
        public string Data46
        {
            get;
            set;
        }

        ///<summary>
        /// Data47
        /// </summary>	
        [ResultColumn]
        public string Data47
        {
            get;
            set;
        }

        ///<summary>
        /// Data48
        /// </summary>	
        [ResultColumn]
        public string Data48
        {
            get;
            set;
        }

        ///<summary>
        /// Data49
        /// </summary>	
        [ResultColumn]
        public string Data49
        {
            get;
            set;
        }

        ///<summary>
        /// Data50
        /// </summary>	
        [ResultColumn]
        public string Data50
        {
            get;
            set;
        }

        ///<summary>
        /// Data51
        /// </summary>	
        [ResultColumn]
        public string Data51
        {
            get;
            set;
        }

        ///<summary>
        /// Data52
        /// </summary>	
        [ResultColumn]
        public string Data52
        {
            get;
            set;
        }

        ///<summary>
        /// Data53
        /// </summary>	
        [ResultColumn]
        public string Data53
        {
            get;
            set;
        }

        ///<summary>
        /// Data54
        /// </summary>	
        [ResultColumn]
        public string Data54
        {
            get;
            set;
        }

        ///<summary>
        /// Data55
        /// </summary>	
        [ResultColumn]
        public string Data55
        {
            get;
            set;
        }

        ///<summary>
        /// Data56
        /// </summary>	
        [ResultColumn]
        public string Data56
        {
            get;
            set;
        }

        ///<summary>
        /// Data57
        /// </summary>	
        [ResultColumn]
        public string Data57
        {
            get;
            set;
        }

        ///<summary>
        /// Data58
        /// </summary>	
        [ResultColumn]
        public string Data58
        {
            get;
            set;
        }

        ///<summary>
        /// Data59
        /// </summary>	
        [ResultColumn]
        public string Data59
        {
            get;
            set;
        }

        ///<summary>
        /// Data60
        /// </summary>	
        [ResultColumn]
        public string Data60
        {
            get;
            set;
        }

        ///<summary>
        /// Data61
        /// </summary>	
        [ResultColumn]
        public string Data61
        {
            get;
            set;
        }

        ///<summary>
        /// Data62
        /// </summary>	
        [ResultColumn]
        public string Data62
        {
            get;
            set;
        }

        ///<summary>
        /// Data63
        /// </summary>	
        [ResultColumn]
        public string Data63
        {
            get;
            set;
        }

        ///<summary>
        /// Data64
        /// </summary>	
        [ResultColumn]
        public string Data64
        {
            get;
            set;
        }


        //========================================================
        ///<summary>
        /// 编制人
        /// </summary>	
        [ResultColumn]
        public string Inspection_personnel_n
        {
            get;
            set;
        }

        ///<summary>
        /// 审核人
        /// </summary>	
        [ResultColumn]
        public string Audit_personnel_n
        {
            get;
            set;
        }

        ///<summary>
        /// 签发人
        /// </summary>	
        [ResultColumn]
        public string issue_personnel_n
        {
            get;
            set;
        }

        ///<summary>
        /// 组名称
        /// </summary>	
        [ResultColumn]
        public string Audit_groupid_n
        {
            get;
            set;
        }

        ///<summary>
        /// 流程操作时间
        /// </summary>	
        [ResultColumn]
        public DateTime? OperateDate
        {
            get;
            set;
        }

    }
}