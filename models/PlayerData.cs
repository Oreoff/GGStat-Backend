namespace GGStat_Backend.models
{
	public class PlayerData
	{
		public int standing { get; set; }
		public Player player { get; set; }
		public Country country { get; set; }
		public Rank rank { get; set; }
		public string race { get; set; }
		public int wins { get; set; }
		public int loses { get; set; }
		public List<Match> matches { get; set; }
	}
}
