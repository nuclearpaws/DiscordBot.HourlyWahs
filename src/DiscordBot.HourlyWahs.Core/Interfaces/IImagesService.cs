using DiscordBot.HourlyWahs.Core.Entities;

namespace DiscordBot.HourlyWahs.Core.Interfaces
{
    public interface IImagesService
    {
        FileData GetRandomWahImage();
    }
}