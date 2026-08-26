using Api.Extensions;
using Api.Mappers;
using Api.Requests;
using Application.Commons;
using Application.Interfaces.UseCases;
using Application.UseCases.NewOperationUseCase.Boundaries;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Endpoints.Operations;

public static class OperationsEndpoints
{
    public static void MapOperationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/operations").RequireAuthorization();

        group.MapPost("",
            async (
                [FromBody] NewOperationRequest request,
                ClaimsPrincipal user,
                [FromServices] INewOperationUseCase useCase,
                [FromServices] IValidator<NewOperationInput> validator,
                CancellationToken cancellationToken
            ) =>
            {
                var input = request.MapToInput(user.GetUserId());

                var validationResult = await validator.ValidateAsync(input, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new Output(validationResult));

                var output = await useCase.Handle(input, cancellationToken);

                if (!output.IsValid)
                    return Results.BadRequest(output);

                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK)
        .Produces<Output>(StatusCodes.Status400BadRequest);
    }
}
