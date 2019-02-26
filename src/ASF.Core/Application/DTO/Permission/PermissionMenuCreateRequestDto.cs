using ASF.Domain.Entities;
using ASF.Domain.Values;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 菜单权限创建请求
    /// </summary>
    public class PermissionMenuCreateRequestDto : IDto
    {
        /// <summary>
        /// 权限代码
        /// </summary>
        [Required, MaxLength(10)]
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
        public Permission To()
        {
            var p = new Permission(this.Code, this.ParentId, this.Name, PermissionType.Menu, this.Description);
            p.Sort = this.Sort;
            return p;
        }
    }
}
