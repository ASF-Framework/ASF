using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 导航权限创建请求
    /// </summary>
    public class PermissionMenuCreateRequestDto : IDto
    {
        /// <summary>
        /// 权限代码
        /// </summary>
        [Required]
        public string Code { get;  set; }
        /// <summary>
        /// 上级权限编号
        /// </summary>
        [MaxLength(100)]
        public string ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 99;
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
