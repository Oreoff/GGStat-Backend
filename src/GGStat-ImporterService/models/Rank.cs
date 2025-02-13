using System.ComponentModel.DataAnnotations;

namespace models
{
	public class Rank
	{
		[Key]
		public int Id { get; set; }
		public int points { get; set; }
		public string league { get; set; }

	}
}
