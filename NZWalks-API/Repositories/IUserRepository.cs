using NZWalks_API.Models.Domain;

namespace NZWalks_API.Repositories
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
    }
}
