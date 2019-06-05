using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using PetaPoco;
namespace Phians_Entity
{
    /// <summary>
    /// 内部评审、环境管\体系文件＼管理评审
    /// </summary>
    [PetaPoco.TableName("TB_FileManagement")]
    [PetaPoco.PrimaryKey("id")]

    public class TB_FileManagement
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
        /// 文件编号
        /// </summary>		
        [Column]
        public string FileNum
        {
            get;
            set;
        }
     
        ///<summary>
        /// 文件编号ID
        /// </summary>		
        [ResultColumn]
        public Guid FileID
        {
            get;
            set;
        }
       
        ///<summary>
        /// 文件类型
        /// </summary>		
        [Column]
        public Guid FileType
        {
            get;
            set;
        }
      
        ///<summary>
        /// 文件原名称
        /// </summary>		
        [Column]
        public string FileName
        {
            get;
            set;
        }
      
        ///<summary>
        /// 文件格式
        /// </summary>		
        [Column]
        public string FileFormat
        {
            get;
            set;
        }
       
        ///<summary>
        /// 文件说明
        /// </summary>		
        [Column]
        public string FileRemark
        {
            get;
            set;
        }
      
        ///<summary>
        /// 文件名称
        /// </summary>		
        [Column]
        public string FileNewName
        {
            get;
            set;
        }
      
        ///<summary>
        /// 文件url
        /// </summary>		
        [Column]
        public string FileUrl
        {
            get;
            set;
        }
       
        ///<summary>
        /// 存档人
        /// </summary>		
        [Column]
        public Guid FilePersonnel
        {
            get;
            set;
        }
     
        ///<summary>
        /// 存档时间
        /// </summary>		
        [Column]
        public string FileDate
        {
            get;
            set;
        }
      
        ///<summary>
        /// 评审人
        /// </summary>		
        [Column]
        public Guid Personnel
        {
            get;
            set;
        }
      
        ///<summary>
        /// 评审时间
        /// </summary>		
        [Column]
        public string AuditDate
        {
            get;
            set;
        }
     
        ///<summary>
        /// 签发人
        /// </summary>		
        [Column]
        public Guid IssuePersonnel
        {
            get;
            set;
        }
   
        ///<summary>
        /// 签发时间
        /// </summary>		
        [Column]
        public string IssueDate
        {
            get;
            set;
        }
       
        ///<summary>
        /// state（1 有效 0无效 2作废）
        /// </summary>		
        [Column]
        public int state
        {
            get;
            set;
        }
      
        ///<summary>
        /// 有效
        /// </summary>		
        [Column]
        public bool Effective
        {
            get;
            set;
        }
     
        ///<summary>
        /// 说明
        /// </summary>		
        [Column]
        public string Remarks
        {
            get;
            set;
        }
        
        //存档人
        [ResultColumn]
        public string FileUserName
        {
            set;
            get;
        }
             
        //评审人
        [ResultColumn]
        public string ReviewName
        {
            get;
            set;
        }
             
        //签发人
        [ResultColumn]
        public string IssuePersonnelName
        {
            get;
            set;
        }
        //文件类别
        [ResultColumn]
        public string FileType_n
        {
            get;
            set;
        }
    }
}