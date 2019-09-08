using ASF.Domain.Entities;
using ASF.Domain.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 权限基本信息响应对象
    /// </summary>
    public class PermissionMenuInfoDetailsResponseDto : PermissionMenuInfoBaseResponseDto
    {
        /// <summary>
        /// 是否系统权限
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 菜单标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 重定向Url
        /// </summary>
        public string Redirect { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// 权限服务地址
        /// </summary>
        public string ApiTemplate { get; private set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <returns></returns>
        public Permission To()
        {
            var p = new Permission(this.Code, this.ParentId, this.Name, PermissionType.Menu, this.Description);
            p.Sort = this.Sort;
            p.MenuHidden = this.Hidden;
            p.MenuIcon = this.Icon;
            p.MenuRedirect = this.Redirect;
            p.SetApiTemplate(this.ApiTemplate);
            return p;
        }

    }
}
