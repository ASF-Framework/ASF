using ASF.Domain.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 详细信息
    /// </summary>
    public class AccountInfoDetailsResponseDto: AccountInfoBaseResponseDto
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get;  set; }
        /// <summary>
        /// 角色集
        /// </summary>
        public new IList<int> Roles { get;  set; } = new List<int>();
        /// <summary>
        /// 创建用户
        /// </summary>
        public string  CreateUser { get;  set; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LoginIp { get;  set; }

    }
}
