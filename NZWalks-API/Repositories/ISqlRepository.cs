namespace NZWalks_API.Repositories
{
    public interface ISqlRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> DeleteAsync(Guid id);
        Task<T> UpdateAsync(Guid id, T entity);
    }
}
