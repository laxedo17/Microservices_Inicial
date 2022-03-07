using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
 public class HttpComandoDataClient : IComandoDataClient
 {
  private readonly HttpClient _httpClient;
  private readonly IConfiguration _config;

  public HttpComandoDataClient(HttpClient httpClient, IConfiguration config)
  {
   _httpClient = httpClient; //inxectamos con Dependency Injection un httpclient que funcionara coa nosa HttpClient factory
   _config = config;
  }
  public async Task SendPlataformaToComando(PlataformaReadDto plat)
  {
   var httpContent = new StringContent(
       JsonSerializer.Serialize(plat),
       Encoding.UTF8,
       "application/json");

   var response = await _httpClient.PostAsync($"{_config["CommandService"]}", httpContent);

   if (response.IsSuccessStatusCode)
   {
    Console.WriteLine("--> Sincronizacion POST a ComandoService funciona");
   }
   else
   {
    Console.WriteLine("--> Sincronizacion POST a ComandoService funciona");
   }
  }
 }
}