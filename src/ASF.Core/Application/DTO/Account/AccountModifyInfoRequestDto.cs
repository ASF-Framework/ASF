using ASF.Domain.Values;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 账户修改信息请求
    /// </summary>
    public class AccountModifyInfoRequestDto : IDto
    {
        /// <summary>
        /// 账号编号
        /// </summary>
        [Required]
        public int AccountId { get; set; }
        /// <summary>
        /// 账户状态
        /// </summary>
        public AccountStatus Status { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Required, MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 角色集
        /// </summary>
        [Required]
        public List<int> Roles { get; set; } = new List<int>();
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
