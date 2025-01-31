using Order.Service.Endpoints;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using OpenTelemetry.Metrics;
using Order.Service.Infrastructure.Data.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServerDatastore(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqEventPublisher();

const string serviceName = "Order";

builder.Services.AddOpenTelemetryTracing(serviceName, builder.Configuration, (traceBuilder) => 
        traceBuilder.WithSqlInstrumentation())
    .AddOpenTelemetryMetrics(serviceName, builder.Services, (metricBuilder) => metricBuilder.AddView("products-per-order", new ExplicitBucketHistogramConfiguration
    {
        Boundaries = [1, 2, 5, 10]
    }));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if(app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}
app.UsePrometheusExporter();

app.RegisterEndpoints();

app.UseHttpsRedirection();


app.Run();

public partial class Program { }