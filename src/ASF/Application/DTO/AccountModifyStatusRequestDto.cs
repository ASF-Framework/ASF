using ASF.Domain.Values;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 修改账户状态
    /// </summary>
    public class AccountModifyStatusRequestDto : IDto
    {
        /// <summary>
        /// 账户标识
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 账户状态
        /// </summary>
        public AccountStatus Status { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
