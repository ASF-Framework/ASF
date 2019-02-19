using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repository
{
    public class LogInfoRepository: ILoggingRepository
    {
        public readonly RepositoryContext _dbContext;
        public LogInfoRepository(RepositoryContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Logging> AddAsync(Logging entity)
        {
            var model = Mapper.Map<Model.LogInfoModel>(entity);
            await _dbContext.AddAsync(model);
            // await _dbContext.SaveChangesAsync();
            return Mapper.Map<Logging>(model);
        }

        public async Task<Logging> GetAsync(long id)
        {
            var model = await _dbContext.LogInfos.FirstOrDefaultAsync(w => w.Id == id);
            return Mapper.Map<Logging>(model);
        }

        public Task RemoveAsync(long primaryKey)
        {
            return Task.FromResult(0);
        }

        public async Task<IList<Logging>> GetList()
        {
            var list = await _dbContext.Permissions.ToListAsync();

            return Mapper.Map<IList<Domain.Entities.Logging>>(list);
        }
    }
}
