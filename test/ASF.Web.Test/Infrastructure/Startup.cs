using ASF.EntityFramework.ModelMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;

namespace ASF.Core.Test.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddASF(build =>
            {
                string dbConnectionString = "Data Source=AppData/ASF.db";
                build.AddDbContext(b => b.UseSqlite(dbConnectionString));
            });
            services.AddLogging();
            services.AddAutoMapper(c =>
            {
                c.RegisterAllMappings(typeof(LogInfoMapper).Assembly);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOcelot(opt =>
            {
                opt.UseASF();
            }).Wait();
        }
    }
}
