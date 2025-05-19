
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
	}
}