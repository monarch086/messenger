﻿using MessageService.Domain.Model;
using MessageService.Domain.Persistence;
using MessageService.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private IMessageRepository _messageRepository;
        private RabbitMQProducer _rabbitMQProducer;

        public MessagesController(IMessageRepository messageRepository, RabbitMQProducer rabbitMQProducer)
        {
            _messageRepository = messageRepository;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpPost]
        public ActionResult Add(Message message)
        {
            _rabbitMQProducer.Produce(message);

            return Ok();
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
