using ASF.Domain.Entities;
using ASF.Domain.Values;
using Newtonsoft.Json;
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
        [Required, MaxLength(30)]
        public string Code { get;  set; }
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
            p.MenuHidden = this.Hidden;
            p.MenuIcon = this.Icon;
            p.MenuRedirect = this.Redirect;
            return p;
        }
        /// <summary>
        /// 转换Json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
