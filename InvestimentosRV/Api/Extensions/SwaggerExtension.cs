using Core.Boundaries.Jwt;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "InvestimentosRV", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the field below.\n\nExample: \"Bearer 12345abcdef\""
            });

            c.OperationFilter<JwtAuthorization>();
        });
    }
}
