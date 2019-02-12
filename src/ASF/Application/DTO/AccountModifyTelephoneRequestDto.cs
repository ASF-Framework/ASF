using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 修改账户手机号码
    /// </summary>
    public class AccountModifyTelephoneRequestDto : IDto
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required, MaxLength(20)]
        public string Number { get; }
        /// <summary>
        /// 手机号码区号
        /// </summary>
        [Required]
        public int AreaCode { get; } = 86;

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
