using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 角色基本信息
    /// </summary>
    public class RoleBaseInfoResponseDto : RoleSimpleInfoResponseDto
    {
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
