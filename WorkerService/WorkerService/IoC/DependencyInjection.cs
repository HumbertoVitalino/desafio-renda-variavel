using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Infra.Data;
using Infra.Repositories;
using Serilog;
using Confluent.Kafka;

namespace WorkerService.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!)
        );

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, AppDbContext>();

        services.AddSingleton<IConsumer<Ignore, string>>(sp =>
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"] ?? "cotacao_consumer_group_default",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            var consumerBuilder = new ConsumerBuilder<Ignore, string>(consumerConfig);

            return consumerBuilder.Build();
        });

        return services;
    }

    public static IHostBuilder AddSerilogLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.File("logs/worker-log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                retainedFileCountLimit: 7,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 1024 * 1024 * 100
            )
        );
        return hostBuilder;
    }
}