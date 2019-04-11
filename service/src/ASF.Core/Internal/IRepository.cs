using System.Threading.Tasks;

namespace ASF
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IRepository
    {
    
    }

    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IRepository<TEntity, TPrimaryKey>: IRepository  where TEntity : class
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">标识ID</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TPrimaryKey id);
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="primaryKey">实体唯一标识</param>
        /// <returns></returns>
        Task RemoveAsync(TPrimaryKey primaryKey);
    }
}
