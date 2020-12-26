using System;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.MasterData.Models
{
    public class TargetChannel
    {
        public string ServerId { get; set; }
        public string ChannelId { get; set; }
        public DateTime DateAdded { get; set; }

        public Server Server { get; set; }
    }
}