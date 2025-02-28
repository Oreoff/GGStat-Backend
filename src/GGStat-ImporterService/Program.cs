﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using data;
using services;

var configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

var services = new ServiceCollection();
services.AddDbContext<PlayersDBContext>(options =>
	options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
services.AddTransient<SetData>();
var serviceProvider = services.BuildServiceProvider();

// Apply migrations automatically
using (var scope = serviceProvider.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<PlayersDBContext>();
	await dbContext.Database.MigrateAsync(); // This applies any pending migrations
}

var setData = serviceProvider.GetRequiredService<SetData>();
await setData.SavePlayersToDatabase();
Console.WriteLine("Data has been successfully written");