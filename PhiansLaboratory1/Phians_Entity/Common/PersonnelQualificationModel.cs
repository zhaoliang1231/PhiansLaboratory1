using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity.Common
{
    /// <summary>
    /// 人员资质
    /// </summary>
    public class PersonnelQualificationModel
    {
        /// <summary>
        /// 模板id
        /// </summary>
        public int TemplateID
        {
            get;
            set;
        }
        /// <summary>
        /// 人员id
        /// </summary>
        public Guid UserId
        {

            get;
            set;
        }
        /// <summary>
        /// 授权类型（0审核；1签发）
        /// </summary>
        public int AuthorizationType
        {

            get;
            set;
        }

    }
}
