using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGStat_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace data
{
	public class PlayersDBContext : DbContext
	{
		public PlayersDBContext(DbContextOptions<PlayersDBContext> options) : base(options) { }

		public DbSet<PlayerData> PlayerData { get; set; }
		public DbSet<Match> Matches { get; set; }
		public DbSet<Chat> Chats { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
			modelBuilder.Entity<PlayerData>()
				.ToTable("PlayerData");

			modelBuilder.Entity<Match>()
				.ToTable("Matches");

			modelBuilder.Entity<Chat>()
				.ToTable("Chats");
			modelBuilder.Entity<PlayerData>()
	.HasMany(pd => pd.matches)
	.WithOne()
	.HasForeignKey(m => m.PlayerDataId)
	.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<Match>()
	.HasMany(m => m.chat)
	.WithOne()
	.HasForeignKey("MatchId")
	.OnDelete(DeleteBehavior.Cascade);
		}
	}
}

