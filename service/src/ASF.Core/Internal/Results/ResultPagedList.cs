using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 分页列表结果对象
    /// </summary>
    public class ResultPagedList<T> : ResultList<T>
    {
        /// <summary>
        /// 实体分页集合结果
        /// </summary>
        public ResultPagedList() { }
        /// <summary>
        /// 实体分页集合结果
        /// </summary>
        /// <param name="data">实体集合</param>
        /// <param name="totalCount">总条数</param>
        public ResultPagedList(IList<T> data,int totalCount) : base(data)
        {
            this.TotalCount = totalCount;
        }
        /// <summary>
        /// 实体分页集合结果
        /// </summary>
        /// <param name="data">实体集合</param>
        /// <param name="result">结果状态</param>
        public ResultPagedList(IList<T> data, int totalCount, ValueTuple<int, string> result) : base(data, result)
        {
            this.TotalCount = totalCount;
        }

        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <param name="data">实体集合</param>
        /// <param name="totalCount">总条数</param>
        /// <returns></returns>
        public  static ResultPagedList<T> ReSuccess(IList<T> data, int totalCount)
        {
            return new ResultPagedList<T>(data, totalCount);
        }
        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public new static ResultPagedList<T> ReSuccess()
        {
            return new ResultPagedList<T>(new List<T>(),0);
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public new static ResultPagedList<T> ReFailure(string message, int status)
        {
            ResultPagedList<T> result = new ResultPagedList<T>();
            result.To(message, status);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public new static ResultPagedList<T> ReFailure(ValueTuple<int, string> result)
        {
            ResultPagedList<T> res = new ResultPagedList<T>();
            res.To(result);
            return res;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public new static ResultPagedList<T> ReFailure(Result result)
        {
            ResultPagedList<T> re = new ResultPagedList<T>();
            re.To(result);
            return re;
        }
        /// <summary>
        /// 转换为 <see cref="Task<PagedListResult<T>>"/>
        /// </summary>
        /// <returns></returns>
        public new Task<ResultPagedList<T>> AsTask()
        {
            return Task.FromResult(this);
        }
    }
}
