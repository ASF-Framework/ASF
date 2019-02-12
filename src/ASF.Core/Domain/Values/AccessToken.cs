using System;

namespace ASF.Domain.Values
{
    /// <summary>
    /// 授权访问Token
    /// </summary>
    public class AccessToken:IValueObject
    {
        public AccessToken()
        {

        }
        public AccessToken(string token,string refreshToken,DateTime expired)
        {
            this.Token = token;
            this.RefreshToken = refreshToken;
            this.Expired = expired;
        }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; private set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; private set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expired { get; private set; }
    }
}
