﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.HourlyWahs.Core.Interfaces;
using DiscordBot.HourlyWahs.Infrastructure.Services.Discord;
using DiscordBot.HourlyWahs.Infrastructure.Services.Images;
using DiscordBot.HourlyWahs.Infrastructure.Services.MasterData;
using DiscordBot.HourlyWahs.Infrastructure.Services.Misc;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiscordBot.HourlyWahs.Infrastructure
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection @this, IConfiguration configuration) 
        {
            // Register Discord Service:
            @this.AddSingleton<Bot>(); // This MUST be a singleton!
            @this.AddTransient<IDiscordService, DiscordService>();

            // Register Image Service:
            @this.AddSingleton<Random>(); // Probably a good idea to just have 1 instance of Random
            @this.AddTransient<IImagesService, ImagesService>();

            // Register Master Data Service:
            const string masterDataConnectionStringConfigurationName = "MasterData";
            var masterDataConnectionString = configuration.GetConnectionString(masterDataConnectionStringConfigurationName);
            if(string.IsNullOrWhiteSpace(masterDataConnectionString))
                throw new ApplicationException($"Missing connection string '{masterDataConnectionStringConfigurationName}' in configuration.");

            @this
                .AddDbContext<MasterDataContext>(builderOptions => builderOptions.UseSqlite(masterDataConnectionString))
                .AddTransient<IMasterDataService, MasterDataService>();
            
            // Register Other Infrastructure Services:
            @this.AddSingleton<IDateTimeService, DateTimeService>();

            // Done:
            return @this;
        }
    }
}
