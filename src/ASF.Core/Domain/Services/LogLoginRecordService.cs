using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 记录登录日志
    /// </summary>
    public class LogLoginRecordService
    {
        private readonly ILoggingRepository _loggingRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogLoginRecordService(ILoggingRepository loggingRepository, IHttpContextAccessor httpContextAccessor)
        {
            this._loggingRepository = loggingRepository;
            this._httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        public void Record(Account account, string loginType)
        {
            if (account == null)
                throw new ArgumentNullException("登录用户不能为空");

            //获取操作的客户端IP

            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(clientIp))
                clientIp = "127.0.0.1";
            //创建日志对象
            var logging = new Logging(account, Values.LoggingType.Login, loginType, clientIp)
            {
                Remark = "登录成功"
            };

            //验证数据聚合有效性
            var result = logging.Valid();
            if (!result.Success)
                return;
            else
            {
                //添加到数据库中
                _loggingRepository.AddAsync(logging);
            }
        }
    }
}
