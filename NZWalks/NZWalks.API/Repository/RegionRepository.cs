using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public IEnumerable<Region> GetAll()
        {
            return nZWalksDbContext.Regions.ToList();
        }
    }
}
