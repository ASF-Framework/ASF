using ASF.Domain.Values;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 账户修改信息请求
    /// </summary>
    public class AccountModifyInfoRequestDto : IDto
    {
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
        /// 头像
        /// </summary>
        [MaxLength(225)]
        public string Avatar { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
