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

        [HttpGet]
        public async Task<IEnumerable<Message>> Get()
        {
            var from = DateTime.Now - TimeSpan.FromDays(1);
            var till = DateTime.Now + TimeSpan.FromDays(1);
            var messages = await _messageRepository.GetMessagesAsync(1, 2, from, till);

            return messages;
        }

        [HttpPost]
        public async Task<Message> Add(Message message)
        {
            var savedMessage = await _messageRepository.AddMessageAsync(message);

            return savedMessage;
        }
    }
}
