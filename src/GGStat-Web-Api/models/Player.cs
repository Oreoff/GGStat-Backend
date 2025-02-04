using System.ComponentModel.DataAnnotations;

namespace GGStat_Backend.models
{
	public class Player
	{
		[Key] 
		public int Id { get; set; }
		public string? name { get; set; }
		public string? region { get; set; }
		public string? alias { get; set; }
		public string? avatar { get; set; }
	}
}
