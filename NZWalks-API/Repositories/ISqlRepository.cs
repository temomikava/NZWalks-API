namespace NZWalks_API.Repositories
{
    public interface ISqlRepository<TEntity,TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(TKey id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TKey id);
        Task<TEntity> UpdateAsync(TKey id, TEntity entity);
    }
}
