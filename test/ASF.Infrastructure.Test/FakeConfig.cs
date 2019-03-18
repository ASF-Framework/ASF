using ASF.DependencyInjection;
using ASF.Infrastructure.ModelMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TestRepository
{
    public static class FakeConfig
    {
        public static IServiceProvider Startup()
        {
            IServiceCollection service = new ServiceCollection();
            service.AddAutoMapper(c =>
            {
                c.RegisterAllMappings(typeof(LogInfoMapper).Assembly);
            });
            new ASFBuilder(service).AddSQLite("Data Source=ASF.db");
            return service.BuildServiceProvider();
        }
    }
}
