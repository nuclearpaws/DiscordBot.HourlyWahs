using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.HourlyWahs.Core.UseCases;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.Discord
{
    internal class Bot
    {
        // This is bad but I have no idea how else to get mediator inside DiscordBotCommands:
        private static IMediator _mediator;

        internal class DiscordBotCommands
        {
            [Command("addchannel")]
            [Aliases("ac")]
            [Description("Add the channel from which it is run to the list of channels to which an hourly wah will be posted.")]
            public async Task AddChannel(CommandContext context)
            {
                var serverId = context.Guild.Id;
                var channelId = context.Channel.Id;
                await _mediator.Send(new AddTargetChannelToServer.Request(serverId, channelId));
                await context.Channel.SendMessageAsync("Channel added to Hourly Wah Posts! OwO");
            }

            [Command("removechannel")]
            [Aliases("rc")]
            [Description("Remove the channel from which it is run from the list of channels to which an hourly wahs are being posted.")]
            public async Task RemoveChannel(CommandContext context)
            {
                var serverId = context.Guild.Id;
                var channelId = context.Channel.Id;
                await _mediator.Send(new RemoveTargetChannelFromServer.Request(serverId, channelId));
                await context.Channel.SendMessageAsync("Channel removed to Hourly Wah Posts! OmO");
            }

            [Command("listchannels")]
            [Aliases("lc")]
            [Description("List channels currently expecting hourly wahs on this server.")]
            public async Task ListChannels(CommandContext context)
            {
                var serverId = context.Guild.Id;
                var channels = await context.Guild.GetChannelsAsync();

                var response = await _mediator.Send(new GetTargetChannelsForServer.Request(serverId));
                var channelNames = channels.Where(c => response.ChannelIds.Contains(c.Id)).Select(c => c.Name);

                var message = new StringBuilder();
                message.AppendLine("Currently Targetted Channels:");
                message.AppendLine();
                foreach(var channelName in channelNames)
                {
                    message.AppendLine($"\t- {channelName}");
                }
                
                await context.Channel.SendMessageAsync(message.ToString());
            }

            [Command("getwah")]
            [Aliases("gw", "wah")]
            [Description("Get a random image of a red panda.")]
            public async Task GetWah(CommandContext context)
            {
                var serverId = context.Guild.Id;
                var channelId = context.Channel.Id;
                await _mediator.Send(new PostRandomWahImageToSpecificChannel.Request(serverId, channelId));
            }
        }

        public bool IsRunning { get; private set; }

        private readonly ILogger _logger;
        private readonly DiscordClient _discordClient;

        public Bot(IMediator mediator, IConfiguration configuration, ILogger<Bot> logger)
        {
            IsRunning = false;

            if(_mediator == null)
                _mediator = mediator;

            var token = configuration.GetValue<string>(Constants.ConfigurationStrings.Discord.Token);
            if(string.IsNullOrWhiteSpace(token))
                throw new ApplicationException($"Configuration property '{Constants.ConfigurationStrings.Discord.Token}' not set.");

            var commandPrefix = configuration.GetValue<string>(Constants.ConfigurationStrings.Discord.CommandPrefix);
            if(string.IsNullOrWhiteSpace(commandPrefix))
                throw new ApplicationException($"Configuration property '{Constants.ConfigurationStrings.Discord.CommandPrefix}' not set.");

            _discordClient = SetupClient(token);
            SetupCommands(commandPrefix);

            _logger = logger;
        }

        private DiscordClient SetupClient(string token)
        {
            var client = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                UseInternalLogHandler = false,
            });

            client.ClientErrored += e => {
                _logger.LogError(e.Exception, $"[{nameof(DiscordClient)}] An unexpected exception has occurred (Event: '{e.EventName}').");
                return Task.CompletedTask;
            };
            client.Heartbeated += e => {
                _logger.LogInformation($"[{nameof(DiscordClient)}] Heartbeat done at '{e.Timestamp}' ({e.Ping}ms ping).");
                return Task.CompletedTask;
            };

            return client;
        }

        private CommandsNextModule SetupCommands(string commandPrefix)
        {
            var commandsNextModule = _discordClient.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = commandPrefix,
                EnableMentionPrefix = true,
                EnableDms = false,
                EnableDefaultHelp = true,
                CaseSensitive = false,
                IgnoreExtraArguments = false,
            });

            commandsNextModule.RegisterCommands<DiscordBotCommands>();

            commandsNextModule.CommandErrored += e => {
                var message = new StringBuilder();
                message.Append($"[{nameof(CommandsNextModule)}] ");
                message.Append($"An unexpected error has occurred with command '{e.Command?.Name ?? "null"}(");
                var arguments = new StringBuilder();
                if(e.Command != null && e.Command.Arguments != null)
                {
                    foreach(var argument in e.Command.Arguments)
                    {
                        arguments.Append($"{argument.Name}[{argument.Type}], ");
                    }
                }
                message.Append($"{arguments.ToString().Trim().Trim(',')})'.");
                _logger.LogError(e.Exception, message.ToString());

                e.Context.Channel.SendMessageAsync("An unexpected error has occurred...");
                return Task.CompletedTask;
            };
            commandsNextModule.CommandExecuted += e => {
                var message = new StringBuilder();
                message.Append($"[{nameof(CommandsNextModule)}] ");
                message.Append($"Executing command '{e.Command?.Name ?? "null"}(");
                var arguments = new StringBuilder();
                if(e.Command != null && e.Command.Arguments != null)
                {
                    foreach(var argument in e.Command.Arguments)
                    {
                        arguments.Append($"{argument.Name}[{argument.Type}], ");
                    }
                }
                message.Append($"{arguments.ToString().Trim().Trim(',')})'.");
                _logger.LogInformation(message.ToString());
                return Task.CompletedTask;
            };

            return commandsNextModule;
        }

        public Task StartAsync()
        {
            var task = _discordClient.ConnectAsync();
            IsRunning = true;
            return task;
        }

        public Task StopAsync()
        {
            IsRunning = false;
            return _discordClient.DisconnectAsync();
        }

        public async Task SendFileAsync(ulong serverId, ulong channelId, Stream dataStream, string fileName, string message = null)
        {
            var server = await _discordClient.GetGuildAsync(serverId);
            var channel = server.GetChannel(channelId);

            if(channel == null)
                throw new ApplicationException($"Channel '{channelId}' is not part of server '{serverId}'.");

            await channel.SendFileAsync(dataStream, fileName, message);
        }
    }
}