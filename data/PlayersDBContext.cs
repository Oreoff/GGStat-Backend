using GGStat_Backend.models;
using Microsoft.EntityFrameworkCore;
namespace GGStat_Backend.data
{
	public class PlayersDBContext:DbContext
	{
		public PlayersDBContext(DbContextOptions<PlayersDBContext> options)
		: base(options)
		{

		}
		public DbSet<PlayerData> PlayerDatas { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<CountryInfo> CountryInfos { get; set; }
		public DbSet<Rank> Ranks { get; set; }
		public DbSet<Match> Matches { get; set; }
		public DbSet<Chat> Chats { get; set; }
	}
}
