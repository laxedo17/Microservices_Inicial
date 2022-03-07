using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")] //vale igualmente escribir [Route("api/Plataformas")] que seria hardcodeando
    [ApiController]
    public class PlataformasController : ControllerBase
    {
        private readonly IPlataformaRepo _repositorio;
        private readonly IMapper _mapper;
        private readonly IComandoDataClient _comandoDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlataformasController(IPlataformaRepo repositorio, IMapper mapper, IComandoDataClient comandoDataClient, IMessageBusClient messageBusClient) //tipico patron con dependency injection
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _comandoDataClient = comandoDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet] //accion Get na ruta especificada en Route arriba
        public ActionResult<IEnumerable<PlataformaReadDto>> GetPlataformas()
        {
            Console.WriteLine("--> Obtendo lista de plataformas......");

            var plataformaItem = _repositorio.GetTodasPlataformas(); //usa IPlataformaRepo.cs e PlataformaRepo.cs

            return Ok(_mapper.Map<IEnumerable<PlataformaReadDto>>(plataformaItem)); //facil devolver un resultado con Automapper, utiliza PlataformasProfile, donde está definido
        }

        [HttpGet("{id}", Name = "GetPlataformaPorId")]
        public ActionResult<PlataformaReadDto> GetPlataformaPorId(int id)
        {
            var plataformaItem = _repositorio.GetPlataformaById(id);

            if (plataformaItem != null)
            {
                return Ok(_mapper.Map<PlataformaReadDto>(plataformaItem));
            }

            return NotFound();
        }

        [HttpPost]
        /// <summary>
        /// Creamos plataforma usando un obxeto PlataformaCreateDto que pasaremos a un repositorio
        /// </summary>
        /// <param name="plataformaCreateDto"></param>
        /// <returns></returns>
        public async Task<ActionResult<PlataformaReadDto>> CreatePlataforma(PlataformaCreateDto plataformaCreateDto)
        {
            var plataformaModel = _mapper.Map<Plataforma>(plataformaCreateDto);
            _repositorio.CreatePlataforma(plataformaModel);
            _repositorio.SaveChanges();

            //Devolvemos 201 e unha uri a localizacion de ese recurso
            var plataformaReadDto = _mapper.Map<PlataformaReadDto>(plataformaModel);

            //Envia message en formato Sync
            try
            {
                await _comandoDataClient.SendPlataformaToComando(plataformaReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Non enviado de forma sincronica: {ex.Message}");
            }

            //Envia mensaxe en formato Async
            try
            {
                var plataformaPublishedDto = _mapper.Map<PlataformaPublishedDto>(plataformaReadDto);
                plataformaPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlataforma(plataformaPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Non se puido enviar mensaxe de forma asincronica: {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlataformaPorId), new { Id = plataformaReadDto.Id }, plataformaReadDto);
        }
    }
}