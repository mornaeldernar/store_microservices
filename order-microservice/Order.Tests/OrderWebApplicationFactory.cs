using Microsoft.AspNetCore.Mvc.Testing;
using Order.Service;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Order.Service.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Order.Tests
{
    public class OrderWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private OrderContext? _orderContext;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }

        private void ApplyMigrations(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            _orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
            _orderContext.Database.Migrate();
        }
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Tests.json")
                    .Build());
            });


            return base.CreateHost(builder);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public new Task DisposeAsync()
        {
            if(_orderContext is not null)
            {
                return _orderContext.Database.EnsureCreatedAsync();
            }
            return Task.CompletedTask;
        }
    }
}
