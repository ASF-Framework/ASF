using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 修改账户邮箱地址
    /// </summary>
    public class AccountModifyEmailRequestDto:IDto
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

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
