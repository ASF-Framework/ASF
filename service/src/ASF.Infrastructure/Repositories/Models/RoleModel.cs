using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ASF.Infrastructure.Model
{
    [Table("Roles")]
    public class RoleModel
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Key]
        public int Id { get; private set; }
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
        /// 创建用户
        /// </summary>
        [Required]
        public int CreateId { get; private set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// 分配的权限
        /// </summary>
        [MaxLength(20000)]
        public string Permissions { get; private set; }
    }
}
