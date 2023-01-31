using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //assign new id
            walk.ID = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingwalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (existingwalk == null)
            {
                return null;

            }
            nZWalksDbContext.Walks.Remove(existingwalk);
            await nZWalksDbContext.SaveChangesAsync() ;
            return existingwalk;
            
        }

        public async Task<IEnumerable<Walk>> GetAllRepository()
        {
            return await
                nZWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid Id)
        {
           return nZWalksDbContext.Walks
                 .Include(x => x.Region)
                 .Include(x => x.WalkDifficulty)
                 .FirstOrDefaultAsync(x => x.ID == Id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
           var existingWalk =await nZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk != null)
            {

                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId= walk.RegionId;
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalk;
            }
            return null;

        }
    }
}
