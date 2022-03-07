using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) }; //creamos factory a estilo HttpClient pero para RabbitMQ
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout); //a partir de aqui estarimos conectados a nosa instancia RabbitMQ
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown; //suscribimonos a event delegate ConnectionShutdown

                Console.WriteLine("++++ Conectad@ a MessageBus de RabbitMQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($".... Non puidemos conectarnos a Message Bus: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("++++ RabbitMQ Conexion finalizada");
        }

        public void PublishNewPlataforma(PlataformaPublishedDto plataformaPublishedDto)
        {
            var mensaxe = JsonSerializer.Serialize(plataformaPublishedDto); //obtemos string serializado

            if (_connection.IsOpen)
            {
                Console.WriteLine("++++ Conexion RabbitMQ aberta, enviando mensaxe");
                EnviarMensaxe(mensaxe);
            }
            else
            {
                Console.WriteLine("++++ Conexions RabbitMQ pechadas, non estamos enviando datos");
            }
        }

        private void EnviarMensaxe(string mensaxe)
        {
            var corpo = Encoding.UTF8.GetBytes(mensaxe); //o body -corpo- da mensaxe transformada en un array de bytes

            _channel.BasicPublish(exchange: "trigger",
                                    routingKey: "",
                                    basicProperties: null,
                                    body: corpo);
            Console.WriteLine($"++++ Enviamos a seguinte mensaxe: {mensaxe}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}