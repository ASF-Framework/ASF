namespace ASF.Application.DTO
{
    /// <summary>
    /// 集合分页排序请求对象
    /// </summary>
    public class ListPagedAndSortedRequestDto: ListPagedRequestDto
    {
        /// <summary>
        /// Sorting information.
        /// Should include sorting field and optionally a direction (ASC or DESC)
        /// Can contain more than one field separated by comma (,).
        /// </summary>
        /// <example>
        /// Examples:
        /// "Name"
        /// "Name DESC"
        /// "Name ASC, Age DESC"
        /// </example>
        public virtual string Sorting { get; set; }
    }
}
