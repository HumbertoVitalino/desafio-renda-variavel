using Application.Interfaces.UseCases;
using Application.UseCases.GetAllAssetsUseCase;
using Application.UseCases.GetAllUserPositionsUseCase;
using Application.UseCases.GetAssetTickersUseCase;
using Application.UseCases.GetLatestQuoteUseCase;
using Application.UseCases.GetTopClientsByPositionValueUseCase;
using Application.UseCases.GetTotalBrokerageRevenueUseCase;
using Application.UseCases.GetUserAssetPositionUseCase;
using Application.UseCases.LoginUserUseCase;
using Application.UseCases.LoginUserUseCase.Boundaries;
using Application.UseCases.NewOperationUseCase;
using Application.UseCases.NewOperationUseCase.Boundaries;
using Application.UseCases.NewUserUseCase;
using Application.UseCases.NewUserUseCase.Boundaries;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INewOperationUseCase, NewOperationUseCase>();
        services.AddScoped<INewUserUseCase, NewUserUseCase>();
        services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
        services.AddScoped<IGetAllUserPositionsUseCase, GetAllUserPositionsUseCase>();
        services.AddScoped<IGetUserAssetPositionUseCase, GetUserAssetPositionUseCase>();
        services.AddScoped<IGetLatestQuoteUseCase, GetLatestQuoteUseCase>();
        services.AddScoped<IGetTopClientsByPositionValueUseCase, GetTopClientsByPositionValueUseCase>();
        services.AddScoped<IGetAllAssetsUseCase, GetAllAssetsUseCase>();
        services.AddScoped<IGetAssetTickersUseCase, GetAssetTickersUseCase>();
        services.AddScoped<IGetTotalBrokerageRevenueUseCase, GetTotalBrokerageRevenueUseCase>();

        services.AddScoped<IValidator<NewOperationInput>, NewOperationInputValidator>();
        services.AddScoped<IValidator<NewUserInput>, NewUserInputValidator>();
        services.AddScoped<IValidator<LoginUserInput>, LoginUserInputValidator>();

        return services;
    }
}
