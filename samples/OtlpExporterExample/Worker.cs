using System.Diagnostics;

namespace OtlpExporterExample {
    public class Worker : BackgroundService {
        public const string ActivitySourceName = "OtlpExporterExample";

        private readonly ILogger<Worker> _logger;

        private readonly ActivitySource _activitySource = new ActivitySource(ActivitySourceName);


        public Worker(ILogger<Worker> logger) {
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                using var activity = _activitySource.StartActivity("WorkerActivity");

                if (_logger.IsEnabled(LogLevel.Information)) {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
