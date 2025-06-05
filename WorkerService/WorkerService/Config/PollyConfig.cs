namespace WorkerService.Config;

public class PollyConfig
{
    public int RetryCount { get; set; }
    public int RetryDelaySeconds { get; set; }
}
