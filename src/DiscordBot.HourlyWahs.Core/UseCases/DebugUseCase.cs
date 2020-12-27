using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    public class DebugUseCase : IRequestHandler<DebugUseCase.Request, DebugUseCase.Response>
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
        }

        private readonly IImagesService _imagesService;
        private readonly IDiscordService _discordService;

        public DebugUseCase(IImagesService imagesService, IDiscordService discordService)
        {
            _imagesService = imagesService;
            _discordService = discordService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var randomImage = _imagesService.GetRandomWahImage();
            await _discordService.SendMessageWithImageAsync(198933308694986752UL, 786345258786488361UL, "Test 123", randomImage);
            return new Response();
        }
    }
}