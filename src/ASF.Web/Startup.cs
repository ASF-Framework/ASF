using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ASF.Web
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
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
            services.AddAuthentication("Bearer").AddJwtBearer((AccessTokenOptions opt) =>
            {
                opt.SecurityType = SecurityType.RsaSha512;
            });

            services.AddASF(build =>
            {
                var asfOptions = Configuration.GetSection("ASF").Get<ASFOptions>();
                build.AddDbContext(b =>
                {
                    switch (asfOptions.DBType.ToLower())
                    {
                        case "sqlite":
                            b.UseSqlite(asfOptions.DBConnectionString);
                            break;
                        case "mysql":
                            b.UseMySql(asfOptions.DBConnectionString);
                            break;
                        case "sqlserver":
                            b.UseSqlServer(asfOptions.DBConnectionString);
                            break;
                        default:
                            break;
                    }
                }, asfOptions.AllowCache);
            });
    
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ASF", Version = "v1" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "ASF.Core.xml");
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //启用中间件服务生成Swagger作为JSON终结点
                app.UseSwagger();
                //启用中间件服务对swagger-ui，指定Swagger JSON终结点
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASF");
                });
            }
            app.UseFileServer();
            app.ASFInitDatabase();
            app.UseASF().Wait();
        }
    }
}
