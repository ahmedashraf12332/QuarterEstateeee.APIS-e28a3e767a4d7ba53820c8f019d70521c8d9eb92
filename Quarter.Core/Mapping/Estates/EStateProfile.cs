using AutoMapper;
using Microsoft.Extensions.Configuration;
using Quarter.Core.Dto;
using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Mapping.Estates
{
    public class EstateProfile:Profile
    {
        public EstateProfile(IConfiguration configuration)
        {
            CreateMap<Estate, EstateDto>()
                .ForMember(d => d.EstateLocationName, options => options.MapFrom(s => s.EstateLocation.City)).
                ForMember(d => d.EstateLocationName, options => options.MapFrom(s => s.EstateLocation.Area)).
                    ForMember(d => d.EstateTypeName, options => options.MapFrom(s => s.EstateType.Name))
             .ForMember(d => d.Images, options => options.MapFrom(s => $"{configuration["BASEURL"]}{s.Images}"));

            CreateMap<EstateLocation, EstateLocationDto>();
            CreateMap<EstateType, EstateTypeDto>();
        }
    }
}
