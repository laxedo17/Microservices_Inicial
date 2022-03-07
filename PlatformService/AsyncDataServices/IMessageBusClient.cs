using PlatformService.Dtos;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlataforma(PlataformaPublishedDto plataformaPublishedDto);
    }
}