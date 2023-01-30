using AutoMapper;
using Intercom.Core;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace NZWalks.API.Profiles
{
    public class Regionsprofile :Profile
    {
        public Regionsprofile()
        {
            CreateMap<Model.Domain.Region, Model.DTO.Region>().ReverseMap();
        }

    }
}
 