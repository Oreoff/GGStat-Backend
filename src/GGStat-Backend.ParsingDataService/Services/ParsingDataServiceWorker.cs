using System.ComponentModel;
using GGStatBackend.Infrastructure;
using GGStatParsingDataService.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using PortWrapper;

namespace GGStatParsingDataService.Services;

public class ParsingDataServiceWorker(
    ILogger<ParsingDataServiceWorker> logger,
    IPortParser portParser,
    ILeaderboardParser dataParser, 
    ICsvParserService csvParserService,
    IPlayerInfoParser playerInfoParser
    ):BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    { 
        Settings.Port = await portParser.GetPort();
        while (!stoppingToken.IsCancellationRequested)
        {
            var filePath = FileDirectoryParser.GetDirectoryForLeaderboardToDocker();
            var dir = Path.GetDirectoryName(filePath);
            string CountryCsvFilePath = FileDirectoryParser.GetDirectoryForPlayerInfoToDocker(); 
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir); 
            }
            await dataParser.WriteListAsync(filePath);
            var data = await csvParserService.ReadData();
            data = await playerInfoParser.GetPlayerInfo(data);
            await csvParserService.WriteToCsvWithCountry(data,CountryCsvFilePath);
            logger.LogInformation($"{data.Count} players saved to CSV.");
        }
    }
}