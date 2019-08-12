using ASF.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ASF
{
    /// <summary>
    /// ASF 通过Ocelot拦截记录日志
    /// </summary>
    public class ASFRequestLoggerMiddleware
    {
        public static async Task Invoke(DownstreamContext context, Func<Task> next)
        {
            await next.Invoke();

            //判断是否响应成功
            if (context.DownstreamResponse == null || 
                context.DownstreamResponse.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            //开始记录日志
            var serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetRequiredService<ILogger<ASFRequestLoggerMiddleware>>();
            try
            {
                if (context.HttpContext.Items.ContainsKey("asf_parmission"))
                {
                    if (context.HttpContext.Items["asf_parmission"] is Permission per)
                    {
                        //判断是否需要记录日志
                        if (per.IsLogger)
                        {
                            await ASFRequestLogger.Record(context, per);
                        }
                    }
                    else
                    {
                        logger.LogError($"HttpContext.Items[\"asf_parmission\"] is not equal to {typeof(Permission)}");

                        return;
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ASF Logging");
                await next.Invoke();
                return;
            }
        }


    }
}
