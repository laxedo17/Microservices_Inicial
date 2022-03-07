using System.Threading.Tasks;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
 public interface IComandoDataClient
 {
  Task SendPlataformaToComando(PlataformaReadDto plat);
 }
}