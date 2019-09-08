using System.Security.Claims;
using System;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// ClaimsPrincipal 扩展类
    /// </summary>
    public static class ClaimsPrincipalExtension
    {
        /// <summary>
        /// 账户Id
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static int UserId(this ClaimsPrincipal principal)
        {
            var sub = principal.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(sub))
                sub = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(sub))
                return 0;
            return Convert.ToInt32( sub);
        }
        /// <summary>
        /// 名称
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string Name(this ClaimsPrincipal principal)
        {
            return principal.FindFirst("name")?.Value;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string NickName(this ClaimsPrincipal principal)
        {
            return principal.FindFirst("nickname")?.Value;
        }
    }
}
