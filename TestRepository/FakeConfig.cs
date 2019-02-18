using ASF.Infrastructure.ModelMapper;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
            service.AddASFRepository();
            return service.BuildServiceProvider();
        }
    }
}
