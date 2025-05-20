
using GGStatParsingDataService.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

internal class Program
{
	private static async Task Main(string[] args)
	{
		 await CSVWorker.WriteLeaderboardToCSV();
		var data = await CSVWorker.SaveData();
		data = await CSVWorker.FindCountry(data);
		await CSVWorker.WriteCountriesToCSV(data);
	}
}