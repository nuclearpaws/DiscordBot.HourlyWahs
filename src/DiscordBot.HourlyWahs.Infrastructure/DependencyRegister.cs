using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.HourlyWahs.Core.Interfaces;
using DiscordBot.HourlyWahs.Infrastructure.Discord;

namespace DiscordBot.HourlyWahs.Infrastructure
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection @this, IConfiguration configuration) 
        {
            @this.AddTransient<IDiscordApi, DiscordApi>();

            return @this;
        }
    }
}
