using ASF.Domain.Entities;
using ASF.Domain.Values;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 功能权限创建请求
    /// </summary>
    public class PermissionActionCreateRequestDto : IDto
    {
        /// <summary>
        /// 权限代码
        /// </summary>
        [Required, MaxLength(30)]
        public string Code { get; set; }
        /// <summary>
        /// 上级权限编号
        /// </summary>
        [Required, MaxLength(100)]
        public string ParentId { get; set; }
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
        /// <summary>
        /// 是否日志记录
        /// </summary>
        public bool IsLogger { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 99;
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// Http 方法集合
        /// </summary>
        public List<string> HttpMethods { get; set; } = new List<string>();
        /// <summary>
        /// 转换Json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Permission To()
        {
            var p = new Permission(this.Code, this.ParentId, this.Name, PermissionType.Action, this.Description);
            p.SetApiTemplate(this.ApiTemplate);
            p.IsLogger = this.IsLogger;
            p.Sort = this.Sort;
            p.HttpMethods = this.HttpMethods.Select(f=> new HttpMethod(f)).ToList();
            return p;
        }
    }
}
