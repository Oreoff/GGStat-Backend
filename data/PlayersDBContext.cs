using GGStat_Backend.models;
using Microsoft.EntityFrameworkCore;
namespace GGStat_Backend.data
{
	public class PlayersDBContext : DbContext
	{
		public PlayersDBContext(DbContextOptions<PlayersDBContext> options) : base(options) { }

		public DbSet<PlayerData> PlayerDatas { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<CountryInfo> CountryInfos { get; set; }
		public DbSet<Rank> Ranks { get; set; }
		public DbSet<Match> Matches { get; set; }
		public DbSet<Chat> Chats { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Визначення зв'язків
			modelBuilder.Entity<PlayerData>()
				.HasOne(pd => pd.player)
				.WithMany()
				.HasForeignKey(pd => pd.PlayerId);

			modelBuilder.Entity<PlayerData>()
				.HasOne(pd => pd.country)
				.WithMany()
				.HasForeignKey(pd => pd.CountryId);

			modelBuilder.Entity<PlayerData>()
				.HasOne(pd => pd.rank)
				.WithMany()
				.HasForeignKey(pd => pd.RankId);

			modelBuilder.Entity<Match>()
				.HasMany(m => m.chat)
				.WithOne()
				.HasForeignKey("MatchId"); 
		}
	}
}
