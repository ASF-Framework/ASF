using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace ASF.Application.DTO
{
    public class PermissionActionInfoDetailsResponseDto:IDto
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 权限编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 上级权限编号
        /// </summary>
        public string ParentId { get; set; }
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
        /// 是否日志记录
        /// </summary>
        public bool IsLogger { get; set; }
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
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// Http 方法集合
        /// </summary>
        public List<string> HttpMethods { get; set; }
    }
}
