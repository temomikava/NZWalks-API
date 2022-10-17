using NZWalks_API.Models.Domain;

namespace NZWalks_API.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
