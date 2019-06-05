using System.Collections.Generic;

namespace Phians_Entity
{
    public class PagedResultBase
    {
        /// <summary>
        /// 获取总记录数。
        /// </summary>
        public int total { get; set; }

        public bool success { get; set; }
        public PagedResultBase()
        {
        }

        public PagedResultBase(int totalItemCount)
        {
            this.total = totalItemCount;
        }

        public string  Message { get; set; }
    }

    public class PagedResult<T> : PagedResultBase
    {
        /// <summary>
        /// 初始化一个新的<c>PagedResult{T}</c>类型的实例。
        /// </summary>
        public PagedResult()
        {
            this.rows = new List<T>();
        }

        public PagedResult(int totalItemCount, List<T> pageData, bool success)
            : this()
        {
            this.total = totalItemCount;
            this.rows = pageData;
            this.success = success;
        }

        public PagedResult(int totalItemCount, List<T> pageData, bool success, string Message)
            : this()
        {
            this.total = totalItemCount;
            this.rows = pageData;
            this.success = success;
            this.Message = Message;
        }
        


        #region Public Properties

        /// <summary>
        /// 获取或设置当前页面的数据。
        /// </summary>
        public List<T> rows { get; set; }

        #endregion
    }
}
