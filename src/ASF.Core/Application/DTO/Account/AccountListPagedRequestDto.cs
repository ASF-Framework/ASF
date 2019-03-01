using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 查询账户信息集合分页请求
    /// </summary>
    public class AccountListPagedRequestDto : ListPagedRequestDto
    {
        /// <summary>
        /// 模糊查询条件  用户名、ID、昵称
        /// </summary>
        [ MaxLength(50)]
        public string Vague { get; set; }
        /// <summary>
        /// 状态 -1:全部 1:正常  2:不允许登录
        /// </summary>
        public int Status { get; set; } = -1;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get;  set; } = false;
    }
}
