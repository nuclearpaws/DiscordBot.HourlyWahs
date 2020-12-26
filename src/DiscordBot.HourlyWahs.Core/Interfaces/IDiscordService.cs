namespace DiscordBot.HourlyWahs.Core.Interfaces
{
    public interface IDiscordService
    {
        void SendMessageWithImage(string serverId, string channelId, string message, object image);
    }
}