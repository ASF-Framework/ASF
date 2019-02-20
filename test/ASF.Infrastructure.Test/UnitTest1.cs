using ASF.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using ASF.Domain;
using System.Collections.Generic;

namespace TestRepository
{
    public class UnitTest1
    {
        [Fact]
        public void Add()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
           var service= FakeConfig.Startup().GetService<IAccountRepository>();
            ASF.Domain.Entities.Account account = new ASF.Domain.Entities.Account("abc","123456") {
            };
            account.SetRoles(list);
          var result=  service.AddAsync(account).GetAwaiter().GetResult();
        }

        [Fact]
        public void UpdateAndRemove()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            var service = FakeConfig.Startup().GetService<IAccountRepository>();
            ASF.Domain.Entities.Account account = new ASF.Domain.Entities.Account("abc2", "123456")
            {
            };
            account.SetRoles(list);
            var result = service.AddAsync(account).GetAwaiter().GetResult();

            list.Add(3);
            result.SetRoles(list);
            service.ModifyAsync(result);

            service.RemoveAsync(result.Id);
        }

        [Fact]
        public void Get()
        {
            var service = FakeConfig.Startup().GetService<IAccountRepository>();
            var result = service.GetAsync(3).GetAwaiter().GetResult();
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            result.SetRoles(list);
            service.ModifyAsync(result);
        }
    }
}
