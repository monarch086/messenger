using Cassandra;
using Cassandra.Mapping;
using MessageService.Domain.Model;
using MessageService.Domain.Persistence;
using MessageService.Persistence.Model;

namespace MessageService.Persistence
{
    public class MessageRepository : IMessageRepository
    {
        private const string KEY_SPACE = "CassandraDbContext";
        private const string TABLE_NAME = "messages";

        private readonly string[] _hosts;

        public MessageRepository(string[] hosts)
        {
            _hosts = hosts;
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            using (var session = Connect())
            {
                session.ChangeKeyspace(KEY_SPACE);

                var query = @$"INSERT INTO {KEY_SPACE}.{TABLE_NAME} (id, sender_id, receiver_id, text, created)
                               VALUES (now(), ?, ?, ?, toTimestamp(now()));";

                var ps = session.Prepare(query);
                var statement = ps.Bind(message.SenderId, message.ReceiverId, message.Text);

                await session.ExecuteAsync(statement);

                var mapper = new Mapper(session);
                query = @$"SELECT id, MAX(created) as created
                           FROM {KEY_SPACE}.{TABLE_NAME};";

                var savedMessage = await mapper.SingleAsync<MessageDto>(query);
                savedMessage.SenderId = message.SenderId;
                savedMessage.ReceiverId = message.ReceiverId;
                savedMessage.Text = message.Text;

                return savedMessage.ToModel();
            }
        }

        public async Task<IEnumerable<Message>> GetLatestMessagesAsync(int userId, Guid lastMessageId, int? friendId = null)
        {
            using (var session = Connect())
            {
                var resultMessages = new List<MessageDto>();
                var mapper = new Mapper(session);
                var query = @$"SELECT id, sender_id, receiver_id, text, created
                               FROM {KEY_SPACE}.{TABLE_NAME}
                               WHERE id > ?
                                 AND sender_id = ?
                                 {(friendId.HasValue? "AND receiver_id = ?" : "")}
                               ALLOW FILTERING;";

                var records = friendId.HasValue
                              ? await mapper.FetchAsync<MessageDto>(query, lastMessageId, userId, friendId)
                              : await mapper.FetchAsync<MessageDto>(query, lastMessageId, userId);

                resultMessages.AddRange(records);

                query = @$"SELECT id, sender_id, receiver_id, text, created
                               FROM {KEY_SPACE}.{TABLE_NAME}
                               WHERE id > ?
                                 AND receiver_id = ?
                                 {(friendId.HasValue ? "AND sender_id = ?" : "")}
                               ALLOW FILTERING;";

                records = friendId.HasValue
                              ? await mapper.FetchAsync<MessageDto>(query, lastMessageId, userId, friendId)
                              : await mapper.FetchAsync<MessageDto>(query, lastMessageId, userId);

                resultMessages.AddRange(records);

                return resultMessages.Select(r => r.ToModel());
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int userId, int friendId, DateTime from, DateTime till)
        {
            using (var session = Connect())
            {
                var resultMessages = new List<MessageDto>();
                var mapper = new Mapper(session);
                var query = @$"SELECT id, sender_id, receiver_id, text, created
                               FROM {KEY_SPACE}.{TABLE_NAME}
                               WHERE id > maxTimeuuid(?) AND id < minTimeuuid(?)
                                 AND sender_id = ? AND receiver_id = ?
                               ALLOW FILTERING;";

                var records = await mapper.FetchAsync<MessageDto>(query, from, till, userId, friendId);
                resultMessages.AddRange(records);

                records = await mapper.FetchAsync<MessageDto>(query, from, till, friendId, userId);
                resultMessages.AddRange(records);

                return resultMessages.Select(r => r.ToModel());
            }
        }

        public void CreateTable()
        {
            using (var session = Connect())
            {
                var query = $"CREATE KEYSPACE IF NOT EXISTS {KEY_SPACE} " +
                            $"WITH replication = {{'class': 'NetworkTopologyStrategy', 'datacenter1': '3'}} " +
                            $"AND durable_writes = true;";
                session.Execute(query);

                query = @$"
                CREATE TABLE IF NOT EXISTS {KEY_SPACE}.{TABLE_NAME} (
                    id timeuuid,
                    sender_id int,
                    receiver_id int,
                    text text,
                    created timestamp,
                    PRIMARY KEY (id, created)
                )
                WITH CLUSTERING ORDER BY (created DESC);";
                session.Execute(query);

                query = $"CREATE INDEX IF NOT EXISTS sender_idx " +
                        $"ON {KEY_SPACE}.{TABLE_NAME} (sender_id);";
                session.Execute(query);

                query = $"CREATE INDEX IF NOT EXISTS receiver_idx " +
                        $"ON {KEY_SPACE}.{TABLE_NAME} (receiver_id);";
                session.Execute(query);
            }
        }

        public void DropTable()
        {
            using (var session = Connect())
            {
                var query = @$"DROP TABLE IF EXISTS {KEY_SPACE}.{TABLE_NAME};";
                session.Execute(query);
            }
        }

        private ISession Connect()
        {
            var builder = Cluster.Builder()
                        .WithLoadBalancingPolicy(new TokenAwarePolicy(new DCAwareRoundRobinPolicy("datacenter1")));

            if (_hosts.Length > 1)
                builder.AddContactPoints(_hosts);
            else
                builder.AddContactPoint(_hosts.Single());

            var cluster = builder.Build();

            return cluster.Connect(KEY_SPACE);
        }
    }
}
