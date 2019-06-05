using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phians_Entity
{
    /// <summary>
    /// 返回执行结果
    /// </summary>
    public class ExecuteResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExecuteResult()
        {
        }
       
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        public ExecuteResult(bool success, string message)
        {
            this.Success = success ;
            this.Message = message;
        }
     
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }


        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        
    }

    public class Result_data
    {
        public Result_data()
        {
        }
       public Result_data(bool success, string message, Guid Resultdata3)
        {
            this.Success = success ;
            this.Message = message;
            this.Resultdata= Resultdata3;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }


        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        public Guid Resultdata { get; set; }
    
    }
    /// <summary>
    /// 实体返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleExecuteResult<T> : ExecuteResult
    {
        public T Data { get; set; }

        public SingleExecuteResult(bool success, string message)
        {
            this.Success = success; 
            this.Message = message;
        }

        public SingleExecuteResult(bool success, string messeage, T data)
            : this(success, messeage)
        {
            this.Data = data;
        }
    }



    /// <summary>
    /// 列表返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListExecuteResult<T> : ExecuteResult
    {
        public List<T> Data { get; set; }

        public ListExecuteResult()
        {
            this.Data = new List<T>();
        }

        public ListExecuteResult(bool success, string message)
        {
            this.Success = success ;
            this.Message = message;
        }

        public ListExecuteResult(bool success, string messeage, List<T> data)
            : this(success, messeage)
        {
            this.Data = data;
        }
    }

   
}
