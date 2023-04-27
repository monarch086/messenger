using MessageService.Domain.Cache;
using MessageService.Domain.Model;
using MessageService.Domain.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private IMessageRepository _messageRepository;
        private ICacheService _cacheService;
        private const int CACHE_EXPIRY_TIME_SEC = 60;
        private const int DEFAULT_HISTORY_PERIOD_DAYS = 7;

        public ChatsController(IMessageRepository repository, ICacheService cacheService)
        {
            _messageRepository = repository;
            _cacheService = cacheService;
        }

        [HttpGet("/chats/{userId}")]
        public async Task<IActionResult> Get(int userId, DateTime from, DateTime till)
        {
            if (from == default || till == default)
            {
                from = DateTime.UtcNow - TimeSpan.FromDays(DEFAULT_HISTORY_PERIOD_DAYS);
                till = DateTime.UtcNow;
            }

            var cacheData = _cacheService.GetData<IEnumerable<Message>>(userId.ToString());

            if (cacheData != null && cacheData.Count() > 0)
                return Ok(cacheData);

            var messages = await _messageRepository.GetMessagesAsync(userId, from, till);

            var chats = FilterByChats(messages);

            var expiryTime = DateTimeOffset.Now.AddSeconds(CACHE_EXPIRY_TIME_SEC);
            _cacheService.SetData<IEnumerable<Message>>(userId.ToString(), messages, expiryTime);

            return Ok(cacheData);
        }

        private IEnumerable<Message> FilterByChats(IEnumerable<Message> messages)
        {
            var chatsSet = new HashSet<string>();
            var resultChats = new List<Message>();

            var sortedMEssages = messages.OrderByDescending(m => m.Created);

            foreach (var message in messages)
            {
                if (chatsSet.Contains(message.ChatKey)) continue;

                chatsSet.Add(message.ChatKey);
                resultChats.Add(message);
            }

            return resultChats;
        }
    }
}
