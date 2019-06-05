using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_BLL.Enum
{
    public enum NDE_report_review//4为完成状态  3为签发状态 当报告二级审核状态提交后为完成状态 三级审核后状态为签发状态
    {

        /// <summary>
        /// 目视报告模板_VT_63  ******  2.doc  VT
        /// </summary>
        VT = 3,

        /// <summary>
        /// DT模板空白_4版   *******  3.docx  DT
        /// </summary>
        DT = 4,

        /// <summary>
        /// PT-液体渗透检验报告  ******** 26.doc  PT
        /// </summary>
        PT = 3,

        /// <summary>
        /// MT-磁轭法和触头法磁粉检验报告3版123 / MT-磁粉检验报告床式 ***** 7.doc/ 8.doc   MT
        /// </summary>
        MT = 3,

        /// <summary>
        /// UT-超声波测厚报告/UT-超声波检验报告1-正页  ***** 10.doc/11.doc  UT
        /// </summary>
        UT = 3,

        /// <summary>
        /// RT-射线检验报告1  ***** 27.doc  RT
        /// </summary>
        RT = 3,

        /// <summary>
        /// LT-氦检漏报告模板_4版123  ***** 6.doc  LT
        /// </summary>
        LT = 3,

        /// <summary>
        /// ECT-涡流检验报告模板RPV / ECT-涡流检验报告模板SG  ***** 4.doc/5.doc  ECT
        /// </summary>
        ECT = 3,

        /// <summary>
        /// 水压试验报告模板21  ***** 12.doc  Water_pressure
        /// </summary>
        Water_pressure = 4,

        /// <summary>
        /// 热处理模板3  ***** 1.docx  Heat_treatment
        /// </summary>
        Heat_treatment = 4
    }
}
