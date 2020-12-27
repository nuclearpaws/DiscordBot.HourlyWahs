using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordBot.HourlyWahs.Core.Entities;
using DiscordBot.HourlyWahs.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DiscordBot.HourlyWahs.Infrastructure.Services.Images
{
    internal class ImagesService : IImagesService
    {
        private readonly Random _random;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ImagesService(Random random, IConfiguration configuration, ILogger<ImagesService> logger)
        {
            _random = random;
            _configuration = configuration;
            _logger = logger;
        }

        public FileData GetRandomWahImage()
        {
            // Get operational parameters:
            var rootDirectory = _configuration.GetValue<string>(Constants.ConfigurationStrings.Files.RootDirectory);
            var maxFileSize = _configuration.GetValue<long>(Constants.ConfigurationStrings.Files.MaxFileSizeInBytes);
            var extensions = _configuration.GetSection(Constants.ConfigurationStrings.Files.AcceptedExtensions).AsEnumerable().Select(v => v.Value).Where(v => v != null).ToArray();

            // Confirm Directory Exists, and if not create directory:
            if(!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            // Get all files for specified extensions in a specific directory:
            var fileInfos = new List<FileInfo>();
            foreach(var extension in extensions)
            {
                var filePaths = Directory.GetFiles(rootDirectory, $"*.{extension}");
                foreach(var filePath in filePaths)
                {
                    var fileInfo = new FileInfo(filePath);
                    if(fileInfo.Length <= maxFileSize)
                        fileInfos.Add(fileInfo);
                }
            }

            // Handle no files existing:
            if(fileInfos.Count <= 0)
                throw new ApplicationException($"There are no files in '{Path.GetFullPath(rootDirectory)}'.");

            // Get random file:
            var selectedRandomFile = fileInfos[_random.Next(0, fileInfos.Count)];

            // Read file:
            var fileData = default(byte[]);
            using(var sr = new StreamReader(selectedRandomFile.FullName))
            {
                using(var ms = new MemoryStream())
                {
                    sr.BaseStream.CopyTo(ms);
                    fileData = ms.ToArray();
                }
            }
            
            // Get file extension:
            var fileFormat = selectedRandomFile.Extension.Trim('.');

            // Generate response:
            var response = new FileData(fileData, fileFormat);
            return response;
        }
    }
}