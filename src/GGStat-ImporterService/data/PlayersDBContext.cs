using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using models;
namespace data
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
			modelBuilder.Entity<PlayerData>()
				.HasOne(pd => pd.player)
				.WithMany()
				.HasForeignKey(pd => pd.PlayerId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<PlayerData>()
				.HasOne(pd => pd.country)
				.WithMany()
				.HasForeignKey(pd => pd.CountryId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<PlayerData>()
				.HasOne(pd => pd.rank)
				.WithMany()
				.HasForeignKey(pd => pd.RankId)
				.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<PlayerData>()
	.HasMany(pd => pd.matches)
	.WithOne(m => m.PlayerData)
	.HasForeignKey(m => m.PlayerDataId)
	.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<Match>()
	.HasMany(m => m.chat)
	.WithOne()
	.HasForeignKey("MatchId")
	.OnDelete(DeleteBehavior.Cascade);
		}

	}
	public class PlayersDBContextFactory : IDesignTimeDbContextFactory<PlayersDBContext>
	{
		public PlayersDBContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<PlayersDBContext>();
			var connectionString = config.GetConnectionString("DefaultConnection");

			optionsBuilder.UseNpgsql(connectionString);

			return new PlayersDBContext(optionsBuilder.Options);
		}
	}
}

