using models;
using System.ComponentModel.DataAnnotations;

public class PlayerData
{
	[Key]
	public int Id { get; set; }
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
}
