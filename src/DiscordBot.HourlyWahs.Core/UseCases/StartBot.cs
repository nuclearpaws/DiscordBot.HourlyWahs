using System.Threading;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Interfaces;
using MediatR;

namespace DiscordBot.HourlyWahs.Core.UseCases
{
    public class StartBot : IRequestHandler<StartBot.Request, StartBot.Response>
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
        }

        private readonly IDiscordService _discordService;

        public StartBot(IDiscordService discordService)
        {
            _discordService = discordService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            await _discordService.StartBotAsync();
            return new Response();
        }
    }
}