namespace ASF.Application.DTO
{
    /// <summary>
    /// 角色信息集合查询请求
    /// </summary>
    public class RoleInfoListPagedRequestDto: ListPagedRequestDto
    {
        /// <summary>
        /// 模糊查询条件  角色标识、名称
        /// </summary>
        public string Vague { get; set; }
        /// <summary>
        /// 状态 -1:全部 1:启用  0:停止
        /// </summary>
        public int Enable { get; set; } = -1;
    }
}
