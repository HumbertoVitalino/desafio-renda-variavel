using Application.Commons;
using Application.Interfaces.UseCases;
using Application.UseCases.GetAllAssetsUseCase.Boundaries;
using Application.UseCases.GetAssetTickersUseCase.Boundaries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Assets;

public static class AssetsEndpoints
{
    public static void MapAssetsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/assets");

        group.MapGet("",
            async (
                [FromServices] IGetAllAssetsUseCase useCase,
                CancellationToken cancellationToken
            ) =>
            {
                var output = await useCase.Handle(new GetAllAssetsInput(), cancellationToken);
                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK);

        group.MapGet("tickers",
            async (
                [FromServices] IGetAssetTickersUseCase useCase,
                CancellationToken cancellationToken
            ) =>
            {
                var output = await useCase.Handle(new GetAssetTickersInput(), cancellationToken);
                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK);
    }
}
