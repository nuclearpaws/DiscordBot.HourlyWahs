using System;
using DiscordBot.HourlyWahs.Core.Interfaces;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.Misc
{
    public class DateTimeService : IDateTimeService
    {
        public System.DateTime GetCurrentDateTime()
        {
            var now = DateTime.Now;
            return now;
        }
    }
}