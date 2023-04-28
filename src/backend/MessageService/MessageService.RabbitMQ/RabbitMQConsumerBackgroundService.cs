using MessageService.Domain.Persistence;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

using RabbitMQModel = MessageService.RabbitMQ.Model;
using DomainModel = MessageService.Domain.Model;
using Newtonsoft.Json;

namespace MessageService.RabbitMQ
{
    public class RabbitMQConsumerBackgroundService : BackgroundService
    {
        private readonly string _host;
        private readonly string _queue;
        private readonly IMessageRepository _messageRepository;

        private IConnection Connection = default!;
        private IModel Channel = default!;

        public RabbitMQConsumerBackgroundService(string host, string queue, IMessageRepository messageRepository)
        {
            _host = host;
            _queue = queue;
            _messageRepository = messageRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _host };
            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();
            var consumer = new EventingBasicConsumer(Channel);

            consumer.Received += (o, a) =>
            {
                var message = Encoding.UTF8.GetString(a.Body.ToArray())!;
                var model = JsonConvert.DeserializeObject<RabbitMQModel.Message>(message);
                HandleMessageAsync(model!);
            };
            Channel.BasicConsume(queue: _queue,
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Connection.Dispose();
            Channel.Dispose();

            return Task.CompletedTask;
        }

        private void HandleMessageAsync(RabbitMQModel.Message message)
        {
            _messageRepository.AddMessageAsync(new DomainModel.Message
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Text = message.Text
            }).GetAwaiter().GetResult();
        }

    }
}