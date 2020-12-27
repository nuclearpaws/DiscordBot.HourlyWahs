using System;
using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    public class RemoveTargetChannelFromServer : IRequestHandler<RemoveTargetChannelFromServer.Request, RemoveTargetChannelFromServer.Response>
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

        public RemoveTargetChannelFromServer(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var discordServer = _masterDataService.GetDiscordServer(request.ServerId);
            if(discordServer == null)
                throw new ApplicationException($"Discord Server '{request.ServerId}' does not exist in the persistance.");

            if(!discordServer.TargetChannelIds.Contains(request.ChannelId))
                throw new ApplicationException($"Discord Server Channel '{request.ChannelId}' does not exist against Discord Server '{request.ServerId}' in persistance.");

            discordServer.TargetChannelIds.Remove(request.ChannelId);
            _masterDataService.UpsertDiscordServer(discordServer);

            return Task.FromResult(new Response());
        }
    }
}