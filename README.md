
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
- Communication with BookCatalogService via REST and gRPC sample


### Infrastructure
- Docker
- Metrics : Prometheus-Grafana
- Tracing : OpenTelemetry, Jaeger
- Centralized Logging : ElasticSearch, LogStash, Kibana by SeriLog (Logtash output : Http, Logstash input : ElasticSearch)
- Centralized Configuration-Secret Management : Hashicorp Vault
- HealthChecks
- Ocelot
- Consul
- supported by dockercomposes
- sharedkernel, sharedmonitoring, sharedconfiguration or sharedsecurity might be nuget packages and you can publish them by using private registry. For instance : ProGet