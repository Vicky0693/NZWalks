using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repository
{
    public interface IRegionRepository
    {
        IEnumerable<Region> GetAll();
    }
}
