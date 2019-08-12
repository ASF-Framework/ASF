namespace System
{
    /// <summary>
    /// 结果返回码
    /// </summary>
    public  class BaseResultCodes
    {
        /// <summary>
        /// 处理成功
        /// </summary>
        public static ValueTuple<int, string> Success = (200,"Success");
        /// <summary>
        /// 错误请求
        /// </summary>
        public static ValueTuple<int, string> BadRequest = (400, "{0}");
        /// <summary>
        /// 未授权
        /// </summary>
        public static ValueTuple<int, string> UnAuthorized = ( 401, "Unauthorized");
        /// <summary>
        /// 拒绝请求
        /// </summary>
        public static ValueTuple<int, string> NotAcceptable = (403,"Not Acceptable");
        /// <summary>
        /// 未找到服务
        /// </summary>
        public static ValueTuple<int, string> NotFound = (404, "Not Found");
        /// <summary>
        /// 系统错误
        /// </summary>
        public static ValueTuple<int, string>  UnknowError = (500, "Internal Server Error");
        /// <summary>
        /// 请求超时
        /// </summary>
        public static ValueTuple<int, string> RequestTimeout = (408, "Request Timeout");

        
    }
    public static class BaseResultCodesExtension
    {
        /// <summary>
     /// 根据替换符进行替换支付
     /// </summary>
     /// <param name="result">结果对象</param>
     /// <param name="args">替换字符</param>
     /// <returns></returns>
        public static ValueTuple<int, string> ToFormat(this ValueTuple<int, string> result, params string[] args)
        {
            result.Item2 = string.Format(result.Item2, args);
            return result;
        }

    }
}
