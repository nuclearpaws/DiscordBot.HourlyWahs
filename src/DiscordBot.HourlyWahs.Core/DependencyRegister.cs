using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.HourlyWahs.Core
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddCore(this IServiceCollection @this, IConfiguration configuration)
        {
            return @this;
        }
    }
}
