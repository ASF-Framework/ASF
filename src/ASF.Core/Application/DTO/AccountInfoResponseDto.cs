using ASF.Domain.Values;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 账户信息响应对象
    /// </summary>
    public class AccountInfoResponseDto
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public AccountStatus Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 角色集
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();
    }
}
