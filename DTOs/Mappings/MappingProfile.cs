using APICatalago.Models;
using AutoMapper;

namespace APICatalago.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile() { 
        CreateMap<Igreja, IgrejaDTO>().ReverseMap();
        CreateMap<Ministerio, MinisterioDTO>().ReverseMap();
    }
}
