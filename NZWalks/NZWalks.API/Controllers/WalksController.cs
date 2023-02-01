using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository WalkRepository, IMapper mapper ,IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            walkRepository = WalkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        [HttpGet]
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "writer")]
        public async Task<ActionResult> AddWalkAsync([FromBody] Model.DTO.AddwalkRequest addwalkRequest)
        {

            //validate the request
            if (!(await ValidateAddWalkAsync(addwalkRequest)))
            {
                return BadRequest(ModelState);
            }
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdatewalkAsync([FromRoute] Guid id, [FromBody] Model.DTO.UpdatewalkRequest updatewalkRequest)
        {
            //validate the request
            if (!(await ValidateUpdatewalkAsync(updatewalkRequest)))
            {
                return BadRequest(ModelState);
            }



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
        [Authorize(Roles = "writer")]
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


        #region Ptivate Methods
        private async Task<bool> ValidateAddWalkAsync(Model.DTO.AddwalkRequest addwalkRequest)
        {
            if (addwalkRequest == null)
            {
                ModelState.AddModelError(nameof(addwalkRequest), $" Add walk Is Required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addwalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addwalkRequest.Name), $"{nameof(addwalkRequest.Name)} Is Required.");
            }
            if (addwalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addwalkRequest.Length), $"{nameof(addwalkRequest.Length)} Cannot be less then zero.");
            }

            var region = await regionRepository.GetAsync(addwalkRequest.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(addwalkRequest.RegionId), $"{nameof(addwalkRequest.RegionId)} it is invalide.");

            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addwalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addwalkRequest.WalkDifficultyId), $"{nameof(addwalkRequest.WalkDifficultyId)} it is invalide.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

        }

        private async Task<bool> ValidateUpdatewalkAsync(Model.DTO.UpdatewalkRequest updatewalkRequest)
        {
            if (updatewalkRequest == null)
            {
                ModelState.AddModelError(nameof(updatewalkRequest), $" Add walk Is Required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updatewalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updatewalkRequest.Name), $"{nameof(updatewalkRequest.Name)} Is Required.");
            }
            if (updatewalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updatewalkRequest.Length), $"{nameof(updatewalkRequest.Length)} Cannot be less then zero.");
            }

            var region = await regionRepository.GetAsync(updatewalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updatewalkRequest.RegionId), $"{nameof(updatewalkRequest.RegionId)} it is invalide.");

            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updatewalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updatewalkRequest.WalkDifficultyId), $"{nameof(updatewalkRequest.WalkDifficultyId)} it is invalide.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

        }


        #endregion

    }
}
