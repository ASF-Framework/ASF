using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ASF.Domain.Entities;

namespace ASF.Infrastructure.Repositories
{
    public class LoggingRepository : ILoggingRepository
    {
        public Task<Logging> AddAsync(Logging entity)
        {
            return Task.FromResult(entity);
        }

        public Task<Logging> GetAsync(long id)
        {
            return Task.FromResult<Logging>(null);
        }

        public Task RemoveAsync(long primaryKey)
        {
            return Task.CompletedTask;
        }
    }
}
