using MessageService.Domain.Model;
using MessageService.Persistence.Model;

namespace MessageService.Persistence
{
    internal static class MappingExtensions
    {
        public static Message ToModel(this MessageDto dto)
        {
            return new()
            {
                Id = dto.Id,
                ReceiverId = dto.ReceiverId,
                SenderId = dto.SenderId,
                Text = dto.Text,
                Created = dto.Created
            };
        }
    }
}
