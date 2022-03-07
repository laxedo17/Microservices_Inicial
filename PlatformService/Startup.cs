using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

namespace PlatformService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env; //para indicar si usar base de datos en memoria ou base de datos SQL Server por si estamos correndo programa en Production ou non
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("+++++ Usando base de datos SQL Server para produccion ++++++");
                services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PlataformasConn")));
            }
            else
            {
                Console.WriteLine("+++++ Usando base de datos InMem ++++++");
                services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem")); //por agora, despois cando pasemos a produccion con Kubernetes, usamos SQL Server
            }
            services.AddScoped<IPlataformaRepo, PlataformaRepo>(); //rexistramos a nosa interface e concrete implementation con Dependency Injection. Tras esto usamos dotnet build
            services.AddHttpClient<IComandoDataClient, HttpComandoDataClient>(); //usamos HttpClient para facer uso dunha client factory, que facilita as conexions
            services.AddSingleton<IMessageBusClient, MessageBusClient>(); //agregamos a Dependency Injection, e queda establecido como Singleton dado que queremos usar a misma conexion en toda a app
            services.AddGrpc(); //configuramos GRPC para Dependency Injection
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //Automapper rexistrado nun dependency injection container para usar en resto de aplicacion
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> ComandoService endpoint {Configuration["CommandService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcPlatformService>();

                //Opcional: Sirve o arquivo proto a un cliente e asi poden usalo
                endpoints.MapGet("/protos/plataformas.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/plataformas.proto"));
                });
            });

            //usamos isntancia de app de IApplicationBuilder para chamar a un service scope, que poderemos usar para crear un DbContext
            //Como e clase estatica podemos usala directamente
            PrepDb.PrepPoblacion(app, env.IsProduction());

        }
    }
}
