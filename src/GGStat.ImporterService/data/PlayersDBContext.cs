using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGStat.ImporterService.models;
using Microsoft.EntityFrameworkCore;
namespace GGStat.ImporterService.data
{
    public class PlayersDBContext:DbContext
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


				
				modelBuilder.Entity<Match>()
					.HasMany(m => m.chat)
					.WithOne()
					.HasForeignKey("MatchId")
					.OnDelete(DeleteBehavior.Cascade); 
			}
		
	}
}
