using NZWalks_API.Models.Domain;

namespace NZWalks_API.Repositories
{
    public interface IWalkDifficultyRepo
    {
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();
        Task<WalkDifficulty> GetAsync(Guid id);
        Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);
        Task<WalkDifficulty> DeleteAsync(Guid id);
        Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);
    }
}
