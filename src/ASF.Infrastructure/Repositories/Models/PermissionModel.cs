using ASF.Domain.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ASF.Infrastructure.Model
{
   public class PermissionModel
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        [Key, Required, MaxLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; private set; }
        /// <summary>
        /// 权限代码
        /// </summary>
        [Required]
        public string Code { get; private set; }
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
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }
        /// <summary>
        /// 是否系统权限
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 权限服务地址
        /// </summary>
        [MaxLength(500)]
        public string ApiAddress { get; set; }
        /// <summary>
        /// 是否日志记录
        /// </summary>
        public bool IsLogger { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; private set; } = DateTime.Now;
    }
}
