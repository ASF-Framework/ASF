using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 集合分页请求对象
    /// </summary>
    public class ListPagedRequestDto
    {
        /// <summary>
        /// 每页显示数量
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int PagedCount { get; set; } = 10;
        /// <summary>
        /// 跳过页
        /// </summary>
        [Range(0, int.MaxValue)]
        public virtual int SkipPage { get; set; }
    }
}
