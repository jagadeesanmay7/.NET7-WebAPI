using AutoMapper;
using World.API.DTO.Country;
using World.API.Models;

namespace World.API.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
        }
    }
}
