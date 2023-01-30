using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Region> AddAsync(Region region)
        {
           region.ID = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;

        }

        public  async Task<Region> DeleteAsync(Guid id)
        {
           var region = await   nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.ID == id);
            if (region == null)
            {
                return null;
            }
            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
           var existingregion =  await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.ID == id);
            if (existingregion == null)
            {
                return null;
            }
            existingregion.Code= region.Code;
            existingregion.Name = region.Name;
            existingregion.Area = region.Area;
            existingregion.Lat = region.Lat;
            existingregion.Long = region.Long;

            await nZWalksDbContext.SaveChangesAsync();
            return existingregion;
        }

    }
}
