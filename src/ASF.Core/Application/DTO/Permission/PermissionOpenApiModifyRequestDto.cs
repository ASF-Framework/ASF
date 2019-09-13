using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 开放API权限修改请求
    /// </summary>
    public class PermissionOpenApiModifyRequestDto : IDto
    {
        /// <summary>
        /// 权限标识
        /// </summary>
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 权限服务地址
        /// </summary>
        [Required, MaxLength(500)]
        public string ApiTemplate { get; set; }
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// Http 方法集合
        /// </summary>
        public List<string> HttpMethods { get; set; } = new List<string>();
    }
}
