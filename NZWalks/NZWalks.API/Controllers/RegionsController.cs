using AutoMapper;
using Intercom.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NZWalks.API.Model.Domain;
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



    }
}
 