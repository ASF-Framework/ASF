using ASF.Domain.Values;

namespace ASF.Application.DTO
{
    /// <summary>
    /// 权限信息列表查询请求Dto
    /// </summary>
    public class PermissionListRequestDto
    {
        /// <summary>
        /// 模糊查询条件  权限标识、名称
        /// </summary>
        public string Vague { get; set; }
        /// <summary>
        /// 状态 -1:全部 1:启用  0:停止
        /// </summary>
        public int Enable { get; set; } = -1;
        /// <summary>
        /// 父亲节点
        /// </summary>
        public string ParamId { get; set; }
        /// <summary>
        /// 权限履行
        /// </summary>
        public PermissionType Type { get; set; }
    }
}
