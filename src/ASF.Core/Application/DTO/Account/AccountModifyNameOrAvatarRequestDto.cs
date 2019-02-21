using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    public class AccountModifyNameOrAvatarRequestDto : IDto
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [ MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        [MaxLength(225)]
        public string Avatar { get; set; }
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
