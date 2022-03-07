using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcesarEvent(string mensaxe)
        {
            var tipoEvento = DeterminarEvent(mensaxe);

            switch (tipoEvento)
            {
                case TipoEvent.PlataformaPublished:
                    addPlataforma(mensaxe);
                    break;
                default:
                    break;

            }
        }

        private TipoEvent DeterminarEvent(string mensaxeNotificacion)
        {
            Console.WriteLine("--> Determinando Evento");

            var tipoEvento = JsonSerializer.Deserialize<GenericEventDto>(mensaxeNotificacion);

            switch (tipoEvento.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("Evento Plataformar Publicada detectado");
                    return TipoEvent.PlataformaPublished;
                default:
                    Console.WriteLine("Tipo de evento non determinado, non foi posible");
                    return TipoEvent.Indeterminado;
            }
        }

        private void addPlataforma(string plataformaPublishedMensaxe)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IComandoRepo>();

                var plataformaPublishedDto = JsonSerializer.Deserialize<PlataformaPublishedDto>(plataformaPublishedMensaxe);

                try
                {
                    var plat = _mapper.Map<Plataforma>(plataformaPublishedDto);
                    if (!repo.PlataformaExternaExiste(plat.ExternaId))
                    {
                        repo.CreatePlataforma(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Plataforma engadida!");
                    }
                    else
                    {
                        Console.WriteLine("--> Plataforma xa existe...");
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"++++ Non se puido agregar Plataforma a Base de datos {ex.Message}");
                }
            }
        }
    }

    enum TipoEvent
    {
        PlataformaPublished,
        Indeterminado
    }
}