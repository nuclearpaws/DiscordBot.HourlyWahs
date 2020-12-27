using System;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.MasterData.Models
{
    public class TargetChannel
    {
        public ulong ServerId { get; set; }
        public ulong ChannelId { get; set; }
        public DateTime DateAdded { get; set; }

        public Server Server { get; set; }
    }
}