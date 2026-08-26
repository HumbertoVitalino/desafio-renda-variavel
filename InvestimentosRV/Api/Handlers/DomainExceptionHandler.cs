using Application.Commons;
using Domain.Abstractions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Handlers;

public sealed class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainException)
            return false;

        Output output = new();
        output.AddErrorMessage(domainException.Message);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(output, cancellationToken);

        return true;
    }
}
