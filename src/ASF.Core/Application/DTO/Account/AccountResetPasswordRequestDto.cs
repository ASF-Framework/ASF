using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 重置管理员密码请求
    /// </summary>
    public class AccountResetPasswordRequestDto : IDto
    {
        /// <summary>
        /// 账户标识
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        [Required, StringLength(20, MinimumLength = 6)]
        public string AdminPassword { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required, StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
        /// <summary>
        /// 转换Json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
