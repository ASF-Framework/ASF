using ASF.Domain.Entities;
using ASF.Domain.Values;
using System.Collections.Generic;

namespace ASF.Infrastructure.Anticorrsives
{
    public interface IAccessTokenGenerate : IAnticorrsive
    {
        /// <summary>
        /// 生成授权访问Tokan
        /// </summary>
        /// <param name="claims">Token 声明</param>
        /// <returns></returns>
        AccessToken Generate(Dictionary<string,string> claims);
    }
}
