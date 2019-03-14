using ASF.Domain.Entities;
using ASF.Domain.Services;
using ASF.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASF
{
    /// <summary>
    /// 日子记录
    /// </summary>
    public class ASFRequestLogger
    {
        private readonly HttpContext context;
        private readonly Permission permission;
        public ASFRequestLogger(HttpContext context, Permission permission)
        {
            this.context = context;
            this.permission = permission;
        }
        public static Task Record(HttpContext context, Permission permission)
        {
            return new ASFRequestLogger(context, permission).Record();
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="httpContext"></param>
        public Task Record()
        {
            var serviceProvider = context.RequestServices;
            var response = context.Response;
             
            if (!response.Body.CanRead || !response.Body.CanSeek)
            {
                response.Body = new MemoryWrappedHttpResponseStream(response.Body);
            }
            context.Response.OnCompleted(async o =>
            {
                var c = o as HttpContext;
                if (c != null)
                {
                    //获取所有请求参数和响应数据
                    var requestData = await this.GetRequestData(context.Request);
                    var responseData = await this.GetResponseData(response);
                    serviceProvider.GetRequiredService<LogOperateRecordService>().Record(permission, requestData, responseData);
                }
            }, context);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private async Task<string> GetRequestData(HttpRequest request)
        {
            if (request.ContentLength > 0)
            {
                await this.EnableRewindAsync(request).ConfigureAwait(false);
                var encoding = this.GetEncoding(request.ContentType);
                return await this.ReadStreamAsync(request.Body, encoding).ConfigureAwait(false);
            }
            return "";
        }
        /// <summary>
        /// 获取响应数据
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> GetResponseData(HttpResponse response)
        {
            if (response.Body.Length > 0)
            {
                var encoding = this.GetEncoding(response.ContentType);
                response.Body.Seek(0, SeekOrigin.Begin);
                return await this.ReadStreamAsync(response.Body, encoding).ConfigureAwait(false);
            }
            return "";
        }
        private Encoding GetEncoding(string contentType)
        {
            var requestMediaType = contentType == null ? default(MediaType) : new MediaType(contentType);
            var requestEncoding = requestMediaType.Encoding;
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            return requestEncoding;
        }
        private async Task EnableRewindAsync(HttpRequest request)
        {
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();

                await request.Body.DrainAsync(CancellationToken.None);
                request.Body.Seek(0L, SeekOrigin.Begin);
            }
        }
        private async Task<string> ReadStreamAsync(Stream stream, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(stream, encoding, true, 1024, true))//这里注意Body部分不能随StreamReader一起释放
            {
                var str = await sr.ReadToEndAsync();
                stream.Seek(0, SeekOrigin.Begin);//内容读取完成后需要将当前位置初始化，否则后面的InputFormatter会无法读取
                return str;
            }
        }
    }
}
