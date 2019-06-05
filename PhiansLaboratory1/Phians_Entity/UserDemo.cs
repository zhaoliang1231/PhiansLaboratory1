using PetaPoco;

namespace Phians_Entity
{
    [TableName("UserDemo")] //表名
    [PrimaryKey("Id")] //主键
    [ExplicitColumns]
    public class UserDemo
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        [Column]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 登录账号
        /// </summary>
        [Column]
        public string UserCount
        {
            get;
            set;
        }

        /// <summary>
        /// 密码
        /// </summary>
        [Column]
        public string UserPwd
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        [Column]
        public string Phone { get; set; }

        /// <summary>
        /// 组Id
        /// </summary>
        [Column]
        public int GroupId { get; set; }

        /// <summary>
        /// 组名称  //关联其他表的要求其他表的结果的用[ResultColumn]
        /// </summary>
        [ResultColumn]
        public string GroupName { get; set; }

    }
}
