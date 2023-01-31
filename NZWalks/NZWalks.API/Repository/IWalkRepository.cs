using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repository
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAllRepository();

        Task<Walk> GetAsync(Guid Id);

         Task<Walk> AddAsync(Walk walk);

        Task<Walk> UpdateAsync( Guid id ,Walk walk);
        Task<Walk> DeleteAsync(Guid id);
    }
}
