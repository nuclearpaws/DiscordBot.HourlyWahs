using System;
using System.Collections.Generic;

namespace DiscordBot.HourlyWahs.Core.Entities
{
    public class DiscordServer
    {
        public ulong ServerId { get; set; }
        public ICollection<ulong> TargetChannelIds { get; set; }
        public DateTime DateAdded { get; set; }

        public DiscordServer()
        {
            TargetChannelIds = new List<ulong>();
        }

        public DiscordServer(ulong serverId, DateTime dateAdded)
            : this()
        {
            ServerId = serverId;
            DateAdded = dateAdded;
        }
    }
}