using System;

namespace ASF.Domain.Values
{
    /// <summary>
    /// 登录失败信息
    /// </summary>
    public class LoginFailed : IValueObject
    {
        /// <summary>
        /// 登录失败信息
        /// </summary>
        /// <param name="username">用户名</param>
        public LoginFailed(string username)
        {
            Username = username;
        }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string Username { get; private  set; }

        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailedCount { get; private set; } = 0;

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// 累计失败次数
        /// </summary>
        public void Accumulative()
        {
            this.FailedCount++;
        }
    }
}
