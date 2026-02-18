using GGStat_Backend.controllers;

namespace GGStat_Backend;

public static class Bootstrap
{
    public static void GetGgStatApiRegister(this IServiceCollection services)
    {
        services.AddSingleton<IReadStore, InMemoryReadStore>();
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetLeaderboardHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetCountryTopHandler).Assembly));
    }
}