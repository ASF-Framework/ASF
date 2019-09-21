using System;
using System.ComponentModel.DataAnnotations;
using Zop.AspNetCore.Authentication.JwtBearer;

namespace ASF.Domain.Values
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginInfo: IValueObject
    {
        public LoginInfo()
        {

        }
        public LoginInfo(string loginIp, AccessToken accessToken )
        {
            this.LoginIp = loginIp;
            this.AccessToken = accessToken;
        }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        [Required, MaxLength(20)]
        public string LoginIp { get; private set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Required]
        public DateTime LoginTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// 授权颁发访问Token
        /// </summary>
        [Required]
        public AccessToken AccessToken { get; private set; } = new AccessToken();
    }
}
