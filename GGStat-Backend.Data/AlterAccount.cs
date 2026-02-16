namespace GGStat_Backend.Data;

public class AlterAccount
{
    public string name { get; set; }
    public string? league { get; set; }
    public int? mmr { get; set; }
    public bool IsQualified { get; set; }
}