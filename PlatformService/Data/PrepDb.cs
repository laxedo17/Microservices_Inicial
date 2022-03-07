using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    /// <summary>
    /// Clase para testeo, non para produccion, e crear mock data para Base de datos, e iniciarse con algo. Clase estatica, non podemos usar constructor dependency injection. Usaremos a DbContext 
    /// </summary>
    public static class PrepDb
    {
        //Metodo publico para configurar o context da database. Usamos o DbContext e non o repositorio que creamos, porque asi podemos crear Migrations que son importantes cando pasemos a SQL Server, e moito mais facil facelo directamente nun DbContext e usaremos metodos que non necesitamos construir no noso repositorio
        public static void PrepPoblacion(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        /// <summary>
        /// Metodo para crear semilla de datos e eventualmente facer as migrations. TODO: Actualizar cando existan datos SQL Server
        /// </summary>
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("+++++ Intentando realizar migracions ++++++");

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"------- Non se puido realizar migracion: {ex.Message} --------");
                }

            }

            //si non temos nada, empuxamos datos aqui
            if (!context.Plataformas.Any())
            {
                Console.WriteLine("--> Germinando datos...");

                context.Plataformas.AddRange(
                    new Plataforma() { Nome = "Dot Net", Creador = "Microsoft", Coste = "Gratis" },
                    new Plataforma() { Nome = "SQL Sever Express", Creador = "Microsoft", Coste = "Gratis" },
                    new Plataforma() { Nome = "Kubernetes", Creador = "Cloud Native Computing Foundation", Coste = "Gratis" }
                );

                context.SaveChanges();
            }
            //senon non facemos nada
            else
            {
                Console.WriteLine("--> Xa temos datos"); //Para facer algun tipo de logging
            }
        }
    }
}