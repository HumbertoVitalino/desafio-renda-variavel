using Infra.IoC;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfra(hostContext.Configuration);
        services.AddHostedService<QuoteConsumerService>();
    })
    .Build()
    .Run();