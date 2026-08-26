using Api.Mappers;
using Api.Requests;
using Application.Commons;
using Application.Interfaces.UseCases;
using Application.UseCases.LoginUserUseCase.Boundaries;
using Application.UseCases.NewUserUseCase.Boundaries;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/user");

        group.MapPost("register",
            async (
                [FromBody] NewUserRequest request,
                [FromServices] INewUserUseCase useCase,
                [FromServices] IValidator<NewUserInput> validator,
                CancellationToken cancellationToken
            ) =>
            {
                var input = request.MapToInput();

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

        group.MapPost("login",
            async (
                [FromBody] LoginUserRequest request,
                [FromServices] ILoginUserUseCase useCase,
                [FromServices] IValidator<LoginUserInput> validator,
                CancellationToken cancellationToken
            ) =>
            {
                var input = request.MapToInput();

                var validationResult = await validator.ValidateAsync(input, cancellationToken);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new Output(validationResult));

                var output = await useCase.Handle(input, cancellationToken);

                if (!output.IsValid)
                    return Results.Json(output, statusCode: StatusCodes.Status401Unauthorized);

                return Results.Ok(output);
            }
        )
        .Produces<Output>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
