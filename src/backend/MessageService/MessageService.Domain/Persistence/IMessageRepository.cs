using MessageService.Domain.Model;

namespace MessageService.Domain.Persistence
{
    public interface IMessageRepository
    {
        Task<Message> AddMessageAsync(Message message);

        Task<IEnumerable<Message>> GetMessagesAsync(int userId, int friendId, DateTime from, DateTime till);

        Task<IEnumerable<Message>> GetLatestMessagesAsync(int userId, int lastMessageId, int? friendId = null);
    }
}
