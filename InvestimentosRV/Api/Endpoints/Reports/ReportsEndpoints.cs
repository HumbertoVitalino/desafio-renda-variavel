using Application.Commons;
using Application.Interfaces.UseCases;
using Application.UseCases.GetTopClientsByPositionValueUseCase.Boundaries;
using Application.UseCases.GetTotalBrokerageRevenueUseCase.Boundaries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Reports;

public static class ReportsEndpoints
{
    public static void MapReportsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/reports").RequireAuthorization();

        group.MapGet("brokerage-revenue",
            async (
                [FromServices] IGetTotalBrokerageRevenueUseCase useCase,
                CancellationToken cancellationToken
            ) =>
            {
                var output = await useCase.Handle(new GetTotalBrokerageRevenueInput(), cancellationToken);

                if (!output.IsValid)
                    return Results.NotFound(output);

                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK);

        group.MapGet("top-10/by-position-value",
            async (
                [FromServices] IGetTopClientsByPositionValueUseCase useCase,
                CancellationToken cancellationToken
            ) =>
            {
                var output = await useCase.Handle(new GetTopClientsByPositionValueInput(), cancellationToken);

                if (!output.IsValid)
                    return Results.NotFound(output);

                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK);
    }
}
