using GGStat_Backend.controllers;
using GGStatParsingDataService.data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GGStatParsingDataService.services
{
	internal class SetData
	{
		private readonly PlayersDBContext _context;

		public SetData(PlayersDBContext context)
		{
			_context = context;
		}

		public async Task SavePlayersToDatabase(int offset)
		{
			var playersFromApi = await GetData.GetPlayersAsync(offset);

			foreach (var player in playersFromApi)
			{
				var existingPlayer = await _context.PlayerDatas
					.AsNoTracking() // Додаємо, щоб уникнути помилки оновлення
					.FirstOrDefaultAsync(p => p.standing == player.standing);

				if (existingPlayer != null)
				{
					player.Id = existingPlayer.Id; // Не змінюємо Id, а залишаємо старий
					_context.PlayerDatas.Update(player); // Оновлюємо запис
				}
				else
				{
					await _context.PlayerDatas.AddAsync(player); // Додаємо новий запис
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
