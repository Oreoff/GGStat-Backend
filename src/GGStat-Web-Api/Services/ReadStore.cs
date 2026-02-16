using GGStat_Backend.Data;

namespace GGStat_Backend.controllers;

public interface IReadStore
{
    IReadOnlyList<PlayerData> Players { get; }
    IReadOnlyDictionary<string, CountryTop> CountryTops { get; }

    void SetPlayers(List<PlayerData> players);
    void SetCountryTops(Dictionary<string, CountryTop> countryTops);
}
public class InMemoryReadStore : IReadStore
{
    private List<PlayerData> _players = new();
    private Dictionary<string, CountryTop> _countryTops = new();

    public IReadOnlyList<PlayerData> Players => _players;
    public IReadOnlyDictionary<string, CountryTop> CountryTops => _countryTops;

    public void SetPlayers(List<PlayerData> players)
    {
        _players = players;
    }

    public void SetCountryTops(Dictionary<string, CountryTop> countryTops)
    {
        _countryTops = countryTops;
    }
}