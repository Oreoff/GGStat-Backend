using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using GGStatParsingDataService.Models;
namespace GGStatParsingDataService.Services;

    public class MatchesTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<Match>();

            var matches = new List<Match>();
            var entries = text.Split('|', StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split(';');
                if (parts.Length >= 10)
                {
                    matches.Add(new Match
                    {
                        match_id = parts[0].Trim(),
                        match_link = parts[1].Trim(),
                        result = parts[2].Trim(),
                        points = int.TryParse(parts[3].Trim(), out var pts) ? pts : 0,
                        timeAgo = parts[4].Trim(),
                        map = parts[5].Trim(),
                        duration = parts[6].Trim(),
                        player_race = parts[7].Trim(),
                        opponent_race = parts[8].Trim(),
                        opponent = parts[9].Trim()
                    });
                }
            }
            return matches;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value is List<Match> matches && matches.Count > 0)
            {
                return string.Join("|", matches.Select(m =>
                    $"{m.match_id};{m.match_link};{m.result};{m.points};{m.timeAgo};" +
                    $"{m.map};{m.duration};{m.player_race};{m.opponent_race};{m.opponent}"));
            }
            return string.Empty;
        }
    }

    public sealed class PlayerDataMap : ClassMap<PlayerData>
    {
        public PlayerDataMap()
        {
            Map(p => p.standing).Default(0);
            Map(p => p.player.name);
            Map(p => p.player.alias);
            Map(p => p.player.region);
            Map(p => p.player.avatar);
            Map(p => p.country.code);
            Map(p => p.country.flag);
            Map(p => p.rank.points).Default(0);
            Map(p => p.rank.league);
            Map(p => p.race);
            Map(p => p.wins).Default(0);
            Map(p => p.loses).Default(0);
            Map(p => p.max_mmr).Default(0);
            Map(p => p.current_mmr).Default(0);
            Map(p => p.accounts);
            Map(p => p.matches).TypeConverter<MatchesTypeConverter>();

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
            Map(p => p.max_mmr);
            Map(p => p.current_mmr);
            Map(p => p.accounts);
            Map(p => p.matches).Convert(p =>
                p.Value == null ? string.Empty : string.Join(" | ", p.Value.matches.Select(m =>
                    $"{m.match_id};{m.match_link};{m.result};{m.points};{m.timeAgo}," +
                    $"{m.map};{m.duration};{m.player_race};{m.opponent_race};{m.opponent}")));
        }
}