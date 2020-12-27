using System;
using System.IO;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Entities;
using DiscordBot.HourlyWahs.Core.Interfaces;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.Discord
{
    internal class DiscordService : IDiscordService
    {
        private readonly Bot _bot;

        public DiscordService(Bot bot)
        {
            _bot = bot;
        }

        public Task StartBotAsync()
        {
            return _bot.StartAsync();
        }

        public Task StopBotAsync()
        {
            return _bot.StopAsync();
        }

        public async Task SendMessageWithImageAsync(ulong serverId, ulong channelId, string message, FileData image)
        {
            if(!_bot.IsRunning)
                throw new ApplicationException("The underlying bot service is not running.");

            var fileName = $"{Guid.NewGuid()}.{image.FileFormat}";
            using(var stream = new MemoryStream(image.Data))
            {
                await _bot.SendFileAsync(serverId, channelId, stream, fileName);
            }
        }
    }
}