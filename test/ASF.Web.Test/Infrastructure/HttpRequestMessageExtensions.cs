using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace System.Net.Http
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage ContextJson<T>(this HttpRequestMessage request,T data)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return request;
        }

        public static HttpRequestMessage Authorization(this HttpRequestMessage request, string scheme, string parameter)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
            return request;
        }
    }
}
