using ASF.Domain.Entities;
using ASF.Domain.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 开放性API权限详细信息
    /// </summary>
    public class PermissionOpenApiInfoDetailsResponseDto : IDto
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限服务地址
        /// </summary>
        public string ApiTemplate { get; set; }
        /// <summary>
        /// 是否系统权限
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <returns></returns>
        public Permission To()
        {
            var p = new Permission(this.Id, null, this.Name, PermissionType.OpenApi, this.Description);
            p.Enable = this.Enable;
            p.IsSystem = this.IsSystem;
            p.SetApiTemplate(this.ApiTemplate);
            return p;
        }

    }
}
