using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();






var serviceName = "ShelveService";
var serviceVersion = "0.0.1";
builder.Services.AddHttpClient();

builder.Services.AddOpenTelemetryTracing(b =>
{
    b
     .AddAspNetCoreInstrumentation(options =>
     {
         options.RecordException = true;
     })
    .AddHttpClientInstrumentation()
    .AddSqlClientInstrumentation()
    .AddEntityFrameworkCoreInstrumentation()
    //.AddConsoleExporter()
    .AddJaegerExporter(options =>
    {
        var agentHost = "jaeger";
        var agentPort = 6831;
        options.AgentHost = agentHost;
        options.AgentPort = agentPort;

    })
    .AddSource(serviceName)
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion));
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

