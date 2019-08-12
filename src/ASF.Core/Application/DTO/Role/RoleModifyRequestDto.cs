using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 角色修改请求
    /// </summary>
    public class RoleModifyRequestDto : IDto
    {
        /// <summary>
        /// 角色标识
        /// </summary>
        [Required]
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required, MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 分配的权限
        /// </summary>
        [Required]
        public List<string> Permissions { get;  set; } = new List<string>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
