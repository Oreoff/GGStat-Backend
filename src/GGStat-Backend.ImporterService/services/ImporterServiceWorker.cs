using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GGStat_Backend.Data;
using GGStatBackend.Infrastructure;

namespace GGStat_Backend.ImporterService;

public class ImporterServiceWorker : BackgroundService
{
    private readonly ICsvParser _csvParser;
    private readonly IPlayersDbRepository _playersDbRepository;
    private readonly ILogger<ImporterServiceWorker> _logger;
    private FileSystemWatcher _watcher;
    private DateTime _lastProcessedTime = DateTime.MinValue;

    public ImporterServiceWorker(
        ICsvParser csvParser,
        IPlayersDbRepository playersDbRepository,
        ILogger<ImporterServiceWorker> logger)
    {
        _csvParser = csvParser;
        _playersDbRepository = playersDbRepository;
        _logger = logger;
    }

    private bool _isProcessing = false;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var csvPath = Environment.GetEnvironmentVariable("PLAYER_INFO_CSV")
                      ?? throw new InvalidOperationException("PLAYER_INFO_CSV is not set");

        Console.WriteLine($"Watching CSV: {csvPath}");

        string? lastHash = null;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (File.Exists(csvPath))
                {
                    using var stream = File.OpenRead(csvPath);
                    var hash = Convert.ToHexString(
                        System.Security.Cryptography.SHA256.HashData(stream)
                    );

                    if (hash != lastHash)
                    {
                        lastHash = hash;
                        Console.WriteLine("CSV changed → importing…");

                        await WaitUntilFileStable(csvPath);

                        var data = _csvParser.GetDataFromCsv();

                        await _playersDbRepository.ClearDatabaseAsync();
                        await _playersDbRepository.SavePlayersToDatabase(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
    private static async Task WaitUntilFileStable(string path)
    {
        long size1, size2;

        do
        {
            size1 = new FileInfo(path).Length;
            await Task.Delay(500);
            size2 = new FileInfo(path).Length;
        }
        while (size1 != size2);
    }
}