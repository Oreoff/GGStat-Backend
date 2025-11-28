using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GGStat_Backend.ImporterService;

public class MatchListConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new List<Match>();
        text = System.Text.RegularExpressions.Regex.Replace(text, @"[\x00-\x1F\x7F]", "");

        return text.Split('|')
            .Select(m => 
            {

                var fields = m.Split(';');
                if (fields.Length > 3)
                {
                    var timeAndMap = fields[4].Split(',', 2);
                    string timeAgo = timeAndMap.Length > 0 ? timeAndMap[0] : "";
                    string map = timeAndMap.Length > 1 ? timeAndMap[1] : "";
                    if (fields.Length < 11)
                    {
                        Console.WriteLine($"Warning: Invalid match data format (missing fields): {m}");
                        while (fields.Length < 11)
                        {
                            Array.Resize(ref fields, fields.Length + 1);
                            fields[fields.Length - 1] = "";
                        }
                    }

                    return new Match
                    {
                        match_id = fields[0],
                        match_link = fields[1],
                        result = fields[2],
                        points = int.TryParse(fields[3], out int p) ? p : 0,
                        timeAgo = timeAgo,
                        map = map,
                        duration = fields[5],
                        player_race = fields[6],
                        opponent_race = fields[7],
                        opponent = fields[8],
                        chat = null
                    };
                }
                return null; 
            })
            .Where(m => m != null)
            .ToList();
    }
			
}