using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ASF.Domain.Services
{
    /// <summary>
    /// 记录操作日志
    /// </summary>
    public class LogOperateRecordService
    {
        private readonly ILoggingRepository _loggingRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogOperateRecordService(ILoggingRepository loggingRepository, IHttpContextAccessor httpContextAccessor)
        {
            this._loggingRepository = loggingRepository;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="permission">记录的权限</param>
        /// <param name="requestData">请求参数</param>
        /// <param name="responseData">响应参数</param>
        public void Record(Permission permission, string requestData, string responseData)
        {
            string subject = string.IsNullOrEmpty(permission.Description) ? permission.Name : permission.Description;
            this.Record((permission.Id, subject), requestData, responseData);
        }

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="permission">记录的权限 标识和权限名称</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="responseData">响应数据</param>
        public void Record((string Id, string Name) permission, object requestData, string responseData)
        {
            string requestStr = requestData == null ? "" : JsonConvert.SerializeObject(requestData);
            this.Record(permission, requestStr, responseData);
        }
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="permission">记录的权限 标识和权限名称</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="responseData">响应数据</param>
        public void Record((string Id, string Name) permission, string requestData, string responseData)
        {
            //获取请求数据
            var httpContext = _httpContextAccessor.HttpContext;
            var accountId = httpContext.User.UserId();
            var accountName = httpContext.User.Name();
            var apiAddress = httpContext.Request.PathBase + httpContext.Request.Path;
            if (!string.IsNullOrEmpty(httpContext.Request?.QueryString.Value))
            {
                apiAddress = apiAddress + "?" + httpContext.Request.QueryString;
            }
            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(clientIp))
                clientIp = "127.0.0.1";
            var log = new Logging(Convert.ToInt32(accountId), accountName, Values.LoggingType.Operate, permission.Name, clientIp)
            {
                ApiAddress = apiAddress,
                ApiRequest = requestData,
                ApiResponse = responseData,
                PermissionId = permission.Id
            };

            //验证数据聚合有效性
            var result = log.Valid();
            if (!result.Success)
                return;
            else
            {
                //添加到数据库中
                _loggingRepository.AddAsync(log);
            }
        }
    }
}
