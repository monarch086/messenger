namespace MessageService.Domain.Model
{
    public class Message
    {
        public Guid Id { get; init; } = default;

        public int SenderId { get; init; }

        public int ReceiverId { get; init; }

        public string Text { get; init; } = string.Empty;

        public DateTime Created { get; init; }

        public Message(Guid id, int senderId, int receiverId, string text, DateTime created)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = receiverId;
            Text = text;
            Created = created;
        }

        public Message() { }
    }
}
