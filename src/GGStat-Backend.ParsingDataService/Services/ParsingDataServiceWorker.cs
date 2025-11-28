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
            int offset = 0;
            List<PlayerData> list = new List<PlayerData>();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (File.Exists(CountryCsvFilePath))
            {
                File.Delete(CountryCsvFilePath);
            }
            bool isFirstBatch = true;
               

            while (offset < Settings.MaxSize)
            {
                int retryCount = 0;
                bool success = false;

                while (!success && retryCount < 7)
                {
                    try
                    {
                        var players = await dataParser.GetPlayersAsync(offset);
                        list.AddRange(players);
                        offset += Settings.BatchSize;
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        logger.LogError($"Error occurred (attempt {retryCount}/7): {ex.Message}");

                        if (retryCount == 7)
                        {
                            logger.LogError("Maximum retry attempts reached. Exiting program.");
                            Environment.Exit(1);
                        }
                        else
                        {
                            await Task.Delay(1000);
                        }
                    }
                }
                if (list.Count > 0 )
                {
                    await csvParserService.WriteLeaderboardToCsvAsync(list, filePath, isFirstBatch);
                    isFirstBatch = false;
                    logger.LogInformation($"{list.Count} players saved to CSV.");
                }
                else
                {
                    logger.LogCritical("No players were found to parse, breaking");
                    break;
                }
                list.Clear();
            }
            var data = await csvParserService.ReadData();
            data = await playerInfoParser.GetPlayerInfo(data);
            
           
            
            await csvParserService.WriteToCsvWithCountry(data,CountryCsvFilePath);
            logger.LogInformation($"{data.Count} players saved to CSV.");
        }
    }
}