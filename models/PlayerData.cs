using GGStat_Backend.models;
using System.ComponentModel.DataAnnotations;

public class PlayerData
{
	[Key]
	public int Id { get; set; }
	public int standing { get; set; }

	public int PlayerId { get; set; } // Зовнішній ключ
	public Player player { get; set; }

	public int CountryId { get; set; } // Зовнішній ключ
	public CountryInfo country { get; set; }

	public int RankId { get; set; } // Зовнішній ключ
	public Rank rank { get; set; }

	public string race { get; set; }
	public int wins { get; set; }
	public int loses { get; set; }

	public List<Match> matches { get; set; }
}
