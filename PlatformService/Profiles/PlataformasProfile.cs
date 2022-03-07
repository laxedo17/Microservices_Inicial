using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlataformasProfile : Profile
    {
        public PlataformasProfile()
        {
            //           Orixen ->  Destino
            CreateMap<Plataforma, PlataformaReadDto>(); //como en ambas hai propiedades casi exactas, non e necesario indicar nada a Automapper
            CreateMap<PlataformaCreateDto, Plataforma>();
            CreateMap<PlataformaReadDto, PlataformaPublishedDto>();
            CreateMap<Plataforma, GrpcPlatformModel>()
                .ForMember(dest => dest.PlataformaId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Creador, opt => opt.MapFrom(src => src.Creador));//estos dous ultimos podrian non ser necesarios pero en caso de ser irian aqui
        }
    }
}