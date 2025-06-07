FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /app
COPY QuoteProducer/QuoteProducer.Console/*.csproj .
RUN dotnet restore

FROM restore AS publish
WORKDIR /app
COPY QuoteProducer/QuoteProducer.Console/. .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuoteProducer.Console.dll"]