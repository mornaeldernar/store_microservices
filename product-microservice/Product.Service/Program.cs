using Product.Service.Infrastructure.Data.EntityFramework;
using Product.Service.Endpoints;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using ECommerce.Shared.Infrastructure.Outbox;
using ECommerce.Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServerDatastore(builder.Configuration);

builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqEventPublisher();

builder.Services.AddOpenTelemetryTracing("Product", builder.Configuration, 
    (traceBuilder) => traceBuilder.WithSqlInstrumentation());

builder.Services.AddOutbox(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
    app.ApplyOutboxMigratios();
}

app.RegisterEndpoints();

app.UseHttpsRedirection();

app.UseJwtAuthentication();

app.Run();

public partial class Program { }