
# Microservice Sample

This microservices project was created to test some basic implementation principles and tools.

### Identity Service
- CRUD base service
- consists of login, register and refrehtoken functions
- repository/service, uow patterns
- Sqlserver
- dockerfile
- efcore
- net7

### BookCatalog Service
- CQRS based service
- consists of only get and post methods regarding book, genre, author
- Sqlserver
- dockerfile
- efcore
- net7

### Review Service
- CQRS based service
- consists of base review functions
- Sqlserver
- dockerfile
- efcore
- net7


### Infrastructure
- Prometheus-Grafana
- OpenTelemetry, Jaeger Tracing
- ElasticSearch, LogStash, Kibana Centreralized Logging by SeriLog
  Logtash output > RabbitMq, Logstash input > ElasticSearch, http enrichments
- HealthChecks
- Ocelot
- Consul