using System;
using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlataformaDataClient>();

                var plataformas = grpcClient.ReturnAllPlataformas();

                SeedData(serviceScope.ServiceProvider.GetService<IComandoRepo>(), plataformas);
            }
        }

        private static void SeedData(IComandoRepo repo, IEnumerable<Plataforma> plataformas)
        {
            Console.WriteLine("Xerminando novas plataformas na base de datos");

            foreach (var plataforma in plataformas)
            {
                if (!repo.PlataformaExternaExiste(plataforma.ExternaId))
                {
                    repo.CreatePlataforma(plataforma);
                }
                repo.SaveChanges();
            }
        }
    }
}