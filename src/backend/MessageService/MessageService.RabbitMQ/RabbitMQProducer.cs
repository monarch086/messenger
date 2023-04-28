using RabbitMQModel = MessageService.RabbitMQ.Model;
using DomainModel = MessageService.Domain.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace MessageService.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly string _host;
        private readonly string _queue;

        public RabbitMQProducer(string host, string queue)
        {
            _host = host;
            _queue = queue;
        }

        public void Produce(DomainModel.Message message)
        {
            var factory = new ConnectionFactory() { HostName = _host };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var model = new RabbitMQModel.Message { ReceiverId = message.ReceiverId, SenderId = message.SenderId, Text = message.Text };
            string msg = JsonConvert.SerializeObject(model);
            var body = Encoding.UTF8.GetBytes(msg);
            channel.BasicPublish("", routingKey: _queue, basicProperties: null, body: body);
        }
    }
}
