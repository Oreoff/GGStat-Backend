
using data;
using GGStat_Backend.Data;
using GGStat_Backend.ImporterService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
            services.DataRegister(connectionString);
            services.ImporterServiceRegister();
        }).Build();
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PlayersDBContext>();

            //TODO comment to not drop db
            await DbUtils.KillConnectionsAsync(db, "GGStatDB");
            await db.Database.EnsureDeletedAsync();
           await db.Database.EnsureCreatedAsync();
        }
        await host.RunAsync();
    }
}