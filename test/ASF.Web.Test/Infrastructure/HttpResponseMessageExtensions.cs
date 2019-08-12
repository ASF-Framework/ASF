using Newtonsoft.Json;
using System.Linq;
using Xunit;

namespace System.Net.Http
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// 是否请求成功
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static HttpResponseMessage IsSuccess(this HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            return response;
        }

        /// <summary>
        ///  获取响应消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static T GetMessage<T>(this HttpResponseMessage response)
        {
            var message = response.GetMessage();
            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// 获取响应消息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetMessage(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
