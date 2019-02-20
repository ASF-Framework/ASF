using System.Collections.Generic;

namespace ASF.Application.DTO
{
    public class PermissionMenuInfoBaseResponseDto:IDto
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 上级权限编号
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 操作集合
        /// </summary>
        public Dictionary<string, string> Actions { get; set; } = new Dictionary<string, string>();
    }
}
