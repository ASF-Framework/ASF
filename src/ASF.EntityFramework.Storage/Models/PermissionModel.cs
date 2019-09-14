using ASF.Domain.Values;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASF.EntityFramework.Model
{
    [Table("Permissions")]
    public class PermissionModel
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        [Key, Required, MaxLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get;  set; }
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
        public string ApiTemplate { get; set; }
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
        /// 菜单图标
        /// </summary>
        [MaxLength(20)]
        public string MenuIcon { get; set; }
        /// <summary>
        /// 菜单重定向Url
        /// </summary>
        [MaxLength(300)]
        public string MenuRedirect { get; set; }
        /// <summary>
        /// 菜单是否隐藏
        /// </summary>
        public bool MenuHidden { get; set; }
        /// <summary>
        /// Http 方法
        /// </summary>
        [MaxLength(100)]
        public string HttpMethods { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; private set; } = DateTime.Now;
    }
}
