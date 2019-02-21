using ASF.Domain.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ASF.Infrastructure.Model
{
    [Table("LogInfo")]
    public class LogInfoModel
    {
        /// <summary>
        /// 日志编号
        /// </summary>
        [Key]
        public long Id { get; private set; }
        /// <summary>
        /// 操作账号编号
        /// </summary>
        public int AccountId { get; private set; }
        /// <summary>
        /// 操作账户昵称
        /// </summary>
        [Required, MaxLength(32)]
        public string AccountName { get; private set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        [Required]
        public LoggingType Type { get; private set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        [Required, MaxLength(200)]
        public string Subject { get; private set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        [Required, MaxLength(20)]
        public string ClientIp { get; private set; }
        /// <summary>
        /// 权限ID
        /// </summary>
        [MaxLength(150)]
        public string PermissionId { get; set; }
        /// <summary>
        /// 日志记录时间
        /// </summary>
        public DateTime AddTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// 请求地址
        /// </summary>
        [MaxLength(500)]
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
        [MaxLength(500)]
        public string Remark { get; set; }
    }
}
