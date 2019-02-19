using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application.DTO
{
   public class RoleInfoSimpleResponseDto:IDto
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
    }
}
