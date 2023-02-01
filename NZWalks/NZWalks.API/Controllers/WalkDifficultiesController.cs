using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository WalkDifficultyRepository ,IMapper mapper)
        {
            walkDifficultyRepository = WalkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var WalkDifficultieDomain = await walkDifficultyRepository.GetAllasync();
            var WalkDifficultieDTO = mapper.Map<List<Model.DTO.WalkDifficulty>>(WalkDifficultieDomain);

            return Ok(WalkDifficultieDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetAllwalkDifficultyById")]
        [Authorize(Roles = "reader")]

        public async Task<IActionResult> GetAllwalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = mapper.Map<Model.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficultyAsync(Model.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {

            //validate the request
            if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            // convert DTO to DOmain
            var WalkDifficultyDomain = new Model.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            WalkDifficultyDomain = await walkDifficultyRepository.AddAsync(WalkDifficultyDomain);

          var WalkDifficultyDTO =  mapper.Map<Model.DTO.WalkDifficulty>(WalkDifficultyDomain);
            return CreatedAtAction(nameof(GetAllwalkDifficultyById), new { id = WalkDifficultyDTO.Id }, WalkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync( Guid id,Model.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest )
        {

            //validate the request
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            // convert DTO to DOmain MOdel

            var WalkDifficultyDomain = new Model.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            // call repostry

            WalkDifficultyDomain= await walkDifficultyRepository.UpdateAsync(id, WalkDifficultyDomain);

            if(WalkDifficultyDomain == null)
            {
                return NotFound();
            }

            var WalkDifficultyDTO = mapper.Map<Model.DTO.WalkDifficulty>(WalkDifficultyDomain);

            //return

            return Ok (WalkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var WalkDifficultyDomain = walkDifficultyRepository.DeleteAsync(id);
            if(WalkDifficultyDomain == null)
            {
                return NotFound();

            }

            // convert to DTO

            var WalkDifficultyDTO = mapper.Map<Model.DTO.WalkDifficulty>(WalkDifficultyDomain);
            return Ok(WalkDifficultyDTO);

        }




        #region Ptivate Methods
        private bool ValidateAddWalkDifficultyAsync(Model.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $" addWalkDifficultyRequest Is Required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} Cannot be null or empty or white space.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

        }

        private bool ValidateUpdateWalkDifficultyAsync(Model.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $" addWalkDifficultyRequest Is Required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} Cannot be null or empty or white space.");
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
