using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuoteProducer.Console.Dtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace QuoteProducer.Console;

public class ProducerService : BackgroundService
{
    private readonly ILogger<ProducerService> _logger;
    private readonly IConfiguration _config;
    private readonly string _kafkaTopic;
    private readonly string _externalApiUrl;
    private readonly int _checkInterval;

    public ProducerService(ILogger<ProducerService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        _kafkaTopic = _config.GetValue<string>("Kafka:TopicName")!;
        _externalApiUrl = _config.GetValue<string>("ExternalApi:Url")!;
        _checkInterval = _config.GetValue<int>("ExternalApi:CheckIntervalInSeconds");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Quote Producer started.");

        var producerConfig = new ProducerConfig { BootstrapServers = _config.GetValue<string>("Kafka:BootstrapServers") };
        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        using var httpClient = new HttpClient();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Fetching quotes from: {Url}", _externalApiUrl);
                var stockList = await httpClient.GetFromJsonAsync<List<StockDto>>(_externalApiUrl, stoppingToken);

                if (stockList is not null && stockList.Any())
                {
                    foreach (var stock in stockList)
                    {
                        var stockJson = JsonSerializer.Serialize(stock);

                        await producer.ProduceAsync(_kafkaTopic, new Message<Null, string> { Value = stockJson }, stoppingToken);
                        _logger.LogInformation("Quote for {Stock} published to topic {Topic}.", stock, _kafkaTopic);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching or publishing quotes.");
            }

            _logger.LogInformation("Waiting {Seconds} seconds for the next fetch.", _checkInterval);
            await Task.Delay(TimeSpan.FromSeconds(_checkInterval), stoppingToken);
        }
    }
}


