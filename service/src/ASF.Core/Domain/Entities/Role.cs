using ASF.Domain.Values;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;

namespace ASF.Domain.Entities
{
    /// <summary>
    /// 管理员角色
    /// </summary>
    public class Role : IEntity
    {
        private List<string> permissions = new List<string>();
        private Role()
        {

        }
        public Role(int id, string name, string description, int createOfAccountId)
        {
            if (createOfAccountId == 0)
                throw new ArgumentNullException("创建用户不能为空");
            this.CreateInfo = new CreateOfAccount(createOfAccountId);
            this.Name = name;
            this.Description = description;
            this.Enable = true;
        }
        /// <summary>
        /// 角色编号
        /// </summary>
        [Key]
        public int Id { get; private set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required, MaxLength(20)]
        public string Name { get; set; }
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
        /// 创建时信息
        /// </summary>
        [Required]
        public CreateOfAccount CreateInfo { get; private set; }
        /// <summary>
        /// 分配的权限
        /// </summary>
        public IReadOnlyList<string> Permissions
        {
            get
            {
                return permissions;
            }
            set
            {
                permissions = value.ToList();
            }
        }

        /// <summary>
        /// 角色是否正常
        /// </summary>
        /// <returns></returns>
        public bool IsNormal()
        {
            return this.Enable;
        }

        /// <summary>
        /// 设置分配的权限
        /// </summary>
        /// <param name="permissions">分配的权限</param>
        public void SetPermissions(List<string> permissions)
        {
            if (permissions == null)
                permissions = new List<string>();
            this.Permissions = permissions;
        }
        /// <summary>
        /// 是否包含对应的权限
        /// </summary>
        /// <param name="permissionId">权限标识</param>
        /// <returns></returns>
        public bool ContainPermission(string permissionId)
        {
            return this.Permissions.Contains(permissionId);
        }
    }
}
