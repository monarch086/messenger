using RabbitMQModel = MessageService.RabbitMQ.Model;
using DomainModel = MessageService.Domain.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;

namespace MessageService.RabbitMQ
{
    public class RabbitMQProducer : BackgroundService
    {
        private readonly string _host;
        private readonly string _queue;
        private IConnection Connection = default!;
        public RabbitMQProducer(string host, string queue)
        {
            _host = host;
            _queue = queue;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _host };
            Connection = factory.CreateConnection();
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Connection.Dispose();
            return Task.CompletedTask;
        }

        public void Produce(DomainModel.Message message)
        {
            using var channel = Connection.CreateModel();
            var model = new RabbitMQModel.Message { ReceiverId = message.ReceiverId, SenderId = message.SenderId, Text = message.Text };
            string msg = JsonConvert.SerializeObject(model);
            var body = Encoding.UTF8.GetBytes(msg);
            channel.BasicPublish("", routingKey: _queue, basicProperties: null, body: body);
            channel.Dispose();
        }
    }
}
