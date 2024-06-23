using AutoMapper;
using RestApi.Models;

namespace RestApi.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<VillaUpdateDto, VillaDTO>().ReverseMap();
        CreateMap<VillaCreateDTO, VillaDTO>().ReverseMap();
        CreateMap<VillaResponse, VillaDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaResponse>().ReverseMap();
    }
}