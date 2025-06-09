using Core.Interfaces;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<QuoteConsumerService>();
    })
    .Build()
    .Run();