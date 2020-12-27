using System;
using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Interfaces;
using DiscordBot.HourlyWahs.Core.UseCases;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot.HourlyWahs.Worker.Workers
{
    public class DiscordWorker : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger _logger;

        public DiscordWorker(IMediator mediator, IDateTimeService dateTimeService, ILogger<DiscordWorker> logger)
        {
            _mediator = mediator;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _mediator.Send(new StartBot.Request());
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _mediator.Send(new StopBot.Request());
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = _dateTimeService.GetCurrentDateTime();
                if(now.Minute == 0)
                    await _mediator.Send(new PostRandomWahImageToAllServers.Request(), cancellationToken);

                _logger.LogInformation("Worker '{worker}' running at: {time}", nameof(DiscordWorker), DateTimeOffset.Now);
                await Task.Delay(30 * 1000, cancellationToken);
            }
        }
    }
}
