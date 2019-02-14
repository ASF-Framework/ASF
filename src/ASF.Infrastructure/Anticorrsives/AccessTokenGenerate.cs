using ASF.Domain.Values;
using ASF.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASF.Infrastructure.Anticorrsives
{
    public class AccessTokenGenerate : IAccessTokenGenerate
    {
        private readonly ASFOptions _options;

        public AccessTokenGenerate( IOptions<ASFOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public AccessToken Generate(Dictionary<string, string> claims)
        {
            TokenOptions tokenOpt = _options.JwtToken;
            var now = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(tokenOpt.Expires);
            var claimList = new List<Claim>();
            var issuer = tokenOpt.Issuer;
            var audience = tokenOpt.Audience;
            var signingCredentials = tokenOpt.SigningCredentials;

            foreach (var claim in claims)
            {
                claimList.Add(new Claim(claim.Key, claim.Value));
            }

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claimList.ToArray(),
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AccessToken(encodedJwt, "Bearer", ConvertDateTimeToInt(expires));
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        private  long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
    }
}
