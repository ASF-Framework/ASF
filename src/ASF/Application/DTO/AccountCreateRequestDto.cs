using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 账号创建Dto
    /// </summary>
    public class AccountCreateRequestDto : IDto
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required, MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required, StringLength(32, MinimumLength = 2)]
        public string Username { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required, StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(225)]
        public string Avatar { get; set; }
        /// <summary>
        /// 角色集
        /// </summary>
        [Required]
        public IList<int> Roles { get;  set; } = new List<int>();
        
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
