using GGStatParsingDataService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortWrapper;

namespace GGStatParsingDataService;

public static class Bootstrap
{
    public static void GGStatParserRegister(this IServiceCollection services)
    {
        services.AddSingleton<ICsvParserService, CsvParserService>();
        services.AddSingleton<ILeaderboardParser, LeaderboardParser>();
        services.AddSingleton<IPlayerInfoParser, PlayerInfoParser>();
        services.AddTransient<IPortParser, PortParser>();
        services.AddHostedService<ParsingDataServiceWorker>();
    }
}