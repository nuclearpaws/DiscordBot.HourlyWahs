using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    public class PostRandomWahImageToSpecificChannel : IRequestHandler<PostRandomWahImageToSpecificChannel.Request, PostRandomWahImageToSpecificChannel.Response>
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

        private readonly IDiscordService _discordService;
        private readonly IImagesService _imagesService;

        public PostRandomWahImageToSpecificChannel(IDiscordService discordService, IImagesService imagesService)
        {
            _discordService = discordService;
            _imagesService = imagesService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var image = _imagesService.GetRandomWahImage();
            await _discordService.SendMessageWithImageAsync(request.ServerId, request.ChannelId, null, image);
            return new Response();
        }
    }
}