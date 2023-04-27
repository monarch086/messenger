using Cassandra;
using Cassandra.Mapping.Attributes;

namespace MessageService.Persistence.Model
{
    internal class MessageDto
    {
        public TimeUuid Id { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }
    }
}
