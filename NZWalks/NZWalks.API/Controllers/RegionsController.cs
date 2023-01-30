using AutoMapper;
using Intercom.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NZWalks.API.Model.Domain;
using NZWalks.API.Repository;

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
        public IActionResult GetAllRegions()
        {
           var regions = regionRepository.GetAll();

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
    }
}
 