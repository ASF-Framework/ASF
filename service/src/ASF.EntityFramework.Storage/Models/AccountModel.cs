using ASF.Domain.Values;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASF.EntityFramework.Model
{
    [Table("Accounts")]
    public class AccountModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        public int Id { get; private set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Required, MaxLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required, StringLength(32, MinimumLength = 2)]
        public string Username { get;  set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required, StringLength(32, MinimumLength = 6)]
        public string Password { get;  set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(225)]
        public string Avatar { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public AccountStatus Status { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get;  set; }
        /// <summary>
        /// 角色集
        /// </summary>
        [Required]
        [MaxLength(20000)]
        public string Roles { get;  set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public int CreateId { get;  set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// 最后登录IP
        /// </summary>
        [MaxLength(20)]
        public string LoginIp { get;  set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LoginTime { get;  set; } = DateTime.Now;
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get;  set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get;  set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expired { get;  set; }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            this.IsDeleted = true;
        }
    }
}
