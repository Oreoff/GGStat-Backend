
using System.ComponentModel.DataAnnotations;

namespace GGStatParsingDataService.Models
{
	public class PlayerData
	{
		[Key] public int Id { get; set; }
		public int standing { get; set; }

		public int PlayerId { get; set; }
		public Player player { get; set; }

		public int CountryId { get; set; }
		public CountryInfo country { get; set; }

		public int RankId { get; set; }
		public Rank rank { get; set; }

		public string race { get; set; }
		public int wins { get; set; }
		public int loses { get; set; }

		public List<Match>? matches { get; set; } = new List<Match>();

		public string MatchesStr => string.Join("|", matches == null
			? string.Empty
			: matches.Select(m =>
				$"{m.match_id};{m.match_link};{m.result};{m.points};{m.timeAgo};" +
				$"{m.map};{m.duration};{m.player_race};{m.opponent_race};{m.opponent}"));

		public override string ToString()
		{
			return this.standing.ToString() + player.name + country.code;
		}
	}
}
