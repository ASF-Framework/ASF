using ASF.Domain.Values;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASF.Infrastructure.Anticorrsives
{
    public class AccessTokenGenerate : IAccessTokenGenerate
    {
        private readonly TokenOptions _options;

        public AccessTokenGenerate(TokenOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public AccessToken Generate(Dictionary<string, string> claims)
        {
            var now = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(_options.Expires);
            var claimList = new List<Claim>();
            var issuer = _options.Issuer;
            var audience = _options.Audience;
            var signingCredentials = _options.SigningCredentials;

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
