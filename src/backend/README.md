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

For starting all backend services run: `docker-compose -p "messenger-backend" up`

For graceful removal of services run: `docker-compose -p "messenger-backend" down`
