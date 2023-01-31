using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository WalkRepository,IMapper mapper)
        {
            walkRepository = WalkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllWlksAsync()
        {
            //fetch data from database
           var walksDomain =await walkRepository.GetAllRepository();
            // convert domain to dto
            var walksDTO = mapper.Map<List<Model.DTO.Walk>>(walksDomain);

            //return responce
            return Ok(walksDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalksAsync")]
        public async Task<ActionResult> GetWalksAsync(Guid Id)
        {
            //Domain object toDatabase
           var walkDomain =  await walkRepository.GetAsync(Id);

            // domain objectb to DTO
            var walkDTO = mapper.Map<Model.DTO.Walk>(walkDomain);

            //Return Response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<ActionResult> AddWalkAsync([FromBody] Model.DTO.AddwalkRequest addwalkRequest)
        {
            //convert  dto to domain object 

            var walkDomain = new Model.Domain.Walk
            {
                Length = addwalkRequest.Length,
                Name = addwalkRequest.Name,
                RegionId = addwalkRequest.RegionId,
                WalkDifficultyId = addwalkRequest.WalkDifficultyId


            };

            // pass domain object to repository to persist this

             walkDomain =await walkRepository.AddAsync(walkDomain);

            // convert the domain object back to DTO

            var walkDTO = new Model.DTO.Walk
            {
                ID = walkDomain.ID,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId

            };

            //send DTO to Responce back to client

            return CreatedAtAction(nameof(GetWalksAsync), new { id = walkDTO.ID }, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatewalkAsync([FromRoute] Guid id, [FromBody] Model.DTO.UpdatewalkRequest updatewalkRequest)
        {
            // convert DTO to Domain

            var walkDomain = new Model.Domain.Walk
            {
                Length = updatewalkRequest.Length,
                Name = updatewalkRequest.Name,
                RegionId = updatewalkRequest.RegionId,
                WalkDifficultyId = updatewalkRequest.WalkDifficultyId
            };

            //pass Details TO repositry or null
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            //handel null
             if(walkDomain == null)
            {
                return NotFound();
            }
            
                //convert back DOmain to DTO
                var walkDTO = new Model.DTO.Walk
                {
                    ID = walkDomain.ID,
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId

                };
            return Ok(walkDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletewalkAsync(Guid id)
        {
            //call reposatry
            var walkDomain = await walkRepository.DeleteAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Model.DTO.Walk>(walkDomain);
            return Ok(walkDTO);

        }


    }
}
