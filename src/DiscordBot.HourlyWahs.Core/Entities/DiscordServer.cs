using System;
using System.Collections.Generic;

namespace DiscordBot.HourlyWahs.Core.Entities
{
    public class DiscordServer
    {
        public string ServerId { get; set; }
        public ICollection<string> TargetChannelIds { get; set; }
        public DateTime DateAdded { get; set; }

        public DiscordServer()
        {
            TargetChannelIds = new List<string>();
        }

        public DiscordServer(string serverId, DateTime dateCreated)
            : this()
        {
        }
    }
}