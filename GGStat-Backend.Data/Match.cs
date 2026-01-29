using System.ComponentModel.DataAnnotations;
using GGStat_Backend.Data;

public class Match
{
	[Key]
	public int Id { get; set; }
	public string? match_id { get; set; }
	public string? match_link { get; set; }
	public string? result { get; set; }
	public int points { get; set; }
	public string? timeAgo { get; set; }
	public string? map { get; set; }
	public string? duration { get; set; }
	public string? player_race { get; set; }
	public string? opponent_race { get; set; }
	public string? opponent { get; set; }
	public ICollection<Chat>? chat { get; set; } = new List<Chat>();
	public int PlayerDataId { get; set; }
}
