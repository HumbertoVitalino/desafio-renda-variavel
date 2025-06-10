using Core.Behaviors;
using Core.UseCase.NewUserUseCase;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Core.IoC;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddMediatr(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
        cfg.RegisterServicesFromAssembly(typeof(NewUser).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(NewUser).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }   
}
