using MessageService.Domain.Model;
using MessageService.Domain.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private IMessageRepository _messageRepository;

        public MessagesController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<Message> Add(Message message)
        {
            var savedMessage = await _messageRepository.AddMessageAsync(message);

            return savedMessage;
        }

        [HttpGet("/massages/{userId}")]
        public async Task<IEnumerable<Message>> Get(int userId, int friendId, DateTime from, DateTime till)
        {
            if (from == default || till == default)
            {
                from = DateTime.UtcNow - TimeSpan.FromDays(7);
                till = DateTime.UtcNow;
            }

            var messages = await _messageRepository.GetMessagesAsync(userId, friendId, from, till);

            return messages;
        }

        [HttpGet("/massages/{userId}/latest")]
        public async Task<IEnumerable<Message>> GetLatest(int userId, Guid lastMessageId, int? friendId = null)
        {
            var messages = await _messageRepository.GetLatestMessagesAsync(userId, lastMessageId, friendId);

            return messages;
        }
    }
}
