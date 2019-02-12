using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASF.Domain.Entities
{
    /// <summary>
    /// 后台系统相关信息
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        [Required]
        public string Name { get; private set; }
        /// <summary>
        /// 系统Logo
        /// </summary>
        [Required]
        public string Logo { get; private set; }
    }
}
