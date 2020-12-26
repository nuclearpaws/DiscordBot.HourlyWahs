using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot.HourlyWahs.Worker.Workers
{
    public class ImageCacheWorker : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ImageCacheWorker(IMediator mediator, ILogger<ImageCacheWorker> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker '{worker}' running at: {time}", nameof(ImageCacheWorker), DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}