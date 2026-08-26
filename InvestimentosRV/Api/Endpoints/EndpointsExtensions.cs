using Api.Endpoints.Assets;
using Api.Endpoints.Operations;
using Api.Endpoints.Positions;
using Api.Endpoints.Quotes;
using Api.Endpoints.Reports;
using Api.Endpoints.Users;

namespace Api.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapMinimalApisV1(this IEndpointRouteBuilder app)
    {
        app.MapUsersEndpoints();
        app.MapPositionsEndpoints();
        app.MapOperationsEndpoints();
        app.MapQuotesEndpoints();
        app.MapAssetsEndpoints();
        app.MapReportsEndpoints();

        return app;
    }
}
