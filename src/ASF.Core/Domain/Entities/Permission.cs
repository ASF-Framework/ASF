using ASF.Domain.Values;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ASF.Domain.Entities
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Permission : IEntity
    {
        private Permission()
        {

        }
        public Permission(string code, string parentId, string name, PermissionType type, string description)
        {
            code = code.ToLower();//code默认小写
            if (!string.IsNullOrEmpty(parentId))
                this.Id = $"{parentId}.{code}";
            else
                this.Id = code;
            this.Code = code;
            this.ParentId = parentId;
            this.Name = name;
            this.Type = type;
            this.Description = description;
            this.Enable = true;
        }
        /// <summary>
        /// 唯一标示
        /// </summary>
        [Key, Required, MaxLength(100)]
        public string Id { get; private set; }
        /// <summary>
        /// 权限代码
        /// </summary>
        [Required, MaxLength(10)]
        public string Code { get; private set; }
        /// <summary>
        /// 上级权限编号
        /// </summary>
        [MaxLength(100)]
        public string ParentId { get;  set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }
        /// <summary>
        /// 是否系统权限
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 权限服务地址
        /// </summary>
        [MaxLength(500)]
        public string ApiTemplate { get; private set; }
        /// <summary>
        /// Api模板的正则表达式
        /// </summary>
        private Regex ApiTemplateRegex { get; set; }
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
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// 权限是否可用
        /// </summary>
        /// <returns></returns>
        public bool IsNormal()
        {
            return this.Enable;
        }

        /// <summary>
        /// 设置权限API模板
        /// </summary>
        /// <param name="apiAddress"></param>
        public void SetApiTemplate(string apiTemplate)
        {
            this.ApiTemplateRegex = new Regex($"^{this.ApiTemplate}$");
            this.ApiTemplate = apiTemplate.ToLower();
        }

        /// <summary>
        /// 匹配Api模板
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        public bool MatchApiTemplate(string requestPath)
        {
            if (ApiTemplateRegex == null)
                this.ApiTemplateRegex = new Regex($"^{this.ApiTemplate}$");
            return this.ApiTemplateRegex.IsMatch(requestPath);
        }
    }
}
