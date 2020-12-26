using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.HourlyWahs.Core.Interfaces;
using DiscordBot.HourlyWahs.Infrastructure.Services.MasterData;
using DiscordBot.HourlyWahs.Infrastructure.Services.Discord;
using DiscordBot.HourlyWahs.Infrastructure.Services.Images;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiscordBot.HourlyWahs.Infrastructure
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection @this, IConfiguration configuration) 
        {
            // Register Discord Service:
            @this.AddTransient<IDiscordService, DiscordService>();

            // Register Image Service:
            @this.AddTransient<IImagesService, ImagesService>();

            // Register Master Data Service:
            const string masterDataConnectionStringConfigurationName = "MasterData";
            var masterDataConnectionString = configuration.GetConnectionString(masterDataConnectionStringConfigurationName);
            if(string.IsNullOrWhiteSpace(masterDataConnectionString))
                throw new ApplicationException($"Missing connection string '{masterDataConnectionStringConfigurationName}' in configuration.");

            @this
                .AddDbContext<MasterDataContext>(builderOptions => builderOptions.UseSqlite(masterDataConnectionString))
                .AddTransient<IMasterDataService, MasterDataService>();
            
            // Done:
            return @this;
        }
    }
}
