using CsvHelper.Configuration;
using GGStatParsingDataService.Models;
namespace GGStatParsingDataService.Services;
    public sealed class PlayerDataMap : ClassMap<PlayerData>
    {
        public PlayerDataMap()
        {
            Map(p => p.standing);
            Map(p => p.player.name);
            Map(p => p.player.alias);
            Map(p => p.player.region);
            Map(p => p.player.avatar);
            Map(p => p.country.code);
            Map(p => p.country.flag);
            Map(p => p.rank.points);
            Map(p => p.rank.league);
            Map(p => p.race);
            Map(p => p.wins);
            Map(p => p.loses);
            Map(p => p.matches);

        }
    }
    public sealed class PlayerDataMapWithCountry : ClassMap<PlayerData>
    {
        public PlayerDataMapWithCountry()
        {
            Map(p => p.standing);
            Map(p => p.player.name);
            Map(p => p.player.alias);
            Map(p => p.player.region);
            Map(p => p.player.avatar);
            Map(p => p.country.code);
            Map(p => p.country.flag);
            Map(p => p.rank.points);
            Map(p => p.rank.league);
            Map(p => p.race);
            Map(p => p.wins);
            Map(p => p.loses);
            Map(p => p.matches).Convert(p =>
                p.Value == null ? string.Empty : string.Join(" | ", p.Value.matches.Select(m =>
                    $"{m.match_id};{m.match_link};{m.result};{m.points};{m.timeAgo}," +
                    $"{m.map};{m.duration};{m.player_race};{m.opponent_race};{m.opponent}")));
        }
}