using Application.Commons;
using Application.Interfaces.UseCases;
using Application.UseCases.GetLatestQuoteUseCase.Boundaries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Quotes;

public static class QuotesEndpoints
{
    public static void MapQuotesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/quotes").RequireAuthorization();

        group.MapGet("{tickerSymbol}/latest",
            async (
                [FromRoute] string tickerSymbol,
                [FromServices] IGetLatestQuoteUseCase useCase,
                CancellationToken cancellationToken
            ) =>
            {
                var input = new GetLatestQuoteInput(tickerSymbol);

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
