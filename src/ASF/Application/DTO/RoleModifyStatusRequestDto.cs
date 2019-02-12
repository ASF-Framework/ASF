using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 修改角色状态
    /// </summary>
    public class RoleModifyStatusRequestDto:IDto
    {
        /// <summary>
        /// 角色标识
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
