using CsvHelper.Configuration;
using GGStat_Backend.Data;

namespace GGStat_Backend.ImporterService;

public class PlayerDataMap : ClassMap<PlayerData>
{
    public PlayerDataMap()
    {
        Map(p => p.standing).Index(0);
        Map(p => p.name).Index(1);
        Map(p => p.code).Index(2);
        Map(p => p.points).Index(3);
        Map(p => p.alias).Index(4);
        Map(p => p.flag).Index(5);
        Map(p => p.league).Index(6);
        Map(p => p.region).Index(7);
        Map(p => p.avatar).Index(8);
        Map(p => p.race).Index(9);
        Map(p => p.wins).Index(10);
        Map(p => p.loses).Index(11);
        Map(p => p.matches).Index(12).TypeConverter<MatchListConverter>();
    }
}