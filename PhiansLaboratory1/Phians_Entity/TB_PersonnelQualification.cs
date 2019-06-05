using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //人员资质
    [PetaPoco.TableName("TB_PersonnelQualification")]
    [PetaPoco.PrimaryKey("ID", AutoIncrement = false)]

    public class TB_PersonnelQualification
    {

        ///<summary>
        /// ID
        /// </summary>

        [Column]
        public Guid ID
        {
            get;
            set;
        }
        ///<summary>
        /// 模板id
        /// </summary>

        [Column]
        public int TemplateID
        {
            get;
            set;
        }
        ///<summary>
        /// 人员id
        /// </summary>

        [Column]
        public Guid UserId
        {
            get;
            set;
        }
        ///<summary>
        /// AuthorizationType
        /// </summary>

        [Column]
        public int AuthorizationType
        {
            get;
            set;
        }
        ///<summary>
        /// 添加人
        /// </summary>

        [Column]
        public Guid AddPersonnel
        {
            get;
            set;
        }
        ///<summary>
        /// 添加时间
        /// </summary>

        [Column]
        public DateTime AddDate
        {
            get;
            set;
        }
        ///<summary>
        /// 备注
        /// </summary>

        [Column]
        public string Remarks
        {
            get;
            set;
        }

        ///<summary>
        /// 人员名字
        /// </summary>

        [ResultColumn]
        public string UserName
        {
            get;
            set;
        }

        ///<summary>
        /// 人员名字
        /// </summary>

        [ResultColumn]
        public string UserCount
        {
            get;
            set;
        }
    }
}