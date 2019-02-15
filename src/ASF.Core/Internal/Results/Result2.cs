using Newtonsoft.Json;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 实体结果对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 实体结果
        /// </summary>
        public Result() { }
        /// <summary>
        /// 实体结果
        /// </summary>
        /// <param name="data"></param>
        public Result(T data) : base(BaseResultCodes.Success)
        {
            this.Data = data;
        }
        public Result(T data, ValueTuple<int, string> result) : base(result)
        {
            this.Data = data;
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public T Data { get; set; }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result<T> ReSuccess(T data)
        {
            return new Result<T>(data);
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public new static Result<T> ReFailure(string message, int status)
        {
            Result<T> result = new Result<T>();
            result.To(message, status);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public new static Result<T> ReFailure(ValueTuple<int, string> result)
        {
            Result<T> res = new Result<T>();
            res.To(result);
            return res;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static Result<T> ReFailure(Result result)
        {
            Result<T> re = new Result<T>();
            re.To(result);
            return re;
        }
        /// <summary>
        /// 转换为 <see cref="Task<Result<T>>"/>
        /// </summary>
        /// <returns></returns>
        public new Task<Result<T>> AsTask()
        {
            return Task.FromResult(this);
        }
    }
}
