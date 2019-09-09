using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 修改菜单权限请求
    /// </summary>
    public class PermissionMenuModifyRequestDto : IDto
    {
        /// <summary>
        /// 权限标识
        /// </summary>
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// 上级权限编号
        /// </summary>
        [MaxLength(100)]
        public string ParentId { get; set; } = "";
        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(20)]
        public string Icon { get; set; }
        /// <summary>
        /// 重定向Url
        /// </summary>
        [MaxLength(300)]
        public string Redirect { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
