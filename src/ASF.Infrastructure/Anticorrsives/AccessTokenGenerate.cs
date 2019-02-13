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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ASFOptions _options;

        public AccessTokenGenerate(IHttpContextAccessor httpContextAccessor, IOptions<ASFOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
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
            if (string.IsNullOrEmpty(issuer))
            {
                issuer = _httpContextAccessor.HttpContext.Request.Host.ToString();
            }
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
            return new AccessToken(encodedJwt, "Bearer", expires);
        }

      
    }
}
