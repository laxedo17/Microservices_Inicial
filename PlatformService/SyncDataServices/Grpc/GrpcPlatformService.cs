using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlataformaRepo _repositorio;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlataformaRepo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var resposta = new PlatformResponse();
            var plataformas = _repositorio.GetTodasPlataformas();

            foreach (var plat in plataformas)
            {
                resposta.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return Task.FromResult(resposta);
        }
    }
}