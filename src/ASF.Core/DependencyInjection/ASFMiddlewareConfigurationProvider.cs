using Microsoft.AspNetCore.Builder;
using Ocelot.Middleware;
using System.Threading.Tasks;

namespace ASF
{
    public static class ASFMiddlewareConfigurationProvider
    {
        public static OcelotMiddlewareConfigurationDelegate Get = builder =>
        {
            builder.Map("/API/ASF", app =>
            {
                app.UseAuthentication();
                app.UseMvc();
            });
            return Task.CompletedTask;
        };
    }
}
