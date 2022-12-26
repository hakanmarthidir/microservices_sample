using sharedmonitoring;
using sharedmonitoring.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddHttpClient();
builder.Services.AddJaegerOpenTelemetryTracing("ShelveService", "0.0.1");

var app = builder.Build();


app.UseCorrelationMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();

