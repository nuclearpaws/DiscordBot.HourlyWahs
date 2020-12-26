using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DiscordBot.HourlyWahs.Core;
using DiscordBot.HourlyWahs.Infrastructure;
using DiscordBot.HourlyWahs.Worker.Workers;

namespace DiscordBot.HourlyWahs.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddInfrastructure(hostContext.Configuration);
                    services.AddCore(hostContext.Configuration);
                    services.AddHostedService<DiscordWorker>();
                    services.AddHostedService<ImageCacheWorker>();
                });
    }
}
