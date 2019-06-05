using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    //模板文件
    [PetaPoco.TableName("TB_TemplateFile")]
    [PetaPoco.PrimaryKey("ID", AutoIncrement = true)]

    public class TB_TemplateFile
    {


        ///<summary>
        /// ID
        /// </summary>	
        [Column]
        public int ID
        {
            get;
            set;
        }

        ///<summary>
        /// FileNum（文件编号）
        /// </summary>	
        [Column]
        public string FileNum
        {
            get;
            set;
        }

        ///<summary>
        /// File Name（文件名）
        /// </summary>	
        [Column]
        public string FileName
        {
            get;
            set;
        }

        ///<summary>
        /// File Type（文件类型））
        /// </summary>	
        [Column]
        public Guid FileType
        {
            get;
            set;
        }

        ///<summary>
        /// ReviewLevel（评审等级：2 or 3）
        /// </summary>	
        [Column]
        public int ReviewLevel
        {
            get;
            set;
        }

        ///<summary>
        /// File Format（文件格式）
        /// </summary>	
        [Column]
        public string FileFormat
        {
            get;
            set;
        }

        ///<summary>
        /// File Url（文件路径）
        /// </summary>	
        [Column]
        public string FileUrl
        {
            get;
            set;
        }

        ///<summary>
        /// Add Person（添加人）
        /// </summary>	
        [Column]
        public string AddPersonnel
        {
            get;
            set;
        }

        ///<summary>
        /// Add Date（添加时间）
        /// </summary>	
        [Column]
        public DateTime ?AddDate
        {
            get;
            set;
        }

        ///<summary>
        /// state   状态（0删除；1在用）
        /// </summary>	
        [Column]
        public bool state
        {
            get;
            set;
        }

        ///<summary>
        /// Remark（附注）
        /// </summary>	
        [Column]
        public string Remark
        {
            get;
            set;
        }

        ///<summary>
        /// 文件类型
        /// </summary>	
        [ResultColumn]
        public string FileType_n
        {
            get;
            set;
        }


    }
}