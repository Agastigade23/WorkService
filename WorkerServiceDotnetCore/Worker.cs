namespace WorkerServiceDotnetCore
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync("https://dp7hmx4z4f.execute-api.us-east-1.amazonaws.com/Dev/swagger/index.html");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("AWS Lambda service is running and the Status code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("AWS Lambda service is is down. Status code {StatusCode}", result.StatusCode);
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}