using GGStat_Backend.controllers;
using GGStatParsingDataService.data;
using GGStatParsingDataService.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
int offset = 0;
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
while(offset <= 3000)
{
	await setData.SavePlayersToDatabase(offset);
	Console.WriteLine("New players added");
	offset += Settings.BatchSize;
}

Console.WriteLine("Data has been succesfully collected");