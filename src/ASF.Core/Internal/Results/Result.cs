using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 结果对象
    /// </summary>
    public class Result
    {
        public Result() { }
        public Result(string message, int status)
        {
            this.Status = status;
            this.Message = message;
        }
        public Result(ValueTuple<int, string> result)
        {
            this.Status = result.Item1;
            this.Message = result.Item2;
        }
        /// <summary>
        /// 执行是否成功
        /// </summary>
        [JsonIgnore]
        public bool Success
        {
            get
            {
                return this.Status == 200;
            }
        }
        /// <summary>
        /// 业务返回码
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        /// <summary>
        /// 执行返回消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(Result result)
        {
            this.Status = result.Status;
            this.Message = result.Message;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(string message, int status)
        {
            this.Status = status;
            this.Message = message;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result">结果对象</param>
        protected void To(ValueTuple<int, string> result)
        {
            this.Status = result.Item1;
            this.Message = result.Item2;
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public static Result ReFailure(string message, int status)
        {
            return new Result(message, status);
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public static Result ReFailure(ValueTuple<int, string> result)
        {
            return new Result(result.Item2, result.Item1);
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static T ReFailure<T>(Result result) where T : Result, new()
        {
            T r = new T();
            r.To(result);
            return r;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public static T ReFailure<T>(string message, int status) where T : Result, new()
        {
            T result = new T();
            result.To(message, status);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public static T ReFailure<T>(ValueTuple<int, string> result) where T : Result, new()
        {
            T ret = new T();
            ret.To(result);
            return ret;
        }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result ReSuccess()
        {
            return new Result(BaseResultCodes.Success);
        }
        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static T ReSuccess<T>() where T : Result, new()
        {
            T result = new T();
            result.To(BaseResultCodes.Success);
            return result;
        }
        /// <summary>
        /// 转换为 <see cref="Task<Result>"/>
        /// </summary>
        /// <returns></returns>
        public Task<Result> AsTask()
        {
            return Task.FromResult(this);
        }
    }
    

  
 
}
