using DomainModel = MessageService.Domain.Model;


namespace MessageService.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        void Produce(DomainModel.Message message);
    }
}