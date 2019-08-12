using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 集合结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultList<T> : Result<IList<T>>
    {
        /// <summary>
        /// 实体集合结果
        /// </summary>
        public ResultList() { }
        /// <summary>
        /// 实体集合结果
        /// </summary>
        /// <param name="data">实体集合</param>
        public ResultList(IList<T> data) : base(data)
        {
        }
        /// <summary>
        /// 实体集合结果
        /// </summary>
        /// <param name="data">实体集合</param>
        /// <param name="result">结果状态</param>
        public ResultList(IList<T> data, ValueTuple<int, string> result) : base(data,result)
        {
        }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public new static ResultList<T> ReSuccess(IList<T> data)
        {
            return new ResultList<T>(data);
        }
        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public new static ResultList<T> ReSuccess()
        {
            return new ResultList<T>(new List<T>());
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public new static ResultList<T> ReFailure(string message, int status)
        {
            ResultList<T> result = new ResultList<T>();
            result.To(message, status);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public new static ResultList<T> ReFailure(ValueTuple<int, string> result)
        {
            ResultList<T> res = new ResultList<T>();
            res.To(result);
            return res;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public new static ResultList<T> ReFailure(Result result)
        {
            ResultList<T> re = new ResultList<T>();
            re.To(result);
            return re;
        }
        /// <summary>
        /// 转换为 <see cref="Task<ListResult<T>>"/>
        /// </summary>
        /// <returns></returns>
        public new Task<ResultList<T>> AsTask()
        {
            return Task.FromResult(this);
        }
    }

}
