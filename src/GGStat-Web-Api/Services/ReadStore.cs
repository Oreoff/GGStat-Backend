using GGStat_Backend.Data;

namespace GGStat_Backend.controllers;

public interface IReadStore
{
    IReadOnlyList<PlayerData> Players { get; }
    IReadOnlyList<CountryTop> CountryTops { get; }

    void SetPlayers(List<PlayerData> players);
    void SetCountryTops(List<CountryTop> countryTops);
}
public class InMemoryReadStore : IReadStore
{
    private List<PlayerData> _players = new();
    private List<CountryTop> _countryTops = new();

    public IReadOnlyList<PlayerData> Players => _players;
    public IReadOnlyList<CountryTop> CountryTops => _countryTops;

    public void SetPlayers(List<PlayerData> players)
    {
        _players = players;
    }

    public void SetCountryTops(List<CountryTop> countryTops)
    {
        _countryTops = countryTops;
    }
}