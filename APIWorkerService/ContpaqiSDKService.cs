namespace APIWorkerService
{
    public class ContpaqiSDKService : BackgroundService
    {
        private readonly ILogger<ContpaqiSDKService> _logger;

        public ContpaqiSDKService(ILogger<ContpaqiSDKService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
