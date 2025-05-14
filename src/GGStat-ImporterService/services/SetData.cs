using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using data;

namespace services
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
			var playersFromCsv = GetDataFromCSV.SaveData();
			Console.WriteLine($"📦 Players loaded from CSV: {playersFromCsv.Count}");

			foreach (var player in playersFromCsv)
			{
				var existingPlayer = await _context.PlayerDatas
					.Include(p => p.matches)
					.FirstOrDefaultAsync(p => p.standing == player.standing);

				if (existingPlayer != null)
				{
					_context.Matches.RemoveRange(existingPlayer.matches);
					await _context.SaveChangesAsync();

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
			Console.WriteLine("🧹 База даних очищена.");
		}
	}
}