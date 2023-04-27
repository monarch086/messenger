﻿using Cassandra;
using Cassandra.Mapping;
using MessageService.Domain.Model;
using MessageService.Domain.Persistence;
using MessageService.Persistence.Model;

namespace MessageService.Persistence
{
    public class MessageRepository : IMessageRepository
    {
        private const string HOST = "127.0.0.1";
        private const string KEY_SPACE = "CassandraDbContext";
        private const string TABLE_NAME = "messages";

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
                query = @$"SELECT *
                           FROM {KEY_SPACE}.{TABLE_NAME}
                           LIMIT 1;";

                var savedMessage = await mapper.SingleAsync<MessageDto>(query);

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
                               ALLOW FILTERING;";

                var records = await mapper.FetchAsync<MessageDto>(query, lastMessageId, userId);

                resultMessages.AddRange(records);

                query = @$"SELECT id, sender_id, receiver_id, text, created
                               FROM {KEY_SPACE}.{TABLE_NAME}
                               WHERE id > ?
                                 AND receiver_id = ?
                               ALLOW FILTERING;";

                records = await mapper.FetchAsync<MessageDto>(query, lastMessageId, userId);

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

        private ISession Connect()
        {
            var cluster = Cluster.Builder()
                     .AddContactPoint(HOST)
                     .WithLoadBalancingPolicy(new TokenAwarePolicy(new DCAwareRoundRobinPolicy("datacenter1")))
                     .Build();

            return cluster.Connect(KEY_SPACE);
        }
    }
}
