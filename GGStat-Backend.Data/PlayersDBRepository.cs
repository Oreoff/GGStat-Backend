using data;
using Microsoft.EntityFrameworkCore;

namespace GGStat_Backend.Data
{
	public interface IPlayersDbRepository
	{
		Task SavePlayersToDatabase(List<PlayerData> playersFromCsv);
		Task ClearDatabaseAsync();
	}
	public class PlayersDbRepository(IDbContextFactory<PlayersDBContext> _contextFactory): IPlayersDbRepository
	{
		public async Task SavePlayersToDatabase(List<PlayerData> playersFromCsv)
		{
			await using var _context = await _contextFactory.CreateDbContextAsync();
			_context.AddRange(playersFromCsv);
			await _context.SaveChangesAsync();
		}

		public async Task ClearDatabaseAsync()
		{
			await using var _context = await _contextFactory.CreateDbContextAsync();
			await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"PlayerData\" RESTART IDENTITY CASCADE;");
			await _context.SaveChangesAsync();
			Console.WriteLine("Db cleared");
		}
	}
}