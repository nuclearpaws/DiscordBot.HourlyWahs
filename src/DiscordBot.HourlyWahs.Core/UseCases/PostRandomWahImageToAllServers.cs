using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    public class PostRandomWahImageToAllServers : IRequestHandler<PostRandomWahImageToAllServers.Request, PostRandomWahImageToAllServers.Response>
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
        }

        private readonly IMasterDataService _masterDataService;
        private readonly IDiscordService _discordService;
        private readonly IImagesService _imagesService;

        public PostRandomWahImageToAllServers(IMasterDataService masterDataService, IDiscordService discordService, IImagesService imagesService)
        {
            _masterDataService = masterDataService;
            _discordService = discordService;
            _imagesService = imagesService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var image = _imagesService.GetRandomWahImage();
            var servers = _masterDataService.GetAllDiscordServers();

            foreach(var server in servers)
            foreach(var channelId in server.TargetChannelIds)
                await _discordService.SendMessageWithImageAsync(server.ServerId, channelId, null, image);
            
            return new Response();
        }
    }
}