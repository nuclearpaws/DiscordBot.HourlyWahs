using System.Collections.Generic;
using DiscordBot.HourlyWahs.Core.Entities;

namespace DiscordBot.HourlyWahs.Core.Interfaces
{
    public interface IMasterDataService
    {
        IEnumerable<DiscordServer> GetAllDiscordServers();
        DiscordServer GetDiscordServer(ulong discordServerId);
        void UpsertDiscordServer(DiscordServer discordServer);
        void DeleteDiscordServer(DiscordServer discordServer);
    }
}