using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.RabbitMQ.Model
{
    internal class Message
    {
        public int SenderId { get; init; }

        public int ReceiverId { get; init; }

        public string Text { get; init; } = string.Empty;
    }
}
