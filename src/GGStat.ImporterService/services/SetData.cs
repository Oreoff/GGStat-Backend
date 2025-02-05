using GGStat.ImporterService.services;
using GGStat.ImporterService.data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GGStat.ImporterService.services
{
	internal class SetData
	{
		private readonly PlayersDBContext _context;

		public SetData(PlayersDBContext context)
		{
			_context = context;
		}

		public async Task SavePlayersToDatabase()
		{
			var playersFromApi = GetDataFromCSV.SaveData();
			Console.WriteLine(playersFromApi.Count);
			foreach (var player in playersFromApi)
			{
				var existingPlayer = await _context.PlayerDatas
					.AsNoTracking() 
					.FirstOrDefaultAsync(p => p.standing == player.standing);

				if (existingPlayer != null)
				{
					player.Id = existingPlayer.Id; 
					_context.PlayerDatas.Update(player); 
				}
				else
				{
					await _context.PlayerDatas.AddAsync(player); 
				}
			}

			await _context.SaveChangesAsync();
		}
		public async Task ClearDatabaseAsync()
		{
			await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"PlayerDatas\" RESTART IDENTITY CASCADE;");
			await _context.SaveChangesAsync();
			Console.WriteLine("База даних очищена.");
		}
	}
}
