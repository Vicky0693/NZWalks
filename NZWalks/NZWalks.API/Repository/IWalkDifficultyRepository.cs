using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repository
{
    public interface IWalkDifficultyRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllasync();
        Task<WalkDifficulty> GetAsync(Guid Id);

        Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);

        Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);


        Task<WalkDifficulty> DeleteAsync(Guid id);
    }
}
