using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsService.HostedService
{
    public class SchedulerHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        public SchedulerHostedService(
            ILogger<SchedulerHostedService> logger
            ) {
            _logger = logger;
        }
        private long performCounter;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Interlocked.Increment(ref performCounter);
                    try
                    {
                        _logger.LogInformation(" excuted {performCounter}", performCounter);
                    }
                    catch (Exception)
                    { 
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
