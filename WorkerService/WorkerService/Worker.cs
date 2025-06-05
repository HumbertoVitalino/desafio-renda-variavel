using Confluent.Kafka;
using Core.Entities;
using Core.Interfaces;
using Polly;
using Polly.Retry;
using System.Text.Json;
using WorkerService.DTOs;
using WorkerService.Config;

namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IConsumer<Ignore, string> _kafkaConsumer;
    private readonly KafkaConsumerConfig _kafkaConfig;
    private readonly PollyConfig _pollyConfig;
    private readonly AsyncRetryPolicy _retryPolicy;

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        IConsumer<Ignore, string> kafkaConsumer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _kafkaConsumer = kafkaConsumer ?? throw new ArgumentNullException(nameof(kafkaConsumer));

        _kafkaConfig = _configuration.GetSection("Kafka").Get<KafkaConsumerConfig>()
                           ?? throw new InvalidOperationException("Configura��o do Kafka n�o encontrada ou inv�lida.");
        _pollyConfig = _configuration.GetSection("Polly").Get<PollyConfig>()
                           ?? new PollyConfig { RetryCount = 3, RetryDelaySeconds = 5 };

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                _pollyConfig.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(_pollyConfig.RetryDelaySeconds, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Falha ao processar mensagem. Retentativa {RetryCount} em {TimeSpan}s. Contexto: {Context}", retryCount, timeSpan.TotalSeconds, context.OperationKey);
                }
            );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker de Cota��es iniciado �s: {time}", DateTimeOffset.Now);

        try
        {
            _kafkaConsumer.Subscribe(_kafkaConfig.Topic);
            _logger.LogInformation("Inscrito no t�pico Kafka: {Topic} com GroupId: {GroupId}", _kafkaConfig.Topic, _kafkaConfig.GroupId);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Falha ao se inscrever no t�pico Kafka {Topic}. O Worker ser� encerrado.", _kafkaConfig.Topic);
            return;
        }

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessSingleMessageWithRetry(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Opera��o cancelada. Worker de Cota��es est� parando.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Erro cr�tico no loop principal do Worker. O Worker ser� parado.");
        }
        finally
        {
            _logger.LogInformation("Fechando consumidor Kafka.");
            _kafkaConsumer.Close();
            _logger.LogInformation("Worker de Cota��es parado �s: {time}", DateTimeOffset.Now);
        }
    }

    private async Task ProcessSingleMessageWithRetry(CancellationToken stoppingToken)
    {
        string? rawMessage = null;
        string messageKeyForLog = "N/A";
        ConsumeResult<Ignore, string>? consumeResult = null;

        try
        {
            consumeResult = _kafkaConsumer.Consume(TimeSpan.FromSeconds(1));

            if (consumeResult == null)
            {
                return;
            }

            if (consumeResult.IsPartitionEOF)
            {
                _logger.LogTrace("Alcan�ou o fim da parti��o: {TopicPartitionOffset}, esperando por novas mensagens.", consumeResult.TopicPartitionOffset);
                return;
            }

            rawMessage = consumeResult.Message.Value;
            messageKeyForLog = consumeResult.Message.Key?.ToString() ?? consumeResult.TopicPartitionOffset.ToString();
            _logger.LogDebug("Mensagem recebida do Kafka ({MessageKeyForLog}): {RawMessage}", messageKeyForLog, rawMessage);

            await _retryPolicy.ExecuteAsync(async (ctx, ct) =>
            {
                await ProcessMessageLogic(rawMessage, messageKeyForLog, ct);
            }, new Context(messageKeyForLog), stoppingToken);

            _kafkaConsumer.Commit(consumeResult);
            _logger.LogInformation("Mensagem ({MessageKeyForLog}) processada e offset commitado.", messageKeyForLog);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Processamento da mensagem ({MessageKeyForLog}) cancelado.", messageKeyForLog);
            throw;
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Erro de deserializa��o para a mensagem ({MessageKeyForLog}): {RawMessage}. A mensagem ser� descartada.", messageKeyForLog, rawMessage);
            if (consumeResult != null) _kafkaConsumer.Commit(consumeResult);
        }
        catch (ConsumeException consumeEx)
        {
            _logger.LogError(consumeEx, "Erro no consumidor Kafka ao tentar ler mensagem ({MessageKeyForLog}).", messageKeyForLog);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao processar mensagem ({MessageKeyForLog}) ap�s todas as retentativas: {RawMessage}. A mensagem ser� descartada.", messageKeyForLog, rawMessage);
            if (consumeResult != null) _kafkaConsumer.Commit(consumeResult);
        }
    }

    private async Task ProcessMessageLogic(string? messageValue, string messageKeyForLog, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(messageValue))
        {
            _logger.LogWarning("Mensagem Kafka ({MessageKeyForLog}) est� vazia ou nula. Ignorando.", messageKeyForLog);
            return;
        }

        CotacaoMessageDto? cotacaoDto;
        try
        {
            cotacaoDto = JsonSerializer.Deserialize<CotacaoMessageDto>(messageValue);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Falha ao deserializar mensagem JSON ({MessageKeyForLog}): {MessageValue}", messageKeyForLog, messageValue);
            throw;
        }

        if (cotacaoDto == null || string.IsNullOrWhiteSpace(cotacaoDto.Ticker) || cotacaoDto.PrecoUnitario <= 0)
        {
            _logger.LogWarning("Mensagem de cota��o inv�lida ou incompleta ({MessageKeyForLog}): {@CotacaoDto}. Ignorando.", messageKeyForLog, cotacaoDto);
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var ativoRepository = scope.ServiceProvider.GetRequiredService<IRepository<Ativo>>();
        var cotacaoRepository = scope.ServiceProvider.GetRequiredService<IRepository<Cotacao>>();
        var posicaoRepository = scope.ServiceProvider.GetRequiredService<IRepository<Posicao>>();

        _logger.LogInformation("Processando cota��o para Ticker: {Ticker}, Pre�o: {Preco}, Data: {DataHora} (MsgKey: {MessageKeyForLog})",
            cotacaoDto.Ticker, cotacaoDto.PrecoUnitario, cotacaoDto.DataHora, messageKeyForLog);

        var ativosEncontrados = await ativoRepository.FindAsync(a => a.Codigo == cotacaoDto.Ticker, cancellationToken);
        var ativo = ativosEncontrados.FirstOrDefault();

        if (ativo == null)
        {
            _logger.LogInformation("Ativo com c�digo {Ticker} n�o encontrado. Criando novo ativo. (MsgKey: {MessageKeyForLog})", cotacaoDto.Ticker, messageKeyForLog);
            ativo = new Ativo(cotacaoDto.Ticker, $"Empresa {cotacaoDto.Ticker}");
            await ativoRepository.AddAsync(ativo, cancellationToken);
        }

        var cotacoesExistentes = await cotacaoRepository.FindAsync(
            c => c.FkIdAtivo == ativo.IdAtivo && c.DataHora == cotacaoDto.DataHora,
            cancellationToken);
        var cotacaoExistente = cotacoesExistentes.FirstOrDefault();

        if (cotacaoExistente != null)
        {
            _logger.LogInformation("Cota��o para {Ticker} em {DataHora} j� existe (ID: {CotacaoId}). Pulando. (MsgKey: {MessageKeyForLog})",
                cotacaoDto.Ticker, cotacaoDto.DataHora, cotacaoExistente.IdCotacao, messageKeyForLog);
            return;
        }

        var novaCotacao = new Cotacao(ativo.IdAtivo, cotacaoDto.PrecoUnitario, cotacaoDto.DataHora);
        await cotacaoRepository.AddAsync(novaCotacao, cancellationToken);
        _logger.LogInformation("Nova cota��o para {Ticker} (ID Ativo: {IdAtivo}, Pre�o: {Preco}, Data: {DataHora}) adicionada para salvar. (MsgKey: {MessageKeyForLog})",
            cotacaoDto.Ticker, ativo.IdAtivo, novaCotacao.PrecoUnitario, novaCotacao.DataHora, messageKeyForLog);

        var posicoesDoAtivo = await posicaoRepository.FindAsync(p => p.FkIdAtivo == ativo.IdAtivo, cancellationToken);
        if (posicoesDoAtivo.Any())
        {
            foreach (var posicao in posicoesDoAtivo)
            {
                _logger.LogDebug("Atualizando P&L para Posi��o ID {IdPosicao} (Usu�rio: {FkIdUsuario}, Ativo: {FkIdAtivo}). (MsgKey: {MessageKeyForLog})",
                    posicao.IdPosicao, posicao.FkIdUsuario, posicao.FkIdAtivo, messageKeyForLog);

                posicao.AtualizarPLComNovaCotacao(novaCotacao.PrecoUnitario);

                posicaoRepository.UpdateAsync(posicao);
                _logger.LogInformation("Posi��o ID {IdPosicao} para ativo {Ticker} atualizada. Novo P&L: {PL}. (MsgKey: {MessageKeyForLog})",
                    posicao.IdPosicao, ativo.Codigo, posicao.PL, messageKeyForLog);
            }
        }
        else
        {
            _logger.LogInformation("Nenhuma posi��o encontrada para o ativo {Ticker} (ID Ativo: {IdAtivo}) para atualizar P&L. (MsgKey: {MessageKeyForLog})",
                ativo.Codigo, ativo.IdAtivo, messageKeyForLog);
        }

        var sucessoCommit = await unitOfWork.CommitAsync(cancellationToken);
        if (sucessoCommit)
        {
            _logger.LogInformation("Altera��es salvas no banco de dados para cota��o de {Ticker} em {DataHora}. (MsgKey: {MessageKeyForLog})",
                cotacaoDto.Ticker, cotacaoDto.DataHora, messageKeyForLog);
        }
        else
        {
            _logger.LogError("Falha ao commitar altera��es no banco de dados para cota��o de {Ticker} em {DataHora}. (MsgKey: {MessageKeyForLog})",
                cotacaoDto.Ticker, cotacaoDto.DataHora, messageKeyForLog);
            throw new InvalidOperationException($"Falha ao commitar transa��o no UnitOfWork para a mensagem {messageKeyForLog}.");
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker de Cota��es recebendo sinal para parar.");
        await base.StopAsync(stoppingToken);
        _logger.LogInformation("Worker de Cota��es finalizou a parada.");
    }
}