namespace DiscordBot.HourlyWahs.Infrastructure
{
    internal static class Constants
    {
        public static class ConfigurationStrings
        {
            public static class ConnectionStrings
            {
                public const string MasterData = "MasterData";
            }

            public static class Discord
            {
                public const string Token = "Discord:Token";
                public const string CommandPrefix = "Discord:CommandPrefix";
            }

            public static class Files
            {
                public const string RootDirectory = "Files:RootDirectory";
                public const string MaxFileSizeInBytes = "Files:MaxFileSizeInBytes";
                public const string AcceptedExtensions = "Files:AcceptedExtensions";
            }
        }
    }
}