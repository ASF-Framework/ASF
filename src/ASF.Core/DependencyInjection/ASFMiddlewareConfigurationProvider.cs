using Microsoft.AspNetCore.Builder;
using Ocelot.Middleware;
using System.Threading.Tasks;

namespace ASF
{
    public static class ASFMiddlewareConfigurationProvider
    {
        public static OcelotMiddlewareConfigurationDelegate Get = builder =>
        {
            builder.Map("/ASF", app =>
            {
                app.UseAuthentication();
                app.UseMvc(routes =>
                {
                    routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                });
            });
            return Task.CompletedTask;
        };
    }
}
