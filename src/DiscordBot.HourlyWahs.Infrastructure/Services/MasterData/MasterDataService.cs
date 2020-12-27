using System;
using System.Collections.Generic;
using System.Linq;
using DiscordBot.HourlyWahs.Core.Entities;
using DiscordBot.HourlyWahs.Core.Interfaces;
using DiscordBot.HourlyWahs.Infrastructure.Services.MasterData.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.MasterData
{
    internal class MasterDataService : IMasterDataService
    {
        private readonly MasterDataContext _context;
        private readonly IDateTimeService _dateTimeService;

        public MasterDataService(MasterDataContext context, IDateTimeService dateTimeService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
        }

        public IEnumerable<DiscordServer> GetAllDiscordServers()
        {
            var discordServers = _context
                .Servers
                .AsNoTracking()
                .Select(e => MapDataToDiscordServer(e))
                .ToList();
            
            return discordServers;
        }

        public DiscordServer GetDiscordServer(ulong discordServerId)
        {
            var server = _context
                .Servers
                .AsNoTracking()
                .Include(e => e.TargetChannels)
                .FirstOrDefault(e => e.ServerId == $"{discordServerId}");

            if(server == null)
                return null;

            var discordServer = MapDataToDiscordServer(server);
            return discordServer;
        }

        public void UpsertDiscordServer(DiscordServer discordServer)
        {
            var server = _context
                .Servers
                .Include(e => e.TargetChannels)
                .FirstOrDefault(e => e.ServerId == $"{discordServer.ServerId}");

            var now = _dateTimeService.GetCurrentDateTime();

            if(server == null)
            {
                server = new Server
                {
                    ServerId = $"{discordServer.ServerId}",
                    DateAdded = now,
                    TargetChannels = discordServer
                        .TargetChannelIds
                        .Select(targetChannelId => new TargetChannel
                        {
                            Server = server,
                            ChannelId = targetChannelId,
                            DateAdded = now,
                        })
                        .ToList(),
                };

                _context
                    .Servers
                    .Add(server);
            }
            else
            {
                var targetChannelsToDelete = server
                    .TargetChannels
                    .Where(e => !discordServer.TargetChannelIds.Contains(e.ChannelId))
                    .ToList();
                
                var targetChannelsToAdd = discordServer
                    .TargetChannelIds
                    .Where(e => !server.TargetChannels.Select(f => f.ChannelId).Contains(e))
                    .Select(e => new TargetChannel
                    {
                        ChannelId = e,
                        Server = server,
                        DateAdded = now,
                    })
                    .ToList();

                _context
                    .TargetChannels
                    .RemoveRange(targetChannelsToDelete);

                _context
                    .TargetChannels
                    .AddRange(targetChannelsToAdd);
            }

            _context.SaveChanges();
        }

        public void DeleteDiscordServer(DiscordServer discordServer)
        {
            var server = _context
                .Servers
                .FirstOrDefault(e => e.ServerId == $"{discordServer.ServerId}");

            if(server == null)
                throw new ApplicationException($"Discord Server '{discordServer.ServerId}' does not exist in persistance.");

            _context
                .Servers
                .Remove(server);

            _context.SaveChanges();
        }

        private DiscordServer MapDataToDiscordServer(Server server)
        {
            var result = new DiscordServer
            {
                ServerId = uint.Parse(server.ServerId),
                TargetChannelIds = server.TargetChannels.Select(channel => channel.ChannelId).ToList(),
                DateAdded = server.DateAdded,
            };

            return result;
        }
    }
}