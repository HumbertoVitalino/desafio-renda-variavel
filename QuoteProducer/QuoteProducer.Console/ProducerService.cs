using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuoteProducer.Console.Dtos;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace QuoteProducer.Console;

public class ProducerService : BackgroundService
{
    private readonly ILogger<ProducerService> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _kafkaTopic;
    private readonly int _checkInterval;

    public ProducerService(
        ILogger<ProducerService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = config;
        _httpClientFactory = httpClientFactory;
        _kafkaTopic = _config.GetValue<string>("Kafka:TopicName")!;
        _checkInterval = _config.GetValue<int>("ExternalApi:CheckIntervalInSeconds");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Quote Producer started.");

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _config.GetValue<string>("Kafka:BootstrapServers")
        };

        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        var httpClient = _httpClientFactory.CreateClient("QuotesClient");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Fetching quotes from external API...");

                var response = await httpClient.GetAsync("", stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    var stockList = await response.Content.ReadFromJsonAsync<List<StockDto>>(cancellationToken: stoppingToken);

                    if (stockList is not null && stockList.Any())
                    {
                        foreach (var stock in stockList)
                        {
                            var stockJson = JsonSerializer.Serialize(stock);
                            await producer.ProduceAsync(_kafkaTopic, new Message<Null, string> { Value = stockJson }, stoppingToken);
                            _logger.LogInformation("Quote for {Ticker} published.", stock.Ticker);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Empty or null stock list received.");
                        await PublishFallbackQuote(producer, stoppingToken);
                    }
                }
                else
                {
                    _logger.LogWarning("API returned unsuccessful status code {StatusCode}. Using fallback.", response.StatusCode);
                    await PublishFallbackQuote(producer, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching or publishing quotes. Using fallback.");
                await PublishFallbackQuote(producer, stoppingToken);
            }

            _logger.LogInformation("Waiting {Seconds} seconds for the next fetch.", _checkInterval);
            await Task.Delay(TimeSpan.FromSeconds(_checkInterval), stoppingToken);
        }
    }

    private async Task PublishFallbackQuote(IProducer<Null, string> producer, CancellationToken stoppingToken)
    {
        var fallbackStock = new StockDto
        {
            Ticker = "FALLBACK",
            Price = 0,
            Name = "Fallback Asset",
            Change = 0,
            YesterdayClosePrice = 0,
            Volume = 0,
            MarketCap = null,
            TradeTime = DateTime.UtcNow
        };

        var stockJson = JsonSerializer.Serialize(fallbackStock);
        await producer.ProduceAsync(_kafkaTopic, new Message<Null, string> { Value = stockJson }, stoppingToken);

        _logger.LogInformation("Fallback quote published.");
    }
}
