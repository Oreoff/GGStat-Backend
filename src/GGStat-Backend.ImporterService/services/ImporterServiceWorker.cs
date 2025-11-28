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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string directory = Path.GetDirectoryName(FileDirectoryParser.GetDirectoryForPlayerInfoToDocker())!;
        string fileName = Path.GetFileName(FileDirectoryParser.GetDirectoryForPlayerInfoToDocker());

        _watcher = new FileSystemWatcher(directory, fileName);
        _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
        _watcher.EnableRaisingEvents = true;

        _watcher.Changed += async (sender, args) =>
        {
            if (_isProcessing)
                return;

            _isProcessing = true;

            try
            {
                await Task.Delay(200); 
                var data = _csvParser.GetDataFromCsv();
                await _playersDbRepository.ClearDatabaseAsync();
                await _playersDbRepository.SavePlayersToDatabase(data);
            }
            finally
            {
                _isProcessing = false;
            }
        };

        return Task.CompletedTask;
    }
}