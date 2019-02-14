using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ASF.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public bool Committed => throw new NotImplementedException();

        public bool Commit(bool autoRollback = false)
        {
            return true;
        }

        public Task<bool> CommitAsync(bool autoRollback = false)
        {
            return Task.FromResult(true);
        }

        public void Rollback()
        {
           

        }
    }
}
