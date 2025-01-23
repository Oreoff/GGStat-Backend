namespace GGStat_Backend.models
{
	public class Match 
	{
		public string result { get; set; }
		public int points { get; set; }
		public string timeAgo { get; set; }
		public string map { get; set; }
		public string duration { get; set; }
		public string player_race { get; set; }
		public string opponent_race { get; set; }
		public string opponent { get; set; }
		public List<Chat> chat { get; set; }
	}
}
