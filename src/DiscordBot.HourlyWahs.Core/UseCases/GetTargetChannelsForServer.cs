using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Entities;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    public class GetTargetChannelsForServer : IRequestHandler<GetTargetChannelsForServer.Request, GetTargetChannelsForServer.Response>
    {
        public class Request : IRequest<Response>
        {
            public ulong ServerId { get; set; }

            public Request(ulong serverId)
            {
                ServerId = serverId;
            }
        }

        public class Response
        {
            public IEnumerable<ulong> ChannelIds { get; set; }
        }

        private readonly IMasterDataService _masterDataService;

        public GetTargetChannelsForServer(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var server = _masterDataService.GetDiscordServer(request.ServerId);
            if(server == null)
                throw new ApplicationException($"Cannot find any server data for server '{request.ServerId}'.");

            var response = new Response
            {
                ChannelIds = server.TargetChannelIds,
            };

            return Task.FromResult(response);
        }
    }
}