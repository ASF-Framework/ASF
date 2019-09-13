using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 修改功能权限请求
    /// </summary>
    public class PermissionActionModifyRequestDto : IDto
    {
        /// <summary>
        /// 权限标识
        /// </summary>
        [Required]
        public string Id { get; set; }
  
        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// API模板
        /// </summary>
        [Required, MaxLength(500)]
        public string ApiTemplate { get; set; }
        /// <summary>
        /// 是否日志记录
        /// </summary>
        public bool IsLogger { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
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
    }
}
