using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DTO
{
    public class RoleInfoDetailsResponseDto: RoleInfoBaseResponseDto
    {
        /// <summary>
        /// 分配的权限
        /// </summary>
        public Dictionary<string,string> Permissions { get; set; } = new Dictionary<string, string>();
    }
}
