using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repository
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        public readonly RepositoryContext _dbContext;
        public UnitOfWorkRepository(RepositoryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Committed => throw new NotImplementedException();

        public bool Commit(bool autoRollback = false)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    //如果未执行到Commit()就执行失败遇到异常了，EF Core会自动进行数据回滚（前提是使用Using） 
                    transaction.Commit();
                }
                catch (Exception ex)
                { 
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }

        public Task<bool> CommitAsync(bool autoRollback = false)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    //如果未执行到Commit()就执行失败遇到异常了，EF Core会自动进行数据回滚（前提是使用Using） 
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(true);
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
