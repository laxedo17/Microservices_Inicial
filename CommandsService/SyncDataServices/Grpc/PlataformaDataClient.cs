using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlataformaDataClient : IPlataformaDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlataformaDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Plataforma> ReturnAllPlataformas()
        {
            Console.WriteLine($"--Chamando a Servicio GRPC{_configuration["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest(); //procedente do arquivo proto

            try
            {
                var resposta = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Plataforma>>(resposta.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Non poidemos chamar ao servidor GRPC {ex.Message}");
                return null;
            }
        }
    }
}