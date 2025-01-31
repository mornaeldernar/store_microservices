using Microsoft.EntityFrameworkCore;

namespace Auth.Service.Infrastructure.Data.EntityFrameworkCore;

public static class EntityFrameworkExtensions
{
    public static void AddSqlServerDatastore(this IServiceCollection services, 
         IConfigurationManager configuration)
    {
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default"),
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(40),
                    errorNumbersToAdd: [0]);
            }));

        services.AddScoped<IAuthStore, AuthContext>();
    }
}