namespace MessageService.Domain.Model
{
    public class Message
    {
        public Guid Id { get; init; }

        public int SenderId { get; init; }

        public int ReceiverId { get; init; }

        public string Text { get; init; }

        public Message(int senderId, int receiverId, string text, Guid id = default)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = receiverId;
            Text = text;
        }
    }
}
