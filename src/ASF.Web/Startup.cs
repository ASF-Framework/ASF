using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;

namespace ASF.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
  
            services.AddOcelot()
                .AddASF(build =>
                {
                    string dbConnectionString = "Data Source=AppData/ASF.db";
                    build.AddSQLite(dbConnectionString);
                    //if (_env.IsDevelopment())
                    //{
                    //    build.AddSQLite(dbConnectionString);
                    //}
                    //else
                    //{
                    //    build.AddSQLiteCache(dbConnectionString);
                    //}
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseFileServer();
            app.UseOcelot(opt =>
            {
                opt.AddASF();
            }).Wait();
        }
    }
}
