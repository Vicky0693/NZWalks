using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repository
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
