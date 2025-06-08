using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoteProducer.Console;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<ProducerService>();
    })
    .Build()
    .Run();