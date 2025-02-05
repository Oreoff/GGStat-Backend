using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GGStat.ImporterService.services;
using GGStat.ImporterService.data;
var configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

var services = new ServiceCollection();
services.AddDbContext<PlayersDBContext>(options =>
	options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
services.AddTransient<SetData>();
var serviceProvider = services.BuildServiceProvider();
var setData = serviceProvider.GetRequiredService<SetData>();
await setData.ClearDatabaseAsync();
await setData.SavePlayersToDatabase();
Console.WriteLine("Data has been succesfully written");