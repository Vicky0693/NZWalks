


using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repository
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
