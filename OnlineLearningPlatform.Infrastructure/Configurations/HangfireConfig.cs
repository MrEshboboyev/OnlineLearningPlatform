using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineLearningPlatform.Infrastructure.Configurations;

public static class HangfireConfig
{
    [Obsolete]
    public static void AddHangfireServices(this IServiceCollection services, string connectionString)
    {
        // Configure Hangfire to use PostgreSQL storage with the updated configuration
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
                  {
                      InvisibilityTimeout = TimeSpan.FromMinutes(5),  // for locks
                      QueuePollInterval = TimeSpan.FromSeconds(15)    // job polling interval
                  });
        });

        // Add the Hangfire server to the service collection
        services.AddHangfireServer();
    }
}

