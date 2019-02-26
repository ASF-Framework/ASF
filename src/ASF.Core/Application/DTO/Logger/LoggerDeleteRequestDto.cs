using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 批量删除日志请求
    /// </summary>
    public class LoggerDeleteRequestDto : IDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? EndTime { get; set; }

       
    }
}
