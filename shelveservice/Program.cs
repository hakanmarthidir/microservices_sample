using sharedmonitoring;
using sharedmonitoring.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilogExtension();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddHttpClient();
builder.Services.AddJaegerOpenTelemetryTracing("ShelveService", "0.0.1");

var app = builder.Build();


app.UseCorrelationMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();

