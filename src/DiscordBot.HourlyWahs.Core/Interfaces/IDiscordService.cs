using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.Entities;

namespace DiscordBot.HourlyWahs.Core.Interfaces
{
    public interface IDiscordService
    {
        Task StartBotAsync();
        Task StopBotAsync();
        Task SendMessageWithImageAsync(ulong serverId, ulong channelId, string message, FileData image);
    }
}