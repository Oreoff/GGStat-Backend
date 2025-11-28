
public static class TimeParser
{
    public static string GetTime(long unixTime)
    {
        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        DateTime normalDate = dateTime.UtcDateTime;
        return normalDate.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string ParseDuration(int duration)
    {
        duration = duration - 12;   //11-12 sec gap between actual duration and time in seconds from api
        return $"{duration/60} minutes, {duration%60} seconds";
    }
}