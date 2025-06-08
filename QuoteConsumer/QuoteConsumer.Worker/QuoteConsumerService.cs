using Confluent.Kafka;
using Core.Domain;
using Core.Interfaces;
using QuoteConsumer.Worker.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

public class QuoteConsumerService : BackgroundService
{
    private readonly ILogger<QuoteConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConsumerConfig _consumerConfig;
    private readonly string _topic;

    public QuoteConsumerService(ILogger<QuoteConsumerService> logger, IServiceScopeFactory scopeFactory, IConfiguration config)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = config.GetValue<string>("Kafka:BootstrapServers"),
            GroupId = config.GetValue<string>("Kafka:GroupId"),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _topic = config.GetValue<string>("Kafka:TopicName")!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Quote consumer started. Waiting for messages...");

        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var quoteDto = JsonSerializer.Deserialize<QuoteMessageDto>(consumeResult.Message.Value);

                if (quoteDto is null)
                {
                    _logger.LogWarning("Message received from Kafka is empty or in an invalid format.");
                    continue;
                }

                using var scope = _scopeFactory.CreateScope();
                var assetRepository = scope.ServiceProvider.GetRequiredService<IAssetRepository>();
                var quoteRepository = scope.ServiceProvider.GetRequiredService<IQuoteRepository>();
                var positionRepository = scope.ServiceProvider.GetRequiredService<IPositionRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var asset = await assetRepository.GetByTickerAsync(quoteDto.Ticker, stoppingToken);
                if (asset is null)
                {
                    _logger.LogWarning("Asset with ticker {Ticker} received from Kafka was not found in the database.", quoteDto.Ticker);
                    continue;
                }
                var existingQuote = await quoteRepository.GetByAssetIdAndTradeTimeAsync(asset.Id, quoteDto.TradeTime, stoppingToken);
                if (existingQuote is not null)
                {
                    _logger.LogWarning("Duplicate quote for {Ticker} at {TradeTime} received. Skipping.", asset.TickerSymbol, quoteDto.TradeTime);
                    continue;
                }
                var newQuote = new Quote(asset.Id, quoteDto.Price, quoteDto.TradeTime);
                await quoteRepository.CreateAsync(newQuote, stoppingToken);

                var positionsToUpdate = await positionRepository.GetAllByAssetIdAsync(asset.Id, stoppingToken);
                foreach (var position in positionsToUpdate)
                {
                    position.UpdateProfitAndLossWithNewQuote(newQuote.UnitPrice);
                }

                await unitOfWork.CommitAsync(stoppingToken);

                _logger.LogInformation("Quote for {Ticker} saved and {Count} positions updated.", asset.TickerSymbol, positionsToUpdate.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming or processing message from Kafka.");
            }
        }

        consumer.Close();
    }
}

