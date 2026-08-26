using Api.Endpoints;
using Api.Extensions;
using Api.Handlers;
using Application.IoC;
using Infra.IoC;
using Serilog;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.AddSerilogApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddJwt(configuration);
builder.Services.AddApplication();
builder.Services.AddInfra(configuration);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseCors(MyAllowSpecificOrigins);

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapMinimalApisV1();

app.Run();
