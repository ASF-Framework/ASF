using ASF.Domain.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DTO
{
    public class LoggerInfoDetailsResponseDto:IDto
    {
        /// <summary>
        /// 日志编号
        /// </summary>
        public long Id { get;  set; }
        /// <summary>
        /// 操作账号编号
        /// </summary>
        public int AccountId { get;  set; }
        /// <summary>
        /// 操作账户昵称
        /// </summary>
        public string AccountName { get;  set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LoggingType Type { get;  set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        public string Subject { get;  set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp { get;  set; }
        /// <summary>
        /// 权限ID
        /// </summary>
        public string PermissionId { get; set; }
        /// <summary>
        /// 日志记录时间
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime AddTime { get;  set; } 
        /// <summary>
        /// 请求地址
        /// </summary>
        public string ApiAddress { get; set; }
        /// <summary>
        /// API请求数据
        /// </summary>
        public string ApiRequest { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public string ApiResponse { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
