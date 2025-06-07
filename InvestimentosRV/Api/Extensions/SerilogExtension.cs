using Serilog;

namespace Api.Extensions;

public static class SerilogExtension
{
    public static void AddSerilogApi(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());
    }
}
