using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Entities;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    internal class AddTargetChannelForServer : IRequestHandler<AddTargetChannelForServer.Request, AddTargetChannelForServer.Response>
    {
        public class Request : IRequest<Response>
        {
            public string ServerId { get; set; }
            public string ChannelId { get; set; }
        }

        public class Response
        {
        }

        private readonly IMasterDataService _masterDataService;
        private readonly IDateTimeService _dateTimeService;

        public AddTargetChannelForServer(IMasterDataService masterDataService, IDateTimeService dateTimeService)
        {
            _masterDataService = masterDataService;
            _dateTimeService = dateTimeService;
        }

        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var discordServer = _masterDataService.GetDiscordServer(request.ServerId);
            if(discordServer == null)
                discordServer = new DiscordServer(request.ServerId, _dateTimeService.GetCurrentDateTime());

            if(!discordServer.TargetChannelIds.Contains(request.ChannelId))
                discordServer.TargetChannelIds.Add(request.ChannelId);

            _masterDataService.UpsertDiscordServer(discordServer);

            return Task.FromResult(new Response());
        }
    }
}