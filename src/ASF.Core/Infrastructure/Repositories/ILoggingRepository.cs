using ASF.Domain.Entities;
using System.Threading.Tasks;
using System;
using ASF.Application.DTO;
using System.Collections.Generic;

namespace ASF.Infrastructure.Repositories
{
    public  interface ILoggingRepository : IRepository<Logging, long>
    {
        /// <summary>
        /// 分页获取日志激活
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<(IList<Logging> Loggings, int TotalCount)> GetList(LoggerListPagedRequestDto dto);
        /// <summary>
        /// 根据时间删除日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Delete(LoggerDeleteRequestDto dto);
    }
}
