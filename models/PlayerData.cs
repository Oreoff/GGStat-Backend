namespace GGStat_Backend.models
{
	public class PlayerData
	{
		public int Standing { get; set; }
		public Player Player { get; set; }
		public Country Country { get; set; }
		public Rank Rank { get; set; }
		public string Race { get; set; }
		public int Wins { get; set; }
		public int Loses { get; set; }
		public List<Match> Matches { get; set; }
	}
}
