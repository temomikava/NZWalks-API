using NZWalks_API.Models.Domain;

namespace NZWalks_API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region> GetAsync(Guid id);
    }
}
