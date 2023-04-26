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
        public IEnumerable<Message> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Message(index, index, $"text {index}"))
            .ToArray();
        }

        [HttpPost]
        public async Task<Message> Add(Message message)
        {
            var savedMessage = await _messageRepository.AddMessageAsync(message);

            return savedMessage;
        }
    }
}
