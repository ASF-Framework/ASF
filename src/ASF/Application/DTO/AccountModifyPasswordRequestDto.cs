using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 账户修改登录密码
    /// </summary>
    public class AccountModifyPasswordRequestDto : IDto
    {
        /// <summary>
        /// 旧登录密码
        /// </summary>
        [Required, StringLength(20, MinimumLength = 6)]
        public string OldPassword { get; private set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required, StringLength(20, MinimumLength = 6)]
        public string Password { get; private set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
