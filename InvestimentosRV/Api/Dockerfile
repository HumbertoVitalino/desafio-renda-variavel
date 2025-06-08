FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["InvestimentosRV/Api/Api.csproj", "InvestimentosRV/Api/"]
COPY ["InvestimentosRV/Core/Core.csproj", "InvestimentosRV/Core/"]
COPY ["InvestimentosRV/Infra/Infra.csproj", "InvestimentosRV/Infra/"]
COPY ["QuoteConsumer/QuoteConsumer.Worker/QuoteConsumer.Worker.csproj", "QuoteConsumer/QuoteConsumer.Worker/"]
COPY ["QuoteProducer/QuoteProducer.Console/QuoteProducer.Console.csproj", "QuoteProducer/QuoteProducer.Console/"]

RUN dotnet restore "InvestimentosRV/Api/Api.csproj"

COPY . .

WORKDIR "/src/InvestimentosRV/Api"
RUN dotnet publish "Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]