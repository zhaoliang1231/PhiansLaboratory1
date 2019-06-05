using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity.Common
{
    /// <summary>
    /// 页面分页传参数
    /// </summary>
    public class TPageModel
    {

        public TPageModel()
        {
            this.SearchList_ = new List<SearchList>();
        }


        /// <summary>
        /// 页入口
        /// </summary>
        public int PageIndex
        {
            get;
            set;

        }
        /// <summary>
        /// 页大小
        /// </summary>
        public int Pagesize
        {
            get;
            set;

        }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortName
        {
            get;
            set;

        }
        /// <summary>
        /// 排序类型 desc asc
        /// </summary>
        public string SortOrder
        {
            get;
            set;

        }
        /// <summary>
        ///  查询内容listmodel
        /// </summary>
        public List<SearchList> SearchList_
        {
            get;
            set;
        }

    }

    #region 查询内容
    /// <summary>
    /// 查询内容
    /// </summary>
    public class SearchList
    {
        /// <summary>
        /// 查询字段
        /// </summary>
        public string Search
        {
            get;
            set;

        }
        /// <summary>
        /// 查询内容
        /// </summary>
        public dynamic Key
        {
            get;
            set;

        }
    }

    #endregion
}
