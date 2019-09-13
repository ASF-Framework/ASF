using ASF.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 创建开放API请求
    /// </summary>
    public class PermissionOpenApiCreateRequestDto : IDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 权限服务地址
        /// </summary>
        [Required, MaxLength(500)]
        public string ApiTemplate { get; set; }
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// Http 方法集合
        /// </summary>
        public List<string> HttpMethods { get; set; } = new List<string>();
        public Permission To()
        {
            string code = this.GetTimeStamp();
            var p = new Permission(code, "", this.Name, Domain.Values.PermissionType.OpenApi, this.Description);
            p.SetApiTemplate(this.ApiTemplate);
            p.HttpMethods = this.HttpMethods.Select(f => new HttpMethod(f)).ToList();
            return p;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }
    }
}
