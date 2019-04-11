using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 管理员身份验证请求
    /// </summary>
    public class AuthoriseByUsernameRequestDto : IDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required, StringLength(32, MinimumLength = 2)]
        public string Username { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required, StringLength(32, MinimumLength = 6)]
        public string Password { get; set; }
        /// <summary>
        /// 验证方式
        /// </summary>
        [Required]
        public string ValidType { get; set; } = "None";
        /// <summary>
        /// 验证码数据
        /// </summary>
        public Dictionary<string, string> Validate { get; set; } = new Dictionary<string, string>();
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
