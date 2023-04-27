# Messenger Backend

## Services List

- haproxy: load-balancer for distributing requests between different instances of a service;
- api-gateway: gateway for reaching out to other services
- user-service: service for User resource
- message-service: service for Message resource

## Build

### api-gateway

To build API Gateway image use command: `docker build -t api-gateway .`

### user-service

### message-service

To build MessageService image use command: `docker build -t message-service .`

### message-broker

## Run

For starting all backend services run: `docker-compose -p "messenger-backend" up -d`

For graceful removal of services run: `docker-compose -p "messenger-backend" down`

## Cassandra emergency init

````SQL
CREATE KEYSPACE IF NOT EXISTS CassandraDbContext WITH replication = {'class': 'NetworkTopologyStrategy', 'datacenter1': '1'}  AND durable_writes = true;

USE CassandraDbContext;

CREATE TABLE IF NOT EXISTS messages (
    id timeuuid,
    sender_id int,
    receiver_id int,
    text text,
    created timestamp,
    PRIMARY KEY (id, created)
)
WITH CLUSTERING ORDER BY (created DESC);

CREATE INDEX sender_idx ON CassandraDbContext.messages (sender_id);

CREATE INDEX receiver_idx ON CassandraDbContext.messages (receiver_id);
````
