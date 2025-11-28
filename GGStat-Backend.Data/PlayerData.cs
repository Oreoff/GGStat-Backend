using System.ComponentModel.DataAnnotations;

namespace GGStat_Backend.Data;
public class PlayerData
{
	[Key]
	public int Id { get; set; }
	public int standing { get; set; }
	public string? name { get; set; }
	public string? region { get; set; }
	public string? alias { get; set; }
	public string? avatar { get; set; }
	public string? code { get; set; }
	public string? flag { get; set; }
	public int points { get; set; }
	public string league { get; set; }
	public string race { get; set; }
	public int wins { get; set; }
	public int loses { get; set; }
	public ICollection<Match>? matches { get; set; } = new List<Match>();
}
