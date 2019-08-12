using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 修改权限排序请求
    /// </summary>
    public class PermissionModifySortRequestDto : IDto
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 99;
    }
}
