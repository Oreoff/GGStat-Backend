namespace GGStatBackend.Infrastructure;

public static class FileDirectoryParser
{
    public static string GetDirectoryForLeaderboard()
    { 
        string solutionDirectory =
        Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
        return Path.Combine(solutionDirectory, "db", "players.csv");
    }

    public static string GetDirectoryForPlayerInfo()
    {
        string solutionDirectory =
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
        return Path.Combine(solutionDirectory, "db", "players_with_countries.csv");
    }

    public static string GetDirectoryForLeaderboardToDocker()
    {
        return Path.Combine("/app/db", "players.csv");
    }
    public static string GetDirectoryForPlayerInfoToDocker()
    {
        return Path.Combine("/app/db", "players_with_countries.csv");
    }
}