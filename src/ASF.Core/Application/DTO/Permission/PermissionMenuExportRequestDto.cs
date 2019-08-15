using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 权限菜单导出请求对象
    /// </summary>
    public class PermissionMenuExportRequestDto : IDto
    {
        /// <summary>
        /// 权限List
        /// </summary>
        public List<PermissionMenuInfoDetailsResponseDto> List { get; set; }
    }
}
