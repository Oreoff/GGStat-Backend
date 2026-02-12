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
            string CountryCsvFilePath = FileDirectoryParser.GetDirectoryForPlayerInfo(); 

            if (Settings.parseOnlyPlayers)
            {
                var filePath = FileDirectoryParser.GetDirectoryForLeaderboardToDocker();
                var dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                await dataParser.WriteListAsync(filePath);
                return;
            }

            var data = await csvParserService.ReadData();

            if (Settings.PlayerInfoOffset > 0)
            {
                logger.LogInformation($"Resuming from offset {Settings.PlayerInfoOffset}, appending to existing file.");
            }

            data = await playerInfoParser.GetPlayerInfo(data, csvParserService, CountryCsvFilePath);
            logger.LogInformation($"{data.Count} players saved to CSV.");
        }
    }
}