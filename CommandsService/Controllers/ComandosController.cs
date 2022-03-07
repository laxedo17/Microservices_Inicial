using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/plataformas/{plataformaId}/[controller]")]
    [ApiController]
    public class ComandosController : ControllerBase
    {
        private readonly IComandoRepo _repositorio;
        private readonly IMapper _mapper;

        public ComandosController(IComandoRepo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ComandoReadDto>> GetComandosParaPlataforma(int plataformaId) //esa plataformaId e a misma que a de arriba
        {
            Console.WriteLine($"--> GetComandosParaPlataforma: {plataformaId}");

            if (!_repositorio.PlataformaExiste(plataformaId))
            {
                return NotFound();
            }

            var comandos = _repositorio.GetComandosParaPlataforma(plataformaId);

            return Ok(_mapper.Map<IEnumerable<ComandoReadDto>>(comandos));
        }

        [HttpGet("{comandoId}", Name = "GetComandoParaPlataforma")]
        public ActionResult<ComandoReadDto> GetComandoParaPlataforma(int plataformaId, int comandoId)
        {
            Console.WriteLine($"--> Observacion se obtemos GetComandoParaPlataforma: {plataformaId} / {comandoId}");

            if (!_repositorio.PlataformaExiste(plataformaId))
            {
                return NotFound();
            }

            var comando = _repositorio.GetComando(plataformaId, comandoId);

            if (comando == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ComandoReadDto>(comando));
        }

        [HttpPost]
        public ActionResult<ComandoReadDto> CreateComandoParaPlataforma(int plataformaId, ComandoCreateDto comandoDto)
        {
            Console.WriteLine($"--> CreateComandoParaPlataforma: {plataformaId}");

            if (!_repositorio.PlataformaExiste(plataformaId))
            {
                return NotFound();
            }

            var comando = _mapper.Map<Comando>(comandoDto);

            _repositorio.CreateComando(plataformaId, comando);
            _repositorio.SaveChanges(); //gardamos os cambios asi comando ten unha id que necesitamos na instruccion de abaixo

            var comandoReadDto = _mapper.Map<ComandoReadDto>(comando); //agora comando ten unha id valida

            //Por esto escribimos en Name do HttpGet o parametro GetComandoParaPlataforma
            return CreatedAtRoute(nameof(GetComandoParaPlataforma),
            new { plataformaId = plataformaId, comandoId = comandoReadDto.Id }, comandoReadDto);

        }
    }
}