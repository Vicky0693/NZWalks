using AutoMapper;
using Intercom.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repository;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository RegionRepository, IMapper mapper )
        {
            regionRepository = RegionRepository;
            this.mapper = mapper;
        }

       

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
           var regions =  await regionRepository.GetAllAsync();

            ////var regionsDTO = new List<Models.DTO.Region>();
            //var regionsDTO = new List<Model.DTO.Region>();

            //regions.ToList().ForEach(Region =>
            //{

            //    var regionDTO = new Model.DTO.Region()
            //    {
            //        ID = Region.ID,
            //        Code= Region.Code,
            //        Name=Region.Name,
            //        Area=Region.Area,
            //        Lat=Region.Lat,
            //        Long=Region.Long,
            //        Population=Region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});

             var regionsDTO = mapper.Map<List<Model.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
           var region = await regionRepository.GetAsync(id);
            if(region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Model.DTO.Region>(region);
            return Ok(regionDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Model.DTO.AddRegionRequest addRegionRequest)
        {
            //validate the request
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

            // DTO to DOmain
            var region = new Model.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };

            //Pass Details To Repositry

             region= await regionRepository.AddAsync(region);
            // Back TO DTO
            var regionDTO = new Model.DTO.Region()
            {
                Code = region.Code,
                ID = region.ID,
                Area = region.Area,
                Name = region.Name,
                Population = region.Population,
                Lat = region.Lat,
                Long = region.Long,

            };

            return CreatedAtAction(nameof(GetRegionAsync),new {id =regionDTO.ID},regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //get region from database
            var region = await regionRepository.DeleteAsync(id);


            //if null not found

            if (region == null)
            {
                return NotFound();
            }
            //Convert responce back to DTO
            var regionDTO = new Model.DTO.Region()
            {
                Code = region.Code,
                ID = region.ID,
                Area = region.Area,
                Name = region.Name,
                Population = region.Population,
                Lat = region.Lat,
                Long = region.Long,

            };

            //Retirn Ok Responce

            return Ok(regionDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]Model.DTO.UpdateRegionRequest updateRegionRequest)
        {
               //validate the request
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }


            // conver dto to domain

            var region = new Model.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,

            };

            // update region reposaty

           region= await regionRepository.UpdateAsync(id, region);
            //if null then not found
            if(region == null)
            {
                return NotFound();
            }
            // convert domain to dto
            var regionDTO = new Model.DTO.Region()
            {
                Code = region.Code,
                ID = region.ID,
                Area = region.Area,
                Name = region.Name,
                Population = region.Population,
                Lat = region.Lat,
                Long = region.Long,

            };

            //return ok responce
            return Ok(regionDTO);

        }

        #region Ptivate Methods
        private bool ValidateAddRegionAsync(Model.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $" Add REgion Is Required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} Cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} Cannot be null or empty or white space.");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} Cannot be less then or eqaul to zero.");
            }


            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} Cannot be less then zero.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

        }

        private bool ValidateUpdateRegionAsync(Model.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $" Add REgion Is Required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} Cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} Cannot be null or empty or white space.");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} Cannot be less then or eqaul to zero.");
            }


            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} Cannot be less then zero.");
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
 