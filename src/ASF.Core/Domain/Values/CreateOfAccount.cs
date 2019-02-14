using ASF.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ASF.Domain.Values
{
    /// <summary>
    /// 创建的账户信息
    /// </summary>
    public class CreateOfAccount : IValueObject
    {
        public CreateOfAccount(Account account)
        {
            if (account != null)
                this.CreateId = account.Id;
        }

        public CreateOfAccount(int createOfAccountId)
        {
            this.CreateId = createOfAccountId;
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        [Required]
        public int CreateId { get; private set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; } = DateTime.Now;
    }
}
