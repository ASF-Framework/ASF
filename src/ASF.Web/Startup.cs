using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.JwtAuthorize;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASF.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    string[] urls = Configuration.GetSection("AllowedCores").Value.Split(',');
                    builder.WithOrigins(urls);
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                });
            });
            services.AddOcelot() 
                .AddASF();

            //services.AddAuthentication();
            services.AddOcelotJwtAuthorize();
            services.AddTokenJwtAuthorize();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ITokenBuilder _tokenBuilder = app.ApplicationServices.GetRequiredService<ITokenBuilder>();
            List<Claim> claims = new List<Claim>();
            //claims.Add(new Claim("role", "admin"));
            claims.Add(new Claim(ClaimTypes.Sid, "1"));
            claims.Add(new Claim(ClaimTypes.Name, "aqa51041500"));
            claims.Add(new Claim("is_super", "1"));
            claims.Add(new Claim(ClaimTypes.Role, "self"));
            var token = _tokenBuilder.BuildJwtToken(claims.ToArray(), DateTime.UtcNow, DateTime.Now.AddHours(5)).TokenValue;
        
            if (env.IsDevelopment())
            {
                app.UseCors();
                app.UseDeveloperExceptionPage();
            }
            app.UseFileServer();
            app.UseOcelot(opt=>
            {
                opt.AddASF();
            }).Wait();
        }
    }
}
