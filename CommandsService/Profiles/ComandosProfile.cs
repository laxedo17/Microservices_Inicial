using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles
{
    public class ComandosProfile : Profile
    {
        public ComandosProfile()
        {
            // Orixen -> Destino
            CreateMap<Plataforma, PlataformaReadDto>();
            CreateMap<ComandoCreateDto, Comando>();
            CreateMap<Comando, ComandoReadDto>();
            CreateMap<PlataformaPublishedDto, Plataforma>()
                .ForMember(dest => dest.ExternaId, opt => opt.MapFrom(src => src.Id)); //no noso destino externo queremos mapear a Id que obtemos da nosa PlataformaPublishedDto
            CreateMap<GrpcPlatformModel, Plataforma>()
                .ForMember(dest => dest.ExternaId, opt => opt.MapFrom(src => src.PlataformaId))
                //pasos opcionales
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Comandos, opt => opt.Ignore());//Automapper ignorara este comando para non facer nada con el
        }
    }
}