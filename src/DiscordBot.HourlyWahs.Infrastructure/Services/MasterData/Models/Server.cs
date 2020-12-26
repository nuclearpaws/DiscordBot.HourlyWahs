using System;
using System.Collections.Generic;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.MasterData.Models
{
    public class Server
    {
        public string ServerId { get; set; }
        public DateTime DateAdded { get; set; }

        public ICollection<TargetChannel> TargetChannels { get; set; }

        public Server()
        {
            TargetChannels = new List<TargetChannel>();
        }
    }
}