using Api.Extensions;
using Application.Commons;
using Application.Interfaces.UseCases;
using Application.UseCases.GetAllUserPositionsUseCase.Boundaries;
using Application.UseCases.GetUserAssetPositionUseCase.Boundaries;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Endpoints.Positions;

public static class PositionsEndpoints
{
    public static void MapPositionsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/positions").RequireAuthorization();

        group.MapGet("{tickerSymbol}",
            async (
                [FromRoute] string tickerSymbol,
                ClaimsPrincipal user,
                [FromServices] IGetUserAssetPositionUseCase useCase,
                CancellationToken cancellationToken
            ) =>
            {
                var input = new GetUserAssetPositionInput(user.GetUserId(), tickerSymbol);

                var output = await useCase.Handle(input, cancellationToken);

                if (!output.IsValid)
                    return Results.NotFound(output);

                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK)
        .Produces<Output>(StatusCodes.Status404NotFound);

        group.MapGet("",
            async (
                ClaimsPrincipal user,
                [FromServices] IGetAllUserPositionsUseCase useCase,
                CancellationToken cancellationToken
            ) =>
            {
                var input = new GetAllUserPositionsInput(user.GetUserId());

                var output = await useCase.Handle(input, cancellationToken);

                if (!output.IsValid)
                    return Results.NotFound(output);

                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK)
        .Produces<Output>(StatusCodes.Status404NotFound);
    }
}
