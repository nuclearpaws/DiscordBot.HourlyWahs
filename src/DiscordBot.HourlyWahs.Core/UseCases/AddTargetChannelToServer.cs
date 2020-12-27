using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Entities;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    public class AddTargetChannelToServer : IRequestHandler<AddTargetChannelToServer.Request, AddTargetChannelToServer.Response>
    {
        public class Request : IRequest<Response>
        {
            public ulong ServerId { get; set; }
            public ulong ChannelId { get; set; }

            public Request(ulong serverId, ulong channelId)
            {
                ServerId = serverId;
                ChannelId = channelId;
            }
        }

        public class Response
        {
        }

        private readonly IMasterDataService _masterDataService;
        private readonly IDateTimeService _dateTimeService;

        public AddTargetChannelToServer(IMasterDataService masterDataService, IDateTimeService dateTimeService)
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