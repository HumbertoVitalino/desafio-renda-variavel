using WorkerService.IoC;
using WorkerService;

var builder = Host.CreateDefaultBuilder(args);

builder.AddSerilogLogging();

builder.ConfigureServices((context, services) =>
{
    services.AddInfrastructure(context.Configuration);
    services.AddHostedService<Worker>();
});

var host = builder.Build();
host.Run();

