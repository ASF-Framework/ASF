using System;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 日志列表分页查询请求
    /// </summary>
    public class LoggerListPagedRequestDto : ListPagedRequestDto
    {
        
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(200)]
        public string Subject { get; set; }
        /// <summary>
        /// 操作账号 操作账户ID 和 操作账号
        /// </summary>
        [MaxLength(200)]
        public string Account { get;  set; }
        /// <summary>
        /// 日志类型 -1 全部，1:登录日志  2:操作日志
        /// </summary>
        public int Type { get; set; } = -1;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 权限ID
        /// </summary>
        [MaxLength(150)]
        public string PermissionId { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp { get;  set; }
    }
}
