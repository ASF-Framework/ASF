using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 权限基本信息响应对象
    /// </summary>
    public class PermissionMenuInfoDetailsResponseDto : PermissionMenuInfoBaseResponseDto
    {
        /// <summary>
        /// 是否系统权限
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreateTime { get; set; }
 
    }
}
