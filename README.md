
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
- Metrics : Prometheus-Grafana
- Tracing : OpenTelemetry, Jaeger
- Centreralized Logging : ElasticSearch, LogStash, Kibana by SeriLog (Logtash output : Http -> RabbitMq, Logstash input : RabbitMq -> ElasticSearch)
- HealthChecks
- Ocelot
- Consul
- supported by dockercomposes