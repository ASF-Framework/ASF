using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;

namespace ASF.Infrastructure.Repositories
{
    public  interface ILoggingRepository : IRepository<Logging, long>
    {
    }
}
