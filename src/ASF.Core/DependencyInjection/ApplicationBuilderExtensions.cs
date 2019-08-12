using Microsoft.Extensions.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static Task<IApplicationBuilder> UseASF(this IApplicationBuilder app)
        {
            return app.UseOcelot(opt =>
             {
                 opt.UseASF();
             });
        }
    }
}
