using data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GGStat_Backend.Data;

public static class Bootstrap
{
    public static void DataRegister(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<PlayersDBContext>(options => options.UseNpgsql(connectionString));
        services.AddSingleton<IPlayersDbRepository, PlayersDbRepository>();
    }
}