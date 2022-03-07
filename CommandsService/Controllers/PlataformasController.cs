using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{

    [Route("api/c/[controller]")] //xa temos un controller plataformas, hai que cambiar algun detalle aqui para que ambas urls non se colapsen unha a outra, por iso a c
    [ApiController]
    public class PlataformasController : ControllerBase
    {
        private readonly IComandoRepo _repositorio;
        private readonly IMapper _mapper;

        public PlataformasController(IComandoRepo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlataformaReadDto>> GetPlataformas()
        {
            Console.WriteLine("+++++ Obtendo plataformas de CommandsService");

            var plataformaItems = _repositorio.GetTodasPlataformas();

            return Ok(_mapper.Map<IEnumerable<PlataformaReadDto>>(plataformaItems)); //mapeamos todas plataformas obtidas
        }

        [HttpPost]
        public ActionResult TestConexionsEntrada()
        {
            Console.WriteLine("--> POST de entrada # Servicio Comandos");
            return Ok("Test de entrada de controller Plataformas");
        }
    }
}