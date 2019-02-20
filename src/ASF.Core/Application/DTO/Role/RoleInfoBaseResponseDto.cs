using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 角色基本信息
    /// </summary>
    public class RoleInfoBaseResponseDto : RoleInfoSimpleResponseDto
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreateTime { get; set; }
       
    }
}
