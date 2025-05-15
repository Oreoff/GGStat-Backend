using GGStatParsingDataService.data;
using GGStatParsingDataService.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
int offset = 0;

string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
string filePath = Path.Combine(solutionDirectory, "db", "players.csv");
Settings.Port = await Settings.GetPort();
while (offset <= 25)
{
	var players = await GetData.GetPlayersAsync(offset); 

	if (players != null && players.Count > 0)
	{
		await CsvWriterService.WriteToCsvAsync(players, filePath);
		Console.WriteLine($"{players.Count} players saved to CSV.");
	}

	offset += Settings.BatchSize;
}

Console.WriteLine("Data has been successfully collected.");