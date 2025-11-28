using Microsoft.Extensions.DependencyInjection;

namespace GGStat_Backend.ImporterService;

public static class Bootstrap
{
    public static void ImporterServiceRegister(this IServiceCollection services)
    {
        services.AddSingleton<ICsvParser, CsvParser>();
        services.AddHostedService<ImporterServiceWorker>();
    }
}