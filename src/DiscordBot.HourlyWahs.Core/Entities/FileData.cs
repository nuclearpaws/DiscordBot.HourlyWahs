namespace DiscordBot.HourlyWahs.Core.Entities
{
    public class FileData
    {
        public byte[] Data { get; private set; }
        public string FileFormat { get; private set; }

        public FileData(byte[] data, string fileFormat)
        {
            Data = data;
            FileFormat = fileFormat;
        }
    }
}